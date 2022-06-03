using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "DATA_Character_", menuName = "Data/Character", order = 2)]
[System.Serializable]
public class DataCharacter : Data
{
    [System.Serializable]
    public struct Capacity
    {
        public string name;
        public string description;
        public string sound;
        public Sprite icon;
        public ActionTypeMode typeA;

        public Capacity(ActionTypeMode typeMode, Data data = null)
        {
            if (data == null)
            {
                this.name = null;
                this.description = null;
                this.icon = null;
                this.sound = null;
            } 
            else
            {
                this.name = data.name;
                this.description = data.description;
                this.icon = data.icon;

                // Recupere le premier tag de la data pour definir le son
                if (data.tags != null && data.tags.Count > 0) { this.sound = data.tags[0]; } else { this.sound = null; }
            }

            this.typeA = typeMode;
        }

        public void SetName(string value) { this.name = value; }
    }

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

    [Header("CAPACITY")]
    [SerializeField] private DataWeapon _weapon;
    [SerializeField] private Data _dataOverwatch;
    [SerializeField] private Data _dataReload;

    [Range(1, 10)]
    [SerializeField] private int _actionPoints = 2;
    [SerializeField] private int _movementCasesAction = 4;


   [Header("MAIN ABILITY")]
    /// <summary> Bool pour indiquer si l'abilité principal est utilisable en jeu</summary>
    public bool AbilityAvailable = false;
    /// <summary> Nom de son abilité principal qui pourra être utiliser dans divers code </summary>
    //public string AbilityName = "";
    /// <summary> L'arme qu'on utilisera pour la première compétence </summary>
    [Tooltip("L'arme qui sera utilisé pour l'icon, le nom etc des boutons")] public DataWeapon WeaponAbility;
    /// <summary> Le material utiliser pour les case lorsqu'on utilisera la première compétence </summary>
    public Material casePreviewAbility;
    /// <summary> Délai en tour avant de pour réutiliser la compétence </summary>
    //[Range(1, 10)]
    //public int CooldownAbility = 1;
     [Header("ALT ABILITY")]
    /// <summary> Bool pour indiquer si l'abilité secondaire est utilisable en jeu</summary>
    public bool AbilityAltAvailable = false;
    /// <summary> Nom de son abilité secondaire qui pourra être utiliser dans divers code </summary>
    //public string AbilityAltName = "";
    
    /// <summary> L'arme qu'on utilisera pour la seconde compétence </summary>
    [Tooltip("L'arme qui sera utilisé pour l'icon, le nom etc des boutons")] public DataWeapon WeaponAbilityAlt;
    /// <summary> Le material utiliser pour les case lorsqu'on utilisera la seconde compétence </summary>
    public Material casePreviewAbilityAlt;
    /// <summary> Délai en tour avant de pour réutiliser la compétence </summary>
    //[Range(1, 10)]
    //public int CooldownAbilityAlt = 1;

    [Header("COST ACTION")]
    [Range(1, 10)]
    public int CostAttack = 1;
    [Range(1, 10)]
    public int CostVigilance = 1;  
    [Range(1, 10)]
    public int CostReload = 1;

    [Header("UI")]
    [SerializeField] private Sprite pointAction;
    [SerializeField] private Sprite ammo;

    public float MoveSpeed { get { return _moveSpeed; } }
    public int MovementCasesAction { get { return _movementCasesAction; } }

    public List<Capacity> ListCapacity { 
                get {
            List<Capacity> listCapacity = new List<Capacity>();

            Capacity capacity;

            if (Weapon != null) { capacity = new Capacity(ActionTypeMode.Attack, (Data)Weapon); } else { capacity = new Capacity(ActionTypeMode.Attack); }
            listCapacity.Add(capacity);

            if (_dataOverwatch != null) { capacity = new Capacity(ActionTypeMode.Overwatch, (Data)_dataOverwatch); } else { capacity = new Capacity(ActionTypeMode.Overwatch); }
            listCapacity.Add(capacity);

            if (WeaponAbility != null) { capacity = new Capacity(ActionTypeMode.Competence1, (Data)WeaponAbility); } else { capacity = new Capacity(ActionTypeMode.Competence1); }
            listCapacity.Add(capacity);

            if (WeaponAbilityAlt != null) { capacity = new Capacity(ActionTypeMode.Competence2, (Data)WeaponAbilityAlt); } else { capacity = new Capacity(ActionTypeMode.Competence2); }
            listCapacity.Add(capacity);

            if (_dataReload != null) { capacity = new Capacity(ActionTypeMode.Reload, (Data)_dataReload); } else { capacity = new Capacity(ActionTypeMode.Reload); }
            listCapacity.Add(capacity);

            return listCapacity;
        }
    }

    public Sprite PointAction
    {
        get { return pointAction; }
    }
    public Sprite Ammo
    {
        get { return ammo; }
    }

    public Color Color { get { return _color; } }

    // info : https://answers.unity.com/questions/1339301/list-of-scripts.html
    [Header("Instantiating")]
    [Tooltip("Le nom du component a ajouté sur le actor qu'on créera")]
    [SerializeField] public string ClassName;

    [SerializeField] public Material mtl_red_flick;
    

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
