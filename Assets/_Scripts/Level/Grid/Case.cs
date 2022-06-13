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
    HalfOccupied,
    Spawner // alors il sera forcemet empty apres car c'est un spawner pour les team
}

[System.Serializable]
[ExecuteAlways] // Cette class fonctionne en edit et en play 
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

    public bool Highlighted { get; set;}
     public bool Selected { get; set;}
    public bool Checked;

    // PathFinding part
    public int gCost;
    public int hCost;
    public int fCost { get { return hCost + gCost; } }
    public Case ParentCase;

     public float distance;


    [Header("Reference")]
    Actor _actor; // Une reference de l'actor qui est dessus

    public Actor Actor{get{ return _actor;} set{ _actor = value;} }

    public Character Character{
        get{ 

            if(Actor is Character)
                return (Character)_actor;
            
            return null ;} 
    }

    public bool HaveActor{ get {return _actor != null;}}
    /* Next properties to include
    Interact _interact // A ref to a interact like a echelle 
    */
    public int x { get { return CaseStatut.x; } set { CaseStatut.x = value; } }
    public int y { get { return CaseStatut.y; } set { CaseStatut.y = value; } }
    public CaseState State { get { return CaseStatut._state; } set { CaseStatut._state = value; } }
    public int index { get { return CaseStatut.index; } set { CaseStatut.index = value; } }

    [Header("DEBUG")]
    SpriteRenderer _sr;
    [SerializeField] TextMeshPro total;
    [SerializeField] TextMeshPro h;
    [SerializeField] TextMeshPro g;


    private void Start()
    {
        _sr = GetComponentInChildren<SpriteRenderer>();
        // Ajoute la case à la grid pour dire qu'elle peut servir de spawn, on verifie si elle est dans le state spawn et qu'elle ny 'ai pas déja
        if (State == CaseState.Spawner && !GridParent.SpawnerCase.Contains(this) ) 
            GridParent.SpawnerCase.Add(this);

    }
    private void OnDestroy() {
        if (State == CaseState.Spawner && GridParent.SpawnerCase.Contains(this) ) 
        {
            GridParent.SpawnerCase.Remove(this);
        }
    }
    public void Update()
    {  
        if (Highlighted || Checked)
            return;
        WatchCaseState();
          Debuga();
        return;
    }

    void Debuga()
    {
        if (GridParent.ShowScorePathFinding)
        {
            total.gameObject.SetActive(true);
            h.gameObject.SetActive(true);
            g.gameObject.SetActive(true);
            total.text = "" + (gCost + hCost);
            h.text = "" + hCost;
            g.text = "" + gCost;

            if(distance > 0)
                total.text = ""+distance;
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
            case CaseState.Spawner:
                ChangeMaterial(GridParent.Data.caseSpawner);
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


    /// <summary> Change le material de la cellule </summary>
    public void ChangeMaterial(Material newMtl)
    {
        //Debug.Log(newMtl.name);
        if(_sr.sharedMaterial != newMtl) // On vérifie si le matérial n'est pas déjà assignés
            _sr.sharedMaterial = newMtl; // On utilise sharedMaterial pour eviter de crée une nouvelle instance
    }
}
