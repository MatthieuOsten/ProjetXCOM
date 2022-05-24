using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NewCharacter", menuName = "ScriptableObjects/Character", order = 2)]
[System.Serializable]
public class DataCharacter : Data
{
    // Permet l'affichage de l'objet et de ces parametres
    [Header("RENDER")]
    [SerializeField] public GameObject _prefabBody;
    [SerializeField] private List<DataCosmetic> _tabCosmetic;
    [SerializeField] private List<DataAccessory> _tabAccessory;

    [Header("STATS")]
    [SerializeField] private int _health = 1; // Vie quand le personnage spawn
    [SerializeField] private int _shield;

    [Header("CAPACITY")]
    [SerializeField] private List<DataWeapon> _weapons;
    [SerializeField] private int _actionPoints;

    [Header("UI")]
    [SerializeField] private Sprite spriteTir;
    [SerializeField] private Sprite spriteVigilance;
    [SerializeField] private Sprite spriteCompetence;
    [SerializeField] private Sprite spriteCompetence2;
    [SerializeField] private Sprite icone;
    [SerializeField] private Sprite pointAction;
    [SerializeField] private Sprite ammo;

    public Sprite PointAction
    {
        get { return pointAction; }
    }
    public Sprite Ammo
    {
        get { return ammo; }
    }
    public Sprite Icone
    {
        get { return icone; }
    }
    public Sprite SpriteCompetence2
    {
        get { return spriteCompetence2; }
    }
    public Sprite SpriteCompetence
    {
        get { return spriteCompetence; }
    }
    public Sprite SpriteVigilance
    {
        get { return spriteVigilance; }
    }
    public Sprite SpriteTir
    {
        get { return spriteTir; }
	}
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
