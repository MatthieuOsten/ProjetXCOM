using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITeam 
{
    string Name{get; set;}

     //ITeam[] Team{get; set;}

    ActorTest[] Squad{get; set;}
    bool ItsYourTurn{get; set;}
    /*
         static ITeam[] allTeams;

     IActor[] Squad;
     bool ItsYourTurn;
    */
    
    /// <summary>
    /// Spawn l'escouade de la team sur la grille
    /// </summary>
    void SpawnSquad();
    /// <summary>
    /// Spawn le personnage
    /// </summary>
    void SpawnActor(ActorTest actor);

}
