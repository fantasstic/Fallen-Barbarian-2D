using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotSetuper : BasePlayerSetiuper
{
    public List<Color> HairColor;
    private Color currentHairColor;

    protected override void ChangeHairColor()
    {
        int randomIndex = Random.Range(0, HairColor.Count);
        currentHairColor = HairColor[randomIndex];
        _hairSpriteRenderer.color = currentHairColor;
        PlayerPrefs.SetFloat("HairColorR", currentHairColor.r);
        PlayerPrefs.SetFloat("HairColorG", currentHairColor.g);
        PlayerPrefs.SetFloat("HairColorB", currentHairColor.b);
        PlayerPrefs.SetFloat("HairColorA", currentHairColor.a);
    }
}
