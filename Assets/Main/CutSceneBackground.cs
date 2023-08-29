using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneBackground : MonoBehaviour
{
    [SerializeField] private GameObject[] _mapPoole;

    private void Awake()
    {
        int lastActiveMap = PlayerPrefs.GetInt("LastActivatedIndex");

        _mapPoole[lastActiveMap].SetActive(true);
    }
}
