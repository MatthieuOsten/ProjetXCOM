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
        List<Case> _range = new List<Case>((8*Range.rightRange) + (8 * Range.diagonalRange));
        int actorX = CurrentCase.x;
        int actorY = CurrentCase.y;
        GridManager parent = CurrentCase.GridParent;
        GridManager.ResetCasesPreview(parent);

        switch(Range.type)
        {
            case RangeType.Simple:
                for(int i = 1 ; i < Range.rightRange+1; i++)
                {
                    _range.Add(GridManager.GetCase(parent , actorX , actorY + (1 * i))); 
                    _range.Add(GridManager.GetCase(parent , actorX+ (1 * i) , actorY)); // Case a droite
                    _range.Add( GridManager.GetCase(parent , actorX- (1 * i) , actorY)); // Case a gauche
                    _range.Add( GridManager.GetCase(parent , actorX , actorY- (1 * i))); // Case en bas
                }
                for(int i = 1 ; i < Range.diagonalRange+1; i++)
                {
                    _range.Add(GridManager.GetCase(parent , actorX+ (1 * i) , actorY+ (1 * i))); // Case au dessus droite
                    _range.Add(GridManager.GetCase(parent , actorX- (1 * i) , actorY+ (1 * i))); // Case au dessus gauche
                    _range.Add( GridManager.GetCase(parent , actorX+ (1 * i) , actorY- (1 * i))); // Case en bas droite
                    _range.Add( GridManager.GetCase(parent , actorX- (1 * i) , actorY- (1 * i))); // Case en bas gauche
                }
            break;
            case RangeType.Radius:
                _range = GridManager.GetRadiusCases(CurrentCase, Range.rightRange);
            break;
        }
        GridManager.SetCasePreview(_range);

        // // void GetRangeCases()
        {
            
        }


        // _range[0] = GridManager.GetCase(parent , actorX , actorY+1* Range.rightRange); // Case au dessus
        // _range[1] = GridManager.GetCase(parent , actorX+1* Range.rightRange , actorY); // Case a droite
        // _range[2] = GridManager.GetCase(parent , actorX-1* Range.rightRange , actorY); // Case a gauche
        // _range[3] = GridManager.GetCase(parent , actorX , actorY-1* Range.rightRange); // Case en bas
        // _range[4] = GridManager.GetCase(parent , actorX+1* Range.diagonalRange , actorY+1* Range.diagonalRange); // Case au dessus droite
        // _range[5] = GridManager.GetCase(parent , actorX-1* Range.diagonalRange , actorY+1* Range.diagonalRange); // Case au dessus gauche
        // _range[6] = GridManager.GetCase(parent , actorX+1* Range.diagonalRange , actorY-1* Range.diagonalRange); // Case en bas droite
        // _range[7] = GridManager.GetCase(parent , actorX-1* Range.diagonalRange , actorY-1* Range.diagonalRange); // Case en bas gauche
        
        for(int i = 0 ; i < Range.rightRange ; i++)
        {

        }
        //
        foreach(Case aCase in _range)
        {
            if(aCase != null)
            {
                aCase.Highlighted = true;
                aCase.ChangeMaterial(aCase.GridParent.Data.caseNone);
            }
                 
        }
    }

    
}
