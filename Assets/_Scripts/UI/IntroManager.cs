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

    /// <summary>
    /// <para> fzaffz </para>
    /// </summary>
    [SerializeField] private byte _pressNbr = 2;
    [SerializeField] private byte _pressActualNbr = 0;

    [Header("TIMER")]

    /// <summary>
    /// C'est le moyen d'afficher a l'utilisateur qu'il peut passer la video
    /// </summary>
    [Tooltip("Text for skip video")]
    [SerializeField] private TextMeshProUGUI _time;
    /// <summary>
    /// Temps qu'il faut attendre pour afficher le texte pour passer la video
    /// </summary>
    [Tooltip("Time of display text")]
    [SerializeField] private double _timeToDisplay, _timerDisplay = 0;
    [Tooltip("Time to skip video")]
    [SerializeField] private double _timeToSkip, _timerSkip = 0;
    [Tooltip("Time of the video")]
    [SerializeField] private double _timeToVideo, _timerVideo = 0;

    byte PressActualNbr { 
        get { return _pressActualNbr; } 
        set { 
            if (value <= _pressNbr)
            { _pressActualNbr = value; } 
            else if (value < 0)
            { _pressActualNbr = 0; }
            else
            { value = _pressNbr; }
        } 
    }

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

            if (_timeToSkip == 0 && _timeToDisplay == 0)
            {
                _timeToSkip = _timeToVideo / 2.5;
                _timeToDisplay = _timeToSkip / 2;
            }
            else if (_timeToDisplay == 0) 
            {
                _timeToDisplay = _timeToVideo / 3;
                _timeToSkip = _timeToDisplay + _timeToSkip;
            } else
            {
                _timeToSkip = _timeToDisplay + _timeToSkip;
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

            if (PressActualNbr >= _pressNbr)
            {
                _timerDisplay = _timeToDisplay;
                _timerSkip = _timeToSkip;
            } 
            else if (PressActualNbr < _pressNbr) 
            {

                _inputManager.System.ForceSkip.performed += ctx => PressActualNbr++;

            }

            UpdateTimerDisplay();
            UpdateTimerSkip();
            UpdateTimerVideo();

        } 
        else
        {
            SceneManager.LoadScene(_mainMenu);
        }
    }

    private void UpdateTimerDisplay()
    {
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
    }

    private void UpdateTimerSkip()
    {
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

    private void UpdateTimerVideo()
    {
        if (_timerVideo >= _timeToVideo)
        {
            SceneManager.LoadScene(_mainMenu);
        }
        else
        {
            _timerVideo += Time.deltaTime;
        }
    }

    /// <summary>
    /// Active l'input manager
    /// </summary>
    void EnableInputManager()
    {
        if (_inputManager == null) _inputManager = new Controller();
        // On active les différents inputs
        _inputManager.System.Enable();
    }

}
