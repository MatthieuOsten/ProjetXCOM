using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacter", menuName = "ScriptableObjects/Character", order = 2)]
[System.Serializable]
public class DataCharacter : Data
{
    // Permet l'affichage de l'objet et de ces parametres
    [Header("RENDER")]
    [SerializeField] private GameObject _prefabBody;
    [SerializeField] private List<DataCosmetic> _tabCosmetic;
    [SerializeField] private List<DataAccessory> _tabAccessory;

    [Header("STATS")]
    [SerializeField] private int _health;
    [SerializeField] private int _shield;

    [Header("CAPACITY")]
    [SerializeField] private List<DataWeapon> _weapons;
    [SerializeField] private int _actionPoints;


    // info : https://answers.unity.com/questions/1339301/list-of-scripts.html
    [Header("Instantiating")]
    [Tooltip("Le nom du component a ajouté sur le actor qu'on créera")]
    [SerializeField] public string ClassName;

    public void OnValidate() {
        if(ClassName == null || ClassName == "")
            Debug.LogError($"Attention ClassName n'est pas défini, il est nécessaire de lui associer un component sinon l'actor ne pourra pas être spawn");
        
        Type abilityType = Type.GetType(ClassName);
        if(abilityType == null) Debug.LogError($"Attention ClassName indique une class qui n'est pas valid, une class valide est nécessaire sinon l'actor ne pourra pas être spawn");

    }

    public int ActionPoints
    {
        get { return _actionPoints; }
    }

    public int Health
    {
        get { return _health; }
    }

    public int Shield
    {
        get { return _shield; }
    }

    public List<DataWeapon> weapons { get { return _weapons; } }

}
