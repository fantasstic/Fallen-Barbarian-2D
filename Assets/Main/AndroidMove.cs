using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UFE3D;
using UnityEngine.EventSystems;

public class AndroidMove : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private MoveSetScript _moveSetScript;
    public GlobalInfo _config;
    public ControlsScript _player;
    private bool buttonPressed;
    private Animator _animator;
    public GUIControlsInterface _gui;

    private void Start()
    {
        UFE.OnRoundBegins += RoundStart;
    }

    private void RoundStart(int newInt)
    {
        _player = UFE.GetControlsScript(1);
        _moveSetScript = _player.GetComponentInChildren<MoveSetScript>();
        _animator = _player.GetComponentInChildren<Animator>();

    }


    public void Move()
    {
        //UFE.FireButton(ButtonPress.Back, _player);
    }

    void Update()
    {
        if (buttonPressed)
        {
            //UFE.FireButton(ButtonPress.Button1, _player);
            

            _moveSetScript.PlayBasicMove(_moveSetScript.basicMoves.blockingHighPose);
            _player.currentHitAnimation = "blockingHighHit";
            _player.isBlocking = true;

            /*_player.isBlocking = true;
            _player.currentSubState = SubStates.Blocking;*/
            //new WaitForSeconds(3);
            //_moveSetScript.PlayBasicMove(_moveSetScript.basicMoves.blockingHighPose);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        buttonPressed = true;
        //_gui.buttons = _gui.inputReferences
        Debug.Log("UI кнопка была нажата!");
    }

    // Метод, вызываемый при отпускании UI кнопки
    public void OnPointerUp(PointerEventData eventData)
    {
        buttonPressed = false;

        Debug.Log("UI кнопка была отпущена!");
    }
}
