using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteAlways]
[System.Serializable]
public class GridManager : MonoBehaviour
{
    [Header("GRID PARAMETERS")]
    public SO_Grid Data; 
   
    // Need to be Moved in Data
    public Case[,] _grid; // A table with double entry with x and y, with for each element the type Case
    // [SerializeField] Case SelectedCaseA, SelectedCaseB;

    int _currentCellCreated = 0; // Le nombre de case crée

    [Header("idk")]
    [SerializeField] bool GenerateAGrid, ResetGrid;
    // [SerializeField] bool SelectMode;
 



    [Header("Debug")]
    // Controller _inputManager;
    [SerializeField] bool _showScorePathFinding;
    public bool ShowScorePathFinding { get{ return _showScorePathFinding;}}

    public int SizeX 
    { 
        get{ return Data.gridSizeX ;} 
        set{
            if(value <= 0)
            {
                Data.gridSizeX = 1;
            }
            else{
                Data.gridSizeX = value;
            }
        }
    }
    public int SizeY 
    { 
        get{ return Data.gridSizeY ;} 
        set{
            if(value <= 0)
            {
                Data.gridSizeY = 1;
            }
            else{
                Data.gridSizeY = value;
            }
        }
    }
    public int CellSize
    {
        get{ return Data.cellSize;}
        set{
            if(value <= 0)
            {
                Data.cellSize = 1;
            }
            else{
                Data.cellSize = value;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // _inputManager = new Controller();
        // _inputManager.TestGrid.Enable();
        //GenerateGrid();
    }


    /*
        Nettoie la grid precedente
    */
    void ClearGrid()
    {
        int childs = transform.childCount;
        for (int i = childs - 1; i >= 0; i--) {
            GameObject.DestroyImmediate( transform.GetChild( i ).gameObject );
        }
        _currentCellCreated = 0;
    }
    /*
        Cette fonction va generer la grille, et chaque cellule sera une entity de type Case
    */
    void GenerateGrid()
    {
        ClearGrid();

        //  if(Data.Grid.Length > 0) // Si une grid est stocké dans Data, on la reprend au lieu d'en generer
        //     _grid = CopyGridFromData();
        //  else
         {  
            _grid = new Case[SizeX, SizeY];
            // On genere les cases pour chaque coordonnée
            for(int x = 0 ; x < SizeX; x++)
            {
                for(int y = 0 ; y < SizeY; y++)
                {
                    _grid[x,y] = GenerateCase(x,y);
                    _currentCellCreated++;
                }
            }
         }
    }

    Case GenerateCase(int x,int y)
    {
        // Debug 
        GameObject plane = Instantiate(Data.CasePrefab, GetCaseWorldPosition(x, y)  , Quaternion.identity , transform );
        plane.transform.localScale = plane.transform.localScale * CellSize;
        plane.name = $"Case n°{_currentCellCreated} [{x};{y}]" ; 
        // On init la case
        Case newCase = plane.GetComponent<Case>();
        newCase.x = x; // On donne a la case ces coordonnées dans le tableau de la Grid
        newCase.y = y; // On donne a la case ces coordonnées dans le tableau de la Grid

        newCase.GridParent = this; // on dit à quelle grid appartient la case
        newCase.index = _currentCellCreated; // son index 
        newCase.state = CaseState.Empty; // son etat
        return newCase;
    }
    /*
        Cette function va copier la grid provenant de Data
    */
    Case[,] CopyGridFromData()
    {
        CaseInfo[] dataCase = Data.Grid;
        _grid = new Case[SizeX, SizeY];
        for(int x = 0 ; x < SizeX; x++)
        {
            for(int y = 0 ; y < SizeY; y++)
            {
                _grid[x,y].It = dataCase[_currentCellCreated];
                _grid[x,y].x = dataCase[_currentCellCreated].x; // Give this coordinate in the grid
                _grid[x,y].y = dataCase[_currentCellCreated].y; // Give this coordinate in the grid
                _grid[x,y].GridParent = this; // give the ref of the grid
                _grid[x,y].index = _currentCellCreated; 
                _grid[x,y].state = dataCase[_currentCellCreated]._state;
                GameObject plane = Instantiate(Data.CasePrefab, GetCaseWorldPosition(x, y)  , Quaternion.identity );
                plane.transform.localScale = plane.transform.localScale * CellSize;
                _currentCellCreated++;
            }
        }
        return _grid;
    }
    /*
        Alors je fais cette function car entre le edit et play mode, la table a double entrer se casse la gueule, pourquoi ? 
        car cest de la merde
    */
    void RegenerateCaseTable()
    {
        // Alors on check si la _grid est niqué et si il a des gosses, si cest le cas on les reattribue 
        if(_grid == null && transform.childCount > 0)
        {
            Debug.LogWarning("[GridManager] La table de grid a du être regen");
            _grid = new Case[SizeX, SizeY];
        
            for(int i = 0 ; i < transform.childCount; i++)
            {
                Transform aCase = transform.GetChild(i);
                Case newCase = aCase.GetComponent<Case>();
                _grid[newCase.x, newCase.y] = newCase;

                Debug.Log($" WTF : {newCase.x} {newCase.y}");
            }
        }
    }

    /*
        !! Surement va etre supprimer car les cases sont maintenant des gameobject
        Permet d'avoir la position de la grille dans le world,
        Necessaire pour certaines situations et vue que la Case n'est pas un gameobject, il y a cette function de disponible
    */

    public Vector3 GetCaseWorldPosition(int x, int y) 
    {

        return new Vector3(x , 0 ,y) * CellSize ;
    }

     public static Vector3 GetCaseWorldPosition(Case caseToCheck) 
    {
        
        return new Vector3(caseToCheck.It.x , 0 ,caseToCheck.It.y) * caseToCheck._gridParent.CellSize ;
    }
    /*
        WIP
        Cette function permet de récupérer une case dans la grille
    */
    public Case GetCase(int x, int y)
    {
        if(x >= SizeX || y >= SizeY || x < 0 || y < 0)
            return null;

        return _grid[x,y];
    }
    public static Case GetCase(GridManager gridParent ,int x, int y)
    {
        if(x >= gridParent.SizeX || y >= gridParent.SizeY || x < 0 || y < 0)
            return null;

        return gridParent._grid[x,y];
    }

    /*
        Comme son nom l'indique cette function permet de get tout les adjacentes cases
    */
    public static Case[] GetAdjacentCases( Case CurrentCase)
    {
        Case[] cases = new Case[8];
        GridManager gridParent = CurrentCase._gridParent;
        // les cases adjacentes
        cases[0] = GetCase(gridParent, CurrentCase.x,CurrentCase.y+1);
        cases[1] = GetCase(gridParent, CurrentCase.x,CurrentCase.y-1);
        cases[2] = GetCase(gridParent, CurrentCase.x+1,CurrentCase.y);
        cases[3] = GetCase(gridParent, CurrentCase.x-1,CurrentCase.y);
        //Diagonal
        cases[4] = GetCase(gridParent, CurrentCase.x-1,CurrentCase.y+1);
        cases[5] = GetCase(gridParent, CurrentCase.x-1,CurrentCase.y-1);
        cases[6] = GetCase(gridParent, CurrentCase.x+1,CurrentCase.y+1);
        cases[7] = GetCase(gridParent, CurrentCase.x+1,CurrentCase.y-1); 
        return cases;
    }

    // Update is called once per frame
    void Update()
    {
        RegenerateCaseTable(); // Existe car entre le edit et runtime la table a double entrer foire // TODO : trouver une autre maniere
        if(GenerateAGrid)
        {
            GenerateGrid();
            GenerateAGrid = false;
        }
        if(ResetGrid)
        {
            ClearGrid();
            ResetGrid = false;
        }

        // WatchCursor();     
    }


    // Deplacer dans GridEditorTool.cs
    public void EditCase(Vector3 pos , CaseState caseState)
    {
        int x = (int)pos.x/CellSize;
        int y = (int)pos.z/CellSize;

        //Case AimCase = GetValidCase(GetCase(x,y));
        //SelectModeWatcher(AimCase);
        //Debug.Log(x+" ; "+y);
        Case caseToEdit = GetCase( x , y );
        caseToEdit.state = caseState;
                    //SaveGridToData(); // TODO : Plus besoin de sauvegarder car cest directement save dans la scene    
        
    }


    void UpdateGrid()
    {
        Debug.DrawLine(GetCaseWorldPosition(  0,  SizeY) , GetCaseWorldPosition(SizeX , SizeY ), Color.red);
        Debug.DrawLine(GetCaseWorldPosition(SizeX,  0) , GetCaseWorldPosition(SizeX , SizeY), Color.red);

    }

    /*
        Cette function va verifier si la case est selectionnable
    */
    public static Case GetValidCase( Case caseToCheck)
    {   
        if(caseToCheck == null)
            return null;

        if(caseToCheck.state == CaseState.Occupied || caseToCheck.state == CaseState.Null)
            return null;

        return caseToCheck;
    }

    /*
        Regarde ce que la souris touche
    */

    // void WatchCursor()
    // {   
    //     Vector3 mousePos = MouseToWorldPosition();
    //     int x = (int)mousePos.x/CellSize;
    //     int y = (int)mousePos.z/CellSize;

    //     Case AimCase = GetValidCase(GetCase(x,y));
    //     SelectModeWatcher(AimCase);    
    // }

    /*
        Cette function genere un recast et renvoi le object toucher 
        TODO : Cette function sera mi dans le controller du player
    */

    // Vector3 MouseToWorldPosition()
    // {
    //     RaycastHit RayHit;
    //     Ray ray;
    //     GameObject ObjectHit;
    //     Vector3 Hitpoint = Vector3.zero;
    //     ray = Camera.main.ScreenPointToRay(_inputManager.TestGrid.MousePosition.ReadValue<Vector2>() ); 
    //     if (Physics.Raycast(ray, out  RayHit))
    //     {
    //         ObjectHit = RayHit.transform.gameObject;
    //         Hitpoint = new Vector3((int)RayHit.point.x,(int)RayHit.point.y,(int)RayHit.point.z);
    //         if (ObjectHit != null)
    //             Debug.DrawLine(Camera.main.transform.position, Hitpoint, Color.blue, 0.5f);

    //     }

    //     return Hitpoint;
    // }


    /*
        
    */
    // void SelectModeWatcher(Case AimCase)
    // {
    //     if(SelectMode)
    //         {
    //             if(_inputManager.TestGrid.Action.ReadValue<float>() == 1)
    //             {
    //                 if(SelectedCaseA == null)
    //                 {
    //                     SelectedCaseA = AimCase;
    //                     SelectedCaseA.Highlighted = true;
    //                     SelectedCaseA.ChangeMaterial(Data.caseHighlight);
    //                     return;
    //                 }
    //                 else
    //                 {
    //                     SelectedCaseB = AimCase;
    //                     SelectedCaseB.Highlighted = true;
    //                     SelectedCaseB.ChangeMaterial(Data.caseHighlight);
    //                     //UIManager.CreateHintString(AimCase.gameObject, "XCOM HINTSTRING OUAAHHH");
    //                 }

    //                 if(SelectedCaseA._actor != null)
    //                 {
    //                     SelectedCaseA._actor.Destination = SelectedCaseB;
    //                 }
    //                 else
    //                 {
    //                     PathFinding.FindPath(SelectedCaseA, SelectedCaseB );                  
    //                 }

    //             }
    //             if(_inputManager.TestGrid.Echap.ReadValue<float>() == 1)
    //             {
    //                 SelectedCaseA.Highlighted = false;
    //                 SelectedCaseA = null;
    //                 SelectedCaseB.Highlighted = false;
    //                 SelectedCaseB = null;
    //             }
    //         }
    // }

    /*
        Permet de sauvegarder la grid dans le fichier data mais cest peut etre inutile donc a voir
    */
    void SaveGridToData()
    {
        CaseInfo[] newCases = new CaseInfo[SizeX * SizeY];
        for(int xi = 0 ; xi < SizeX; xi++)
        {
            for(int yi = 0 ; yi < SizeY; yi++)
            {
                Debug.Log(_grid[xi,yi].index);
                newCases[_grid[xi,yi].index] = _grid[xi,yi].It;              
            }
        }
        Data.Grid = newCases;
    }

    
        

}


