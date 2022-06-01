using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "DATA_char_", menuName = "ScriptableObjects/Character", order = 2)]
[System.Serializable]
public class DataCharacter : Data
{
    // Permet l'affichage de l'objet et de ces parametres
    [Header("RENDER")]
    [SerializeField] public GameObject _prefabBody;
    [SerializeField] private List<DataCosmetic> _tabCosmetic;
    [SerializeField] private List<DataAccessory> _tabAccessory;
    [SerializeField] Color _color;

    [Header("STATS")]
    [SerializeField] private int _health = 1; // Vie quand le personnage spawn
    [SerializeField] private int _shield;
    [Range(1f,20)]
    [SerializeField] float _moveSpeed = 10;
    public float MoveSpeed{ get { return _moveSpeed; }}


    [Header("CAPACITY")]
    [SerializeField] private DataWeapon _weapon;
    [Range(1, 10)]
    [SerializeField] private int _actionPoints = 2;
    [SerializeField] private int _movementCasesAction = 4;
    public int MovementCasesAction{ get { return _movementCasesAction; }}

   [Header("MAIN ABILITY")]
    /// <summary> Bool pour indiquer si l'abilité principal est utilisable en jeu</summary>
    public bool AbilityAvailable = false;
    [Range(1, 10)]
    public int CostCompetence = 1;
    /// <summary> Nom de son abilité principal qui pourra être utiliser dans divers code </summary>
    public string AbilityName = "";
    /// <summary> L'arme qu'on utilisera pour la première compétence </summary>
    public DataWeapon WeaponAbility;
    /// <summary> Le material utiliser pour les case lorsqu'on utilisera la première compétence </summary>
    public Material casePreviewAbility;
     [Header("ALT ABILITY")]
    /// <summary> Bool pour indiquer si l'abilité secondaire est utilisable en jeu</summary>
    public bool AbilityAltAvailable = false;
      [Range(1, 10)]
    public int CostCompetenceAlt = 1;
    /// <summary> Nom de son abilité secondaire qui pourra être utiliser dans divers code </summary>
    public string AbilityAltName = "";
    /// <summary> L'arme qu'on utilisera pour la seconde compétence </summary>
    public DataWeapon WeaponAbilityAlt;
    /// <summary> Le material utiliser pour les case lorsqu'on utilisera la seconde compétence </summary>
    public Material casePreviewAbilityAlt;


    [Header("COST ACTION")]
    [Range(1, 10)]
    public int CostAttack = 1;
    [Range(1, 10)]
    public int CostVigilance = 1;  
    [Range(1, 10)]
    public int CostReload = 1;

    [Header("UI")]
    [SerializeField] private Sprite spriteTir;
    [SerializeField] private Sprite spriteVigilance;
    [SerializeField] private Sprite spriteCompetence;
    [SerializeField] private Sprite spriteCompetence2; 
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

    public Color Color { get { return _color; } }
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

    public DataWeapon Weapon { get { return _weapon; } }

}
