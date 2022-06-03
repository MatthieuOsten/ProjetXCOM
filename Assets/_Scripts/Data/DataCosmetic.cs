using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DATA_Cosmetic_", menuName = "Data/Cosmetic", order = 5)]
[System.Serializable]
public class DataCosmetic : Data
{
    [Header("RENDER")]
    [SerializeField] private GameObject prefabObject;
}
