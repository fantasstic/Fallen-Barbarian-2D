using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UFE3D;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    private bool _isTutorialActive;

    public UFE3D.CharacterInfo Player1;
    public UFE3D.CharacterInfo Player2;
    public GameObject MoveTutorial;

    private void Start()
    {
        if (!PlayerPrefs.HasKey("FirstGame"))
            PlayerPrefs.SetString("FirstGame", "Yes");
    }

    public void StartGameButton()
    {
        if (!PlayerPrefs.HasKey("FirstGame") || PlayerPrefs.GetString("FirstGame") == "Yes")
        {
            MoveTutorial.SetActive(true);
            _isTutorialActive = true;
        }
        else
        {
            UFE.StartGame();
            UFE.SetPlayer1(Player1);
            UFE.SetPlayer2(Player2);
            UFE.SetStage("Main");
        }
    }

    private void Update()
    {
        if(_isTutorialActive)
        {
            /*PlayerPrefs.SetString("FirstGame", "No");*/

            if (PlayerPrefs.GetString("FirstGame") == "Yes" && Input.anyKey)
            {
                MoveTutorial.SetActive(false);
                PlayerPrefs.SetString("FirstGame", "No");
                UFE.StartGame();
                UFE.SetPlayer1(Player1);
                UFE.SetPlayer2(Player2);
                UFE.SetStage("Main");
            }
        }
    }
}
