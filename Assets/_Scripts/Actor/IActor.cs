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
    /// <summary> Correspond � la case ou se trouve l'actor </summary>
    [SerializeField] Case CurrentCase { get; }
    /// <summary> Correspond � l'�tat de l'actor lorsqu'il est instanci� </summary>
    [SerializeField] ActorState State { get; set; }
    int Health { get;  }
    
    /// <summary> Le fonction Death executer par l'actor lorsqu'il meurt </summary>
    void Death();
    /// <summary> Cette fonction applique des d�gats � l'actor lui m�me </summary>
    void DoDamage(int amount);
    /// <summary> Cette fonction lance une attaque sur le actor target  </summary>
    void Attack(Actor target);
    /// <summary> Cette fonction active la premiere comp�tence de l'actor  </summary>
    void EnableAbility(Actor target);
    /// <summary> Cette fonction active la seconde comp�tence de l'actor </summary>
    void EnableAbilityAlt(Actor target);
    /// <summary> Cette fonction donne la zone d'attack de l'actor </summary>
    Case[] AttackRange();
}
