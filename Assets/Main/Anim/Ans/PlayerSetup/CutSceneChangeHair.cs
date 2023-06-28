using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneChangeHair : MonoBehaviour
{
    public SpriteRenderer Hair;

    private void Start()
    {
        float r = PlayerPrefs.GetFloat("HairColorR");
        float g = PlayerPrefs.GetFloat("HairColorG");
        float b = PlayerPrefs.GetFloat("HairColorB");
        float a = PlayerPrefs.GetFloat("HairColorA");

        Color savedColor = new Color(r, g, b, a);
        Hair.color = savedColor;
        
    }
}
