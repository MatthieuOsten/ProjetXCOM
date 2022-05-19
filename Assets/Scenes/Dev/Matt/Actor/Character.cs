using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Actor
{
    [SerializeField] private DataCharacter _data;

    public DataCharacter Data { get { return _data; } }

    public override int Health { 
        
        get { return base.Health; } 
        
        protected set {

            // Empeche la vie de monter au dessus du maximum
            if (value > Data.Health)
            {
                value = Data.Health;
            }

            base.Health = value; 
        } 
    
    }

    // Effectue une action a la mort du personnage //
    public override void Dead()
    {
        if (State == ActorState.Dead)
        {
            
        }
    }

    // Effectue une action a lorsque le personnage prend des degats //
    public override void DoDamage(int amount)
    {
        Data.Health -= amount;

        // Si la vie du personnage est a zéro, alors il effectue son action de mort
        if (Data.Health == 0)
        {
            State = ActorState.Dead;
            Dead();
        }
    }

    // Deplace le personnage d'un point A a un point B
    public virtual void Movement(Vector2 destination)
    {
        
    }
}