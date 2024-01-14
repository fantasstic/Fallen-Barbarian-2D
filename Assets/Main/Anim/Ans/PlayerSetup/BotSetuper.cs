using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotSetuper : BasePlayerSetiuper
{
    public List<Color> HairColor;
    private Color currentHairColor;

    protected override void ChangeHairColor()
    {
        /*var enemy = UFE.GetControlsScript(2);*/

        int wins = PlayerPrefs.GetInt("Wins");
        Debug.Log(wins);
        //int randomIndex = Random.Range(0, HairColor.Count);
        currentHairColor = HairColor[wins];
        _hairSpriteRenderer.color = currentHairColor;
        PlayerPrefs.SetFloat("HairColorR", currentHairColor.r);
        PlayerPrefs.SetFloat("HairColorG", currentHairColor.g);
        PlayerPrefs.SetFloat("HairColorB", currentHairColor.b);
        PlayerPrefs.SetFloat("HairColorA", currentHairColor.a);
    }
}
