using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CaseState
{
    Null,
    Empty,
    Occupied,
    HalfOccupied
}

[System.Serializable]
public class Case 
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
    public GridPoint Point; // position in the grid 
    public CaseState _state; // the state of the grid
    public int index = -1; // the index of the case in the grid
    public Material mtl;
    public GameObject plane;


    public int PathFindDistanceFromStart;
    public int PathFindDistanceFromEnd;
    public bool Highlighted;
    public bool PointCase;
    public bool Checked;
    public bool BlackList;
    public bool goodPath;
    /* Next properties to include
    Actor _actor; // A ref to the actor in the case, if no actor, the ref will be null
    Interact _interact // A ref to a interact like a echelle 
    */ 

    public GridManager GridParent
    {
        set{ _gridParent = value;}
    }

    /* 
        Update pour la cellule
    */
    public void UpdateCell()
    {
        Color caseColor;
        switch(_state)
        {
            case CaseState.Null:
                caseColor = Color.black;
            break;
            case CaseState.HalfOccupied:
                caseColor = Color.yellow;
            break;
            case CaseState.Occupied:
                caseColor = Color.red;
            break;
            case CaseState.Empty:
                caseColor = Color.green;
            break;
            default:
                caseColor = Color.black;
            break;

        }

        mtl.color = caseColor;

        if(Highlighted)
            mtl.color = Color.cyan;

        if(PointCase)
        {
            mtl.color = Color.magenta;
        }

        if(goodPath)
        {
            mtl.color = Color.white;
        }
    }

    void CheckIfCollide()
    {
          RaycastHit hit;
        Vector3 Hitpoint = Vector3.zero;
        Debug.Log(_gridParent.GetCaseWorldPosition(Point.x,Point.y));

        Vector3 StartPosA = _gridParent.GetCaseWorldPosition(Point.x,Point.y);
        Vector3 EndPosA = _gridParent.GetCaseWorldPosition(Point.x+1,Point.y+1);

        Vector3 StartPosB = _gridParent.GetCaseWorldPosition(Point.x+1,Point.y);
        Vector3 EndPosB = _gridParent.GetCaseWorldPosition(Point.x,Point.y+1);

        if (   ( Physics.Raycast(StartPosA , EndPosA - StartPosA, out hit, _gridParent.cellSize+1) ) 
        ||      Physics.Raycast(StartPosB, EndPosB - StartPosB , out hit, _gridParent.cellSize+1) )

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
