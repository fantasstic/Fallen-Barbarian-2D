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

    private bool _badEndAnimationStarted = false;
    public int CurrentBadEndSceneIndex = 0;

    private const string LastPlayedAnimationKey = "LastPlayedAnimationIndex";

    private void Start()
    {
        Invoke("EnableHair", 2.1f);
        /*Invoke("LoadBattleScene", _sceneDeley);*/
        Invoke("StartBadEndAnimation", _badEndSceneDeley);
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _positionToMove.position, _speed * Time.deltaTime);

        if(Input.anyKey && !_badEndAnimationStarted)
            StartBadEndAnimation();

        if(Input.GetKey(KeyCode.Escape) || Input.GetKey(KeyCode.Space))
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

        int lastPlayedIndex = PlayerPrefs.GetInt(LastPlayedAnimationKey, -1);

        do
        {
            CurrentBadEndSceneIndex = Random.Range(0, _badEndScenes.Count);
        } while (CurrentBadEndSceneIndex == lastPlayedIndex);

        PlayerPrefs.SetInt(LastPlayedAnimationKey, CurrentBadEndSceneIndex);

        GameObject selectedBadEndScene = _badEndScenes[CurrentBadEndSceneIndex];
        _badEndScenes.RemoveAt(CurrentBadEndSceneIndex);
        if(CurrentBadEndSceneIndex == 1)
            _panel.SetActive(false);


        selectedBadEndScene.SetActive(true);
        
    }
}
