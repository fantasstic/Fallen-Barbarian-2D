using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Video;

public class MP4Player : MonoBehaviour
{
    [SerializeField] private GameObject _ui; 
    [SerializeField] private AudioSource _audioSource, _coughSource, _stepSource, _oralSquishSource, _autoModeStepSoruce, _swordSource, _birdSource, _pussySource;
    [SerializeField] private AudioClip[] _audioClips, _coughClips, _stepClips, _oralCleps, _autoModeStepClips, _swordClips, _birdClips, _pussyClips;
    [SerializeField] private Transform _zoomedTr;
    [SerializeField] private float _yOffsetAfterZoom;
    [SerializeField] private TMP_Text _animationCountUI, _skinsCountUI, _animationSpeedUI, _zoomUI, _daysCounter;
    [SerializeField] private AudioClip _oralMainClip;

    private Vector3 _initialScale;
    private bool _isZoomed = false;
    private Vector3 _initialPosition;
    private bool _isSoundPlay = false;

    public float ZoomFactor = 0.5f;
    public float ZoomSpeed = 2f;
    public VideoPlayer videoPlayer;
    public AudioSource audioSource;
    public string[] videoFileNames;
    public AudioClip[] audioClips;
    public VideoClip[] videoClips;

    public int currentIndex = 0;
    private bool isAutoMode = true;
    private float autoSwitchDelay = 5.0f;
    private float currentSwitchTimer = 0.0f;
    private float playbackSpeed = 1.0f;
    int _previousIndex = -1;

    private float minSpeed = 0.1f;
    private float maxSpeed = 1.9f;
    private float speedStep = 0.1f;
    private int _birdClipIndex = 0;
    private int _swordClipIndex = 0;
    private int currentClipIndex = 0;
    private int currentSecondClipIndex = 0;
    private float _primaryProbability = 0.75f;
    private float initialSpeedPercent = 0.0f;

    private void Start()
    {
        _ui.SetActive(true);
        _initialScale = _zoomedTr.localScale;
        _initialPosition = _zoomedTr.position;
        
        PlayMediaAtIndex(currentIndex);

        StartCoroutine(SwitchAudioClipWithInterval(3f, _swordSource, _swordClips, _swordClipIndex, true));

        StartCoroutine(SwitchAudioClipWithInterval(4f, _audioSource, _audioClips, currentClipIndex, true));

        StartCoroutine(SwitchAudioClipWithInterval(15f, _birdSource, _birdClips, _birdClipIndex, true));

        StartCoroutine(SwitchAudioThirdClipWithInterval(1f, _coughSource, _coughClips, currentSecondClipIndex, true));
        /*StartCoroutine(PlayAudioWithProbability(_oralSquishSource, _oralMainClip, _oralCleps, _primaryProbability));*/
    }

    private void Update()
    {
        if (currentIndex == 0 || currentIndex == 4)
        {
            _audioSource.mute = true;
            _swordSource.mute = false;
        }
        else if (currentIndex > 0 || currentIndex < 4)
        {
            _audioSource.mute = false;
            _swordSource.mute = true;
        }

        if (currentIndex == 2)
            _audioSource.loop = false;
        else
            _audioSource.loop = true;

        if (currentIndex == 3)
            _audioSource.mute = true;

        if (currentIndex == 2 || currentIndex == 3)
        {
            _coughSource.mute = false;
            _oralSquishSource.mute = false;
        }
        else if (currentIndex == 0 || currentIndex == 1 || currentIndex == 4)
        {
            _coughSource.mute = true;
            _oralSquishSource.mute = true;
        }

        if (isAutoMode)
        {
            _animationCountUI.text = "(Auto)";
            currentSwitchTimer += Time.deltaTime;

            if (currentSwitchTimer >= autoSwitchDelay)
            {
                currentSwitchTimer = 0f;
                currentIndex++;

                if (currentIndex >= videoClips.Length)
                {
                    currentIndex = 0;
                }

                PlayMediaAtIndex(currentIndex);
            }

            if (Input.GetKeyDown(KeyCode.D) && isAutoMode)
            {
                isAutoMode = false;
                currentIndex = 0;
                PlayMediaAtIndex(currentIndex);
            }
        }
        else
        {
            _animationCountUI.text = "(" + (currentIndex + 1).ToString() + ")";

            if (Input.GetKeyDown(KeyCode.A) && currentIndex > 0)
            {
                currentIndex--;
                PlayMediaAtIndex(currentIndex);
            }
            else if (Input.GetKeyDown(KeyCode.D) && currentIndex < videoClips.Length - 1)
            {
                currentIndex++;
                Debug.Log(currentIndex);
                PlayMediaAtIndex(currentIndex);
            }

            if (Input.GetKeyDown(KeyCode.A) && currentIndex == 0)
            {
                isAutoMode = true;
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
                
                _zoomUI.text = "Back";

            }
            else
            {
                _zoomedTr.localScale = _initialScale;
                _zoomedTr.position = _initialPosition;
                _isZoomed = false;
                _zoomUI.text = "Zoom";
            }
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            playbackSpeed = Mathf.Min(playbackSpeed + speedStep, maxSpeed);
           // videoPlayer.playbackSpeed = playbackSpeed;
            SetPlaybackSpeed(playbackSpeed);
            _audioSource.pitch += 0.1f;
            _coughSource.pitch += 0.1f;
            UpdateSpeedText();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            playbackSpeed = Mathf.Max(playbackSpeed - speedStep, minSpeed);
            //videoPlayer.playbackSpeed = playbackSpeed;
            SetPlaybackSpeed(playbackSpeed);
            _audioSource.pitch -= 0.1f;
            _coughSource.pitch -= 0.1f;
            UpdateSpeedText();
        }
        //_animationSpeedUI.text = (CalculatePercentFromSpeed(playbackSpeed) * 10).ToString("F0") + "%";
    }

    

    private void SetPlaybackSpeed(float speed)
    {
        videoPlayer.playbackSpeed = speed;
        //_audioSource.pitch = speed;

        
    }

    private void UpdateSpeedText()
    {
        if (playbackSpeed == 1)
            _animationSpeedUI.text = "0";
        else if (playbackSpeed > 1)
            _animationSpeedUI.text = "+" + ((playbackSpeed - 1) / 1 * 100f).ToString("0") + "%";
        else if (playbackSpeed < 1)
            _animationSpeedUI.text = ((playbackSpeed - 1) / 1 * 100f).ToString("0") + "%";
    }

    void UpdateIndex(int newIndex)
    {
        if (_previousIndex != 4 && newIndex == 4 && !_isSoundPlay)
        {
            int newClipIndex = Random.Range(0, _pussyClips.Length);

            _pussySource.clip = _pussyClips[newClipIndex];
            _pussySource.Play();
            _isSoundPlay = true;
        }
        else
        {
            _isSoundPlay = false;
        }

        _previousIndex = newIndex;
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

    void PlayMediaAtIndex(int index)
    {
        if (index >= 0 && index < videoClips.Length)
        {
            // Play video
            videoPlayer.Stop();
            videoPlayer.clip = videoClips[index];
            videoPlayer.Play();

            UpdateIndex(index);
        }
    }
}
