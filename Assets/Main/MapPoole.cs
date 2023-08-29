using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPoole : MonoBehaviour
{
    [SerializeField] private GameObject[] _mapPoole;
    private string _lastActivatedIndexKey = "LastActivatedIndex";

    private void Start()
    {
        // Проверяем, есть ли объекты для активации
        if (_mapPoole.Length > 0)
        {
            int lastActivatedIndex = PlayerPrefs.GetInt(_lastActivatedIndexKey, -1);

            // Генерируем случайный индекс, исключая последний активированный
            int randomIndex = Random.Range(0, _mapPoole.Length);
            while (randomIndex == lastActivatedIndex)
            {
                randomIndex = Random.Range(0, _mapPoole.Length);
            }

            // Активируем случайно выбранный объект
            _mapPoole[randomIndex].SetActive(true);

            // Сохраняем индекс последнего активированного объекта
            PlayerPrefs.SetInt(_lastActivatedIndexKey, randomIndex);
        }
        else
        {
            Debug.LogWarning("No objects to activate!");
        }
    }
}
