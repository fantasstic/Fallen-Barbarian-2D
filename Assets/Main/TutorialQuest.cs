using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UFE3D;
using System;
using Naninovel;
using TMPro;
using FPLibrary;

public enum TutorialStage
{
    None,
    StartTutorial,
    SecondTutorial,
    ThirdTutorial,
    FourthTutorial,
    FifthTutorial,
    SixthTutorial,
    SevenTutorial,
    EighthTutorial,
    NineTutorial,
    TenthTutorial,
    EleventhTutorial,
    TwelfthTutorial

}

public class TutorialQuest : MonoBehaviour
{
    private int _hitCount;
    private TutorialStage _currentStage = TutorialStage.StartTutorial;
    private ControlsScript _player1;
    private ControlsScript _player2;

    public CustomInterface Mover;
    public GameObject StartTutorial, HgAttckTutorial, HgBlockTutorial, LowBlockTutorial, LowAttackTutorial, RollForwardTutorial, RollBackTutorial, RunTutorial, JumpTutorial, JumpAttackTutorial, SuperHgTutorial, SuperLowTutorial, FinalDialoge;
    public TMP_Text HitCounter;
    public bool TutorialDone;

    private void OnEnable()
    {
        
        if (!TutorialDone)
        {
            UFE.OnHit += OnHit;
            UFE.OnRoundBegins += OnRoundBegins;
            UFE.OnBasicMove += OnBasicMove;
            UFE.OnMove += OnMove;
        }
    }

    private void OnDisable()
    {
        if (!TutorialDone)
        {
            UFE.OnHit -= OnHit;
            UFE.OnRoundBegins -= OnRoundBegins;
            UFE.OnBasicMove -= OnBasicMove;
            UFE.OnMove -= OnMove;
        }

    }

    private void OnMove(MoveInfo move, ControlsScript player)
    {
        if (!TutorialDone)
        {
            switch (_currentStage)
            {
                case TutorialStage.SevenTutorial:
                    if (move.name == "RollMoveLeft")
                    {
                        LessonCounter();

                    }
                    TutorialLesson(RunTutorial, TutorialStage.TwelfthTutorial, CombatStances.Stance9);
                    break;
                case TutorialStage.TwelfthTutorial:
                    if (move.name == "Run")
                    {
                        LessonCounter();

                    }
                    TutorialLesson(JumpTutorial, TutorialStage.EighthTutorial, CombatStances.Stance9);
                    break;

            }
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
                    {
                        LessonCounter();

                    }

                    TutorialLesson(HgAttckTutorial, TutorialStage.ThirdTutorial, CombatStances.Stance2);

                    break;
                case TutorialStage.FourthTutorial:
                    if (basicMove == BasicMoveReference.BlockingCrouchingPose)
                    {
                        LessonCounter();

                    }

                    TutorialLesson(LowAttackTutorial, TutorialStage.FifthTutorial, CombatStances.Stance3);
                    break;
                case TutorialStage.EighthTutorial:
                    if(basicMove == BasicMoveReference.JumpStraight)
                    {
                        LessonCounter();
                    }
                    TutorialLesson(JumpAttackTutorial, TutorialStage.NineTutorial, CombatStances.Stance6);
                    break;

            }
        }
    }

    private void TutorialLesson(GameObject nextTutorialUI, TutorialStage nextStage, CombatStances playerStance)
    {
        if (_hitCount >= 3)
        {
            if(_currentStage != TutorialStage.TwelfthTutorial)
            {
                _player1.worldTransform.position = new FPVector(-5 , 0 , 0);
                _player2.worldTransform.position = new FPVector(5 , 0 , 0);
            }
            _hitCount = 0;
            _player1.MoveSet.ChangeMoveStances(playerStance);
            HitCounter.text = _hitCount.ToString() + "/3";
            nextTutorialUI.SetActive(true);
            //Mover.enabled = false;
            _currentStage = nextStage;
        }
    }

    private void LessonCounter()
    {
        _hitCount++;
        HitCounter.text = _hitCount.ToString() + "/3";
    }

    private void OnRoundBegins(int newInt)
    {
        if(!TutorialDone)
        {
            _player1 = UFE.GetControlsScript(1);
            _player2 = UFE.GetControlsScript(2);
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
                        LessonCounter();
                    }

                    TutorialLesson(HgBlockTutorial, TutorialStage.SecondTutorial, CombatStances.Stance1);

                    break;
                case TutorialStage.ThirdTutorial:
                    if (move.name == "HgAttakMove")
                    {
                        LessonCounter();

                    }

                    TutorialLesson(LowBlockTutorial, TutorialStage.FourthTutorial, CombatStances.Stance2);

                    break;
                case TutorialStage.FifthTutorial:
                    if(move.name == "Attack_Low_Move")
                    {
                        LessonCounter();
                    }
                    TutorialLesson(RollForwardTutorial, TutorialStage.SixthTutorial, CombatStances.Stance4);
                    break;
                case TutorialStage.SixthTutorial:
                    if(move.name == "RollMove")
                    {
                        LessonCounter();
                    }
                    TutorialLesson(RollBackTutorial, TutorialStage.SevenTutorial, CombatStances.Stance5);
                    break;
                case TutorialStage.NineTutorial:
                    if(move.name == "jump_kick_Move")
                    {
                        LessonCounter();
                    }
                    TutorialLesson(SuperHgTutorial, TutorialStage.TenthTutorial, CombatStances.Stance7);
                    break;
                case TutorialStage.TenthTutorial:
                    if (move.name == "SuperChop_Move")
                    {
                        LessonCounter();
                    }
                    TutorialLesson(SuperLowTutorial, TutorialStage.EleventhTutorial, CombatStances.Stance8);
                    break;
                case TutorialStage.EleventhTutorial:
                    if (move.name == "SuperStrikeMove")
                    {
                        LessonCounter();
                    }
                    if (_hitCount >= 3)
                    {
                        TutorialDone = true;
                        HitCounter.gameObject.SetActive(false);
                        FinalDialoge.SetActive(true);
                        Mover.enabled = false;
                        
                    }
                    break;

            }
        }
    }

}
