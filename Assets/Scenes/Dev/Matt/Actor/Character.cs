using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour, Actor
{
    [SerializeField] private DataCharacter _data;
    [SerializeField] private Case _currentCase;

    public DataCharacter Data { get { return _data; } }
    public Case CurrentCase { get { return _currentCase; } }

    // Effectue une action a la mort du personnage //
    public void Dead()
    {
        if (Data.IsDead)
        {

        }
    }

    // Effectue une action a lorsque le personnage prend des degats //
    public void TakeDamage(int damage)
    {
        Data.Health -= damage;

        // Si la vie du personnage est a zéro, alors il effectue son action de mort
        if (Data.Health == 0)
        {
            Data.IsDead = true;
            Dead();
        }
    }

    // Deplace le personnage d'un point A a un point B
    public void Movement(Vector2 destination)
    {
        
    }
}