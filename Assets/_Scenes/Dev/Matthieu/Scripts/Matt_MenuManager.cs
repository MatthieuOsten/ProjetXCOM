using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class Matt_MenuManager : MonoBehaviour
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

    [Header("SCENE")]
    [SerializeField] private string scenePlay;

    [Header("BACKGROUND")]
    [SerializeField] private string _stringLoadBackground;
    [SerializeField] private GameObject _panelBackground;
    [SerializeField] private Sprite _spriteBackground;

    [Header("VERSION")]
    [SerializeField] private string _versionString;
    [SerializeField] private GameObject _versionObject;

    private void Awake()
    {
        if(_stringLoadBackground != null)
        {
            SceneManager.LoadScene(_stringLoadBackground, LoadSceneMode.Additive);
        }
        else if (_spriteBackground != null)
        {
            Image imageBackground = _panelBackground.GetComponent<Image>();

            imageBackground.sprite = _spriteBackground;
            imageBackground.color = new Color(255, 255, 255, 255);
        }
        else
        {
            Debug.LogWarning("Aucun fond referencer");
        }
    }

    private void Start()
    {
        EnableInputManager();

        _canvasMenu.renderMode = RenderMode.ScreenSpaceCamera;
        _canvasMenu.worldCamera = Camera.current;

        _buttonPlay.onClick.AddListener(() => goToScene(scenePlay));
        _buttonTutorial.onClick.AddListener(() => SwitchPanel(_panelTutorial));
        _buttonQuit.onClick.AddListener(() => QuitGame());

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

    /// <summary>
    /// Met a jour la version du jeu sur le menu par rapport aux parametres utilisateur unity "Application.version"
    /// </summary>
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
