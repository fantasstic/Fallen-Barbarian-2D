using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UFE3D;
using UnityEngine.EventSystems;

public class AndroidMove : MonoBehaviour
{
    [Header("Direction Thresholds")]
    [SerializeField] private float forwardThreshold = 0.5f;
    [SerializeField] private float backwardThreshold = -0.5f;
    [SerializeField] private float upThreshold = 0.5f;
    [SerializeField] private float downThreshold = -0.5f;
    [SerializeField] private float bottomRightThreshold = 0.5f;
    [SerializeField] private float bottomLeftThreshold = -0.5f;
    [SerializeField] private CustomInterface _interface;

    public bool isMovingForward;
    public bool isMovingBackward;
    public bool isMovingUp;
    public bool isMovingDown;
    public bool isMovingBottomRight;
    public bool isMovingBottomLeft;

    private void Update()
    {
        float vertical = Input.GetAxis("P1JoystickVertical");
        float horizontal = Input.GetAxis("P1JoystickHorizontal");
        //Debug.Log(CheckGamepadConnection());
        if(!CheckGamepadConnection())
        {
            if (Input.GetKey(KeyCode.W))
                vertical = 1;
            else if(Input.GetKey(KeyCode.S))
                vertical = -1;
            else
                vertical = 0;

            if (Input.GetKey(KeyCode.D))
                horizontal = 1;
            else if(Input.GetKey(KeyCode.A))
                horizontal = -1;
            else
                horizontal = 0;
        }
       // else
        //{
            if (vertical > upThreshold)
                isMovingUp = true;
            else
                isMovingUp = false;
            if (horizontal > forwardThreshold && vertical > downThreshold && vertical < upThreshold)
                isMovingForward = true;
            else
                isMovingForward = false;
            if (horizontal < backwardThreshold && vertical > downThreshold && vertical < upThreshold)
                isMovingBackward = true;
            else
                isMovingBackward = false;
            if (vertical < downThreshold && horizontal < bottomRightThreshold && horizontal > bottomLeftThreshold)
                isMovingDown = true;
            else
                isMovingDown = false;
            if (horizontal > bottomRightThreshold && vertical < downThreshold)
                isMovingBottomRight = true;
            else
                isMovingBottomRight = false;
            if (horizontal < bottomLeftThreshold && vertical < downThreshold)
                isMovingBottomLeft = true;
            else
                isMovingBottomLeft = false;
            if (Input.GetButtonDown("P1Button2"))
                _interface.Button2Pressed = true;
            else
                _interface.Button2Pressed = false;
            if (Input.GetButtonDown("P1Button3"))
                _interface.JumpButtonPressed = true;
            else
                _interface.JumpButtonPressed = false;

            if (isMovingBackward)
                _interface.BackButtonPressed = true;
            else
                _interface.BackButtonPressed = false;
            if (isMovingUp)
                _interface.Button1Pressed = true;
            else
                _interface.Button1Pressed = false;
            if (isMovingForward)
                _interface.ForwardButtonPressed = true;
            else
                _interface.ForwardButtonPressed = false;
            if (isMovingDown)
                _interface.CrouchButtonPressed = true;
            else
                _interface.CrouchButtonPressed = false;
            if (isMovingBottomRight)
                _interface.RollForwardButtonPressed = true;
            else
                _interface.RollForwardButtonPressed = false;
            if (isMovingBottomLeft)
                _interface.RollBackButtonPressed = true;
            else
                _interface.RollBackButtonPressed = false;

        //}


    }

    private bool _gamepadConnected;

    private bool CheckGamepadConnection()
    {
        string[] joystickNames = Input.GetJoystickNames();
        foreach (string joystickName in joystickNames)
        {
            if (!string.IsNullOrEmpty(joystickName))
            {
                _gamepadConnected = true;
                Debug.Log("Геймпад подключен.");
                //return;
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
