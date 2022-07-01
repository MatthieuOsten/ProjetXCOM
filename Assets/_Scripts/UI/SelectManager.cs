using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;

public class SelectManager : MonoBehaviour
{
    [Header("CONTROLLER")]
    [SerializeField] private Controller _inputManager;
    [SerializeField] private string _returnMenu;

    [Header("OBJECT")]
    [SerializeField] private GameObject _prefabMap;
    [SerializeField] private Transform _parent;
    [SerializeField] private List<Map> _maps;

    [Header("DEBUG")]
    [SerializeField] private bool _resetButton;
    [SerializeField] private bool _updateObject;

    [System.Serializable]
    public class Map
    {
        [Header("DATA")]
        public string _name;
        public string _nameScene;

        [Header("OBJECT")]
        public GameObject _actualObject;
        public Button _buttonMap;
        public TextMeshProUGUI _titleMap;
        public VideoPlayer _videoMap;
        public RawImage _rawImageMap;

        [Header("DISPLAY")]
        public VideoClip _video;
        public RenderTexture _render;

        public void SetObject(GameObject mapObject)
        {
            _actualObject = mapObject;

            _titleMap = mapObject.transform.Find("Title").GetComponent<TextMeshProUGUI>();
            _videoMap = mapObject.transform.Find("Video").GetComponent<VideoPlayer>();
            _rawImageMap = mapObject.transform.Find("Video").GetComponent<RawImage>();
            _buttonMap = mapObject.GetComponent<Button>();

            SetProprities();

        }

        public void SetProprities()
        {
            _videoMap.clip = _video;
            _titleMap.text = _name;

            _videoMap.renderMode = VideoRenderMode.RenderTexture;
            _videoMap.targetTexture = _render;
            _rawImageMap.texture = _render;

            _videoMap.audioOutputMode = VideoAudioOutputMode.None;
            _videoMap.Prepare();
            _videoMap.Stop();

            // -- Initialise "DisplayDescription" sur le boutton -- //
            if (_buttonMap != null) // Verifie que la description est remplie
            {
                _buttonMap.onClick.RemoveAllListeners();
                _buttonMap.onClick.AddListener(() => { SceneManager.LoadScene(_nameScene); });

                // Recupere le "EventTrigger" du boutton
                EventTrigger eventTrigger;

                if (_buttonMap.TryGetComponent<EventTrigger>(out eventTrigger))
                {
                    eventTrigger.triggers.Clear();

                    // Initialise un event "EventTrigger"
                    EventTrigger.Entry onSelected = new EventTrigger.Entry();
                    // Nettoie la liste d'evenement
                    onSelected.callback.RemoveAllListeners();
                    // Le met en mode "UpdateSelected" afin de detecter lorsque la souris est sur le boutton
                    onSelected.eventID = EventTriggerType.PointerEnter;
                    // Insert dans sa liste de reaction, l'affichage de la pop-up de description
                    onSelected.callback.AddListener((eventData) => { _videoMap.Play(); });

                    // Ajoute le composant et ces parametres dans le boutton
                    eventTrigger.triggers.Add(onSelected);


                    // Initialise un event "EventTrigger"
                    EventTrigger.Entry onDeselected = new EventTrigger.Entry();
                    // Nettoie la liste d'evenement
                    onDeselected.callback.RemoveAllListeners();
                    // Le met en mode "UpdateSelected" afin de detecter lorsque la souris est sur le boutton
                    onDeselected.eventID = EventTriggerType.PointerExit;
                    // Insert dans sa liste de reaction, l'affichage de la pop-up de description
                    onDeselected.callback.AddListener((eventData) => { _videoMap.Pause(); });
                    // Ajoute le composant et ces parametres dans le boutton
                    eventTrigger.triggers.Add(onDeselected);
                }

            }
        }

    }

#if UNITY_EDITOR

    private void OnValidate()
    {
        if (_resetButton)
        {
            _resetButton = false;

            for (int i = 0; i < _maps.Count; i++)
            {
                if (_maps[i]._actualObject != null)
                {
                    _maps[i]._actualObject = null;
                }
                
            }

            for (int i = 0; i < _parent.childCount; i++)
            {
                DestroyImmediate(_parent.GetChild(i));
            }

            InitialiseButtonMap();
        }

        if (_updateObject)
        {
            _updateObject = false;

            UpdateButtonMap();
        }
    }

#endif

    private void Start()
    {
        EnableInputManager();

        UpdateButtonMap();
    }

    private void Update()
    {

        if (_inputManager.System.Exit.WasPressedThisFrame())
        {
            SceneManager.LoadScene(_returnMenu);
        }

    }

    private void UpdateButtonMap()
    {
        for (int i = 0; i < _maps.Count; i++)
        {
            if (_maps[i]._actualObject != null)
            {
                _maps[i].SetProprities();
            }

        }
    }

    private void InitialiseButtonMap()
    {
        for (int i = 0; i < _maps.Count; i++)
        {
            if (_maps[i]._actualObject == null)
            {
                GameObject mapObject = Instantiate(_prefabMap, _parent);

                _maps[i]._actualObject = mapObject;
            }
        }
    }

    void EnableInputManager()
    {

        if (_inputManager == null) _inputManager = new Controller();
        // On active les différents inputs
        _inputManager.System.Enable(); // TODO : faudra assembler les inputs
    }

}
