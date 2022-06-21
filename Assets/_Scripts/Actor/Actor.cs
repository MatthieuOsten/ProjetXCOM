using UnityEngine;


public abstract class Actor : MonoBehaviour, IActor
{
    [Header("Actor Info")]
    [Tooltip("Correspond � la case ou il se trouve")]
    [SerializeField] Case currentCase;
    [Tooltip("L'�tat actuelle du personnage")]
    [SerializeField] ActorState state = ActorState.Alive;
    [Tooltip("Vie du personnage")]
    [SerializeField] private int _health;
    [Tooltip("La team auquelle le personnage appartient")]
    public Team Owner;

    public virtual Case CurrentCase { get { return currentCase; } set { currentCase = value; 
                                                                        currentCase.Actor = this;} }

    public virtual ActorState State { get { return state; } set { state = value; } }



    public virtual int Health
    {
        get { return _health; }

        set
        {
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

    public virtual void Update()
    {
        if (State == ActorState.Dead || Health <= 0)
            Death();
    
    }
    public virtual void FixedUpdate() { }
    public virtual void Start()
    {
        // Permet de cr�e la bulle d'information au dessus du personnage
        UIManager.CreateBoxActorInfo(gameObject, "test");

    }



    public virtual Case[] AttackRange(DataWeapon weapon , Material specificMaterial = null)
    {
        return null;
    }

    public virtual void Death()
    {
        Destroy(gameObject);
    }

    public virtual void DoDamage(int amount)
    {
        // Si un perso en overwatch se fait attaquer, on l'enleve l'overwatch
        if(State == ActorState.Overwatch )
            State = ActorState.Alive;

        Health -= amount;
       
    }

    public virtual void Attack(Actor target, Actor[] detectedTargets)
    {
        throw new System.NotImplementedException();
    }
    

    public virtual void EnableAbility(Actor target)
    {
        throw new System.NotImplementedException();
    }
    public virtual void EnableAbilityAlt(Actor target)
    {
        throw new System.NotImplementedException();
    }
}
