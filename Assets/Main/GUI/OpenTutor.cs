using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenTutor : MonoBehaviour
{
    private bool _isActive = false;

    public GameObject Tutorial;

    private void Update()
    {

        if(Input.GetKey(KeyCode.Space))
        {
            DefaultVersusModeAfterBattleScreen afterBattleScreen = GetComponent<DefaultVersusModeAfterBattleScreen>();
            afterBattleScreen.RepeatBattle();
        }
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
}
