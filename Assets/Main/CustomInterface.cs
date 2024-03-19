using System;
using System.Collections.Generic;
using UnityEngine;
using FPLibrary;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CustomInterface : AbstractInputController
{
    [SerializeField] private Button _pauseButton;

    private Fix64 hAxis = 0;
    private Fix64 vAxis = 0;
    private bool b1;
    private bool b2;
    private bool b10;

    public bool BackButtonPressed, Button2Pressed, Button1Pressed, ForwardButtonPressed, JumpButtonPressed, CrouchButtonPressed, RollForwardButtonPressed, RollBackButtonPressed;

    private void Start()
    {
        _pauseButton.onClick.AddListener(OnPauseButtonClick);
    }

    private void OnDestroy()
    {
        _pauseButton.onClick.RemoveListener(OnPauseButtonClick);
    }

    // Initialize Abstract Input Controller
    public override void Initialize(IEnumerable<InputReferences> inputs)
    {
        base.Initialize(inputs);
    }

    private void Update()
    {
        if (Button1Pressed)
            b1 = true;
        else
            b1 = false;

        if (BackButtonPressed)
            hAxis = -1;
        else if(ForwardButtonPressed)
            hAxis = 1;
        else
            hAxis = 0;

        if (JumpButtonPressed)
            vAxis = 1;
        else if(CrouchButtonPressed)
        {
            vAxis = -1;
            b1 = true;
        }
        else
            vAxis = 0;

        if (Button2Pressed)
            b2 = true;
        else
            b2 = false;

        if(RollForwardButtonPressed)
        {
            vAxis = -1;
            hAxis = 1;
        }

        if (RollBackButtonPressed)
        {
            vAxis = -1;
            hAxis = -1;
        }

    }

    public override InputEvents ReadInput(InputReferences inputReference)
    {
        if (inputReference != null)
        {
            if (inputReference.inputType == InputType.HorizontalAxis)
            {
                // Sends hAxis value as a Horizontal Axis Input Event
                //Debug.Log("allasd0");
                return new InputEvents(hAxis);
            }
            else if (inputReference.inputType == InputType.VerticalAxis)
            { // Sends vAxis value as a Vertical Axis Input Event
                //Debug.Log("allasd01");
                return new InputEvents(vAxis);
            }
            else if (inputReference.inputType == InputType.Button && inputReference.engineRelatedButton == ButtonPress.Button1)
            { // Sends Button 1 Input Event
                Debug.Log("allasd1: " + b1);
                return new InputEvents(b1);
            }
            else if (inputReference.inputType == InputType.Button && inputReference.engineRelatedButton == ButtonPress.Button2)
            { // Sends Button 2 Input Event
                //Debug.Log("allasd2");
                return new InputEvents(b2);
            }
            else if (inputReference.inputType == InputType.Button && inputReference.engineRelatedButton == ButtonPress.Button11)
            {
                return new InputEvents(b10);
            }
            //else if (inputReference.inputType == InputType.Button && inputReference.engineRelatedButton == ButtonPress.Button9)
            //{ // Sends Button 2 Input Event
            //    //Debug.Log("allasd2");
            //    return new InputEvents(b2);
            //}
        }
        return InputEvents.Default;
    }

    public void OnPauseButtonClick()
    {
        if (UFE.isPaused())
            UFE.PauseGame(false);
        else
            UFE.PauseGame(true);
    }
}
