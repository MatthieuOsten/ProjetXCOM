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
    [SerializeField] private int _actualHealth;

    [SerializeField] private int _shield;
    [SerializeField] private int _actualShield;

    [Header("CAPACITY")]
    [SerializeField] private int _actionPoints;

    [Header("STATUT")]
    [SerializeField] private bool _isDead;

    public int ActionPoints
    {
        get { return _actionPoints; }
        protected set { _actionPoints = value; }
    }

    public int Health
    {
        get { return _health; }
        set { _health = value; }
    }
    public int ActualHealth
    {
        get { return _actualHealth; }
        set
        {
            // Empeche la valeur d'aller en dessous de zero ou d'etre superieur a la valeur maximum -- //
            if (value > 0 && value < Shield)
            {
                _actualShield = value;
            }
            else if (value > Shield)
            {
                _actualShield = Shield;
            }

            if (value < 0 || _health == 0)
            {
                _actualShield = 0;
                _isDead = true;
            }
        }
    }

    public int Shield
    {
        get { return _shield; }
        set { _shield = value; }
    }

    public int ActualShield
    {
        get { return _actualShield; }
        set
        {
            // Empeche la valeur d'aller en dessous de zero ou d'etre superieur a la valeur maximum -- //
            if (value > 0 && value < Shield)
            {
                _actualShield = value;
            }
            else if (value > Shield)
            {
                _actualShield = Shield;
            }
            else
            {
                _actualShield = 0;
            }

        }
    }

    public bool IsDead
    {
        get { return _isDead; }
        set { _isDead = value; }
    }

    // Met a zero les données
    protected void InitializeData()
    {
        IsDead = false;
        ActualHealth = Health;
        ActualShield = Shield;
    }

}
