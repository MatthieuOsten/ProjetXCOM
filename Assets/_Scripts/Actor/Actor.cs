using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RangeType
{
    Simple,
    Radius
}

[System.Serializable]
public struct Range
{   
    [Range(0,10)]
    [SerializeField] int right, diagonal;
    public int rightRange{get { return right; }  set{ right = value;} }
    public int diagonalRange{get { return diagonal; }  set{ diagonal = value;} }
    [SerializeField] public RangeType type;

}
public abstract class Actor : MonoBehaviour, IActor
{
    [SerializeField] Case currentCase;
    ActorState state = ActorState.Alive;
     [SerializeField] private int _health;
    public Team Owner;

    [SerializeField] Range _range;
    public virtual Range Range{get { return _range; }  set{ _range = value;} }

    public virtual Case CurrentCase { get { return currentCase; }  set{ currentCase = value;} }

    public virtual ActorState State { get { return state; } set { state = value; } }

    

    public virtual int Health { get { return _health; } 
        
        protected set {

            // Empeche la valeur d'aller en dessous de zero -- //
            if (value >= 0)
            {
                _health = value;
            }

            if (value < 0 || _health == 0)
            {
                _health = 0;
                State = ActorState.Dead;
                Death();
            }

        } 
    }

    public virtual  void Update() {
        if(State == ActorState.Dead || Health <= 0)
            Death();
    }
    public virtual  void FixedUpdate() {
    }
    public virtual  void Start() {
        UIManager.CreateBoxActorInfo(gameObject, "test");
    }



    public virtual Case[] AttackRange()
    {
        return null; 
    }

    public virtual void Death()
    {
        Destroy(gameObject);
        throw new System.NotImplementedException();
    }

    public virtual void DoDamage(int amount)
    {
        Health -= amount;
        throw new System.NotImplementedException();
    }

    public virtual void Attack(Actor target)
    {
        throw new System.NotImplementedException();
    }

    public virtual void EnableAbility()
    {
        throw new System.NotImplementedException();
    }
}
