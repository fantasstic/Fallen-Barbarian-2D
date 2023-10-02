using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using UnityEngine.UI;
using TMPro;

public class SpineAnim : MonoBehaviour
{
    [SerializeField] private GameObject _ui, _panel, _firstAnimPanel;
    [SerializeField] private TMP_Text _animationCountUI,_skinsCountUI, _animationSpeedUI, _zoomUI, _daysCounter;
    [SerializeField] private AudioSource _audioSource, _coughSource, _stepSource, _oralSquishSource, _autoModeStepSoruce, _swordSource, _birdSource, _pussySource;
    [SerializeField] private AudioClip[] _audioClips, _coughClips, _stepClips, _oralCleps, _autoModeStepClips, _swordClips, _birdClips, _pussyClips;
    [SerializeField] private AudioClip _oralMainClip;
    [SerializeField] private GoblinScene _goblicScene;
    [SerializeField] private Transform _zoomedTr;
    [SerializeField] private List<string> _skinNames = new List<string>();
    [SerializeField] private string _animName;
    [SerializeField] private float _yOffsetAfterZoom;

    private float _primaryProbability = 0.75f;
    private float _currentSwitchSkins, _autoSkinSwitch = 7f;
    private int _birdClipIndex = 0;
    private int _swordClipIndex = 0;
    private int _currentAutoStepClipIndex = 0;
    private int currentClipIndex = 0;
    private int currentStepClipIndex = 0;
    private int _previousAnimationCount = 0;
    private int currentSecondClipIndex = 0;
    private int _animationCount;
    private int _skinsCount = 0;
    private bool _isSoundPlayed = false;
    private bool _isZoomed = false;
    private Vector3 _initialScale;
    private Vector3 _initialPosition;
    private bool _isAutoMode = true;
    private float _autoSwitchDelay = 5f;
    private float _currentSwitchTimer = 0f;
    private bool _isFirstAnimation = false;
    private float _initialAnimationSpeedFactor = 1f;
    private const float _minAnimationSpeedFactor = 0.1f; 
    private const float _maxAnimationSpeedFactor = 1.9f;
    private int _autoModeChangeCounter = 0;
    private int _daysCount = 1;

    private bool _isCumStep = false;
    private bool _isSkinsAutoMode = true;
    public SkeletonAnimation BadEndAnimation;
    public List<string> AnimationStates = new List<string>();
    public List<string> AnimationSkins = new List<string>();
    public float ZoomFactor = 0.5f;
    public float ZoomSpeed = 2f;
    public float AnimationSpeedFactor = 1f;

    void Start()
    {
        if (_goblicScene.CurrentBadEndSceneIndex == 2)
        {
            _audioSource.loop = false;
            _audioSource.clip = _audioClips[0];
            _audioSource.Play();

        }
        _ui.SetActive(true);
        _initialScale = _zoomedTr.localScale;
        _initialPosition = _zoomedTr.position;

        BadEndAnimation.initialSkinName = AnimationSkins[0];
        BadEndAnimation.Initialize(true);
        BadEndAnimation.AnimationState.SetAnimation(0, AnimationStates[0], true);
        _skinsCountUI.text = "(" + _skinsCount + ")";
        _skinsCount = 0;
        _animationCountUI.text = "(Auto)";


        StartCoroutine(SwitchAudioClipWithInterval(3f, _swordSource, _swordClips, _swordClipIndex, true));

        if(_goblicScene.CurrentBadEndSceneIndex == 0 || _goblicScene.CurrentBadEndSceneIndex == 1)
            StartCoroutine(SwitchAudioClipWithInterval(1f, _audioSource, _audioClips, currentClipIndex, true));
        else
            StartCoroutine(SwitchAudioClipWithInterval(4f, _audioSource, _audioClips, currentClipIndex, true));

        StartCoroutine(SwitchAudioClipWithInterval(15f, _birdSource, _birdClips, _birdClipIndex, true));
        StartCoroutine(SwitchAudioThirdClipWithInterval(1f, _coughSource, _coughClips, currentSecondClipIndex, true));
        StartCoroutine(PlayAudioWithProbability(_oralSquishSource, _oralMainClip, _oralCleps, _primaryProbability));
    }

    private void Update()
    {
        /*_daysCounter.text = (_animationCount + 1).ToString();*/
        if (_animationCount == 0 && _goblicScene.CurrentBadEndSceneIndex == 0)
        {
            _firstAnimPanel.SetActive(true);
            _audioSource.mute = true;
            _swordSource.mute = false;
        }
        else if (_animationCount > 0)
        {
            _audioSource.mute = false;
            _swordSource.mute = true;
        }

        if (_animationCount == 2)
        {
            _coughSource.mute = false;
            _oralSquishSource.mute = false;
        }
        else if (_animationCount != 2)
        {
            _coughSource.mute = true;
            _oralSquishSource.mute = true;
        }

        if (_isAutoMode && _previousAnimationCount == 2)
        {
            if (!_isSoundPlayed && _currentSwitchTimer >= 4) 
            {
                Debug.Log("Play");

                int newClipIndex = Random.Range(0, _autoModeStepClips.Length);

                while (newClipIndex == _currentAutoStepClipIndex)
                    newClipIndex = Random.Range(0, _autoModeStepClips.Length);

                _currentAutoStepClipIndex = newClipIndex;

                _autoModeStepSoruce.clip = _autoModeStepClips[_currentAutoStepClipIndex];

                _autoModeStepSoruce.Play();

                _isSoundPlayed = true;
            }
        }

        if (_previousAnimationCount == 2 && _animationCount != 2)
        {
            int newClipIndex = Random.Range(0, _stepClips.Length);

            while (newClipIndex == currentStepClipIndex)
                newClipIndex = Random.Range(0, _stepClips.Length);

            currentStepClipIndex = newClipIndex;

            _stepSource.clip = _stepClips[currentStepClipIndex];

            _stepSource.Play();
        }

        if(_stepSource.isPlaying)
        {
            _audioSource.volume = 0;
            _coughSource.volume = 0;
            _oralSquishSource.volume = 0;
            _autoModeStepSoruce.volume = 0;
            _swordSource.volume = 0;
        }
        else
        {
            _audioSource.volume = 1;
            _coughSource.volume = 1;
            _oralSquishSource.volume = 1;
            _autoModeStepSoruce.volume = 1;
            _swordSource.volume = 1;
        }

        _previousAnimationCount = _animationCount;

        if (_isAutoMode && _goblicScene.CurrentBadEndSceneIndex == 0)
        {
            _animationCountUI.text = "(Auto)";
            _currentSwitchTimer += Time.deltaTime;
            if (_currentSwitchTimer >= _autoSwitchDelay)
            {
                _currentSwitchTimer = 0f;
                _daysCount++;
                if (_animationCount < AnimationStates.Count - 1)
                {
                    _animationCount++;
                    
                }
                else
                {
                    int randomIndex = Random.Range(0, _animationCount);
                    _animationCount = randomIndex;
                    _isSoundPlayed = false;
                    
                }
                BadEndAnimation.AnimationState.SetAnimation(0, AnimationStates[_animationCount], true);
            }

            if (Input.GetKeyDown(KeyCode.D) && _isAutoMode)
            {
                _daysCount++;
                _isAutoMode = false;
                _isFirstAnimation = true;
                _animationCount = 0; 
                BadEndAnimation.AnimationState.SetAnimation(0, AnimationStates[_animationCount], true);
            }
        }
        else if(!_isAutoMode && _goblicScene.CurrentBadEndSceneIndex == 0)
        {
            if (Input.GetKeyDown(KeyCode.A) && _animationCount > 0)
            {
                _animationCount--;
                BadEndAnimation.AnimationState.SetAnimation(0, AnimationStates[_animationCount], true);
                _daysCount++;
            }
            else if (Input.GetKeyDown(KeyCode.D) && _animationCount < AnimationStates.Count - 1)
            {
                _animationCount++;
                _daysCount++;
                _isFirstAnimation = false;
                BadEndAnimation.AnimationState.SetAnimation(0, AnimationStates[_animationCount], true);
                Debug.Log(_animationCount);
            }

            if (Input.GetKeyDown(KeyCode.A) && _animationCount == 0)
            {
                if (!_isFirstAnimation)
                {
                    _isFirstAnimation = true;
                }
                else
                {
                    _isAutoMode = true;
                    Debug.Log("Auto mode activated");
                }
            }
            _animationCountUI.text = "(" + (_animationCount + 1).ToString() + (")");

        }

        if (_isAutoMode && _goblicScene.CurrentBadEndSceneIndex == 1 /*|| _isAutoMode && _goblicScene.CurrentBadEndSceneIndex == 2*/)
        {
            _animationCountUI.text = "(Auto)";
            
            _currentSwitchTimer += Time.deltaTime;
            if (_currentSwitchTimer >= _autoSwitchDelay)
            {
                _currentSwitchTimer = 0f;
                _daysCount++;

                if (_animationCount < AnimationStates.Count - 1)
                {
                    _autoSwitchDelay = 5f;
                    BadEndAnimation.initialSkinName = AnimationSkins[0];
                    BadEndAnimation.Initialize(true);
                    _animationCount++;
                    BadEndAnimation.AnimationState.SetAnimation(0, AnimationStates[_animationCount], true);
                    _autoModeChangeCounter++;
                    Debug.Log(_autoModeChangeCounter);
                }
                else if(_autoModeChangeCounter < 6 && _animationCount >= AnimationStates.Count - 1)
                {
                    _autoSwitchDelay = 5f;
                    BadEndAnimation.initialSkinName = AnimationSkins[0];
                    BadEndAnimation.Initialize(true);
                    int randomIndex = Random.Range(0, _animationCount);
                    _animationCount = randomIndex;
                    BadEndAnimation.AnimationState.SetAnimation(0, AnimationStates[_animationCount], true);
                    _isSoundPlayed = false;
                    _autoModeChangeCounter++;
                }
                else if(_autoModeChangeCounter >= 6)
                {
                    _autoModeChangeCounter = 0;
                    _autoSwitchDelay = 10f;
                    int variationAnim = Random.Range(0, 2);
                    if(variationAnim == 0)
                    {
                        BadEndAnimation.initialSkinName = _skinNames[0];
                        BadEndAnimation.Initialize(true);
                        BadEndAnimation.AnimationState.SetAnimation(0, _animName, true);
                        int newClipIndex = Random.Range(0, _autoModeStepClips.Length);
                        while (newClipIndex == _currentAutoStepClipIndex)
                            newClipIndex = Random.Range(0, _autoModeStepClips.Length);
                        _currentAutoStepClipIndex = newClipIndex;
                        _autoModeStepSoruce.clip = _autoModeStepClips[_currentAutoStepClipIndex];
                        _autoModeStepSoruce.Play();
                    }
                    else
                    {
                        BadEndAnimation.initialSkinName = _skinNames[1];
                        BadEndAnimation.Initialize(true);
                        BadEndAnimation.AnimationState.SetAnimation(0, _animName, true);
                        int newClipIndex = Random.Range(0, _pussyClips.Length);
                        _pussySource.clip = _pussyClips[newClipIndex];
                        _pussySource.Play();
                    }

                }

            }

            if (Input.GetKeyDown(KeyCode.D) && _isAutoMode)
            {
                _isAutoMode = false;
                _daysCount++;
                _isFirstAnimation = true;
                _animationCount = 0; 
                BadEndAnimation.AnimationState.SetAnimation(0, AnimationStates[_animationCount], true);
            }
        }
        else if (!_isAutoMode && _goblicScene.CurrentBadEndSceneIndex == 1)
        {
            if (Input.GetKeyDown(KeyCode.A) && _animationCount > 0)
            {
                Debug.Log("1");

                _animationCount--;
                _daysCount++;
                if (_animationCount == 2)
                {
                    Debug.Log("2");
                    BadEndAnimation.initialSkinName = AnimationSkins[0];
                    BadEndAnimation.Initialize(true);
                    BadEndAnimation.AnimationState.SetAnimation(0, AnimationStates[_animationCount], true);
                }
                else
                {
                    BadEndAnimation.AnimationState.SetAnimation(0, AnimationStates[_animationCount], true);
                }

            }
            else if (Input.GetKeyDown(KeyCode.D) && _animationCount < 3)
            {
                _animationCount++;
                _daysCount++;
                if (/*_animationCount < AnimationStates.Count - 1 &&*/ _animationCount <= 2)
                {

                    _isFirstAnimation = false;
                    Debug.Log("skinNale: " + BadEndAnimation.initialSkinName);
                    BadEndAnimation.AnimationState.SetAnimation(0, AnimationStates[_animationCount], true);
                    Debug.Log(_animationCount);

                }
                else if (_animationCount == 3)
                {
                
                    int variationAnim = Random.Range(0, 2);
                    if (variationAnim == 0)
                    {
                        BadEndAnimation.initialSkinName = _skinNames[0];
                        BadEndAnimation.Initialize(true);
                        BadEndAnimation.AnimationState.SetAnimation(0, _animName, true);
                        int newClipIndex = Random.Range(0, _autoModeStepClips.Length);
                        while (newClipIndex == _currentAutoStepClipIndex)
                            newClipIndex = Random.Range(0, _autoModeStepClips.Length);
                        _currentAutoStepClipIndex = newClipIndex;
                        _autoModeStepSoruce.clip = _autoModeStepClips[_currentAutoStepClipIndex];
                        _autoModeStepSoruce.Play();
                    }
                    else
                    {
                        BadEndAnimation.initialSkinName = _skinNames[1];
                        BadEndAnimation.Initialize(true);
                        BadEndAnimation.AnimationState.SetAnimation(0, _animName, true);
                        int newClipIndex = Random.Range(0, _pussyClips.Length);
                        _pussySource.clip = _pussyClips[newClipIndex];
                        _pussySource.Play();
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.A) && _animationCount == 0)
            {
                if (!_isFirstAnimation)
                {
                    _isFirstAnimation = true;
                }
                else
                {
                    _isAutoMode = true;
                    Debug.Log("Auto mode activated");
                }
            }
            _animationCountUI.text = "(" + (_animationCount + 1).ToString() + (")");

        }

        if (Input.GetKeyDown(KeyCode.J) && _goblicScene.CurrentBadEndSceneIndex != 2)
        {
            if (AnimationSkins.Count > _skinsCount)
            {
                BadEndAnimation.initialSkinName = AnimationSkins[_skinsCount];
                BadEndAnimation.Initialize(true);
                BadEndAnimation.AnimationState.SetAnimation(0, AnimationStates[_animationCount], true);
                _skinsCount++;
            }
            else
            {
                _skinsCount = 0;
                BadEndAnimation.initialSkinName = AnimationSkins[_skinsCount];
                BadEndAnimation.Initialize(true);
                BadEndAnimation.AnimationState.SetAnimation(0, AnimationStates[_animationCount], true);
                _skinsCount++;
            }
            _skinsCountUI.text = "(" + (_skinsCount).ToString() + ")";
        }

        if (Input.GetKeyDown(KeyCode.D) && _goblicScene.CurrentBadEndSceneIndex == 2)
        {
            
            _daysCount++;

            if (_isSkinsAutoMode)
            {
                _isSkinsAutoMode = false;
                _skinsCount = 0;
            }
            
            if (_skinsCount < AnimationSkins.Count - 1)
            {
                _skinsCount++;
            }
            else
            {
                int skinNumber = Random.Range(0, AnimationSkins.Count);

                _skinsCount = skinNumber;
            }
            BadEndAnimation.initialSkinName = AnimationSkins[_skinsCount];
            BadEndAnimation.Initialize(true);
            BadEndAnimation.AnimationState.SetAnimation(0, AnimationStates[_animationCount], true);
            _skinsCountUI.text = "(" + (_skinsCount).ToString() + ")";
        }
        else if (Input.GetKeyDown(KeyCode.A) && _goblicScene.CurrentBadEndSceneIndex == 2)
        {
            if(_skinsCount == 0)
            {
                _isSkinsAutoMode = true; 
                _skinsCountUI.text = "Auto";
            }

            else 
            {
                if(_daysCount > 1)
                {
                    _daysCount--;
                }
                _skinsCount--;
            }
            
            BadEndAnimation.initialSkinName = AnimationSkins[_skinsCount];
            BadEndAnimation.Initialize(true);
            BadEndAnimation.AnimationState.SetAnimation(0, AnimationStates[_animationCount], true);
            _skinsCountUI.text = "(" + (_skinsCount).ToString() + ")";
        }

        if (_isSkinsAutoMode && _goblicScene.CurrentBadEndSceneIndex == 2)
        {
            _currentSwitchSkins += Time.deltaTime;
            _skinsCountUI.text = "Auto";


            if (_currentSwitchSkins >= _autoSkinSwitch)
            {
                _currentSwitchSkins = 0;
                _daysCount++;

                if (_skinsCount > 0 && _autoModeChangeCounter < 6)
                {
                    _autoSkinSwitch = 7f;
                    _autoModeChangeCounter++;
                    _skinsCount--;
                    BadEndAnimation.initialSkinName = AnimationSkins[_skinsCount];
                    BadEndAnimation.Initialize(true);
                    BadEndAnimation.AnimationState.SetAnimation(0, AnimationStates[_animationCount], true);
                }
                else if (_autoModeChangeCounter < 6 && _skinsCount <= 0)
                {
                    _autoSkinSwitch = 7f;
                    _autoModeChangeCounter++;
                    _skinsCount = AnimationSkins.Count - 1;
                    BadEndAnimation.initialSkinName = AnimationSkins[_skinsCount];
                    BadEndAnimation.Initialize(true);
                    BadEndAnimation.AnimationState.SetAnimation(0, AnimationStates[_animationCount], true);
                }
                else if (_autoModeChangeCounter >= 6)
                {
                    _autoModeChangeCounter = 0;
                    _autoSkinSwitch = 10f;
                    int variationAnim = Random.Range(0, 2);
                    if (variationAnim == 0)
                    {
                        BadEndAnimation.initialSkinName = _skinNames[0];
                        BadEndAnimation.Initialize(true);
                        BadEndAnimation.AnimationState.SetAnimation(0, _animName, true);
                        int newClipIndex = Random.Range(0, _pussyClips.Length);
                        _pussySource.clip = _pussyClips[newClipIndex];
                        _pussySource.Play();
                    }
                    else
                    {
                        BadEndAnimation.initialSkinName = _skinNames[1];
                        BadEndAnimation.Initialize(true);
                        BadEndAnimation.AnimationState.SetAnimation(0, _animName, true);
                        int newClipIndex = Random.Range(0, _pussyClips.Length);
                        _pussySource.clip = _pussyClips[newClipIndex];
                        _pussySource.Play();
                    }
                }

                /*BadEndAnimation.initialSkinName = AnimationSkins[_skinsCount];
                BadEndAnimation.Initialize(true);
                BadEndAnimation.AnimationState.SetAnimation(0, AnimationStates[_animationCount], true);*/
            }

        }

        if (Input.GetMouseButtonDown(0))
        {
            if (!_isZoomed)
            {
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));

                var zoom = _zoomedTr.localScale * ZoomFactor;
                zoom.z = 1;
                _zoomedTr.localScale = zoom;

                var pos = _zoomedTr.position + mousePosition * (1 - ZoomFactor);
                pos.y += _yOffsetAfterZoom; 
                _zoomedTr.position = pos;


                _isZoomed = true;
                _panel.SetActive(false);
                _zoomUI.text = "Back";
               
            }
            else
            {
                _zoomedTr.localScale = _initialScale;
                _zoomedTr.position = _initialPosition;
                _isZoomed = false;
                _panel.SetActive(true);
                _zoomUI.text = "Zoom";
            }
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            AnimationSpeedFactor += _initialAnimationSpeedFactor * 0.1f;
            if (AnimationSpeedFactor > _maxAnimationSpeedFactor * _initialAnimationSpeedFactor)
                AnimationSpeedFactor = _maxAnimationSpeedFactor * _initialAnimationSpeedFactor;

            UpdateAnimationSpeedAndPitch();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            AnimationSpeedFactor -= _initialAnimationSpeedFactor * 0.1f;
            if (AnimationSpeedFactor < _minAnimationSpeedFactor * _initialAnimationSpeedFactor)
                AnimationSpeedFactor = _minAnimationSpeedFactor * _initialAnimationSpeedFactor;

            UpdateAnimationSpeedAndPitch();
        }

        _daysCounter.text = _daysCount.ToString();
    }

    private void UpdateAnimationSpeedAndPitch()
    {
        float newPitch = 0.6f + (AnimationSpeedFactor - _initialAnimationSpeedFactor) * 0.3f;
        BadEndAnimation.AnimationState.TimeScale = AnimationSpeedFactor;
        _audioSource.pitch = newPitch;
        _oralSquishSource.pitch = newPitch;

        if (AnimationSpeedFactor == _initialAnimationSpeedFactor)
            _animationSpeedUI.text = "0";
        else if (AnimationSpeedFactor > _initialAnimationSpeedFactor)
            _animationSpeedUI.text = "+" + ((AnimationSpeedFactor - _initialAnimationSpeedFactor) / _initialAnimationSpeedFactor * 100f).ToString("0") + "%";
        else if (AnimationSpeedFactor < _initialAnimationSpeedFactor)
            _animationSpeedUI.text = ((AnimationSpeedFactor - _initialAnimationSpeedFactor) / _initialAnimationSpeedFactor * 100f).ToString("0") + "%";
    }

    private IEnumerator SwitchAudioClipWithInterval(float interval, AudioSource audioSource, AudioClip[] audioClips, int currentIndex, bool isPlay)
    {
        while (isPlay)
        {
            yield return new WaitForSeconds(interval);

            int newClipIndex = Random.Range(0, audioClips.Length);

            while (newClipIndex == currentIndex)
                newClipIndex = Random.Range(0, audioClips.Length);

            currentIndex = newClipIndex;
            
            audioSource.clip = audioClips[currentIndex];

            audioSource.Play();
            
        }
    }

    private IEnumerator SwitchAudioThirdClipWithInterval(float interval, AudioSource audioSource, AudioClip[] audioClips, int currentIndex, bool isPlay)
    {
        while (isPlay)
        {
            yield return new WaitForSeconds(interval);

            float randomValue = Random.Range(0f, 1f);

            if (randomValue <= 0.85f)
            {
                int newClipIndex = Random.Range(0, audioClips.Length);

                while (newClipIndex == currentIndex)
                    newClipIndex = Random.Range(0, audioClips.Length);

                currentIndex = newClipIndex;

                audioSource.clip = audioClips[currentIndex];

                audioSource.Play();
            }
        }
    }

    private IEnumerator PlayAudioWithProbability(AudioSource audioSource, AudioClip primaryClip, AudioClip[] secondaryClips, float primaryProbability)
    {
        while (true)
        {
            float randomValue = Random.Range(0f, 1f);

            AudioClip selectedClip = (randomValue <= primaryProbability) ? primaryClip : secondaryClips[Random.Range(0, secondaryClips.Length)];

            audioSource.clip = selectedClip;

            audioSource.Play();

            yield return new WaitForSeconds(selectedClip.length);
        }
    }
}

