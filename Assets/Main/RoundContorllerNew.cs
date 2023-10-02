using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UFE3D;
using UFENetcode;
using UnityEngine.SceneManagement;
using System;

public class RoundContorllerNew : MonoBehaviour
{
    [SerializeField] private float _maxYPos;
    [SerializeField] private MoveInfo _rollMove, _moveKick, _runMove;
    [SerializeField] private RoundContorllerNew _roundContorller;

    private GameObject _background;
    private GameObject _gameUI;
    private bool _roundEnd;
    private bool _roundStart;
    private ControlsScript _player;
    private ControlsScript _enemy;

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
            /*_background.SetActive(true);*/
        }
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
            playerControl.DamageMe(100, false);
        }

        if (player.name == "Player1" && move.name == "RollMove" && strokeHitBox.type == HitBoxType.low)
        {
            Debug.Log("Damage Back");

            enemyControl.DamageMe(100, false);
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
    }

    private void OnRoundEnds(ControlsScript winner, ControlsScript loser)
    {
        Debug.Log(winner);
        _gameUI = GameObject.Find("CanvasGroup");

        if(winner == _enemy)
        {
            EnemyWinner = true;
        }
        
        /*_gameUI.SetActive(false);*/
        UFE.StartVersusModeAfterBattleScreen(0.1f);
        
        _roundEnd = true;

        /*if (_gameUI != null)
            _gameUI.SetActive(false);

        //LoadGoblinScene(); 
        Invoke("LoadGoblinScene", 2f);*/
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
                Invoke("LoadGoblinScene", 2f);
            }
        }
    }
}
