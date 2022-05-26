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
    [Tooltip("Correspond à la case ou il se trouve")]
    [SerializeField] Case CurrentCase { get; }
     ActorState State { get; set; }
    int Health { get;  }
    //Range Range {get;}

    void Death();
    void DoDamage(int amount);
    void Attack(Actor target);
    void EnableAbility();
    Case[] AttackRange();
}
