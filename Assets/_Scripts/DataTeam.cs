using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum UserType
{
    Player,
    Bot
}

[CreateAssetMenu(fileName = "NewTeam", menuName = "ScriptableObjects/Team", order = 3)]
[System.Serializable]
public class DataTeam : Data
{
    [Tooltip("Correspond à la composition de l'équipe")]
    public DataCharacter[] SquadComposition;
    [Tooltip("Permet de dire si c'est un joueur ou alors un bot qui controle la team")]
    public UserType userType;
    
    
    // Permet l'affichage de l'objet et de ces parametres
    

    public void OnValidate() {
        
    }

}
