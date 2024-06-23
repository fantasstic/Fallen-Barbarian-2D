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
    [SerializeField] private GameObject _hair, _blur1, _blur2;
    [SerializeField] private bool _shuffleMode = false;

    private List<int> _alredyUsedRandomNumbersHistory = new List<int>();
    private int _lastActiveScene = 0;
    private bool _badEndAnimationStarted = false;

    public int CurrentBadEndSceneIndex = 0;
    public bool IsTestMod;

    private void Start()
    {
        if(PlayerPrefs.GetString("PSFW") == "Yes")
            LoadBattleScene();

        if (!PlayerPrefs.HasKey("ShuffleMode"))
            PlayerPrefs.SetString("ShuffleMode", "No");

        if (PlayerPrefs.GetString("ShuffleMode") != "No")
            _shuffleMode = true;
        else
            _shuffleMode = false;

        if (PlayerPrefs.GetString("SFW") == "Yes")
        {
            _blur1.SetActive(true);
            _blur2.SetActive(false);
        }
        else
        {
            _blur1.SetActive(false);
            _blur2.SetActive(false);
        }

        Invoke("EnableHair", 2.1f);
        /*Invoke("LoadBattleScene", _sceneDeley);*/

        if (PlayerPrefs.GetString("SFW") == "Yes")
            Invoke("LoadBattleScene", _badEndSceneDeley);
        else
            Invoke("StartBadEndAnimation", _badEndSceneDeley);
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _positionToMove.position, _speed * Time.deltaTime);

        if (Input.anyKey && !_badEndAnimationStarted)
        {
            if(PlayerPrefs.GetString("SFW") == "Yes")
                LoadBattleScene();
            else
                StartBadEndAnimation();
        }    

        if (Input.GetKey(KeyCode.Escape) || Input.GetKey(KeyCode.Space) || Input.GetButtonDown("Menu"))
        {
            LoadBattleScene();
        }
    }

    private void EnableHair()
    {
        if(PlayerPrefs.GetString("SFW") == "Yes")
        {
            _blur1.SetActive(false);
            _blur2.SetActive(true);
        }
        _hair.SetActive(true);
    }

    public void LoadBattleScene()
    {
        SceneManager.LoadScene(0);
    }

    private void StartBadEndAnimation()
    {
        if(IsTestMod)
        {
            int scenesCount = _badEndScenes.Count;
            _badEndScenes[scenesCount - 1].SetActive(true);
            return;
        }

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

        // проверка (Х) 
        if (_badEndScenes.Count == 1)
        {
            randomIndex = 0;
        }
        else
        {
            // смотрим пустая ли история или нет
            if (_alredyUsedRandomNumbersHistory.Count == 0)
            {
                // если пустая то создаем список из всех возможных индексов
                var allIndexes = Enumerable.Range(0, _badEndScenes.Count);
                // затем этот список рандомим и сохраняем в истроию
                _alredyUsedRandomNumbersHistory = allIndexes.OrderBy(x => random.Next()).ToList();

                // далее из этой истории удаляем последний индекс (чтобы он не повторился сразу)
                if (_alredyUsedRandomNumbersHistory.Contains(CurrentBadEndSceneIndex))
                    _alredyUsedRandomNumbersHistory.Remove(CurrentBadEndSceneIndex);
            }

            // ну и потом просто из нашей истории берем первый индекс и сразу же удаляем его из истории и так пока список не заколнчится (а потом опять рандомим его и по кругу)
            randomIndex = _alredyUsedRandomNumbersHistory[0];
            _alredyUsedRandomNumbersHistory.RemoveAt(0);
        }


        // ну и дальше логика без изменений
        GameObject selectedBadEndScene = _badEndScenes[randomIndex];
        //_badEndScenes.RemoveAt(randomIndex);
        CurrentBadEndSceneIndex = randomIndex;

        selectedBadEndScene.SetActive(true);
    }
}
