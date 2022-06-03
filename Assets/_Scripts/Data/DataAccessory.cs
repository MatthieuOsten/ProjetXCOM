using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DATA_Accessory_", menuName = "Data/Accessory", order = 4)]
[System.Serializable]
public class DataAccessory : Data
{
    [Header("RENDER")]
    [SerializeField] private GameObject prefabObject;
}
