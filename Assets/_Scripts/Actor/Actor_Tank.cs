using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor_Tank : Character 
{
    bool AbilityEnabled;
    
    [SerializeField] Character _victim;

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

    public override void Attack(Actor target , Actor[] detectedTargets)
    {
        
        Character targetChar = null;
        if( target is Character )
        {
            targetChar = (Character)target;
            targetChar.CurrentActionPoint--;
        }
        target.DoDamage(Data.Weapon.Damage);
        
        base.Attack(target ,  detectedTargets );
    }

    /*
     Description “Réduction du portée de déplacement et de tir: Grande portée d’action sur un seul ennemi a la fois utiliser 
    grâce à son Pistolame réduisant sa porté de déplacement 
    et de tir pendant 2 tours le Tank a ensuite un CD de 4 tours.
     */

    public override void EnableAbility(Actor target)
    {
        if (cooldownAbility <= 0)
        {
            Character _char = null;
            if (target is Character)
            {
                _char = (Character)target;
                if(_char.Owner == Owner)
                {
                    UIManager.CreateSubtitle("Le personnage visée n'est pas un ennemi ",2);
                    return;
                }

                _victim = _char;
                Debug.Log("Bite");
                cooldownAbility = GetAbilityCooldown;
                 base.EnableAbility(target);
            }

        }
        else
        {
            
            UIManager.CreateSubtitle("Le Tank peut réutiliser sa compétence dans " + cooldownAbility + " tours");
        }
        
    }

  
    public override void EndTurnActor()
    {
        // On verifie si le tank a une victim pour lequel on va appliquer les malus
        if (cooldownAbility <= 2)
            _victim = null;

        if(_victim != null)
        {
            // Si la victim est défini on enlève -2
            _victim.LimitCaseMovement -= 2;
            _victim._rangeDebuffValue = 2;

        }

        cooldownAbility--;
        base.EndTurnActor();
    }

     void OnDestroy()
    {
        if(_victim != null)
        {
            _victim._rangeDebuffValue = 0;

        }
    }



}
