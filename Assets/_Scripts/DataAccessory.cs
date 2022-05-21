using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAccessory", menuName = "ScriptableObjects/Accessory", order = 4)]
[System.Serializable]
public class DataAccessory : Data
{
    [Header("RENDER")]
    [SerializeField] private GameObject prefabObject;
}
