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
    [SerializeField] private GameObject _prefabBody;
    [SerializeField] private List<DataCosmetic> _tabCosmetic;
    [SerializeField] private List<DataAccessory> _tabAccessory;

    [Header("STATS")]
    [SerializeField] private int _health;
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
