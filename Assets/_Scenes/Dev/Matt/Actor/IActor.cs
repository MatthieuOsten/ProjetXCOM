using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActorState
{
    Alive,
    Overwatch,
    Dead
}

public interface IActor
{

    Case CurrentCase { get; }
    ActorState State { get; set; }
    float Health { get; set; }

    void Dead();
    void DoDamage(int amount);
    void Attack();
    void EnableAbility();

}