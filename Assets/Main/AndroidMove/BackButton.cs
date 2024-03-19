using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class BackButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private CustomInterface _interface;

    private string _boolName;

    private void Start()
    {
        _boolName = gameObject.tag;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        switch (_boolName)
        {
            case "Back":
                _interface.BackButtonPressed = true;
                break;
            case "Button2":
                _interface.Button2Pressed = true;
                break;
            case "Button1":
                _interface.Button1Pressed = true;
                break;
            case "Forward":
                _interface.ForwardButtonPressed = true;
                break;
            case "Jump":
                _interface.JumpButtonPressed = true;
                break;
            case "Crouch":
                _interface.CrouchButtonPressed = true;
                break;

        }

        
    }

    // Метод, вызываемый при отпускании UI кнопки
    public void OnPointerUp(PointerEventData eventData)
    {
        switch (_boolName)
        {
            case "Back":
                _interface.BackButtonPressed = false;
                break;
            case "Button2":
                _interface.Button2Pressed = false;
                break;
            case "Button1":
                _interface.Button1Pressed = false;
                break;
            case "Forward":
                _interface.ForwardButtonPressed = false;
                break;
            case "Jump":
                _interface.JumpButtonPressed = false;
                break;
            case "Crouch":
                _interface.CrouchButtonPressed = false;
                break;

        }
    }
}
