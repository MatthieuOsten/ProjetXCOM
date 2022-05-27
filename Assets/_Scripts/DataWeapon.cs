using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "ScriptableObjects/Weapon", order = 3)]
[System.Serializable]
public class DataWeapon : Data
{
    public enum typeWeapon
    {
        melee,
        distance,
        grenade
    }

    [Header("RENDER")]
    [SerializeField] private GameObject _prefabWeapon;
    [SerializeField] private List<DataCosmetic> _tabCosmetic;
    [SerializeField] private List<DataAccessory> _tabAccessory;

    [Header("STATISTIQUE")]
    [SerializeField] private typeWeapon _typeW;
    [SerializeField] private int _damage;
    [SerializeField] private float _accuracy;
    [SerializeField] private int _costPoint;
    [SerializeField] public Range _range;

    [Header("ANIMATION")]

    [SerializeField] private Animation _animIdle;
    [SerializeField] private Animation _animFire;

    [Header("SOUND")]

    [SerializeField] private AudioSource _soundFire;
    [SerializeField] private AudioSource _soundReload;

    public typeWeapon TypeW { get { return _typeW; } }
    public Range Range { get { return _range; } }
    public int Damage { get {return _damage;} }
    public float Accuracy { get { return _accuracy; } }
    public float CostPoint { get { return _costPoint; } }

    public Animation AnimIdle { get { return _animIdle; } }
    public Animation AnimFire { get { return _animFire; } }
    public AudioSource SoundFire { get { return _soundFire; } }
    public AudioSource SoundReload { get { return _soundReload; } }

    

}