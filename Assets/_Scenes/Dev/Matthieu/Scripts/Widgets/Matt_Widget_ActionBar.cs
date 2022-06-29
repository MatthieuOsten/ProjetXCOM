using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Matt_Widget_ActionBar : Matt_Widgets
{
    [Header("ACTION BAR")]
    [SerializeField] private GameObject _layoutGroup;
    [SerializeField] private List<GameObject> _actionButton;

    [Header("DATA")]
    [SerializeField] private GameObject _prefabButton;
    [SerializeField] private List<DataCharacter.Capacity> _actionCapacity;
    [SerializeField] private int _difference;

    [Header("EXTENSION")]
    [SerializeField] private Matt_Widget_PopUpAction _popUp;

    [Space]
    [Header("DEBUG")]
    [SerializeField] private bool _updateActionBar;

    #if UNITY_EDITOR

    public override void SystemDebug()
        {
            base.SystemDebug();

            SystemStart();

            SystemUpdate();

        }

    #endif

    #region SYSTEM

    public override void SystemReset() {

        _popUp.HidePopUp();

    }

    public override void SystemStart()
    {
        base.SystemStart();

        ShadeBar();
        UpdateButtonInformation();
    }

    public override void SystemUpdate()
    {
        base.SystemUpdate();

        UpdateActionBar();
    }

    #endregion

    #region FUNCTION

    //gere si la barre doit etre invisible ou non
    private void ShadeBar()
    {
        // DataCharacter data = _pC.GetCurrentCharactedSelected.Data;


        if (_layoutGroup != null)
        {
            Image _barreAction = _layoutGroup.GetComponent<Image>();

            List<Image> children = new List<Image>();

            foreach (var button in _actionButton)
            {
                children.Add(button.GetComponent<Image>());
            }


            //Si pas en mode action quasi invisible
            if (hudManager.Character == null)
            {
                _layoutGroup.gameObject.SetActive(false); // desactive la barre d'action

                _popUp.HidePopUp();
            }
            //si en mode action apparente
            else
            {
                _barreAction.color = new Color(_barreAction.color.r, _barreAction.color.g, _barreAction.color.b, 0f);
                foreach (Image image in children)
                {
                    image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);
                }
                _layoutGroup.gameObject.SetActive(true);  // reactive la barre d'action

            }

        }

    }

    private void UpdateButtonInformation()
    {
        if (hudManager.Character == null) return;

        // ---- Initialise chaque bouttons en rapport avec les capacités actuel ---- //
        for (int i = 0; i < _actionButton.Count; i++)
        {

            GameObject button = _actionButton[i].transform.Find("Button").gameObject;

            if (_actionCapacity.Count < i) { break; }

            // Nettoie la liste d'action du boutton
            button.GetComponent<Button>().onClick.RemoveAllListeners();

            // -- Initialise les données du boutton initialiser -- //

            // Insert l'action effectuer si le boutton est appuyer
            int index = i;
            button.GetComponent<Button>().onClick.AddListener(() => hudManager.SetActionMode(_actionCapacity[index].typeA, _actionCapacity[index].sound));


            // // Verifie que l'objet a une icone et l'affiche
            // if (_actionCapacity[i].icon != null)
            // {
            //     _actionButton[i].GetComponent<Image>().sprite = _actionCapacity[i].icon;
            // }

            if (_actionCapacity[index].typeA == ActionTypeMode.Reload)
            {
                WatchReloadButton(button);
            }

            if (_actionCapacity[index].typeA == ActionTypeMode.Competence1)
            {
                AdaptIcon(button, hudManager.Character.GetCurrentAbilityCooldown);
            }

            if (_actionCapacity[index].typeA == ActionTypeMode.Competence2)
            {
                AdaptIcon(button, hudManager.Character.GetCurrentAbilityAltCooldown);
            }


            for (int j = 0; j < button.transform.childCount; j++)
            {
                if (button.transform.GetChild(j).TryGetComponent<Image>(out Image jimage))
                {
                    jimage.sprite = _actionCapacity[i].icon;
                }
            }

            // Verifie que l'objet a un nom et l'ecrit
            if (_actionCapacity[i].name != null)
                button.transform.Find("Title").GetComponent<TextMeshProUGUI>().text = _actionCapacity[i].name;

            // -- Initialise "DisplayDescription" sur le boutton -- //
            if (_actionCapacity[i].description != null && _popUp != null) // Verifie que la description est remplie
            {
                // Recupere le "EventTrigger" du boutton
                EventTrigger eventTrigger;

                if (button.TryGetComponent<EventTrigger>(out eventTrigger))
                {
                    eventTrigger.triggers.Clear();

                    // Initialise un event "EventTrigger"
                    EventTrigger.Entry onSelected = new EventTrigger.Entry();
                    // Nettoie la liste d'evenement
                    onSelected.callback.RemoveAllListeners();
                    // Le met en mode "UpdateSelected" afin de detecter lorsque la souris est sur le boutton
                    onSelected.eventID = EventTriggerType.PointerEnter;
                    // Insert dans sa liste de reaction, l'affichage de la pop-up de description
                    int indexTrigger = i;
                    onSelected.callback.AddListener((eventData) => { _popUp.DisplayPopUp(_actionCapacity[indexTrigger], _actionButton[indexTrigger].transform.position, _actionCapacity[indexTrigger].description, _actionCapacity[indexTrigger].name); });


                    // Ajoute le composant et ces parametres dans le boutton
                    eventTrigger.triggers.Add(onSelected);



                    // Initialise un event "EventTrigger"
                    EventTrigger.Entry onDeselected = new EventTrigger.Entry();
                    // Nettoie la liste d'evenement
                    onDeselected.callback.RemoveAllListeners();
                    // Le met en mode "UpdateSelected" afin de detecter lorsque la souris est sur le boutton
                    onDeselected.eventID = EventTriggerType.PointerExit;
                    // Insert dans sa liste de reaction, l'affichage de la pop-up de description
                    onDeselected.callback.AddListener((eventData) => { _popUp.HidePopUp(); });
                    // Ajoute le composant et ces parametres dans le boutton
                    eventTrigger.triggers.Add(onDeselected);
                }

            }

        }
    }

    /// <summary>
    /// Met a jour la barre d'action, ses boutton par rapport a l'actuel "Actor" selectionner
    /// </summary>
    private void UpdateActionBar()
    {
        if (hudManager.Character != null)
        {
            _layoutGroup.SetActive(true);

            // Recupere les données du personnage actuellement selectionner
            DataCharacter data = GetData();

            _actionCapacity.Clear();
            _actionCapacity = data.ListCapacity;

            // Nettoie la liste des actions
            if (_actionCapacity.Count > 0)
            {
                _actionCapacity.RemoveAll(item => item.name == null);
            }

            // Verifie si il contient assez de bouton comparer au nombre de capacité du personnage
            if (_actionButton.Count != _actionCapacity.Count)
            {

                // Si il y a moins de boutons que de capacités alors rajoute des boutons
                if (_actionButton.Count < _actionCapacity.Count)
                {
                    Debug.Log("on rajoute Start " + _actionButton.Count + " End " + _actionCapacity.Count);

                    InstantiateButton(_actionButton.Count, _actionCapacity.Count, _prefabButton, _layoutGroup);
                }
                // Supprime les bouttons en trop
                else if (_actionButton.Count > _actionCapacity.Count)
                {

                    // Detruit chaque bouttons un par un
                    foreach (var button in _actionButton)
                    {
                        //Debug.Log("Boutton : " + button.name + " " + button.GetInstanceID() + " Supprimer");
                        Destroy(button);

                    }
                    _actionButton.Clear();
                    Debug.Log("supprime les bouttons en trop  Start " + _actionButton.Count + " End " + _actionCapacity.Count);
                    InstantiateButton(_actionButton.Count, _actionCapacity.Count, _prefabButton, _layoutGroup);

                }

                // Verifie que les donnée soit bien initialiser avant de procedé
                if (data != null && _actionCapacity.Count > 0 && _actionButton.Count > 0)
                {
                    UpdateButtonInformation();
                }

            }

        }

    }

    /// <summary>
    /// Cree un nombre d'objet definit avec un for, a la position de son parent
    /// </summary>
    /// <param name="start">Nombre d'objet deja initialiser</param>
    /// <param name="end">Nombre total d'objet voulu</param>
    /// <param name="prefab">Prefab de l'objet</param>
    /// <param name="parent">Parent de l'objet instancier</param>
    private void InstantiateButton(int start, int end, GameObject prefab, GameObject parent)
    {
        // Initialise des bouttons en plus si besoin
        for (int i = start; i < end; i++)
        {
            _actionButton.Add(Instantiate(prefab, parent.transform.position, Quaternion.identity, parent.transform));
        }
    }

    /// <summary>
    /// Recupere les donnees d'un personnage "DataCharacter" 
    /// </summary>
    private DataCharacter GetData()
    {
        DataCharacter data;

        if (hudManager.ActualPlayer != null)
        {
            // Recupere la base de donnee du personnage selectionner
            data = hudManager.ActualPlayer.CharacterPlayer[hudManager.ActualPlayer.CharacterIndex].GetComponent<Character>().Data;
            return data;
        }
        else
        {

            if (hudManager.ActualCharacter != null)
            {
                data = hudManager.ActualCharacter;
                return data;
            }

            return new DataCharacter();
        }

    }


    /// <summary>
    /// Chech si je dois recharger
    /// </summary>
    /// <param name="_reload"></param>
    void WatchReloadButton(GameObject _reload)
    {
        Color colorReload = _reload.GetComponent<Image>().color;

        //si doit recharger
        if (hudManager.Character.GetWeaponCurrentAmmo() < hudManager.Character.GetWeaponCapacityAmmo())
        {
            colorReload.a = 1f;
            _reload.GetComponent<Image>().color = colorReload;
            _reload.GetComponent<Button>().interactable = true;
            _reload.GetComponentInChildren<ButtonAction>().ICONE.color = colorReload;
            // Debug.Log("marche");
        }


        else
        {
            colorReload.a = 0.5f;
            _reload.GetComponent<Image>().color = colorReload;
            _reload.GetComponent<Button>().interactable = false;
            _reload.GetComponentInChildren<ButtonAction>().ICONE.color = colorReload;
        }

    }

    /// <summary>
    /// Met le bouton requis pour la competence
    /// </summary>
    /// <param name="competenceInstance"></param>
    private void AdaptIcon(GameObject competenceInstance, int _currentCooldown)
    {
        Color colorCompetence1 = competenceInstance.GetComponent<Image>().color;
        ButtonAction buttonAction = competenceInstance.GetComponentInChildren<ButtonAction>();
        Image image = competenceInstance.GetComponent<Image>();
        Button button = competenceInstance.GetComponent<Button>();

        if (_currentCooldown == 0)
        {
            colorCompetence1.a = 1f;
            button.interactable = true;
            image.color = colorCompetence1;
            buttonAction.Cooldown.text = string.Empty;
            buttonAction.ICONE.color = colorCompetence1;
        }

        else
        {
            colorCompetence1.a = 0.5f;
            image.color = colorCompetence1;
            button.interactable = false;
            buttonAction.Cooldown.text = Mathf.RoundToInt(_currentCooldown).ToString();
            buttonAction.ICONE.color = colorCompetence1;
        }
    }


    #endregion

}
