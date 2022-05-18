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
    

    void SpawnSquad();
}
