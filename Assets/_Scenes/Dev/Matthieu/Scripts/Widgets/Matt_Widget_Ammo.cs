using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(HorizontalLayoutGroup))]
public class Matt_Widget_Ammo : Matt_Widgets
{

    #if UNITY_EDITOR
    
    public override void SystemDebug()
    {
        base.SystemDebug();

        SystemStart();

        SystemUpdate();

    }
    
    #endif

    [Header("AMMO")]
    [SerializeField] private GameObject _prefabAmmo;
    [SerializeField] private List<Transform> _actualAmmo;

    [Header("NUMBER")]
    [SerializeField] private int _widgetAmmoMax = 0;
    [SerializeField] private int _ammoMax = 0, _ammoActual = 0;

    [Header("ANIMATION")]
    [SerializeField] private Animator _animationAmmo;
    [SerializeField] private string _animOn, _animActual;

    public override void SystemStart() {

        base.SystemStart();

        GetWidgetAmmoMax();

        if (_actualAmmo.Count == 0 && transform.childCount == 0)
            _actualAmmo = InitialiseAmmo(_prefabAmmo, _widgetAmmoMax, "Ammo");
        else if (_actualAmmo.Count != _widgetAmmoMax && transform.childCount > 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }

            for (int i = 0; i < _actualAmmo.Count; i++)
            {
                DestroyImmediate(_actualAmmo[i].gameObject);
            }

            _actualAmmo.Clear();

            _actualAmmo = InitialiseAmmo(_prefabAmmo, _widgetAmmoMax, "Ammo");

        } 
        else if (_actualAmmo.Count != _widgetAmmoMax)
        {
            for (int i = 0; i < _actualAmmo.Count; i++)
            {
                DestroyImmediate(_actualAmmo[i].gameObject);
            }

            _actualAmmo.Clear();

            _actualAmmo = InitialiseAmmo(_prefabAmmo, _widgetAmmoMax, "Ammo");

        } else if (transform.childCount > 0) {

            for (int i = 0; i < transform.childCount; i++)
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }

            _actualAmmo = InitialiseAmmo(_prefabAmmo, _widgetAmmoMax, "Ammo");
        }

    }

    public override void SystemUpdate() {

        if (hudManager != null)
        {
            // Si un personnage est selectionner et qu'il a une arme alors active le widget et le met a jour
            if (hudManager.ActualCharacter != null && hudManager.ActualCharacter.Weapon != null)
            {

                _ammoMax = hudManager.ActualPlayer.GetCurrentCharactedSelected.GetMainWeaponInfo().MaxAmmo;
                _ammoActual = hudManager.ActualPlayer.GetCurrentCharactedSelected.Ammo[0];

                // Si le personnage a des munitions 
                if (_ammoMax > 0)
                {
                    gameObject.SetActive(true);

                    SetDisplayAmmo();

                } else { gameObject.SetActive(false); }

            }
            else { gameObject.SetActive(false); }

        } else { gameObject.SetActive(false); }
    
    }

    /// <summary>
    /// Met a jour les munitions pour les afficher ou cacher
    /// </summary>
    private void SetDisplayAmmo()
    {
        for (int i = 0; i < _actualAmmo.Count; i++)
        {
            // Recupere l'animator de la munition actuellement verifier
            _animationAmmo = _actualAmmo[i].GetComponent<Animator>();

            // Si les munition sont en dessous du nombre maximum les active sinon desactive les munitions en trop
            if (i < _ammoMax) { _actualAmmo[i].gameObject.SetActive(true); }
            else { _actualAmmo[i].gameObject.SetActive(false); }

            // Si les munitions sont en dessous du nombre actuel allume la munition
            if (i < _ammoActual)
            {
                if (_animationAmmo.GetBool(_animOn) == false)
                    _animationAmmo.SetBool(_animOn, true);

                if (_animationAmmo.GetBool(_animActual) == true)
                    _animationAmmo.SetBool(_animActual, false);
            }
            // Si la munition est la derniere munition active l'animation de la munition actuel
            else if (i == _ammoActual)
            {
                if (_animationAmmo.GetBool(_animOn) == false)
                    _animationAmmo.SetBool(_animOn, true);

                if (_animationAmmo.GetBool(_animActual) == false)
                    _animationAmmo.SetBool(_animActual, true);
            }
            // Si la munition est au dessus de la munition actuel eteint la munition
            else
            {
                if (_animationAmmo.GetBool(_animOn) == true)
                    _animationAmmo.SetBool(_animOn, false);

                if (_animationAmmo.GetBool(_animActual) == true)
                    _animationAmmo.SetBool(_animActual, false);
            }
        }
    }

    /// <summary>
    /// Met a jour la variable "_widgetAmmoMax" en recuperant la plus haute 
    /// </summary>
    private void GetWidgetAmmoMax()
    {
        if (FindObjectOfType<Matt_LevelManager>() != null)
        {
            foreach (var item in Matt_LevelManager.listTeam)
            {
                foreach (var itemSquad in item.Data.SquadComposition)
                {
                    if (_widgetAmmoMax < itemSquad.Weapon.MaxAmmo)
                    {
                        _widgetAmmoMax = itemSquad.Weapon.MaxAmmo;
                    }

                }

            }
        }

    }

    /// <summary>
    /// Genere les munitions 
    /// </summary>
    /// <param name="prefab">Prefab</param>
    /// <param name="nbrAmmo">Nombre de munition a generer</param>
    /// <param name="name">Nom des objets</param>
    /// <returns></returns>
    private List<Transform> InitialiseAmmo(GameObject prefab, int nbrAmmo = 0, string name = "Object")
    {
        List<Transform> actualInstance = new List<Transform>();

        if (prefab != null & nbrAmmo > 0) {

            for (int i = 0; i < nbrAmmo; i++)
            {
                // Initialise les munitions a l'ecran
                GameObject ammo;
                ammo = Instantiate<GameObject>(prefab,transform);

                // Si le nom n'est pas definit alors laisse le nom de la prefab
                if (name == "Object")
                    ammo.name += (i + 1);
                else
                    ammo.name = name + (i + 1);

                actualInstance.Add(ammo.transform);
            }

        }

        return actualInstance;
    }

}