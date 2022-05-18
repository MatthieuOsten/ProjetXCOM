using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Actor
{

    [SerializeField] Case CurrentCase { get; }

     void Dead();
     void TakeDamage(int damage);

}