using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UFE3D;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    private bool _isTutorialActive;
    private bool _isGameStart, _isGameRunning;
    private int _wins;

    public UFE3D.CharacterInfo Player1;
    public UFE3D.CharacterInfo Player2;
    public GameObject MoveTutorial;

    private void Start()
    {
        if (!PlayerPrefs.HasKey("FirstGame"))
            PlayerPrefs.SetString("FirstGame", "Yes");
        Debug.Log(_isTutorialActive);

        _wins = PlayerPrefs.GetInt("Wins");

        switch (_wins)
        {
            case 0:
                Player2.characterName = "BLOOD BLONDE";
                break;
            case 1:
                Player2.characterName = "GREENCH";
                break;
            case 2:
                Player2.characterName = "BLUE ICE";
                break;
            case 3:
                Player2.characterName = "PURPLE KILLER";
                break;
            case 4:
                Player2.characterName = "PINKY";
                break;
            case 5:
                Player2.characterName = "MEGA BLONDE";
                break;
            case 6:
                Player2.characterName = "RED HEAD";
                break;


        }

        //Player2.characterName = "BLOOD BLONDE";
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
