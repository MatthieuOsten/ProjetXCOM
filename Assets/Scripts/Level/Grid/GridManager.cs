using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class GridManager : MonoBehaviour
{
    [Header("GRID PARAMETERS")]
    [SerializeField] SO_Grid DataGrid; 
    [SerializeField] public int gridSizeX = 10; // Size of the grid in X
    [SerializeField] public int gridSizeY = 10; // Size of the grid in Y 
    public Case[,] _grid; // A table with double entry with x and y, with for each element the type Case
    public int cellSize = 10; // Size of the cell in the world
    public int heuristicScale = 8;
    public int heuristicScaleDiagonale = 4;
    

    [Header("EDIT")]
    [SerializeField] bool Editmode;
    public Vector3 CurrentCellSelected;
    [SerializeField] CaseState CaseNewChanged;

    int _currentCellCreated = 0; // a counter of cell created
    public GameObject _plane;


    [Header("PATH FINDING")]
    [SerializeField] bool RandomStartAndGoal;
    
    public Case StartCase;
    public Case Destination;


    // Start is called before the first frame update
    void Start()
    {
        
        GenerateGrid();
    }

    /*
        Cette fonction va generer la grille, et chaque cellule sera une entity de type Case
    */
    void GenerateGrid()
    {
         if(DataGrid.Grid.Length > 0)
            _grid = CopyGridFromData();
         else
         {
            _grid = new Case[gridSizeX, gridSizeY];
            for(int x = 0 ; x < gridSizeX; x++)
            {
                for(int y = 0 ; y < gridSizeY; y++)
                {
                    _grid[x,y] = new Case();
                    _grid[x,y].Point = new Case.GridPoint(x,y); // Give this coordinate in the grid
                    _grid[x,y].GridParent = this; // give the ref of the grid
                    _grid[x,y].index = _currentCellCreated; 
                    _grid[x,y]._state = CaseState.Empty;
                    GameObject plane = Instantiate(_plane, GetCaseWorldPosition(x, y)  , Quaternion.identity );
                        plane.transform.localScale = plane.transform.localScale * cellSize;
                        _grid[x,y].plane =  plane;

                    _grid[x,y].mtl =  plane.transform.GetComponent<MeshRenderer>().material;
                    // if(_currentCellCreated % 2 == 0)
                    // {
                    //     _grid[x,y]._state = CaseState.Occupied;
                    //     plane.transform.GetComponent<MeshRenderer>().material.color = Color.red;
                    // }
                        


                    _currentCellCreated++;
                }
            }
         }
           
        
        
       

    }

    Case[,] CopyGridFromData()
    {
        Case[] dataCase = DataGrid.Grid;
        _grid = new Case[DataGrid.gridSizeX, DataGrid.gridSizeY];
        for(int x = 0 ; x < gridSizeX; x++)
        {
            for(int y = 0 ; y < gridSizeY; y++)
            {
                _grid[x,y] = dataCase[_currentCellCreated];
                _grid[x,y].Point = dataCase[_currentCellCreated].Point; // Give this coordinate in the grid
                _grid[x,y].GridParent = this; // give the ref of the grid
                _grid[x,y].index = _currentCellCreated; 
                _grid[x,y]._state = dataCase[_currentCellCreated]._state;
                GameObject plane = Instantiate(_plane, GetCaseWorldPosition(x, y)  , Quaternion.identity );
                plane.transform.localScale = plane.transform.localScale * cellSize;
                _grid[x,y].plane =  plane;
                _grid[x,y].mtl =  plane.transform.GetComponent<MeshRenderer>().material;
                _currentCellCreated++;
            }
        }
        return _grid;

    }

    void CreateCase()
    {

    }

    /*
        Get the world position of the case 
    */

    public Vector3 GetCaseWorldPosition(int x, int y) 
    {
        return new Vector3(x , 0 ,y) * cellSize ;
    }
    
    public Case GetCase(int x, int y)
    {
        if(x >= gridSizeX || y >= gridSizeY || x < 0 || y < 0)
            return null;

        return _grid[x,y];
    }

    // Update is called once per frame
    void Update()
    {
        if(RandomStartAndGoal)
        {
            for(int x = 0 ; x < gridSizeX; x++)
            {
                for(int y = 0 ; y < gridSizeY; y++)
                {
                    _grid[x,y].Checked = false;
                    _grid[x,y].BlackList = false;
                    _grid[x,y].Highlighted = false;
                    _grid[x,y].PointCase = false;
                    _grid[x,y].goodPath = false;
                    
                }
            }
            StartCase.PointCase = false;
            Destination.PointCase = false;
            StartCase = _grid[Random.Range(0, gridSizeX) , Random.Range(0,gridSizeY)];
            Destination = _grid[Random.Range(0, gridSizeX) , Random.Range(0,gridSizeY)];
            StartCase.PointCase = true;
            Destination.PointCase = true;
            RandomStartAndGoal = false;
        }

        UpdateGrid();
       
        if(StartCase.index != -1 && Destination.index != -1)
        {
            FindPath();
            StartCase = null;
            Destination = null;
        }

         WatchCursor();
    }


    /*
        function heuristic(node) =
        dx = abs(node.x - goal.x)
        dy = abs(node.y - goal.y)
        return D * (dx + dy) + (D2 - 2 * D) * min(dx, dy)
    */

    int GetScore(int nodeAx, int nodeAy, int nodeBx, int nodeBy)
    {
        int dx = Mathf.Abs(nodeAx - nodeBx);
        int dy = Mathf.Abs(nodeAy - nodeBy);
        return heuristicScale * (dx + dy) + (heuristicScaleDiagonale - 2 * heuristicScale) * Mathf.Min(dx, dy);

    }
    void FindPath()
    {
       

        int CurrentScore = 0;

        Case CurrentCase = StartCase; 

        for(int x = 0 ; x < gridSizeX; x++)
        {
            for(int y = 0 ; y < gridSizeY; y++)
            {
                _grid[x,y].PathFindDistanceFromStart = GetScore(StartCase.Point.x, StartCase.Point.y, x, y);
                _grid[x,y].PathFindDistanceFromEnd = GetScore(Destination.Point.x, Destination.Point.y, x, y) ;
                //Debug.Log("Case : "+_grid[x,y].index+" score : "+(_grid[x,y].PathFindDistanceFromEnd+_grid[x,y].PathFindDistanceFromStart) );
                _grid[x,y].plane.name = "Case : "+_grid[x,y].index+" score : "+(_grid[x,y].PathFindDistanceFromEnd +" + "+_grid[x,y].PathFindDistanceFromStart) ;
            }
        }
        Case SmallestCase = null;
    
        while(true)
        {
            Case[] cases = new Case[9];
            cases[0] = GetCase(CurrentCase.Point.x,CurrentCase.Point.y);
            cases[1] = GetCase(CurrentCase.Point.x,CurrentCase.Point.y+1);
            cases[2] = GetCase(CurrentCase.Point.x-1,CurrentCase.Point.y+1);
            cases[3] = GetCase(CurrentCase.Point.x-1,CurrentCase.Point.y-1);
            cases[4] = GetCase(CurrentCase.Point.x+1,CurrentCase.Point.y+1);
            cases[5] = GetCase(CurrentCase.Point.x+1,CurrentCase.Point.y-1);   
            cases[6] = GetCase(CurrentCase.Point.x,CurrentCase.Point.y-1);
            cases[7] = GetCase(CurrentCase.Point.x+1,CurrentCase.Point.y);
            cases[8] = GetCase(CurrentCase.Point.x-1,CurrentCase.Point.y);
            
            SmallestCase = cases[1];

            for(int i = 0 ; i < cases.Length; i++)
            {
                if( cases[i] == null || cases[i].BlackList || cases[i] == StartCase || cases[i] == CurrentCase )
                {
                    continue; 
                }
                    

                int Prevscore = SmallestCase.PathFindDistanceFromEnd+SmallestCase.PathFindDistanceFromStart;
                int newScore = cases[i].PathFindDistanceFromEnd+cases[i].PathFindDistanceFromStart;

                if(!cases[i].Checked ) 
                {
                    if(Prevscore == newScore && SmallestCase.PathFindDistanceFromStart > cases[i].PathFindDistanceFromStart)
                    {
                        SmallestCase = cases[i];
                        cases[i].Highlighted = true;
                        cases[i].Checked = true;
                    }
                    if(Prevscore >= newScore  )
                    {
                        SmallestCase = cases[i];
                    
                        cases[i].Highlighted = true;
                        cases[i].Checked = true;
                        
                    }
                }
                
                //else if()
                

            }
             if(CurrentCase == Destination)
            {
                Debug.Log("Destination done");
                break;
            }  

            if(CurrentCase != SmallestCase)
            {
                CurrentCase = SmallestCase;
                 CurrentCase.goodPath = true;
            } 
            else
            {
                
                CurrentCase.BlackList = true;
                CurrentCase = StartCase;
                for(int x = 0 ; x < gridSizeX; x++)
                {
                    for(int y = 0 ; y < gridSizeY; y++)
                    {
                        _grid[x,y].goodPath = false;
                    }
                }
            }

             
            Destination.goodPath = false;
            Destination.Highlighted = false;
                
            
        }

        
    }

    void UpdateGrid()
    {
        for(int x = 0 ; x < gridSizeX; x++)
        {
            for(int y = 0 ; y < gridSizeY; y++)
            {
                //Case acase = GetCase(x, y);
                _grid[x,y].UpdateCell();               
            }
        }
        Debug.DrawLine(GetCaseWorldPosition(  0,  gridSizeY) , GetCaseWorldPosition(gridSizeX , gridSizeY ), Color.red);
        Debug.DrawLine(GetCaseWorldPosition(gridSizeX,  0) , GetCaseWorldPosition(gridSizeX , gridSizeY), Color.red);

    }

    void WatchCursor()
    {
        RaycastHit RayHit;
        Ray ray;
        GameObject ObjectHit;
        Vector3 Hitpoint = Vector3.zero;
        //Debug.Log(Input.mousePosition);

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);  

        if (Physics.Raycast(ray, out  RayHit))
        {
            ObjectHit = RayHit.transform.gameObject;
            Hitpoint = new Vector3((int)RayHit.point.x,(int)RayHit.point.y,(int)RayHit.point.z);
            if (ObjectHit != null)
                Debug.DrawLine(Camera.main.transform.position, Hitpoint, Color.blue, 0.5f);
 
            int x = (int)Hitpoint.x/cellSize;
            int y = (int)Hitpoint.z/cellSize;

            Debug.DrawLine(GetCaseWorldPosition(x  ,  y) , GetCaseWorldPosition(x , y+1 ), Color.green);
            Debug.DrawLine(GetCaseWorldPosition((int)Hitpoint.x /cellSize,  y) , GetCaseWorldPosition(x+1 , y), Color.green);
            
            

            if(Editmode && Input.GetKeyDown(KeyCode.Mouse0))
            {
                Case caseToEdit = GetCase( x , y );
                caseToEdit._state = CaseNewChanged;

                Case[] newCases = new Case[gridSizeX * gridSizeY];
                for(int xi = 0 ; xi < gridSizeX; xi++)
                {
                    for(int yi = 0 ; yi < gridSizeY; yi++)
                    {
                        Debug.Log(_grid[xi,yi].index);
                        newCases[_grid[xi,yi].index] = _grid[xi,yi];              
                    }
                }


                DataGrid.Grid = newCases;
            }

            Debug.Log(GetCase(x,y)._state);

        }

    }
}
