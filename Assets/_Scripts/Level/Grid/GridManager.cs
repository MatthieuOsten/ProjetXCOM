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
    [SerializeField] Case SelectedCaseA, SelectedCaseB;

    int _currentCellCreated = 0; // Le nombre de case crée

    [Header("EDIT")]
    [SerializeField] bool GenerateAGrid, ResetGrid;
    [SerializeField] bool Editmode, SelectMode;
    public Vector3 CurrentCellSelected;
    [SerializeField] CaseState CaseNewChanged;

    // Va etre deplacer dans le script pathfinding
    [Header("PATH FINDING")]
    public int heuristicScale = 4; 
    public int heuristicScaleDiagonale = 8;
    [SerializeField] bool RandomStartAndGoal;
    public Case StartCase, Destination;

    [Header("Debug")]
    [SerializeField] private Controller _inputManager;


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
        _inputManager = new Controller();
        _inputManager.TestGrid.Enable();
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

         if(Data.Grid.Length > 0) // Si une grid est stocké dans Data, on la reprend au lieu d'en generer
            _grid = CopyGridFromData();
         else
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
                //_grid[x,y].plane =  plane;
                //_grid[x,y].mtl =  plane.transform.GetComponent<MeshRenderer>().material;
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
            Debug.LogError("[GridManager] La table de grid a du être regen");
            _grid = new Case[SizeX, SizeY];
        
            for(int i = 0 ; i < transform.childCount; i++)
            {
                Transform aCase = transform.GetChild(i);
                Case newCase = aCase.GetComponent<Case>();
                _grid[newCase.x, newCase.y] = newCase;

                Debug.Log($" WTF : {newCase.x} {newCase.y}");
            }
        }
        //else
            //Debug.Log("[GridManager] La table de grid est ok.");
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

    // Update is called once per frame
    void Update()
    {
        RegenerateCaseTable();
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
        if(RandomStartAndGoal)
        {
           
            StartCase.PointCase = false;
            Destination.PointCase = false;
            StartCase = _grid[Random.Range(0, SizeX) , Random.Range(0,SizeY)];
            Destination = _grid[Random.Range(0, SizeX) , Random.Range(0,SizeY)];
            if(StartCase.state == CaseState.Occupied)
                StartCase = null;
            
            if(Destination.state == CaseState.Occupied)
                Destination = null;
            StartCase.PointCase = true;
            Destination.PointCase = true;
            RandomStartAndGoal = false;
        }

        //UpdateGrid();
        WatchCursor();

        if(StartCase != null && Destination != null)
        { 
            //FindPath();
            //StartCase = null;
            //Destination = null;
        }

        
    }


    void UpdateGrid()
    {
        for(int x = 0 ; x < SizeX; x++)
        {
            for(int y = 0 ; y < SizeY; y++)
            {
                //Case acase = GetCase(x, y);
                //GetCase(x, y).UpdateCell();               
            }
        }
        Debug.DrawLine(GetCaseWorldPosition(  0,  SizeY) , GetCaseWorldPosition(SizeX , SizeY ), Color.red);
        Debug.DrawLine(GetCaseWorldPosition(SizeX,  0) , GetCaseWorldPosition(SizeX , SizeY), Color.red);

    }

    /*
        Cette function va verifier si la case est selectionnable
    */
    Case GetValidCase( Case caseToCheck)
    {   
        if(caseToCheck == null)
            return null;

        if(caseToCheck.state == CaseState.Occupied || caseToCheck.state == CaseState.Null)
            return null;

        return caseToCheck;
    }

    void WatchCursor()
    {
        RaycastHit RayHit;
        Ray ray;
        GameObject ObjectHit;
        Vector3 Hitpoint = Vector3.zero;
        //Debug.Log(Input.mousePosition);

        ray = Camera.main.ScreenPointToRay(_inputManager.TestGrid.MousePosition.ReadValue<Vector2>() ); 

        if (Physics.Raycast(ray, out  RayHit))
        {
            ObjectHit = RayHit.transform.gameObject;
            Hitpoint = new Vector3((int)RayHit.point.x,(int)RayHit.point.y,(int)RayHit.point.z);
            if (ObjectHit != null)
                Debug.DrawLine(Camera.main.transform.position, Hitpoint, Color.blue, 0.5f);
 
            int x = (int)Hitpoint.x/CellSize;
            int y = (int)Hitpoint.z/CellSize;


          //  Debug.Log(GetCase(x,y).x +" ; "+GetCase(x,y).y);

          //  Debug.DrawLine(GetCaseWorldPosition(x  ,  y) , GetCaseWorldPosition(x , y+1 ), Color.green);
          //  Debug.DrawLine(GetCaseWorldPosition((int)Hitpoint.x /CellSize,  y) , GetCaseWorldPosition(x+1 , y), Color.green);
            

            Case AimCase = GetValidCase(GetCase(x,y));


            if(SelectMode && _inputManager.TestGrid.Action.ReadValue<float>() == 1)
            {

                if(SelectedCaseA != AimCase && SelectedCaseB != null)
                {
                    if(SelectedCaseA != null)
                        SelectedCaseA.Highlighted = false;

                    SelectedCaseA = AimCase;
                    SelectedCaseA.Highlighted = true;
                    SelectedCaseA.ChangeMaterial(Data.caseHighlight);
                    return;
                }    
                else if(SelectedCaseB != AimCase && SelectedCaseA != AimCase)
                {
                    if(SelectedCaseB != null)
                        SelectedCaseB.Highlighted = false;

                    SelectedCaseB = AimCase;
                    SelectedCaseB.Highlighted = true;
                    SelectedCaseB.ChangeMaterial(Data.caseHighlight);
                    return;
                }
                 StartCase = SelectedCaseA;
                Destination = SelectedCaseB;
                FindPath();
                //SelectMode = false;
                //StartCase = null;
                //Destination = null;
                    
            }

            if(Editmode && _inputManager.TestGrid.Action.ReadValue<float>() == 1)
            {
                Case caseToEdit = GetCase( x , y );
                caseToEdit.state = CaseNewChanged;
                SaveGridToData();
                
            }

//            Debug.Log(GetCase(x,y).state);

        }

    }
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

    
    /*
    
        PATH FINDING WORK IN PROGRESS
        Pour linstant tout est dans GridManager mais tout cela sera amener vers une nouvelle class, pour linstant cest en travaux
    
    */

    /*
        function heuristic(node) =
        dx = abs(node.x - goal.x)
        dy = abs(node.y - goal.y)
        return D * (dx + dy) + (D2 - 2 * D) * min(dx, dy)
    */

    int SetScore(int nodeAx, int nodeAy, int nodeBx, int nodeBy, bool isDiagonal = true)
    {
        int dx = Mathf.Abs(nodeAx - nodeBx);
        int dy = Mathf.Abs(nodeAy - nodeBy);
        //if(isDiagonal)
            return heuristicScale * (dx + dy) + (heuristicScaleDiagonale - 2 * heuristicScale) * Mathf.Min(dx, dy);
        //else
        //    return heuristicScale * (dx + dy) + (heuristicScale) * Mathf.Min(dx, dy);

    }
    int GetTotalScore(Case aCase)
    {
        return aCase.hCost+aCase.gCost;
    }


    // Function qui s'occupe de trouver le chemin le plus court
    void FindPath()
    {
        // Permet d'éviter des hard crash
        if(GetValidCase(StartCase) == null || GetValidCase(Destination) == null )
        {
            Debug.LogWarning("PathFinding : Attention l'un des deux point est une case non valide");
        }

        // Reset toutes les cases de prévisualisation
        for(int x = 0 ; x < SizeX; x++)
        {
            for(int y = 0 ; y < SizeY; y++)
            {
                    if(_grid[x,y] != Destination && _grid[x,y] != StartCase)
                    {
                        _grid[x,y].Checked = false;
                        _grid[x,y].BlackList = false;
                        _grid[x,y].Highlighted = false;
                        _grid[x,y].PointCase = false;
                        _grid[x,y].goodPath = false;
                        _grid[x,y].hCost = 0;
                        _grid[x,y].gCost = 0;
                        _grid[x,y].ChangeMaterial(Data.caseDefault);
                    }
                    
                    
            }
        }

        Case CurrentCase = StartCase; 
        //Case SmallestCase = null;  
        int iteration = 1000000; // Securiter car encore des crash
           Case TheLowestCase;

        
        Case[] GetVoisinsamere( Case CurrentCase)
        {
            Case[] cases = new Case[8];
            // les cases adjacentes
            cases[0] = GetCase(CurrentCase.x,CurrentCase.y+1);
            cases[1] = GetCase(CurrentCase.x,CurrentCase.y-1);
            cases[2] = GetCase(CurrentCase.x+1,CurrentCase.y);
            cases[3] = GetCase(CurrentCase.x-1,CurrentCase.y);
            //Diagonal
            cases[4] = GetCase(CurrentCase.x-1,CurrentCase.y+1);
            cases[5] = GetCase(CurrentCase.x-1,CurrentCase.y-1);
            cases[6] = GetCase(CurrentCase.x+1,CurrentCase.y+1);
            cases[7] = GetCase(CurrentCase.x+1,CurrentCase.y-1); 
            return cases;
        }
        
        
        List<Case> openSet = new List<Case>();
        HashSet<Case> closedSet = new HashSet<Case>();
        openSet.Add(StartCase);



        while(openSet.Count > 0)
        {
            Case currentNode = openSet[0];
            for(int i = 1 ; i < openSet.Count; i++)
            {
                if(openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost )
                {
                    
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);
             
            if(currentNode == Destination)
            {
                RetracePath(StartCase, Destination);
                return;
            }

            foreach( Case voisin in GetVoisinsamere(currentNode))
            {
                if( voisin == null || voisin.state != CaseState.Empty || closedSet.Contains(voisin) )
                {
                    continue;
                }

                int newMovementCostToNeighbour = (int) currentNode.gCost + (int) Vector3.Distance(currentNode.transform.position, voisin.transform.position);
                if(newMovementCostToNeighbour < voisin.gCost || !openSet.Contains(voisin))
                {
                    voisin.gCost = newMovementCostToNeighbour;
                    voisin.hCost = (int)Vector3.Distance(voisin.transform.position, Destination.transform.position);
                    voisin.parentCase = currentNode;
                    voisin.Checked = true;
                    if(!openSet.Contains(voisin))
                        openSet.Add(voisin);
                }
            }
            
            /*
            TheLowestCase = null;
            iteration--;
            Case[] cases = new Case[8];
            // les cases adjacentes
            cases[0] = GetCase(CurrentCase.x,CurrentCase.y+1);
            cases[1] = GetCase(CurrentCase.x,CurrentCase.y-1);
            cases[2] = GetCase(CurrentCase.x+1,CurrentCase.y);
            cases[3] = GetCase(CurrentCase.x-1,CurrentCase.y);
            //Diagonal
            cases[4] = GetCase(CurrentCase.x-1,CurrentCase.y+1);
            cases[5] = GetCase(CurrentCase.x-1,CurrentCase.y-1);
            cases[6] = GetCase(CurrentCase.x+1,CurrentCase.y+1);
            cases[7] = GetCase(CurrentCase.x+1,CurrentCase.y-1); 
            
            for(int i = 0 ; i < cases.Length; i++)
            {
                if(GetValidCase(cases[i]) != null && cases[i] != CurrentCase && !cases[i].BlackList && cases[i] != CurrentCase.parentCase )  // Verifie si la case est valide
                {   
                    cases[i].gCost = SetScore(StartCase.x, StartCase.y, cases[i].x, cases[i].y );
                    cases[i].hCost = SetScore(Destination.x, Destination.y, cases[i].x, cases[i].y ) ;
                }
                else // on check les suivantes
                    continue;

                if(TheLowestCase == null) // Si aucune case de choix existe et beh on lui attribue une par defaut
                {
                     TheLowestCase = cases[i];
                }
                // si le score de la precedente est plus grande que la nouvelle cest que la nouvelle case est plus petite
                else if(GetTotalScore(TheLowestCase) > GetTotalScore(cases[i])) 
                {
                    cases[i].ChangeMaterial(Data.caseNone);
                    TheLowestCase = cases[i];
                }
                else if(GetTotalScore(TheLowestCase) == GetTotalScore(cases[i]) && TheLowestCase.gCost < cases[i].gCost)
                {
                    cases[i].ChangeMaterial(Data.caseNone);
                    TheLowestCase = cases[i];
                }
                    

                // 
            }
            if(TheLowestCase == null)
            {
                CurrentCase.BlackList = true;
                CurrentCase = CurrentCase.parentCase;
                continue;
            }
            TheLowestCase.Checked = true;
            TheLowestCase.parentCase = CurrentCase;
            CurrentCase = TheLowestCase;
            // // for(int i = 1 ; i < cases.Length; i++)
            // // {
            // //     if(GetValidCase(cases[i])  != null)
            // //     {
            // //         SmallestCase = cases[i];
            // //     }
            // // }
            // // if(SmallestCase == null)
            // // {
            // //     Debug.LogWarning("PathFinding : Potentiel crash, smallestcase n'a pas ete defini parmi toutes les cases adjacentes");
            // //     break;
            // // }
            // //SmallestCase = cases[1];

            // for(int i = 1 ; i < cases.Length; i++)
            // {
            //     /* On verifie si la case provenant des cases adjacente, est valide
            //     on checkant si elle n'est pas null, pas blacklist, qu'elle nest pas la case du début, si ce n'est pas la case main, ou si son etat nest pas
            //     occupied  
            //     */
            //     if( cases[i] == null || cases[i].BlackList || cases[i] == StartCase || cases[i] == CurrentCase || cases[i].state == CaseState.Occupied
            //     || CurrentCase == cases[i].parentCase)
            //     {
            //         if( cases[i] != null && cases[i].state == CaseState.Occupied)
            //             Debug.LogWarning("Une case etait occuper");

            //         Debug.Log("go skip");
            //         continue; 
            //     }
            //     // la case est valide on recupere ces coordonnée
            //     int x = cases[i].x;
            //     int y = cases[i].y;
            //     // on fait les calculs
            //     bool isDiagonal = i > 4;
               
            //         cases[i].gCost = SetScore(StartCase.x, StartCase.y, x, y ,isDiagonal);
            //         cases[i].hCost = SetScore(Destination.x, Destination.y, x, y ,isDiagonal) ;
                
                
            //     //cases[i].gameObject.name = "Case : "+cases[i].index+" score : "+(cases[i].hCost +" "+(cases[i].hCost+cases[i].gCost)+"+ "+cases[i].gCost) ;

            //     // on verifie la main case 
            //     int Prevscore;  
            //     if(SmallestCase == null)
            //     {
            //         Prevscore = 99999;
            //     }
            //     else
            //         Prevscore = SmallestCase.hCost+SmallestCase.gCost;
                
            //     int newScore = cases[i].hCost+cases[i].gCost;

            //     if(!cases[i].Checked ) 
            //     {
            //         if(Prevscore == newScore && SmallestCase.gCost < cases[i].gCost)
            //         {
            //             cases[i].parentCase = SmallestCase;
            //              SmallestCase = cases[i];


            //              cases[i].Checked = true;
            //         }
            //         else if(Prevscore > newScore  )
            //         {
            //             cases[i].parentCase = SmallestCase;
            //             SmallestCase = cases[i];
            //             cases[i].Checked = true;
                        
            //         }
            //     }
                
                
                

            // }
            //  if(CurrentCase == Destination)
            // {
            //     //Destination.Highlighted = true;
            //     Debug.Log("Destination done");
                
            // }  

            // if(CurrentCase != SmallestCase)
            // {
            //     CurrentCase = SmallestCase;
            //     CurrentCase.goodPath = true;
            // } 
            // else
            // {
            //     // Si on arrive la c'est que la current case dans laquelle nous checkons, n'amene à aucun bon chemin
            //     // Dans ce cas le chemin est considéré comme invalide
            //     CurrentCase.BlackList = true; // On black list la case pour dire que ca sert à rien de la checker
            //     CurrentCase = CurrentCase.parentCase;
            //     for(int x = 0 ; x < SizeX; x++)
            //     {
            //         for(int y = 0 ; y < SizeY; y++)
            //         {
            //             _grid[x,y].goodPath = false;
            //             //_grid[x,y].Checked = false;
            //         }
            //     }
            // }

             
            // Destination.goodPath = false;
            // Destination.Highlighted = true;
                
            
        }

        void RetracePath(Case StartNode, Case endNode)
        {
            List<Case> path = new List<Case>();
            Case currentNode = endNode;
            while(currentNode != StartNode)
            {
                currentNode.Checked = true;
                currentNode.ChangeMaterial(Data.caseNone);
                path.Add(currentNode);
                currentNode = currentNode.parentCase;

            }
            path.Reverse();

            
        }
        

    }

}
