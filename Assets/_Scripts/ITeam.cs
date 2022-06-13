using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITeam 
{
    string Name{get; set;}
    Actor[] Squad{get; set;}
    /// <summary> Indique si la team peut jouer ou non </summary>
    bool CanPlay{get; set;}
    /// <summary> Spawn l'escouade de la team sur la grille </summary>
    void SpawnSquad();
    /// <summary> Spawn le personnage � partir des DataCharacter </summary>
    Character SpawnActor(DataCharacter actor, Case spawnCase);

    /// <summary> Cette fonction se lance quand c'est le tour de la team </summary>
    void StartTurn();

}
