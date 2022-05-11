using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class GridManager : MonoBehaviour
{
    [Header("GRID PARAMETERS")]
    [SerializeField] public int gridSizeX = 10; // Size of the grid in X
    [SerializeField] public int gridSizeY = 10; // Size of the grid in Y 
    public Case[,] _grid; // A table with double entry with x and y, with for each element the type Case
    public int cellSize = 10; // Size of the cell in the world
    int _currentCellCreated = 0; // a counter of cell created

    // Start is called before the first frame update
    void Start()
    {

        GenerateGrid();
    }

    /*
        This function will generate the grid, and create for each iteration a case 
    */
    void GenerateGrid()
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
                _currentCellCreated++;
            }
        }

        // for(int x = 0 ; x < gridSizeX; x++)
        // {
        //     for(int y = 0 ; y < gridSizeY; y++)
        //     {
        //         Debug.Log( _grid[x,y].index);
        //     }
        // }
    }

    /*
        Get the world position of the case 
    */

    public Vector3 GetCaseWorldPosition(int x, int y) 
    {
        return new Vector3(x , y) * cellSize ;
    }
    

    // Update is called once per frame
    void Update()
    {
        for(int x = 0 ; x < gridSizeX; x++)
        {
            for(int y = 0 ; y < gridSizeY; y++)
            {
                _grid[x,y].UpdateCell();
                Debug.DrawLine(GetCaseWorldPosition(x  ,  y) , GetCaseWorldPosition(x , y+1 ), Color.red);
                Debug.DrawLine(GetCaseWorldPosition(x ,  y) , GetCaseWorldPosition(x+1 , y), Color.red);
            }
        }
        Debug.DrawLine(GetCaseWorldPosition(  0,  gridSizeY) , GetCaseWorldPosition(gridSizeX , gridSizeY ), Color.red);
        Debug.DrawLine(GetCaseWorldPosition(gridSizeX,  0) , GetCaseWorldPosition(gridSizeX , gridSizeY), Color.red);


            RaycastHit RayHit;
            Ray ray;
            GameObject ObjectHit;
            Vector3 Hitpoint = Vector3.zero;

           ray = Camera.main.ScreenPointToRay(Input.mousePosition);                    
                 if (Physics.Raycast(ray, out  RayHit))
                 {
                     ObjectHit = RayHit.transform.gameObject;
                     Hitpoint = new Vector3((int)RayHit.point.x,(int)RayHit.point.y);
                     if (ObjectHit != null)
                         Debug.DrawLine(Camera.main.transform.position, Hitpoint, Color.blue, 0.5f);


                    // Debug.Log(Hitpoint);    

                     Debug.DrawLine(GetCaseWorldPosition((int)Hitpoint.x/cellSize  ,  (int)Hitpoint.y/cellSize) , GetCaseWorldPosition((int)Hitpoint.x/cellSize , (int)Hitpoint.y/cellSize+1 ), Color.green);
                        Debug.DrawLine(GetCaseWorldPosition((int)Hitpoint.x /cellSize,  (int)Hitpoint.y/cellSize) , GetCaseWorldPosition((int)Hitpoint.x/cellSize+1 , (int)Hitpoint.y/cellSize), Color.green);

                    Debug.Log((int)Hitpoint.x/cellSize+","+(int)Hitpoint.y/cellSize);

                    Debug.Log( _grid[(int)Hitpoint.x/cellSize,(int)Hitpoint.y/cellSize].index );

                 
                }
    }
}
