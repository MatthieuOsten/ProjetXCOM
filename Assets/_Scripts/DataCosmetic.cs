using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCosmetic", menuName = "ScriptableObjects/Cosmetic", order = 5)]
[System.Serializable]
public class DataCosmetic : Data
{
    [Header("RENDER")]
    [SerializeField] private GameObject prefabObject;
}
