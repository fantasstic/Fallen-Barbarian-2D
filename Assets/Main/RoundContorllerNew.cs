using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UFE3D;
using UFENetcode;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;

public class RoundContorllerNew : MonoBehaviour
{
    [SerializeField] private float _maxYPos;
    [SerializeField] private MoveInfo _rollMove, _moveKick, _runMove, _kickBot;
    [SerializeField] private RoundContorllerNew _roundContorller;
    [SerializeField] private List<Sprite> _backGrounds = new List<Sprite>();
    [SerializeField] private Image _back;
    [SerializeField] private MoveSetScript _moveSetScript;
    [SerializeField] private GameObject _buttonManager;
    [SerializeField] private UFE3D.GlobalInfo _globalInfo;
    [SerializeField] private ObjectPool _fallEffectPool;

    private GameObject _background;
    private GameObject _gameUI;
    private bool _roundEnd;
    public bool RoundStart;
    private bool _enemyDamageBack;
    private ControlsScript _player;
    private ControlsScript _enemy;
    private OpenTutor _openTutor;
    private int _wins;
    private bool _gamepadConnected = false;
    public bool IsMainScreen = true;
    public bool CanJump;

    private static bool _secondStart;
    public bool EnemyWinner;

    private void Awake()
    {
        _rollMove.hits[0].unblockable = false;
        UFE.OnRoundBegins += OnRoundBegins;
        UFE.OnRoundEnds += OnRoundEnds;
        UFE.OnBlock += OnBlock;
        UFE.OnBasicMove += UFE_OnBasicMove;
        UFE.OnLifePointsChange += OnLifePointsChange;

        if (_secondStart)
        {
            UFE.StartVersusModeAfterBattleScreen();

            if (PlayerPrefs.GetString("Win") == "Enemy")
            {
                _back.gameObject.SetActive(false);
            }
            else
            {
                int currentBack = UnityEngine.Random.Range(0, _backGrounds.Count);
                _back.sprite = _backGrounds[currentBack];
                _back.gameObject.SetActive(true);
                Invoke("OffBack", 3f);
            }

        }
        else
        {
            if (!PlayerPrefs.HasKey("ShuffleMode"))
                PlayerPrefs.SetString("ShuffleMode", "No");

            PlayerPrefs.SetString("ShuffleMode", "No");

            PlayerPrefs.SetInt("Wins", 1);
        }
    }

    private void OnDestroy()
    {
        UFE.OnRoundBegins -= OnRoundBegins;
        UFE.OnRoundEnds -= OnRoundEnds;
        UFE.OnBlock -= OnBlock;
        UFE.OnBasicMove -= UFE_OnBasicMove;
        UFE.OnLifePointsChange -= OnLifePointsChange;
    }

    void OnLifePointsChange(float newFloat, ControlsScript player)
    {
        if(player.name == "Player2")
        {
            _enemyDamageBack = false;
        }
    }

    private void OffBack()
    {
        _back.gameObject.SetActive(false);
    }

    private void UFE_OnBasicMove(BasicMoveReference basicMove, ControlsScript player)
    {
        

        if (basicMove == BasicMoveReference.BlockingHighHit || basicMove == BasicMoveReference.BlockingHighPose)
        {
            if(CanJump)
            {
                Debug.Log("Jump");
                player.Physics.Jump();
            }
            _rollMove.hits[0].unblockable = true;
        }
        else
        {
            if (_player == null || _enemy == null)
                return;

            if (_player.currentBasicMove is not (BasicMoveReference.BlockingHighHit or BasicMoveReference.BlockingHighPose) 
                && _enemy.currentBasicMove is not (BasicMoveReference.BlockingHighHit or BasicMoveReference.BlockingHighPose))
                _rollMove.hits[0].unblockable = false;

            if (basicMove == BasicMoveReference.HitStandingMidKnockdown)
            {
                var otherPlayer = _enemy == player ? _player : _enemy;

                if (otherPlayer.currentMove.name == _moveKick.name || otherPlayer.currentMove.name == _kickBot.name)
                {
                    player.Physics.ResetForces(true, false);
                    player.Physics.AddForce(new FPLibrary.FPVector(-25, 0, 0), otherPlayer.transform.position.x > player.transform.position.x ? 1 : -1);
                    player.stunTime = 0.5f;
                }

                /*if (otherPlayer.currentMove.name == _runMove.name)
                {
                    player.Physics.ResetForces(true, false);
                    player.Physics.AddForce(new FPLibrary.FPVector(-38, 0, 0), otherPlayer.transform.position.x > player.transform.position.x ? 1 : -1);
                    player.stunTime = 1;
                }*/
            }

            if(basicMove == BasicMoveReference.HitStandingHighKnockdown || basicMove == BasicMoveReference.HitStandingMidKnockdown)
            {
                //var otherPlayer = _enemy == player ? _player : _enemy;

                StartCoroutine(CreateEffectWithDelay(0.3f, player));
            }
        }
    }

    private IEnumerator CreateEffectWithDelay(float delay, ControlsScript player)
    {
        yield return new WaitForSeconds(delay);
        Vector3 spawnPos = player.transform.position;
        if (player.mirror == 1)
        {
            spawnPos.x += 3; // Спавн эффект левее игрока
        }
        else
        {
            spawnPos.x -= 3; // Спавн эффект правее игрока
        }

        GameObject effect = _fallEffectPool.GetObject();
        effect.transform.position = spawnPos;
        effect.transform.rotation = Quaternion.identity;
    }

    private void OnBlock(HitBox strokeHitBox, MoveInfo move, ControlsScript player)
    {
        var playerControl = UFE.GetControlsScript(1);
        var enemyControl = UFE.GetControlsScript(2);

        if (player.name == "Player2" && move.name == "RollMove" && strokeHitBox.type == HitBoxType.low)
        {
            playerControl.DamageMe(10, false);
        }

        if (player.name == "Player1" && move.name == "RollMove" && strokeHitBox.type == HitBoxType.low)
        {
            player.blockStunned = false;

            _enemy.KillCurrentMove();
            _moveSetScript.PlayBasicMove(_moveSetScript.basicMoves.getHitHighKnockdown);
            _enemy.currentSubState = SubStates.Stunned;
            _enemy.stunTime = 1;
            enemyControl.DamageMe(10, false);
        }

        if (player.name == "Player2" && move.name == "HgAttackMove" && strokeHitBox.type == HitBoxType.low)
        {
            enemyControl.DamageMe(10, false);
        }

        if (player.name == "Player1" && move.name == "HgAttackMove" && strokeHitBox.type == HitBoxType.low)
        {
            playerControl.DamageMe(10, false);
        }

        if (player.name == "Player1" && move.name == "jump_kick_Move" && strokeHitBox.type == HitBoxType.low)
        {
            playerControl.DamageMe(10, false);
        }

        if (player.name == "Player2" && move.name == "jump_kick_Move" && strokeHitBox.type == HitBoxType.low)
        {
            enemyControl.DamageMe(10, false);
        }

        if (player.name == "Player1" && move.name == "jump_kick_Move" && strokeHitBox.type == HitBoxType.high)
        {
            if(!_enemyDamageBack)
            {
                player.blockStunned = false;
                _enemy.KillCurrentMove();
                _moveSetScript.PlayBasicMove(_moveSetScript.basicMoves.getHitHighKnockdown);
                _enemy.currentSubState = SubStates.Stunned;
                _enemy.stunTime = 1;
                _enemy.currentState = PossibleStates.Down;
                enemyControl.DamageMe(10, false);
                _enemyDamageBack = true;
            }
        }

        if (player.name == "Player2" && move.name == "jump_kick_Move" && strokeHitBox.type == HitBoxType.high)
        {
            playerControl.DamageMe(10, false);
        }

        if (player.name == "Player1" && move.name == "Attack_Low_Bot" && strokeHitBox.type == HitBoxType.low)
        {
            player.blockStunned = false;

            _enemy.KillCurrentMove();
            _moveSetScript.PlayBasicMove(_moveSetScript.basicMoves.getHitHigh);
            _enemy.currentSubState = SubStates.Stunned;
            _enemy.stunTime = 0.5f;
            _enemy.currentState = PossibleStates.Stand;
        }

        if (player.name == "Player1" && move.name == "HgAttakBot" && strokeHitBox.type == HitBoxType.high)
        {
            player.blockStunned = false;

            _enemy.KillCurrentMove();
            _moveSetScript.PlayBasicMove(_moveSetScript.basicMoves.getHitHigh);
            _enemy.currentSubState = SubStates.Stunned;
            _enemy.stunTime = 0.5f;
            _enemy.currentState = PossibleStates.Stand;
        }
    }

    private void OnRoundBegins(int newInt)
    {
        RoundStart = true;

        RuntimePlatform platform = Application.platform;

        if (platform == RuntimePlatform.Android)
            _buttonManager.SetActive(true);

        _player = UFE.GetControlsScript(1);
        _enemy = UFE.GetControlsScript(2);
        _moveSetScript = _enemy.GetComponentInChildren<MoveSetScript>();

        if(!PlayerPrefs.HasKey("Wins"))
        {
            _wins = 0;
            PlayerPrefs.SetInt("Wins", _wins);
            _enemy.MoveSet.ChangeMoveStances(CombatStances.Stance1);
        }
        else
        {
            //_wins = 4;
            _wins = PlayerPrefs.GetInt("Wins");
        }

        
        //_wins = 3;

        switch (_wins)
        {
            case 0:
                _enemy.MoveSet.ChangeMoveStances(CombatStances.Stance1);
                break;
            case 1:
                _enemy.MoveSet.ChangeMoveStances(CombatStances.Stance2);
                break;
            case 2:
                _enemy.MoveSet.ChangeMoveStances(CombatStances.Stance3);
                break;
            case 3:
                _enemy.MoveSet.ChangeMoveStances(CombatStances.Stance4);
                break;
            case 4:
                _enemy.MoveSet.ChangeMoveStances(CombatStances.Stance5);
                break;
            case 5:
                _enemy.MoveSet.ChangeMoveStances(CombatStances.Stance6);
                break;
            case 6:
                _enemy.MoveSet.ChangeMoveStances(CombatStances.Stance7);
                break;


        }
    }

    private void OnRoundEnds(ControlsScript winner, ControlsScript loser)
    {
        if (_roundEnd)
            return;

        if (UFE.config.inputOptions.inputManagerType == InputManagerType.CustomClass)
            _buttonManager.SetActive(false);

        RoundStart = false;
        Debug.Log(winner);
        _gameUI = GameObject.Find("CanvasGroup");

        if(winner == _enemy)
        {
            PlayerPrefs.SetString("Win", "Enemy");
            EnemyWinner = true;
        }
        else
        {
            PlayerPrefs.SetString("Win", "Player");

            int wins = PlayerPrefs.GetInt("Wins");
            wins++;

            Debug.Log(wins);

            if(wins > 6)
                PlayerPrefs.SetInt("Wins", 1);
            else
                PlayerPrefs.SetInt("Wins", wins);
        }

        UFE.StartVersusModeAfterBattleScreen(0.1f);
        
        _roundEnd = true;
    }

    private void LoadGoblinScene()
    {
        _secondStart = true;

        if(EnemyWinner)
            SceneManager.LoadScene(1);
        else
            SceneManager.LoadScene(0);
    }

    private float _lastEnemyYPos, _lastPlayerYPos;

    private void CheckHitInJump(ControlsScript player, ref float lastYPos)
    {
        if ((player.currentState == PossibleStates.ForwardJump 
            || player.currentState == PossibleStates.NeutralJump 
            || player.currentState == PossibleStates.BackJump) 
            && (player.currentHit != null))
        {
            var diff = Mathf.Abs(lastYPos - player.transform.position.y);

            player.Physics.currentAirJumps = 0;

            if (diff < 0.01f)
            {
                player.Physics.ForceGrounded();
            }

            lastYPos = player.transform.position.y;
        }
    }

    private void ForceCharToGround(ControlsScript character)
    {
        if (character.transform.position.y >= 6)
            character.Physics.ForceGrounded();
    }

    private void Update()
    {
        if(RoundStart)
        {
            CheckHitInJump(_enemy, ref _lastEnemyYPos);
            CheckHitInJump(_player, ref _lastPlayerYPos);

            _player.isAirRecovering = false;
            _enemy.isAirRecovering = false;

            _player.airRecoveryType = AirRecoveryType.AllowMoves;
            _enemy.airRecoveryType = AirRecoveryType.AllowMoves;

            ForceCharToGround(_enemy);
            ForceCharToGround(_player);

            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Menu") && IsMainScreen)
            {
                if(UFE.isPaused())
                {
                    UFE.PauseGame(false);
                    IsMainScreen = true;
                }
                else
                {
                    UFE.PauseGame(true);
                    IsMainScreen = true;
                }
            }

            if(Input.GetKeyDown(KeyCode.Space))
            {
                /*if(_player.currentBasicMove == BasicMoveReference.BlockingHighHit || _player.currentBasicMove == BasicMoveReference.BlockingHighPose || _player.currentBasicMove == BasicMoveReference.BlockingCrouchingPose)
                {
                    _player.Physics.Jump();
                }*/
                if (_player.isBlocking)
                {
                    _player.currentSubState = SubStates.Resting;
                    _player.Physics.Jump();
                }
            }
            //Debug.Log(_player.stunTime);
        }

        if (_roundEnd)
        {
            if(_gameUI != null || EnemyWinner)
            {
                _gameUI.SetActive(false);
                Invoke("LoadGoblinScene", 0.3f);
            }
        }
    }

    private bool CheckGamepadConnection()
    {
        string[] joystickNames = Input.GetJoystickNames();
        foreach (string joystickName in joystickNames)
        {
            if (!string.IsNullOrEmpty(joystickName))
            {
                _gamepadConnected = true;
                Debug.Log("Геймпад подключен.");
            }
            else
            {
                _gamepadConnected = false;
                Debug.Log("Геймпад не подключен.");
            }
        }


        return _gamepadConnected;
    }
}
