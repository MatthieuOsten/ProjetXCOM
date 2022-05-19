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
