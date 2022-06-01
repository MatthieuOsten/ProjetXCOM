using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor_Tank : Character 
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
        
        Character targetChar = null;
        if( target is Character )
        {
            targetChar = (Character)target;
            targetChar.CurrentActionPoint--;
        }
        target.DoDamage(Data.weapons[0].Damage);
        
        base.Attack(target);
    }
   

    
}
