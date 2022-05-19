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
    public int x, y;
    public CaseState _state; // le statut de la case sur la grille
    public int index = -1; // l'id de la case sur la grille
}

[ExecuteAlways]
public class Case : MonoBehaviour
{
    public GridManager GridParent; // la grille propriétaire de la case
    [SerializeField] CaseInfo _it;
    public CaseInfo CaseStatut { get { return _it; } }

    public bool Highlighted { private get; set;}
    public bool Checked;

    // PathFinding part
    public int gCost;
    public int hCost;
    public int fCost { get { return hCost + gCost; } }
    public Case ParentCase;


    [Header("Reference")]
    public ActorTest _actor; // Une reference de l'actor qui est dessus
    /* Next properties to include
    Interact _interact // A ref to a interact like a echelle 
    */
    public int x { get { return CaseStatut.x; } set { CaseStatut.x = value; } }
    public int y { get { return CaseStatut.y; } set { CaseStatut.y = value; } }
    public CaseState state { get { return CaseStatut._state; } set { CaseStatut._state = value; } }
    public int index { get { return CaseStatut.index; } set { CaseStatut.index = value; } }

    [Header("DEBUG")]
    SpriteRenderer _sr;
    [SerializeField] TextMeshPro total;
    [SerializeField] TextMeshPro h;
    [SerializeField] TextMeshPro g;


    private void Start()
    {
        _sr = GetComponentInChildren<SpriteRenderer>();
    }
    public void Update()
    { 
        Debug();
        if (Highlighted || Checked)
            return;
        WatchCaseState();
        return;
    }

    void Debug()
    {
        if (GridParent.ShowScorePathFinding)
        {
            total.gameObject.SetActive(true);
            h.gameObject.SetActive(true);
            g.gameObject.SetActive(true);
            total.text = "" + (gCost + hCost);
            h.text = "" + hCost;
            g.text = "" + gCost;
        }
        else
        {
            total.gameObject.SetActive(false);
            h.gameObject.SetActive(false);
            g.gameObject.SetActive(false);
        }
    } 

    void WatchCaseState()
    {
        switch (CaseStatut._state)
        {
            case CaseState.Null:
                ChangeMaterial(GridParent.Data.caseNone);
                break;
            case CaseState.HalfOccupied:
                ChangeColor(Color.yellow, 0);
                break;
            case CaseState.Occupied:
                ChangeMaterial(GridParent.Data.caseLocked);
                break;
            case CaseState.Empty:
                ChangeMaterial(GridParent.Data.caseDefault);
                break;
            default:
                ChangeMaterial(GridParent.Data.caseNone);
                break;

        }
    }

    /* Change la couleur de la cellule 
        Cava etre jarter car ca cree une nouvelle instance de material, vaut mieux changer directement le material
    */
    void ChangeColor(Color newColor, float emissiveIntensity)
    {
    }


    /// <summary> Change la couleur de la cellule </summary>
    public void ChangeMaterial(Material newMtl)
    {
        _sr.material = newMtl;
    }
}
