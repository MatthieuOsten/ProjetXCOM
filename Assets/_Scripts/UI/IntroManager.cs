using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using TMPro;

public class IntroManager : MonoBehaviour
{
    [SerializeField] private VideoPlayer _video;
    [SerializeField] private string _mainMenu;

    [SerializeField] private Controller _inputManager;
    [SerializeField] private string _textInputPart1,_textInputPart2;

    [SerializeField] private TextMeshProUGUI _time;
    [SerializeField] private double _timerSkip, _timerDisplay, _timeToSkip, _timeToDisplay;

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
            if (_timeToSkip == 0) { 
                _timeToSkip = _video.time / 2;
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

            if (playerCurrentFrame >= playerFrameCount)
            {
                SceneManager.LoadScene(_mainMenu);
            }

            if (_time != null)
            {
                if (_timerDisplay >= _timeToDisplay && _timerSkip < _timeToSkip)
                {
                    if (!_time.IsActive()) { _time.enabled = true; }
                    _time.text = ((int)_timerSkip - (int)_timerDisplay).ToString();
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
                            _time.text = _textInputPart1 + _inputManager.System.Exit.name + _textInputPart2;
                        }
                    }
                    else
                    {
                        if (_time.IsActive()) { _time.enabled = false; }
                    }
                }

                if (_inputManager != null && _inputManager.System.Exit.IsPressed())
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
