using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour
{
    [Header("CONTROLLER")]
    [SerializeField] private Controller _inputManager;

    [Header("OBJECT")]
    [SerializeField] private GameObject _panelTutorial;
    [SerializeField] private Canvas _canvasMenu;

    [Header("BUTTON")]
    [SerializeField] private Button _buttonPlay;
    [SerializeField] private Button _buttonTutorial;
    [SerializeField] private Button _buttonQuit;

    [SerializeField] private Button _buttonQuitTutorial;

    [Header("SCENE")]
    [SerializeField] private string scenePlay;

    [Header("BACKGROUND")]
    [SerializeField] private string _stringLoadBackground;

    [Header("VERSION")]
    [SerializeField] private string _versionString;
    [SerializeField] private GameObject _versionObject;

    private void Start()
    {
        EnableInputManager();

            if (_stringLoadBackground != null) 
            {
                SceneManager.LoadScene(_stringLoadBackground, LoadSceneMode.Additive);
            } 
            else
            {
                Debug.LogWarning("Aucun fond referencer");
            }

        _canvasMenu.renderMode = RenderMode.ScreenSpaceCamera;
        _canvasMenu.worldCamera = Camera.current;


        List<Button> buttons = new List<Button>();
        buttons.Add(_buttonPlay);
        buttons.Add(_buttonTutorial);
        buttons.Add(_buttonQuit);
        buttons.Add(_buttonQuitTutorial);

        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].onClick.RemoveAllListeners();
        }

        _buttonPlay.onClick.AddListener(() => goToScene(scenePlay));
        _buttonTutorial.onClick.AddListener(() => SwitchPanel(_panelTutorial));
        _buttonQuit.onClick.AddListener(() => QuitGame());

        _buttonQuitTutorial.onClick.AddListener(() => SwitchPanel(_panelTutorial));

        foreach (var button in buttons)
        {
            string nameSoundClick = button.animationTriggers.pressedTrigger;

            button.onClick.AddListener(() => { AudioManager.PlaySoundAtPosition(nameSoundClick, Vector3.zero); });

            // Recupere le "EventTrigger" du boutton
            EventTrigger eventTrigger;

            if (button.TryGetComponent<EventTrigger>(out eventTrigger))
            {
                eventTrigger.triggers.Clear();

                nameSoundClick = button.animationTriggers.highlightedTrigger;

                // Initialise un event "EventTrigger"
                EventTrigger.Entry onSelected = new EventTrigger.Entry();
                // Nettoie la liste d'evenement
                onSelected.callback.RemoveAllListeners();
                // Le met en mode "UpdateSelected" afin de detecter lorsque la souris est sur le boutton
                onSelected.eventID = EventTriggerType.PointerEnter;
                // Insert dans sa liste de reaction, l'affichage de la pop-up de description
                onSelected.callback.AddListener((eventData) => { AudioManager.PlaySoundAtPosition(nameSoundClick, Vector3.zero); });

                // Ajoute le composant et ces parametres dans le boutton
                eventTrigger.triggers.Add(onSelected);

            }

        }
    
        UpdateVersion();

    }

    private void Update()
    {

        if (_inputManager.System.Exit.WasPressedThisFrame())
        {
            if (_panelTutorial.activeSelf) { SwitchPanel(_panelTutorial); } else { Application.Quit(); }
        }

    }

    public void QuitGame()
    {
        Application.Quit();
    }

    void EnableInputManager()
    {

        if (_inputManager == null) _inputManager = new Controller();
        // On active les différents inputs
        _inputManager.System.Enable(); // TODO : faudra assembler les inputs
    }

    // -- Permet de charger une nouvelle scene -- //
    public void goToScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    // -- Permet d'afficher et de cacher l'ecran de tutoriel -- //
    public void SwitchPanel(GameObject panel)
    {
        if (panel == null) { return; }

        if (panel.activeSelf)
        { panel.SetActive(false); }
        else
        { panel.SetActive(true); }

    }

    private void UpdateVersion()
    {
        TextMeshProUGUI textMeshProUGUI;

        if (_versionObject != null && _versionObject.TryGetComponent<TextMeshProUGUI>(out textMeshProUGUI))
        {
            Debug.Log("Le texte de la version a etais mise en place sur le composant TEXTMESHPROUGUI");
            textMeshProUGUI.text = _versionString + Application.version.ToString();
        }
        else if (_versionObject == null)
        {
            Debug.Log("l'objet n'as pas etais referencer");
        }
        else
        {
            Debug.Log("Aucun composant texte n'as etais trouver dans l'objet");
        }
    }
}
