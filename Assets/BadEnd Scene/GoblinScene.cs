using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UFE3D;

public class GoblinScene : MonoBehaviour
{
    [SerializeField] private Transform _positionToMove;
    [SerializeField] private float _speed;
    [SerializeField] private float _sceneDeley;
    [SerializeField] private List<GameObject> _badEndScenes = new List<GameObject>();
    [SerializeField] private float _badEndSceneDeley;
    [SerializeField] private GameObject _hair;
    [SerializeField] private GameObject _panel;

    private List<GameObject> _badEndScenesCopy; // Копия исходного списка
    private bool _shuffleMode = false;
    private bool _badEndAnimationStarted = false;
    private int _lastTwoIndexes = -1;
    public int CurrentBadEndSceneIndex = 0;

    private const string LastPlayedAnimationKey = "LastPlayedAnimationIndex";

    private void Start()
    {
        Invoke("EnableHair", 2.1f);
        Invoke("StartBadEndAnimation", _badEndSceneDeley);
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _positionToMove.position, _speed * Time.deltaTime);

        if (Input.anyKey && !_badEndAnimationStarted)
            StartBadEndAnimation();

        if (Input.GetKey(KeyCode.Escape) || Input.GetKey(KeyCode.Space))
        {
            LoadBattleScene();
        }
    }

    private void EnableHair()
    {
        _hair.SetActive(true);
    }

    private void LoadBattleScene()
    {
        SceneManager.LoadScene(0);
    }

    private void StartBadEndAnimation()
    {
        if (!_shuffleMode)
        {
            if (_badEndScenesCopy == null || _badEndScenesCopy.Count == 0)
            {
                // Создаем копию исходного списка для перемешивания только при первом запуске
                _badEndScenesCopy = new List<GameObject>(_badEndScenes);
                ShuffleScenes();
            }

            if (_badEndAnimationStarted)
            {
                return;
            }

            _badEndAnimationStarted = true;

            int lastPlayedIndex = PlayerPrefs.GetInt(LastPlayedAnimationKey, -1);

            do
            {
                CurrentBadEndSceneIndex = Random.Range(0, _badEndScenesCopy.Count);
            } while (CurrentBadEndSceneIndex == lastPlayedIndex || CurrentBadEndSceneIndex == _lastTwoIndexes);

            PlayerPrefs.SetInt(LastPlayedAnimationKey, CurrentBadEndSceneIndex);

            GameObject selectedBadEndScene = _badEndScenesCopy[CurrentBadEndSceneIndex];
            _badEndScenesCopy.RemoveAt(CurrentBadEndSceneIndex);
            if (CurrentBadEndSceneIndex == 1)
                _panel.SetActive(false);

            selectedBadEndScene.SetActive(true);

            _lastTwoIndexes = lastPlayedIndex;
            if (_badEndScenesCopy.Count == 0)
            {
                // Все объекты были показаны без повторений, переключаемся в режим перемешивания
                _shuffleMode = true;
            }
        }
        else
        {
            if (_badEndScenesCopy.Count == 0)
            {
                // Все объекты были показаны в режиме перемешивания
                Debug.LogWarning("All bad end scenes have been shown.");
                return;
            }

            if (_badEndAnimationStarted)
            {
                return;
            }

            _badEndAnimationStarted = true;

            int randomIndex = Random.Range(0, _badEndScenesCopy.Count);
            GameObject selectedBadEndScene = _badEndScenesCopy[randomIndex];
            _badEndScenesCopy.RemoveAt(randomIndex);

            if (randomIndex == 1)
                _panel.SetActive(false);

            selectedBadEndScene.SetActive(true);
        }
    }

    private void ShuffleScenes()
    {
        // Перемешиваем сцены
        for (int i = 0; i < _badEndScenesCopy.Count; i++)
        {
            int randomIndex = Random.Range(i, _badEndScenesCopy.Count);
            GameObject temp = _badEndScenesCopy[i];
            _badEndScenesCopy[i] = _badEndScenesCopy[randomIndex];
            _badEndScenesCopy[randomIndex] = temp;
        }
    }
}
