using System;
using System.Collections.Generic;
using UnityEngine;
using FPLibrary;

public class GUIControlsInterface : AbstractInputController
{
    private Fix64 hAxis = 0;
    private Fix64 vAxis = 0;
    private bool b1;
    private bool b2;

    // Initialize Abstract Input Controller
    public override void Initialize(IEnumerable<InputReferences> inputs)
    {
        base.Initialize(inputs);
    }

    // Displays the GUI elements that triggers the inputs
    private void OnGUI()
    {
        bool upEvent = GUI.RepeatButton(new Rect(140, 320, 40, 40), "U"); // Up Axis
        bool downEvent = GUI.RepeatButton(new Rect(140, 400, 40, 40), "D"); // Down Axis
        bool leftEvent = GUI.RepeatButton(new Rect(80, 360, 40, 40), "L"); // Left Axis
        bool rightEvent = GUI.RepeatButton(new Rect(200, 360, 40, 40), "R"); // Right Axis
        bool button1Event = GUI.Button(new Rect(640, 360, 40, 40), "B1"); // Button 1
        bool button2Event = GUI.Button(new Rect(700, 360, 40, 40), "B2"); // Button 2

        if (upEvent) {
            vAxis = 1;
        } else if (downEvent) {
            vAxis = -1;
        } else {
            vAxis = 0;
        }

        if (rightEvent) {
            hAxis = 1;
        } else if (leftEvent) {
            hAxis = -1;
        } else {
            hAxis = 0;
        }

        if (button1Event) {
            b1 = true;
        } else {
            b1 = false;
        }

        if (button2Event) {
            b2 = true;
        } else {
            b2 = false;
        }
    }

    // Override ReadInput so it can send the information back to UFE
    public override InputEvents ReadInput(InputReferences inputReference)
    {
        if (inputReference != null)
        {
            if (inputReference.inputType == InputType.HorizontalAxis) { // Sends hAxis value as a Horizontal Axis Input Event
                return new InputEvents(hAxis);
            } else if (inputReference.inputType == InputType.VerticalAxis) { // Sends vAxis value as a Vertical Axis Input Event
                return new InputEvents(vAxis);
            } else if (inputReference.inputType == InputType.Button && inputReference.engineRelatedButton == ButtonPress.Button1) 
            { // Sends Button 1 Input Event
                Debug.Log("allasd");
                return new InputEvents(b1);
            } else if (inputReference.inputType == InputType.Button && inputReference.engineRelatedButton == ButtonPress.Button2) { // Sends Button 2 Input Event
                return new InputEvents(b2);
            }
        }
        return InputEvents.Default;
    }
}
