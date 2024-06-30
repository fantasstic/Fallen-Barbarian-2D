using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UFE3D;
using System;
using Naninovel;
using TMPro;

public enum TutorialStage
{
    None,
    StartTutorial,
    SecondTutorial,
    ThirdTutorial,
    FourthTutorial

}

public class TutorialQuest : MonoBehaviour
{
    private int _hitCount;
    private TutorialStage _currentStage = TutorialStage.StartTutorial;
    private ControlsScript _player1;

    public CustomInterface Mover;
    public GameObject StartTutorial, HgAttckTutorial, HgBlockTutorial, LowBlockTutorial;
    public TMP_Text HitCounter;
    public bool TutorialDone;

    private void OnEnable()
    {
        if (!TutorialDone)
        {
            UFE.OnHit += OnHit;
            UFE.OnRoundBegins += OnRoundBegins;
            UFE.OnBasicMove += OnBasicMove;
        }
    }

    private void OnDisable()
    {
        if (!TutorialDone)
        {
            UFE.OnHit -= OnHit;
            UFE.OnRoundBegins -= OnRoundBegins;
            UFE.OnBasicMove -= OnBasicMove;
        }

    }

    private void OnBasicMove(BasicMoveReference basicMove, ControlsScript player)
    {
        if (!TutorialDone)
        {
            switch (_currentStage)
            {
                case TutorialStage.SecondTutorial:
                    if(basicMove == BasicMoveReference.BlockingHighPose)
                        _hitCount++;
                    HitCounter.text = _hitCount.ToString() + "/3";

                    if (_hitCount >= 3)
                    {
                        _hitCount = 0;
                        _player1.MoveSet.ChangeMoveStances(CombatStances.Stance2);
                        HitCounter.text = _hitCount.ToString() + "/3";
                        HgAttckTutorial.SetActive(true);
                        Mover.enabled = false;
                        _currentStage = TutorialStage.ThirdTutorial;
                    }
                    break;
            }
        }
    }

    private void OnRoundBegins(int newInt)
    {
        if(!TutorialDone)
        {
            _player1 = UFE.GetControlsScript(1);
            _player1.MoveSet.ChangeMoveStances(CombatStances.Stance1);
            Mover.enabled = false;
            StartTutorial.SetActive(true);
            HitCounter.gameObject.SetActive(true);
        }
    }

    private void OnHit(HitBox strokeHitBox, MoveInfo move, ControlsScript player)
    {
        if (!TutorialDone)
        {
            switch (_currentStage)
            {
                case TutorialStage.StartTutorial:
                    if (move.name == "Move_Kick")
                    {
                        _hitCount++;
                        HitCounter.text = _hitCount.ToString() + "/3";
                    }

                    if (_hitCount >= 3)
                    {
                        _hitCount = 0;
                        HitCounter.text = _hitCount.ToString() + "/3";
                        HgBlockTutorial.SetActive(true);
                        Mover.enabled = false;
                        _currentStage = TutorialStage.SecondTutorial;
                    }
                    break;
                case TutorialStage.ThirdTutorial:
                    if (move.name == "HgAttakMove")
                    {
                        _hitCount++;
                        HitCounter.text = _hitCount.ToString() + "/3";
                    }

                    if (_hitCount >= 3)
                    {
                        _currentStage = TutorialStage.FourthTutorial;
                        _hitCount = 0;
                        HitCounter.text = _hitCount.ToString() + "/3";
                        Mover.enabled = false;
                    }
                    break;
                    // добавл€йте другие стадии по мере необходимости
            }
        }
    }

}
