using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor_Support : Character 
{
    Character AllieBuffed;

    /*
        Ici un belle exemple de l'interet de l'héritage
        Admettons que notre soldat TestSoldier a une capacité de resistance, et bien
        ici en overridant DoDommage, on va pouvoir appliquer une modification sur les dégat recu

    */
    public override void DoDamage(int amount)
    {
        base.DoDamage(amount);
    }

    public override void Attack(Actor target, Actor[] detectedTargets)
    {
        target.DoDamage(Data.Weapon.Damage);
        foreach( Actor _target in detectedTargets)
        {
            if(_target == target) continue;

            _target.DoDamage(Data.Weapon.Damage);
            ActionAnimation(GetMainWeaponInfo(), _target);
        }
        base.Attack(target,detectedTargets);
    }

    /*
        “heal over time” : grande distance de lancé , 
        ce sort permet à un allié ciblé d’avoir un régénération de vie pour 2 tours ( +2 pv /tour)
        Cout  1 PA. 4 tours de cd
    */
    public override void EnableAbility(Actor target)
    {
        if(cooldownAbility <= 0)
        {
            Character _char = null;
            if( target is Character)
            {
                _char = (Character)target;
            
                if (_char.Owner != Owner )
                {
                    UIManager.CreateSubtitle("Le personnage visée n'est pas un allié ", 2);
                    return;
                }
                
                AllieBuffed = _char;
                AllieBuffed.Health += AllieBuffed.MaxHealth/4;
                cooldownAbility = GetAbilityCooldown;
             
                base.EnableAbility(target);
            }
            
        }
        else
        {
            UIManager.CreateSubtitle("Le support peut réutiliser sa compétence dans " + cooldownAbility + " tours");
            return;
        }
        

    }
    /*Description “ A l’attaque” : moyenne distance de lancé , 
     * ce sort permet à un allié ciblé d’avoir un point d’action supplémentaire.
        cout : 1 PA. 3 tours de cd
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
                if(_char == this)
                {
                    UIManager.CreateSubtitle("Le support ne peut pas se donner des PA sur lui même", 2);
                    return;
                }
                 if (_char.IsOverwatching)
                {
                    UIManager.CreateSubtitle("Le personnage visée est en overwatch", 2);
                    return;
                }
               
          
                _char.CurrentActionPoint ++;
                cooldownAbilityAlt = GetAbilityAltCooldown;
                base.EnableAbilityAlt(target);
            }

        }
        else
        {
            UIManager.CreateSubtitle("Le support peut réutiliser sa compétence dans " + cooldownAbilityAlt + " tours");
        }
    }

    public override void StartTurnActor()
    {
        if(AllieBuffed != null)
            AllieBuffed.Health += AllieBuffed.MaxHealth/4;
    }

    // On diminue le cooldown de l'ability du support à chaque tour
    public override void EndTurnActor()
    {
        // Si il y 'a coold down on le diminue
        if(cooldownAbility > 0)
            cooldownAbility--;

        if(cooldownAbility <= 1)
            AllieBuffed = null;
        

        if (cooldownAbilityAlt > 0)
            cooldownAbilityAlt--;

        base.EndTurnActor();
    }



}
