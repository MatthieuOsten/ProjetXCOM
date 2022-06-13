using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProtoMenuManager : MonoBehaviour
{
    [SerializeField] private Controller _inputManager;
    [SerializeField] private GameObject _panelTutorial;
    private void Start()
    {
        EnableInputManager();
    }

    private void Update()
    {
        if (_inputManager != null)
        {
                if (_inputManager.System.Exit.WasPressedThisFrame())
            {
                if (_panelTutorial.activeSelf) { SwitchTutorial(); } else { Application.Quit(); }
            }
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
    public void SwitchTutorial()
    {
        if (_panelTutorial == null) { return; }

        if (_panelTutorial.activeSelf)
            { _panelTutorial.SetActive(false); } 
        else 
            { _panelTutorial.SetActive(true); }

    }
}