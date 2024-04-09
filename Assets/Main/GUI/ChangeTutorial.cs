using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeTutorial : MonoBehaviour
{
    public Image Image;
    public Sprite[] Sprites;
    private bool _gamepadConnected;

    public void ChageImage()
    {
        if(CheckGamepadConnection())
        {
            Image.sprite = Sprites[1];
        }
        else
        {
            Image.sprite = Sprites[0];
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
