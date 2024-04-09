using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UFE3D;
using System.Linq;

public class GoblinScene : MonoBehaviour
{
    [SerializeField] private Transform _positionToMove;
    [SerializeField] private float _speed;
    [SerializeField] private float _sceneDeley;
    [SerializeField] private List<GameObject> _badEndScenes = new List<GameObject>();
    [SerializeField] private float _badEndSceneDeley;
    [SerializeField] private GameObject _hair;
    [SerializeField] private bool _shuffleMode = false;

    private List<int> _alredyUsedRandomNumbersHistory = new List<int>();
    private int _lastActiveScene = 0;
    private bool _badEndAnimationStarted = false;

    public int CurrentBadEndSceneIndex = 0;

    private void Start()
    {
        if (!PlayerPrefs.HasKey("ShuffleMode"))
            PlayerPrefs.SetString("ShuffleMode", "No");

        if (PlayerPrefs.GetString("ShuffleMode") != "No")
            _shuffleMode = true;
        else
            _shuffleMode = false;

        Invoke("EnableHair", 2.1f);
        /*Invoke("LoadBattleScene", _sceneDeley);*/
        Invoke("StartBadEndAnimation", _badEndSceneDeley);
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _positionToMove.position, _speed * Time.deltaTime);

        if (Input.anyKey && !_badEndAnimationStarted)
            StartBadEndAnimation();

        if (Input.GetKey(KeyCode.Escape) || Input.GetKey(KeyCode.Space) || Input.GetButtonDown("Menu"))
        {
            LoadBattleScene();
        }
    }

    private void EnableHair()
    {
        _hair.SetActive(true);
    }

    public void LoadBattleScene()
    {
        SceneManager.LoadScene(0);
    }

    private void StartBadEndAnimation()
    {
        //PlayerPrefs.SetString("ShuffleMode", "Yes");

        if (_badEndScenes.Count == 0)
        {
            Debug.LogWarning("No more bad end scenes available.");
            return;
        }

        if (_badEndAnimationStarted)
        {
            return;
        }

        _badEndAnimationStarted = true;

        if (_shuffleMode)
        {
            /*int randomIndex = Random.Range(0, _badEndScenes.Count);
            GameObject selectedBadEndScene = _badEndScenes[randomIndex];
            //_badEndScenes.RemoveAt(randomIndex);
            CurrentBadEndSceneIndex = randomIndex;

            selectedBadEndScene.SetActive(true);*/
            NewShuffleMode();
        }
        else
        {
            _lastActiveScene = PlayerPrefs.GetInt("LastPlayedAnimationKey");

            if(_lastActiveScene < _badEndScenes.Count)
            {
                _lastActiveScene++;
                
                GameObject selectedBadEndScene = _badEndScenes[_lastActiveScene - 1];
                //_badEndScenes.RemoveAt(randomIndex);
                CurrentBadEndSceneIndex = _lastActiveScene - 1;
                PlayerPrefs.SetInt("LastPlayedAnimationKey", _lastActiveScene);

                selectedBadEndScene.SetActive(true);
                
            }
            else
            {
                _shuffleMode = true;
                ShuffleMode();
            }
        }
    }

    private void ShuffleMode()
    {
        int randomIndex = Random.Range(0, _badEndScenes.Count);
        GameObject selectedBadEndScene = _badEndScenes[randomIndex];
        //_badEndScenes.RemoveAt(randomIndex);
        CurrentBadEndSceneIndex = randomIndex;

        selectedBadEndScene.SetActive(true);
    }

    private System.Random random = new System.Random();
    private void NewShuffleMode()
    {
        int randomIndex;

        // �������� (�) 
        if (_badEndScenes.Count == 1)
        {
            randomIndex = 0;
        }
        else
        {
            // ������� ������ �� ������� ��� ���
            if (_alredyUsedRandomNumbersHistory.Count == 0)
            {
                // ���� ������ �� ������� ������ �� ���� ��������� ��������
                var allIndexes = Enumerable.Range(0, _badEndScenes.Count);
                // ����� ���� ������ �������� � ��������� � �������
                _alredyUsedRandomNumbersHistory = allIndexes.OrderBy(x => random.Next()).ToList();

                // ����� �� ���� ������� ������� ��������� ������ (����� �� �� ���������� �����)
                if (_alredyUsedRandomNumbersHistory.Contains(CurrentBadEndSceneIndex))
                    _alredyUsedRandomNumbersHistory.Remove(CurrentBadEndSceneIndex);
            }

            // �� � ����� ������ �� ����� ������� ����� ������ ������ � ����� �� ������� ��� �� ������� � ��� ���� ������ �� ����������� (� ����� ����� �������� ��� � �� �����)
            randomIndex = _alredyUsedRandomNumbersHistory[0];
            _alredyUsedRandomNumbersHistory.RemoveAt(0);
        }


        // �� � ������ ������ ��� ���������
        GameObject selectedBadEndScene = _badEndScenes[randomIndex];
        //_badEndScenes.RemoveAt(randomIndex);
        CurrentBadEndSceneIndex = randomIndex;

        selectedBadEndScene.SetActive(true);
    }

    /*[SerializeField] private Transform _positionToMove;
    [SerializeField] private float _speed;
    [SerializeField] private float _sceneDeley;
    [SerializeField] private List<GameObject> _badEndScenes = new List<GameObject>();
    [SerializeField] private float _badEndSceneDeley;
    [SerializeField] private GameObject _hair;
    [SerializeField] private GameObject _panel;

    private List<GameObject> _badEndScenesCopy; // ����� ��������� ������
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
                // ������� ����� ��������� ������ ��� ������������� ������ ��� ������ �������
                _badEndScenesCopy = new List<GameObject>(_badEndScenes);
                //ShuffleScenes();
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
                // ��� ������� ���� �������� ��� ����������, ������������� � ����� �������������
                _shuffleMode = true;
            }
        }
        else
        {
            if (_badEndScenesCopy.Count == 0)
            {
                // ��� ������� ���� �������� � ������ �������������
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
        // ������������ �����
        for (int i = 0; i < _badEndScenesCopy.Count; i++)
        {
            int randomIndex = Random.Range(i, _badEndScenesCopy.Count);
            GameObject temp = _badEndScenesCopy[i];
            _badEndScenesCopy[i] = _badEndScenesCopy[randomIndex];
            _badEndScenesCopy[randomIndex] = temp;
        }
    }*/
}
