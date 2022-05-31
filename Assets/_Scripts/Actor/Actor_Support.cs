using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor_Support : Character 
{
    bool AbilityEnabled;

    int cooldownAbility = 0;
    int cooldownAbilityAlt = 0;
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
        target.DoDamage(Data.weapons[0].Damage);
        base.Attack(target);
    }

    public override void EnableAbility(Actor target)
    {
        if(cooldownAbility <= 0)
        {
            Character _char = null;
            if( target is Character)
            {
                _char = (Character)target;
                if (_char.Owner != Owner)
                {
                    UIManager.CreateSubtitle("Le personnage visée n'est pas un allié ", 2);
                    return;
                }
                if (_char.Health == _char.MaxHealth)
                {
                    UIManager.CreateSubtitle("Le personnage visée à déjà sa vie au maximum ", 2);
                    return;
                }
                _char.Health = _char.MaxHealth;
                cooldownAbility = 4;
                CurrentActionPoint--;
            }
            
        }
        else
        {
            UIManager.CreateSubtitle("Le support peut réutiliser sa compétence dans " + cooldownAbility + " tours");
        }
    }
    /*Description “ A l’attaque” : moyenne distance de lancé , 
     * ce sort permet à un allié ciblé d’avoir un point d’action supplémentaire.
        cout : 1 PA. 3 tours de cd
     * 
     * 
     */
    public override void EnableAbilityAlt(Actor target)
    {
        if (cooldownAbilityAlt <= 0)
        {
            Character _char = null;
            if (target is Character)
            {
                _char = (Character)target;
                if (_char.Owner != Owner)
                {
                    UIManager.CreateSubtitle("Le personnage visée n'est pas un allié ", 2);
                    return;
                }
          
                _char.CurrentActionPoint ++;
                cooldownAbilityAlt = 3;
                CurrentActionPoint--;
            }

        }
        else
        {
            UIManager.CreateSubtitle("Le support peut réutiliser sa compétence dans " + cooldownAbilityAlt + " tours");
        }
    }


    // On diminue le cooldown de l'ability du support à chaque tour
    public override void EndTurnActor()
    {
        // Si il y 'a coold down on le diminue
        if(cooldownAbility > 0)
            cooldownAbility--;

        if (cooldownAbilityAlt > 0)
            cooldownAbilityAlt--;

        base.EndTurnActor();
    }



}
