using UnityEngine;

public enum RangeType
{
    Simple,
    Radius
}

[System.Serializable]
public struct Range
{
    [Range(0, 10)]
    [SerializeField] int right, diagonal;
    public int RightRange { get { return right; } set { right = value; } }
    public int DiagonalRange { get { return diagonal; } set { diagonal = value; } }
    [SerializeField] public RangeType type;

}
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

    public virtual Case CurrentCase { get { return currentCase; } set { currentCase = value; } }

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



    public virtual Case[] AttackRange(DataWeapon weapon)
    {
        return null;
    }

    public virtual void Death()
    {
        Destroy(gameObject);
    }

    public virtual void DoDamage(int amount)
    {
        Health -= amount;
    }

    public virtual void Attack(Actor target)
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
