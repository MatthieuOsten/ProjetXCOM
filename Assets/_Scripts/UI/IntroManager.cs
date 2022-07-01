using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using TMPro;

public class IntroManager : MonoBehaviour
{
    [Header("CINEMATIC")]
    [SerializeField] private VideoPlayer _video;
    [SerializeField] private string _mainMenu;

    [Header("INPUT")]
    [SerializeField] private Controller _inputManager;
    [SerializeField] private string _textInput;

    [Header("TIMER")]
    [SerializeField] private TextMeshProUGUI _time;
    [SerializeField] private double _timerSkip = 0, _timerDisplay = 0, _timerVideo = 0;
    [SerializeField] private double _timeToSkip, _timeToDisplay, _timeToVideo;

    private void Awake()
    {
        if (_video != null)
        {
            _video.Prepare();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (_timeToDisplay == 0)
        {
            _timeToDisplay = _timeToSkip / 2;
        }

        if (_video != null)
        {
            _timeToVideo = _video.length;

            if (_timeToSkip == 0) { 
                _timeToSkip = _timeToVideo / 2.5;
                _timeToDisplay = _timeToSkip / 2;
            }

            _video.Play();
        }

        EnableInputManager();
    }

    // Update is called once per frame
    void Update()
    {
        long playerCurrentFrame = _video.frame;
        long playerFrameCount = (long)_video.frameCount;

        if (_video != null)
        {   

            if (_timerVideo >= _timeToVideo)
            {
                SceneManager.LoadScene(_mainMenu);
            } else
            {
                _timerVideo += Time.deltaTime;
            }

            if (_time != null)
            {
                if (_timerDisplay >= _timeToDisplay && _timerSkip < _timeToSkip)
                {
                    if (!_time.IsActive()) { _time.enabled = true; }
                    int number = ((int)_timerSkip - (int)_timerDisplay) - ((int)_timeToSkip - (int)_timeToDisplay);
                    _time.text = (-number).ToString();
                }
                else if (_timerDisplay < _timeToDisplay)
                {
                    if (_time.IsActive()) { _time.enabled = false; }
                    _timerDisplay += Time.deltaTime;
                }
            }


            if (_timerSkip >= _timeToSkip)
            {
                if (_time != null)
                {
                    if (_inputManager != null)
                    {
                        if (_time != null && _timerDisplay >= _timeToDisplay)
                        {
                            if (!_time.IsActive()) { _time.enabled = true; }
                            _time.text = _textInput;
                        }
                    }
                    else
                    {
                        if (_time.IsActive()) { _time.enabled = false; }
                    }
                }

                if (_inputManager != null && _inputManager.System.Skip.IsPressed())
                {
                    SceneManager.LoadScene(_mainMenu);
                } 
                else if (_inputManager == null)
                {
                    SceneManager.LoadScene(_mainMenu);
                }

            } 
            else
            {
                _timerSkip += Time.deltaTime;
            }


        } 
        else
        {
            SceneManager.LoadScene(_mainMenu);
        }
    }

    void EnableInputManager()
    {
        if (_inputManager == null) _inputManager = new Controller();
        // On active les différents inputs
        _inputManager.System.Enable();
    }

}
