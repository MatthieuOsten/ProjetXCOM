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

    public override void AttackRange()
    {
        Debug.Log("ShowAttackRange");
        Case[] Range = new Case[8];
        int actorX = CurrentPos.x;
        int actorY = CurrentPos.y;
        GridManager parent = CurrentPos.GridParent;
        Range[0] = GridManager.GetCase(parent , actorX , actorY+1); // Case au dessus
        Range[1] = GridManager.GetCase(parent , actorX+1 , actorY); // Case a droite
        Range[2] = GridManager.GetCase(parent , actorX-1 , actorY); // Case a gauche
        Range[3] = GridManager.GetCase(parent , actorX , actorY-1); // Case en bas
        Range[4] = GridManager.GetCase(parent , actorX+1 , actorY+1); // Case au dessus droite
        Range[5] = GridManager.GetCase(parent , actorX-1 , actorY+1); // Case au dessus gauche
        Range[6] = GridManager.GetCase(parent , actorX+1 , actorY-1); // Case en bas droite
        Range[7] = GridManager.GetCase(parent , actorX-1 , actorY-1); // Case en bas gauche

        foreach(Case aCase in Range)
        {
            if(aCase != null)
                aCase.Checked = true;
        }
    }

    
}
