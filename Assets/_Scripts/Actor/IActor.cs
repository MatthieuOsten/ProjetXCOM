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
    [SerializeField] Case CurrentCase { get; }
     ActorState State { get; set; }
    int Health { get;  }

    void Death();
    void DoDamage(int amount);
    void Attack();
    void EnableAbility();
    void AttackRange();
}
