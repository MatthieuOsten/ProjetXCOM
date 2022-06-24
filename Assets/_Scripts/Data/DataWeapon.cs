using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;


public enum RangeType
{
    Simple,
    Radius
}


[System.Serializable]
public struct Range
{
    [Range(0, 10)]
    [SerializeField] int right, diagonal;
    public int RightRange { get { return right; } set { right = value; } }
    public int DiagonalRange { get { return diagonal; } set { diagonal = value; } }
    public RangeType type;
    public Material caseRange;
    public Material casePreviewRange;

}


[CreateAssetMenu(fileName = "DATA_Weapon_", menuName = "Data/Weapon", order = 3)]
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
    //[SerializeField] private float _accuracy;
    [Range(1, 10)]
    [SerializeField] private int _costPoint = 1;
    [SerializeField] public Range _range;
    [SerializeField] private int _maxAmmo;
    public int Cooldown = 1;

    [Header("ANIMATION")]

    [SerializeField] private Animation _animIdle;
    [SerializeField] private Animation _animFire;

    [Header("SOUND")]

    [SerializeField] private string _soundFire;
    [SerializeField] private string _soundReload;

    public typeWeapon TypeW { get { return _typeW; } }
    public Range Range { get { return _range; } }
    public int Damage { get {return _damage;} }
    //public float Accuracy { get { return _accuracy; } }
    /// <summary> Le nombre de point que l'arme va utiliser lorsqu'elle est utilisé, que ce soit pour une attaque ou une compétence</summary>
    public int CostPoint { get { return _costPoint; } }
    /// <summary> Le nombre max de munitions que l'arme possède</summary>
    public int MaxAmmo { get { return _maxAmmo; } }

    public Animation AnimIdle { get { return _animIdle; } }
    public Animation AnimFire { get { return _animFire; } }
    public string SoundFire { get { return _soundFire; } }
    public string SoundReload { get { return _soundReload; } }

    [Header("FX")]
    public GameObject fxImpact;
    public GameObject fxProjectileTrail;
    public GameObject fxMuzzle; 

    private void OnEnable() 
    {
      // On init les case preview des action avec moins d'opacité
        // foreach(DataWeapon weapon in Weapons)
        // {
            if(Range.caseRange != null)
            {
                _range.casePreviewRange = new Material(Range.caseRange);
                Color _color = _range.caseRange.GetColor("_EmissiveColor");
                _range.casePreviewRange.SetColor("_EmissiveColor", _color * 0.25f); 
            }
            
        // }
    }
}