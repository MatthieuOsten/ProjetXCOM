using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : Character
{

    // -- Recupere la cible et verifie qu'elle soit iligible -- //
    public bool TakeTarget(Case target, out Actor actor)
    {
        actor = new Character();

        if (target != null)
        {
            return false;
        }

        return false;
    }

    
}
