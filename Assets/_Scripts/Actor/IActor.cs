using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActorState
{
    Alive,
    Overwatch,
    Dead
}
// Linterface dit ce que fait la class
public interface IActor
{
    //[SerializeField] Case CurrentCase { get; set;}
    float Health{get;}
    ActorState State{get;}

    void Death();
    void DoDamage(int damage);
    void Attack();
    void EnableAbility();
}
