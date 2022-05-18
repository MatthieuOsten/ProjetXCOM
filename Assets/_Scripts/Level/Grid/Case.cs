using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using TMPro;

public enum CaseState
{
    Null,
    Empty,
    Occupied,
    HalfOccupied
}


[System.Serializable]
[ExecuteAlways]
public class CaseInfo
{
    public int x,y;
    public CaseState _state; // the state of the grid
    public int index = -1; // the index of the case in the grid
}

[ExecuteAlways]

public class Case : MonoBehaviour
{
    // CURRENTLY WORK IN PROGRESS, EVERYTHING WILL BE POLISHED
    /*
        Case class will be used by the gridManager, each case of the grid will be a Case.
        Currently, this class will not be a component so monobehavior will be remove after some approbation.
    */
    

    public struct GridPoint
    {
        /*
            Coordinate of the case in the grid
        */
        public int x { get; }
        public int y { get; }

        public GridPoint(int x , int y)
        {
            this.x = x;
            this.y = y; 
        }
    }

    public GridManager _gridParent; // the owner of the case
    //public GridPoint Point; // position in the grid cest casser ca va virer
   
    public CaseInfo It;

    public Material mtl;
    //DecalProjector _proj; // Ce qui projete la case sur le monde
    public SpriteRenderer sr;

    [Header("PathFinding")]
    [Tooltip(" G cost : Distance from starting case")]
    public int gCost;
    [Tooltip(" H cost (heuristic) : Distance from end case")]
    public int hCost;
    [Tooltip(" F cost: G cost + H cost")]
    public int fCost{ get{return hCost+gCost;}}
    
    public bool Highlighted, PointCase, Checked, BlackList,goodPath;

    public Case parentCase;

    public ActorTest _actor; // Une reference de l'actor qui est dessus

    /* Next properties to include
    
    Interact _interact // A ref to a interact like a echelle 
    */ 

    public int x { get{ return It.x;} set{ It.x = value;} }
    public int y { get{ return It.y;} set{ It.y = value;}}
    public CaseState state { get{ return It._state;} set{ It._state = value;}}
    public int index { get{ return It.index;} set{ It.index = value;}}

    [Header("DEBUG")]
    public TextMeshPro total;
    public TextMeshPro h;
    public TextMeshPro g; 

    public GridManager GridParent
    {
        set{ _gridParent = value;}
    }


    private void Start() {
        //_proj = GetComponentInChildren<DecalProjector>();    
        // Creates a new material instance for the DecalProjector.
        // here why https://docs.unity.cn/Packages/com.unity.render-pipelines.high-definition@12.0/manual/creating-a-decal-projector-at-runtime.html
        //_proj.material = new Material(_proj.material);
        mtl = GetComponentInChildren<SpriteRenderer>().sharedMaterial;
        sr = GetComponentInChildren<SpriteRenderer>();

    }
    /* 
        Update pour la cellule
    */
    public void Update()
    {
        if(_gridParent.ShowScorePathFinding)
        {
            total.gameObject.SetActive(true);
            h.gameObject.SetActive(true);
            g.gameObject.SetActive(true);
            total.text = ""+(gCost+hCost);
            h.text = ""+hCost;
            g.text = ""+gCost;
        }
        else
        {
            total.gameObject.SetActive(false);
            h.gameObject.SetActive(false);
            g.gameObject.SetActive(false);
        }


        if( Highlighted||  PointCase||  Checked||  BlackList|| goodPath)
            return;

        switch(It._state)
        {
            case CaseState.Null:
                ChangeMaterial(_gridParent.Data.caseNone);
            break;
            case CaseState.HalfOccupied:
                ChangeColor(Color.yellow, 0);
            break;
            case CaseState.Occupied:
                ChangeMaterial(_gridParent.Data.caseLocked);
            break;
            case CaseState.Empty:
                ChangeMaterial(_gridParent.Data.caseDefault);
            break;
            default:
                ChangeMaterial(_gridParent.Data.caseNone);
            break;

        }
        return;
        

        if(Highlighted)
        {
            ChangeMaterial(_gridParent.Data.caseHighlight);
            sr.enabled = true;
        }
           
        else if(PointCase)
        {
            ChangeColor(Color.magenta, 0);
                        sr.enabled = true;

        }
        else if(Checked)
        {
            ChangeMaterial(_gridParent.Data.caseNone);

                        sr.enabled = true;

        }
        else
        {
            sr.enabled = true;
        }
    }

    /* Change la couleur de la cellule 
        Cava etre jarter car ca cree une nouvelle instance de material, vaut mieux changer directement le material
    */
    void ChangeColor(Color newColor, float emissiveIntensity)
    {
       // if(mtl.GetColor("_Color") != newColor )
        {
            //.SetColor("_EmissiveColor", newColor * emissiveIntensity);
            //mtl.SetColor("_Color", newColor );
        } 
       
    }

     /* Change la couleur de la cellule */
    public void ChangeMaterial(Material newMtl)
    {
        {
            sr.material =  newMtl;
        } 
       
    }

    void CheckIfCollide()
    {
          RaycastHit hit;
        Vector3 Hitpoint = Vector3.zero;
        Debug.Log(_gridParent.GetCaseWorldPosition(x,y));

        Vector3 StartPosA = _gridParent.GetCaseWorldPosition(x,y);
        Vector3 EndPosA = _gridParent.GetCaseWorldPosition(x+1,y+1);

        Vector3 StartPosB = _gridParent.GetCaseWorldPosition(x+1,y);
        Vector3 EndPosB = _gridParent.GetCaseWorldPosition(x,y+1);

        if (   ( Physics.Raycast(StartPosA , EndPosA - StartPosA, out hit, _gridParent.CellSize+1) ) 
        ||      Physics.Raycast(StartPosB, EndPosB - StartPosB , out hit, _gridParent.CellSize+1) )

        {
            Debug.DrawLine(StartPosA, EndPosA, Color.red);
            Debug.DrawLine(StartPosB, EndPosB, Color.red);

            Debug.Log("Did Hit");
        }
        else
        {
            Debug.DrawLine(StartPosA, EndPosA , Color.gray);
            Debug.DrawLine(StartPosB, EndPosB , Color.white);
            Debug.Log("Did not Hit");
        }


    }

}
