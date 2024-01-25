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
    [SerializeField] private MoveInfo _rollMove, _moveKick, _runMove;
    [SerializeField] private RoundContorllerNew _roundContorller;
    [SerializeField] private List<Sprite> _backGrounds = new List<Sprite>();
    [SerializeField] private Image _back;

    private GameObject _background;
    private GameObject _gameUI;
    private bool _roundEnd;
    private bool _roundStart;
    private ControlsScript _player;
    private ControlsScript _enemy;
    private OpenTutor _openTutor;
    private int _wins;

    private static bool _secondStart;
    public bool EnemyWinner;

    private void Awake()
    {
        _rollMove.hits[0].unblockable = false;

        UFE.OnRoundBegins += OnRoundBegins;
        UFE.OnRoundEnds += OnRoundEnds;
        UFE.OnBlock += OnBlock;
        UFE.OnBasicMove += UFE_OnBasicMove;

        /*_background = GameObject.Find("Background");
        _background.SetActive(false);*/


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

            /*_background.SetActive(true);*/
        }
        else
        {
            if (!PlayerPrefs.HasKey("ShuffleMode"))
                PlayerPrefs.SetString("ShuffleMode", "No");

            PlayerPrefs.SetString("ShuffleMode", "No");

            PlayerPrefs.SetInt("Wins", 0);
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
            _rollMove.hits[0].unblockable = true;
            Debug.Log(_rollMove.hits[0].unblockable);
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

                if (otherPlayer.currentMove.name == _moveKick.name)
                {
                    player.Physics.ResetForces(true, false);
                    player.Physics.AddForce(new FPLibrary.FPVector(-25, 0, 0), otherPlayer.transform.position.x > player.transform.position.x ? 1 : -1);
                }

                if (otherPlayer.currentMove.name == _runMove.name)
                {
                    player.Physics.ResetForces(true, false);
                    player.Physics.AddForce(new FPLibrary.FPVector(-38, 0, 0), otherPlayer.transform.position.x > player.transform.position.x ? 1 : -1);
                }
            }
        }
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
            Debug.Log("Damage Back");

            enemyControl.DamageMe(10, false);
        }

        if (player.name == "Player2" && move.name == "HgAttackMove" && strokeHitBox.type == HitBoxType.low)
        {
            enemyControl.DamageMe(10, false);
        }

        if (player.name == "Player1" && move.name == "HgAttackMove" && strokeHitBox.type == HitBoxType.low)
        {
            Debug.Log("Damage Back");
            playerControl.DamageMe(10, false);
        }

        if (player.name == "Player1" && move.name == "jump_kick_Move" && strokeHitBox.type == HitBoxType.low)
        {
            playerControl.DamageMe(15, false);
        }

        if (player.name == "Player2" && move.name == "jump_kick_Move" && strokeHitBox.type == HitBoxType.low)
        {
            enemyControl.DamageMe(15, false);
        }

        if (player.name == "Player1" && move.name == "jump_kick_Move" && strokeHitBox.type == HitBoxType.high)
        {
            enemyControl.DamageMe(15, false);
        }

        if (player.name == "Player2" && move.name == "jump_kick_Move" && strokeHitBox.type == HitBoxType.high)
        {
            playerControl.DamageMe(15, false);
        }
    }

    private void OnRoundBegins(int newInt)
    {
        Debug.Log("OnRoundBegins");

        _roundStart = true;

        _player = UFE.GetControlsScript(1);
        _enemy = UFE.GetControlsScript(2);

        if(!PlayerPrefs.HasKey("Wins"))
        {
            _wins = 0;
            PlayerPrefs.SetInt("Wins", _wins);
            _enemy.MoveSet.ChangeMoveStances(CombatStances.Stance1);
        }
        else
        {
            _wins = PlayerPrefs.GetInt("Wins");
        }

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
                PlayerPrefs.SetInt("Wins", 0);
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

    private void Update()
    {
        if(_roundStart)
        {
            CheckHitInJump(_enemy, ref _lastEnemyYPos);
            CheckHitInJump(_player, ref _lastPlayerYPos);

            _player.isAirRecovering = false;
            _enemy.isAirRecovering = false;

            _player.airRecoveryType = AirRecoveryType.AllowMoves;
            _enemy.airRecoveryType = AirRecoveryType.AllowMoves;

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if(UFE.isPaused())
                    UFE.PauseGame(false);
                else
                    UFE.PauseGame(true);
            }
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
}
