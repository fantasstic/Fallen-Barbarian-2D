using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenTutor : MonoBehaviour
{
    [SerializeField] private List<Sprite> _backGrounds = new List<Sprite>();
    [SerializeField] private Image _back;

    private bool _isActive = false;
    private RoundContorllerNew _controller;

    public GameObject Tutorial;
    public Image Image;
    public Sprite[] Sprites;
    private bool _gamepadConnected;

    private void Awake()
    {
        /*_controller = FindObjectOfType<RoundContorllerNew>();

        _back.gameObject.SetActive(false);

        if (_controller.EnemyWinner)
        {
            _back.gameObject.SetActive(false);
        }
        else
        {
            int currentBack = Random.Range(0, _backGrounds.Count);
            _back.sprite = _backGrounds[currentBack];
            _back.gameObject.SetActive(true);
        }*/
    }

    public void CloseBack()
    {
        /*_controller = FindObjectOfType<RoundContorllerNew>();

        *//*_back.gameObject.SetActive(false);*//*

        if (_controller.EnemyWinner)
        {
            _back.gameObject.SetActive(false);
        }
        else
        {
            int currentBack = Random.Range(0, _backGrounds.Count);
            _back.sprite = _backGrounds[currentBack];
            _back.gameObject.SetActive(true);
        }*/
    }

    private void Update()
    {

        /*if(Input.GetKey(KeyCode.Space))
        {
            DefaultVersusModeAfterBattleScreen afterBattleScreen = GetComponent<DefaultVersusModeAfterBattleScreen>();
            afterBattleScreen.RepeatBattle();
        }*/
    }

    public void OpetTutorial()
    {
        if(!_isActive)
        {
            Tutorial.SetActive(true);
            _isActive = true;
        }
        else
        {
            Tutorial.SetActive(false);
            _isActive = false;
        }
    }

    public void ChageImage()
    {
        if (CheckGamepadConnection())
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
