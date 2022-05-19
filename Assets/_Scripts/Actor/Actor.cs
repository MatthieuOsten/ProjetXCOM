using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Actor : MonoBehaviour, IActor
{
    Case currentCase;
    ActorState state = ActorState.Alive;
    private int _health;

    public virtual Case CurrentCase { get { return currentCase; } }

    public virtual ActorState State { get { return state; } set { state = value; } }

    

    public virtual int Health { get { return _health; } 
        
        protected set {

            // Empeche la valeur d'aller en dessous de zero -- //
            if (value > 0)
            {
                _health = value;
            }

            if (value < 0 || _health == 0)
            {
                _health = 0;
                State = ActorState.Dead;
            }

        } 
    }

    public virtual  void Update() {
        
    }
    public virtual  void Start() {
        
    }



    public virtual void AttackRange()
    {}

    public virtual void Death()
    {
        throw new System.NotImplementedException();
    }

    public virtual void DoDamage(int amount)
    {
        Health -= amount;
        throw new System.NotImplementedException();
    }

    public virtual void Attack()
    {
        throw new System.NotImplementedException();
    }

    public virtual void EnableAbility()
    {
        throw new System.NotImplementedException();
    }
}