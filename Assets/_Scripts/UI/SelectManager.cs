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
    [SerializeField] private GameObject _prefabMap;
    [SerializeField] private Transform _parent;
    [SerializeField] private List<Map> _maps;

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
        public Image _imageMap;
        public TextMeshProUGUI _titleMap;
        public VideoPlayer _videoMap;

        [Header("DISPLAY")]
        public VideoClip _video;
        public Sprite _image;

        public void SetObject(GameObject mapObject)
        {
            _actualObject = mapObject;

            _imageMap = mapObject.transform.Find("Image").GetComponent<Image>();
            _titleMap = mapObject.transform.Find("Title").GetComponent<TextMeshProUGUI>();
            _videoMap = mapObject.transform.Find("Video").GetComponent<VideoPlayer>();
            _buttonMap = mapObject.GetComponent<Button>();

            SetProprities();

        }

        public void SetProprities()
        {
            _videoMap.clip = _video;
            _imageMap.sprite = _image;
            _titleMap.text = _name;

            _videoMap.Prepare();
            _videoMap.Stop();

            // -- Initialise "DisplayDescription" sur le boutton -- //
            if (_buttonMap != null) // Verifie que la description est remplie
            {
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
                    onSelected.callback.AddListener((eventData) => { PlayVideo(); });

                    // Ajoute le composant et ces parametres dans le boutton
                    eventTrigger.triggers.Add(onSelected);


                    // Initialise un event "EventTrigger"
                    EventTrigger.Entry onDeselected = new EventTrigger.Entry();
                    // Nettoie la liste d'evenement
                    onDeselected.callback.RemoveAllListeners();
                    // Le met en mode "UpdateSelected" afin de detecter lorsque la souris est sur le boutton
                    onDeselected.eventID = EventTriggerType.PointerExit;
                    // Insert dans sa liste de reaction, l'affichage de la pop-up de description
                    onDeselected.callback.AddListener((eventData) => { StopVideo(); });
                    // Ajoute le composant et ces parametres dans le boutton
                    eventTrigger.triggers.Add(onDeselected);
                }

            }
        }

        public void PlayVideo()
        {
            _imageMap.enabled = false;
            _videoMap.enabled = true;

            _videoMap.Play();
        }

        public void StopVideo()
        {
            _imageMap.enabled = true;
            _videoMap.enabled = false;

            _videoMap.Pause();
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

            for (int i = 0; i < _maps.Count; i++)
            {
                if (_maps[i]._actualObject != null)
                {
                    _maps[i].SetProprities();
                }

            }
        }
    }

#endif

    private void Start()
    {
        InitialiseButtonMap();
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

}
