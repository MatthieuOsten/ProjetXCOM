using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    [Header("PATH FINDING")]
    static int heuristicScale = 4; 
    static int heuristicScaleDiagonale = 8;

    /*
        function heuristic(node) =
        dx = abs(node.x - goal.x)
        dy = abs(node.y - goal.y)
        return D * (dx + dy) + (D2 - 2 * D) * min(dx, dy)

        Cette function permet de get la distance entre un point a et point b dans une grille
    */
    static int GetScore(int nodeAx, int nodeAy, int nodeBx, int nodeBy)
    {
        int dx = Mathf.Abs(nodeAx - nodeBx);
        int dy = Mathf.Abs(nodeAy - nodeBy);
        return heuristicScale * (dx + dy) + (heuristicScaleDiagonale - 2 * heuristicScale) * Mathf.Min(dx, dy);

    }
    static int GetScore(Case a, Case b)
    {
        return GetScore(a.x, a.y , b.x, b.y);
    }

    // Reset toutes les cases de prévisualisation
    static void ResetCasesPreview(Case startCase, Case endCase)
    {
        if(startCase._gridParent != endCase._gridParent)
        {
            Debug.LogWarning("PathFinding : Attention les cases ne viennent pas de la même grille");
            return;
        }
        GridManager grid = startCase._gridParent;
        for(int x = 0 ; x < grid.SizeX; x++)
        {
            for(int y = 0 ; y < grid.SizeY; y++)
            {
                if(grid._grid[x,y] != endCase && grid._grid[x,y] != startCase)
                {
                    grid._grid[x,y].Checked = false;
                    grid._grid[x,y].BlackList = false;
                    grid._grid[x,y].Highlighted = false;
                    grid._grid[x,y].PointCase = false;
                    grid._grid[x,y].goodPath = false;
                    grid._grid[x,y].hCost = 0;
                    grid._grid[x,y].gCost = 0;
                    grid._grid[x,y].ChangeMaterial(grid.Data.caseDefault);
                }       
            }
        }
    }

    // Function qui s'occupe de trouver le chemin le plus court
    public static void FindPath(Case startCase, Case endCase)
    {
        // Permet d'éviter des hard crash
        if(GridManager.GetValidCase(startCase) == null || GridManager.GetValidCase(endCase) == null )
        {
            Debug.LogWarning("PathFinding : Attention l'un des deux point est une case non valide");
            return;
        }
        // On verifie si les cases viennent de la meme grille
        if(startCase._gridParent != endCase._gridParent)
        {
            Debug.LogWarning("PathFinding : Attention les cases ne viennent pas de la même grille");
            return;
        }

        ResetCasesPreview( startCase, endCase);

        List<Case> openSet = new List<Case>();
        HashSet<Case> closedSet = new HashSet<Case>();
        openSet.Add(startCase);
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
             
            if(currentNode == endCase)
            {
                // on atteint la case de endCase, ainsi on retrace le chemin grace au parent de chaque case
                RetracePath( startCase, endCase);
                return;
            }

            foreach( Case adjacentCase in GridManager.GetAdjacentCases(currentNode))
            {
                if( adjacentCase == null || adjacentCase.state != CaseState.Empty || closedSet.Contains(adjacentCase) )
                {
                    continue;
                }

                int newMovementCostToNeighbour = currentNode.gCost + GetScore(currentNode, adjacentCase);
                if(newMovementCostToNeighbour < adjacentCase.gCost || !openSet.Contains(adjacentCase))
                {
                    adjacentCase.gCost = newMovementCostToNeighbour;
                    adjacentCase.hCost = GetScore(adjacentCase, endCase);
                    adjacentCase.parentCase = currentNode;
                    adjacentCase.Checked = true;
                    if(!openSet.Contains(adjacentCase))
                        openSet.Add(adjacentCase);
                }
            }
                   
        }

        static void RetracePath(Case StartNode, Case endNode)
        {
            List<Case> path = new List<Case>();
            Case currentNode = endNode;
            while(currentNode != StartNode)
            {
                currentNode.Checked = true;
                currentNode.ChangeMaterial(StartNode._gridParent.Data.caseNone);
                path.Add(currentNode);
                currentNode = currentNode.parentCase;

            }
            path.Reverse();
        }
    }
}
