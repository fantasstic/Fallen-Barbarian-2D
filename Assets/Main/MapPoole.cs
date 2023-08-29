using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPoole : MonoBehaviour
{
    [SerializeField] private GameObject[] _mapPoole;
    private string _lastActivatedIndexKey = "LastActivatedIndex";

    private void Start()
    {
        // ���������, ���� �� ������� ��� ���������
        if (_mapPoole.Length > 0)
        {
            int lastActivatedIndex = PlayerPrefs.GetInt(_lastActivatedIndexKey, -1);

            // ���������� ��������� ������, �������� ��������� ��������������
            int randomIndex = Random.Range(0, _mapPoole.Length);
            while (randomIndex == lastActivatedIndex)
            {
                randomIndex = Random.Range(0, _mapPoole.Length);
            }

            // ���������� �������� ��������� ������
            _mapPoole[randomIndex].SetActive(true);

            // ��������� ������ ���������� ��������������� �������
            PlayerPrefs.SetInt(_lastActivatedIndexKey, randomIndex);
        }
        else
        {
            Debug.LogWarning("No objects to activate!");
        }
    }
}
