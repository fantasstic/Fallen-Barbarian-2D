using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UFE3D;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    private bool _isTutorialActive;
    private bool _isGameStart, _isGameRunning;

    public UFE3D.CharacterInfo Player1;
    public UFE3D.CharacterInfo Player2;
    public GameObject MoveTutorial;

    private void Start()
    {
        if (!PlayerPrefs.HasKey("FirstGame"))
            PlayerPrefs.SetString("FirstGame", "Yes");
        Debug.Log(_isTutorialActive);
    }

    public void StartGameButton()
    {
        if(!_isGameRunning)
        {

            if (!PlayerPrefs.HasKey("FirstGame") || PlayerPrefs.GetString("FirstGame") == "Yes")
            {
                MoveTutorial.SetActive(true);
                _isTutorialActive = true;
            }
            else
            {
                _isGameRunning = true;
                UFE.StartGame();
                UFE.SetPlayer1(Player1);
                UFE.SetPlayer2(Player2);
                UFE.SetStage("Main");
            }
        }
    }

    /*private void StartGames()
    {
        if (!_isGameStart && Input.GetKeyUp(KeyCode.Space))
        {
            _isGameStart = true;
            Debug.Log("StartGameButton called");
            StartGameButton();
        }
    }*/

    private void Update()
    {
        if(_isTutorialActive)
        {
            /*PlayerPrefs.SetString("FirstGame", "No");*/

            if (PlayerPrefs.GetString("FirstGame") == "Yes" && Input.anyKey)
            {
                MoveTutorial.SetActive(false);
                PlayerPrefs.SetString("FirstGame", "No");
                _isGameRunning = true;
                UFE.StartGame();
                UFE.SetPlayer1(Player1);
                UFE.SetPlayer2(Player2);
                UFE.SetStage("Main");
            }
        }

        if (!_isGameStart && !_isTutorialActive && Input.GetKeyDown(KeyCode.Space))
        {
            _isGameStart = true;
            Debug.Log("StartGameButton called");
            StartGameButton();
        }
    }
}
