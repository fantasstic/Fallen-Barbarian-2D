using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UFE3D;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    [SerializeField] private UFE3D.GlobalInfo _globalInfo;

    private bool _isTutorialActive;
    private bool _isGameStart, _isGameRunning, _firstStart = true;
    private int _wins;
    private bool _gamepadConnected = false;

    public UFE3D.CharacterInfo Player1;
    public UFE3D.CharacterInfo Player2;
    public UFE3D.CharacterInfo Player3;
    public GameObject MoveTutorial, GamePadTutorial;
    public bool _isTuttorialMode;

    private void Start()
    {
        if (!PlayerPrefs.HasKey("FirstGame"))
            PlayerPrefs.SetString("FirstGame", "Yes");
        Debug.Log(_isTutorialActive);

        _wins = PlayerPrefs.GetInt("Wins");
        //_wins = 3;

        if(!_isTuttorialMode)
        {

            switch (_wins)
            {
                case 0:
                    Player2.characterName = "BLOOD BLONDE";
                    _globalInfo.aiOptions.selectedDifficultyLevel = AIDifficultyLevel.VeryEasy;
                    break;
                case 1:
                    Player2.characterName = "GREENCH";
                    _globalInfo.aiOptions.selectedDifficultyLevel = AIDifficultyLevel.VeryEasy;
                    break;
                case 2:
                    Player2.characterName = "BLUE ICE";
                    _globalInfo.aiOptions.selectedDifficultyLevel = AIDifficultyLevel.Easy;
                    break;
                case 3:
                    Player2.characterName = "PURPLE KILLER";
                    _globalInfo.aiOptions.selectedDifficultyLevel = AIDifficultyLevel.VeryHard;
                    break;
                case 4:
                    Player2.characterName = "PINKY";
                    _globalInfo.aiOptions.selectedDifficultyLevel = AIDifficultyLevel.VeryHard;
                    break;
                case 5:
                    Player2.characterName = "ORANGIE";
                    _globalInfo.aiOptions.selectedDifficultyLevel = AIDifficultyLevel.VeryHard;
                    break;
                case 6:
                    Player2.characterName = "RED HEAD";
                    _globalInfo.aiOptions.selectedDifficultyLevel = AIDifficultyLevel.VeryHard;
                    break;


            }
        }
        else
        {
            Player2.characterName = "Tutorial";
            _globalInfo.aiOptions.selectedDifficultyLevel = AIDifficultyLevel.Impossible;
        }

        //Player2.characterName = "BLOOD BLONDE";
    }

    public void TutorialButton()
    {
        UFE.StartTrainingMode();
    }

    public void StartGameButton()
    {
        /*RuntimePlatform platform = Application.platform;

        if (platform == RuntimePlatform.Android || CheckGamepadConnection())
            UFE.config.inputOptions.inputManagerType = InputManagerType.CustomClass;
        else
            UFE.config.inputOptions.inputManagerType = InputManagerType.UnityInputManager;*/

        if (!_isGameRunning)
        {

            if (!PlayerPrefs.HasKey("FirstGame") || PlayerPrefs.GetString("FirstGame") == "Yes")
            {
                bool mover = CheckGamepadConnection();
                if (!mover)
                    MoveTutorial.SetActive(true);
                else
                    GamePadTutorial.SetActive(true);

                _isTutorialActive = true;
            }
            else
            {
                _isGameRunning = true;
                Debug.Log(UFE.gameMode);
                //UFE.gameMode = GameMode.ChallengeMode;
                UFE.StartTrainingMode();
                //UFE.StartGame();
                if (_firstStart)
                {

                    UFE.SetPlayer1(Player1);

                    UFE.SetPlayer2(Player2);
                    UFE.SetStage("Main");
                    _firstStart = false;
                }
                //Debug.Log(UFE.GetPlayer1());
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
                GamePadTutorial.SetActive(false);
                PlayerPrefs.SetString("FirstGame", "No");
                _isGameRunning = true;
                UFE.StartGame();
                UFE.SetPlayer1(Player1);
                UFE.SetPlayer2(Player3);
                //UFE.SetPlayer2(Player2);
                UFE.SetStage("Main");
                //var player = UFE.GetPlayer1();
            }
        }

        if (!_isGameStart && !_isTutorialActive && Input.GetKeyDown(KeyCode.Space) || !_isGameStart && !_isTutorialActive && Input.GetButtonDown("P1Start") 
            || !_isGameStart && !_isTutorialActive && Input.GetButtonDown("Submit") && UFE.currentScreen.name == "VersusModeAfterBattleScreen(Clone)"
            || !_isGameStart && !_isTutorialActive && Input.GetButtonDown("Cancel") && UFE.currentScreen.name == "VersusModeAfterBattleScreen(Clone)")
        {
            _isGameStart = true;
            Debug.Log("StartGameButton called");
            StartGameButton();
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
