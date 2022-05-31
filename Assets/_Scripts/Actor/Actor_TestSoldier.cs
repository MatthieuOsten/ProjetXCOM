using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor_TestSoldier : Character 
{
    bool AbilityEnabled;
    

    /*
        Ici un belle exemple de l'interet de l'héritage
        Admettons que notre soldat TestSoldier a une capacité de resistance, et bien
        ici en overridant DoDommage, on va pouvoir appliquer une modification sur les dégat recu

    */
    public override void DoDamage(int amount)
    {
        if(AbilityEnabled)
        {
            amount = amount/2;
        }
        base.DoDamage(amount);
    }

    public override void Attack(Actor target)
    {
        target.DoDamage(10);
        base.Attack(target);
    }
   

    
}
