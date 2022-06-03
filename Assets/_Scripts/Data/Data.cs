using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DATA_", menuName = "Data/Data", order = 1)]
[System.Serializable]
public class Data : ScriptableObject
{
    // -- Permet de decrire l'objet -- //
    [Header("DESCRIPTION")]
    public new string name;
    public string description;
    public string type;
    public List<string> tags;

    // -- represente le type de l'objet -- //
    [Header("RENDER")]
    public Sprite icon;
}