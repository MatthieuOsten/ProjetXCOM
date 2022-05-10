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

    

    [Header("EDIT")]
    [SerializeField] bool Editmode;
    public Vector3 CurrentCellSelected;
    [SerializeField] CaseState CaseNewChanged;

    int _currentCellCreated = 0; // a counter of cell created
    public GameObject _plane;
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
        return _grid[x,y];
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGrid();
        WatchCursor();

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
