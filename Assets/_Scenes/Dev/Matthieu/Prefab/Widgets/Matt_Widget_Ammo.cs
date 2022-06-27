using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(HorizontalLayoutGroup))]
public class Matt_Widget_Ammo : Matt_Widgets
{

    [Header("AMMO")]
    private GameObject _prefabAmmo;
    private List<Transform> _actualAmmo;

    [Header("NUMBER")]
    private int _widgetAmmoMax = 0;
    private int _ammoMax = 0, _ammoActual = 0;

    [Header("ANIMATION")]
    private Animator _animationAmmo;
    private string _animOff, _animOn, _animActual;

    public void SystemStart() {

        GetWidgetAmmoMax();

        _actualAmmo = InitialiseAmmo(_prefabAmmo, _ammoMax, "Ammo");

    }

    public void SystemUpdate() { 
    
        
    
    }

    /// <summary>
    /// Met a jour la variable "_widgetAmmoMax" en recuperant la plus haute 
    /// </summary>
    private void GetWidgetAmmoMax()
    {
        foreach (var item in LevelManager.listTeam)
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