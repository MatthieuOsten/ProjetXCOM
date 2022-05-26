using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteAlways]
//[System.Serializable]
public class GridManager : MonoBehaviour
{
    [Header("GRID PARAMETERS")]
    public SO_Grid Data;

    // Need to be Moved in Data
    public Case[,] _grid; // A table with double entry with x and y, with for each element the type Case
    int _currentCellCreated = 0; // Le nombre de case crée

    [Header("Generation")]
    [SerializeField] bool GenerateAGrid, ResetGrid;

    [SerializeField] bool _showScorePathFinding;
    public bool ShowScorePathFinding { get { return _showScorePathFinding; } }

    public int SizeX
    {
        get { return Data.gridSizeX; }
        set
        {
            if (value <= 0)
            {
                Data.gridSizeX = 1;
            }
            else
            {
                Data.gridSizeX = value;
            }
        }
    }
    public int SizeY
    {
        get { return Data.gridSizeY; }
        set
        {
            if (value <= 0)
            {
                Data.gridSizeY = 1;
            }
            else
            {
                Data.gridSizeY = value;
            }
        }
    }
    public int CellSize
    {
        get { return Data.cellSize; }
        set
        {
            if (value <= 0)
            {
                Data.cellSize = 1;
            }
            else
            {
                Data.cellSize = value;
            }
        }
    }

    /// <summary> Nettoie la grille existantes</summary>
    void ClearGrid()
    {
        int childs = transform.childCount;
        for (int i = childs - 1; i >= 0; i--)
        {
            GameObject.DestroyImmediate(transform.GetChild(i).gameObject);
        }
        _currentCellCreated = 0;
    }
    /// <summary> Cette fonction va generer la grille, et chaque cellule sera une entity de type Case </summary>
    void GenerateGrid()
    {
        ClearGrid();
        _grid = new Case[SizeX, SizeY];
        // On genere les cases pour chaque coordonnée
        for (int x = 0; x < SizeX; x++)
        {
            for (int y = 0; y < SizeY; y++)
            {
                _grid[x, y] = GenerateCase(x, y);
                _currentCellCreated++;
            }
        }
    }
    /// <summary> Génère la case et la relie correctement à la grille </summary>
    Case GenerateCase(int x, int y)
    {
        // Debug 
        GameObject plane = Instantiate(Data.CasePrefab, GetCaseWorldPosition(x, y), Quaternion.identity, transform);
        plane.transform.localScale = plane.transform.localScale * CellSize;
        plane.name = $"Case n°{_currentCellCreated} [{x};{y}]";
        // On init la case
        Case newCase = plane.GetComponent<Case>();
        GameObject decal = plane.GetComponentInChildren<SpriteRenderer>().gameObject;
        decal.transform.localScale = plane.transform.localScale;
        newCase.x = x; // On donne a la case ces coordonnées dans le tableau de la Grid
        newCase.y = y; // On donne a la case ces coordonnées dans le tableau de la Grid

        newCase.GridParent = this; // on dit à quelle grid appartient la case
        newCase.index = _currentCellCreated; // son index 
        newCase.state = CaseState.Empty; // son etat
        return newCase;
    }


    /// <summary> Alors je fais cette function car entre le edit et play mode, la table a double entrer se casse la gueule, pourquoi ? car cest de la merde </summary>
    void RegenerateCaseTable()
    {
        // Alors on check si la _grid est niqué et si il a des gosses, si cest le cas on les reattribue 
        if (_grid == null && transform.childCount > 0)
        {
            Debug.LogWarning("[GridManager] La table de grid a du être regen");
            _grid = new Case[SizeX, SizeY];

            for (int i = 0; i < transform.childCount; i++)
            {
                Transform aCase = transform.GetChild(i);
                Case newCase = aCase.GetComponent<Case>();
                _grid[newCase.x, newCase.y] = newCase;
            }
        }
    }

    /// <summary>
    ///         !! Surement va etre supprimer car les cases sont maintenant des gameobject
    /// Permet d'avoir la position de la grille dans le world,
    /// Necessaire pour certaines situations et vue que la Case n'est pas un gameobject, il y a cette function de disponible
    /// </summary>

    public Vector3 GetCaseWorldPosition(int x, int y)
    {
        return new Vector3(x, 0, y) * CellSize;
    }

    public static Vector3 GetCaseWorldPosition(Case caseToCheck)
    {
        return new Vector3(caseToCheck.CaseStatut.x, 0, caseToCheck.CaseStatut.y) * caseToCheck.GridParent.CellSize;
    }
    /*
        WIP
       
    */
    /// <summary> Récupére une case dans la grille</summary>

    public Case GetCase(int x, int y)
    {
        if (x >= SizeX || y >= SizeY || x < 0 || y < 0)
            return null;

        return _grid[x, y];
    }

    /// <summary> Récupére une case dans la grille indiquer en paramètre</summary>
  
    public static Case GetCase(GridManager gridParent, int x, int y)
    {
        if (x >= gridParent.SizeX || y >= gridParent.SizeY || x < 0 || y < 0)
            return null;

        return gridParent._grid[x, y];
    }

    /// <summary> Recupere tout les cases adjacente à celle qu'on envoie en paramètre</summary>
    public static Case[] GetAdjacentCases(Case CurrentCase)
    {
        Case[] cases = new Case[8];
        GridManager gridParent = CurrentCase.GridParent;
        int x = CurrentCase.x;
        int y = CurrentCase.y;
        // les cases adjacentes
        cases[0] = GetCase(gridParent, x, y + 1);
        cases[1] = GetCase(gridParent, x, y - 1);
        cases[2] = GetCase(gridParent, x + 1, y);
        cases[3] = GetCase(gridParent, x - 1, y);
        //Diagonal
        cases[4] = GetCase(gridParent, x - 1, y + 1);
        cases[5] = GetCase(gridParent, x - 1, y - 1);
        cases[6] = GetCase(gridParent, x + 1, y + 1);
        cases[7] = GetCase(gridParent, x + 1, y - 1);
        return cases;
    }


    public static List<Case> GetRadiusCases(Case CurrentCase, int radius )
    {
        bool inside_circle(Case center, Case tile, float radius) 
        {
            int dx = center.x - tile.x,
            dy = center.y - tile.y;
            float distance  = Mathf.Sqrt(dx*dx + dy*dy);
            return distance <= radius ;
        }

        List<Case> RadiusCase = new List<Case>();

        // Pour eviter de checker toute la grille, on va faire un zone en carré pour délimiter
        int xStart =  CurrentCase.x - radius;
        int yStart =  CurrentCase.y - radius;
        int xEnd =  CurrentCase.x + radius;
        int yEnd =  CurrentCase.y + radius;

        for (int y = yStart;  y <= yEnd; y++) {
            for (int x = xStart; x <= xEnd; x++) {
                Case caseToCheck = CurrentCase.GridParent.GetCase(x, y);
                if ( caseToCheck != null && inside_circle(CurrentCase, caseToCheck, radius)) {
                    RadiusCase.Add(caseToCheck);
                    
                }
            }
        }

        return RadiusCase;
    }

     /// <summary> Reset toutes les cases de prévisualisation </summary>
    public static void ResetCasesPreview(GridManager grid,Case startCase = null, Case endCase = null )
    {
        for (int x = 0; x < grid.SizeX; x++)
        {
            for (int y = 0; y < grid.SizeY; y++)
            {
                if (grid._grid[x, y] != endCase && grid._grid[x, y] != startCase)
                {
                    ResetCasePreview(grid._grid[x, y]);
                }
            }
        }
    }
    public static void ResetCasePreview(Case _case)
    {
        _case.Checked = false;
        _case.Highlighted = false;
        _case.hCost = 0;
        _case.gCost = 0;
    }

    public static void SetCasePreview(Case aCase, bool Reset = false)
    {
        if(GetValidCase(aCase) == null) return;
        
         if(Reset) ResetCasesPreview(aCase.GridParent);

        aCase.Highlighted = true;
        aCase.ChangeMaterial(aCase.GridParent.Data.caseHighlight);

    }
    public static void SetCasePreview(List<Case> cases, bool Reset = false)
    {
        if(cases == null || cases.Count == 0) return;
        if(Reset)
            ResetCasesPreview(cases[0].GridParent); 
            
        for(int i = 0 ; i < cases.Count ; i++)
        {
            SetCasePreview(cases[i]);
        }
    }

    void Start()
    {
        RegenerateCaseTable(); // Existe car entre le edit et runtime la table a double entrer foire // TODO : trouver une autre maniere

    }

    // Update is called once per frame
    void Update()
    {
        RegenerateCaseTable(); // Existe car entre le edit et runtime la table a double entrer foire // TODO : trouver une autre maniere

    


        if (GenerateAGrid)
        {
            GenerateGrid();
            GenerateAGrid = false;
        }
        if (ResetGrid)
        {
            ClearGrid();
            ResetGrid = false;
        }

        
        // On check si la taille de la grid a changé
        if(_grid.GetLength(0) != SizeX || _grid.GetLength(1) != SizeY)
        {
            Case[,] tempGrid = new Case[SizeX, SizeY];
            _currentCellCreated = 0;
             // On genere les cases pour chaque coordonnée
            for (int x = 0; x < SizeX; x++)
            {
                for (int y = 0; y < SizeY; y++)
                {   
                   // Debug.Log($"{_grid.GetLength(0)} && {_grid.GetLength(1)} with {x};{y}");
                    if(x < _grid.GetLength(0) && y < _grid.GetLength(1) && _grid[x,y] != null)
                    {
                        tempGrid[x,y] = _grid[x,y];
                    }
                    else
                    {
                         tempGrid[x, y] = GenerateCase(x, y);
                    }
                   
                    _currentCellCreated++;
                }
            }
            
            int childs = transform.childCount;
                for (int i = childs - 1; i >= 0; i--)
                {   
                    Case child = transform.GetChild(i).GetComponent<Case>();
                    if(child.x >= SizeX || child.y >= SizeY)
                    {
                        GameObject.DestroyImmediate(transform.GetChild(i).gameObject);
                    }

                    
                }
               
          
            _grid = tempGrid;
            
        }
        
    }

    // Deplacer dans GridEditorTool.cs
    public void EditCase(Vector3 pos, CaseState caseState)
    {
        int x = (int)pos.x / CellSize;
        int y = (int)pos.z / CellSize;

        Case caseToEdit = GetCase(x, y);
        caseToEdit.state = caseState;
    }

    /// <summary> Verifie si la case est accessible </summary>
    public static Case GetValidCase(Case caseToCheck)
    {
        if (caseToCheck == null)
            return null;

        if (caseToCheck.state == CaseState.Occupied || caseToCheck.state == CaseState.Null )
            return null;

        return caseToCheck;
    }


    public Case GetRandomCase()
    {
        int x = Random.Range(0, SizeX);
        int y = Random.Range(0, SizeY);
        Case randomCase = GetCase(x,y);
        while(GetValidCase(randomCase) == null)
        {
            x = Random.Range(0, SizeX);
            y = Random.Range(0, SizeY);
            randomCase = GetCase(x,y);
        }
        
        return randomCase;
    }
    // /*
    //     Permet de sauvegarder la grid dans le fichier data mais cest peut etre inutile donc a voir
    // */
    // void SaveGridToData()
    // {
    //     CaseInfo[] newCases = new CaseInfo[SizeX * SizeY];
    //     for(int xi = 0 ; xi < SizeX; xi++)
    //     {
    //         for(int yi = 0 ; yi < SizeY; yi++)
    //         {
    //             Debug.Log(_grid[xi,yi].index);
    //             newCases[_grid[xi,yi].index] = _grid[xi,yi].CaseStatut;              
    //         }
    //     }
    //     Data.Grid = newCases;
    // }

}


