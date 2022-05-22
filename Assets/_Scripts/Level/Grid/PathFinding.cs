using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// System qui permet de calculer le chemin le plus court dans une grille
/// </summary>
public class PathFinding : MonoBehaviour
{
    [Header("PATH FINDING")]
    static int heuristicScale = 4;
    static int heuristicScaleDiagonale = 8;

    /// <summary> get la distance entre un point a et point b dans une grille </summary>
    static int GetScore(int nodeAx, int nodeAy, int nodeBx, int nodeBy)
    {
        int dx = Mathf.Abs(nodeAx - nodeBx);
        int dy = Mathf.Abs(nodeAy - nodeBy);
        return heuristicScale * (dx + dy) + (heuristicScaleDiagonale - 2 * heuristicScale) * Mathf.Min(dx, dy);

    }
    static int GetScore(Case a, Case b)
    {
        return GetScore(a.x, a.y, b.x, b.y);
    }

    // Reset toutes les cases de prévisualisation
    static void ResetCasesPreview(Case startCase = null, Case endCase = null)
    {
        if (startCase.GridParent != endCase.GridParent)
        {
            Debug.LogWarning("PathFinding : Attention les cases ne viennent pas de la même grille");
            return;
        }
        GridManager.ResetCasesPreview(startCase.GridParent, startCase,endCase);
    }

    /// <summary> Function qui s'occupe de trouver le chemin le plus court </summary>
    public static Case[] FindPath(Case startCase, Case endCase)
    {
        // Permet d'éviter des hard crash
        if (GridManager.GetValidCase(startCase) == null || GridManager.GetValidCase(endCase) == null)
        {
            Debug.LogWarning("PathFinding : Attention l'un des deux point est une case non valide");
            return null;
        }
        if (startCase.GridParent != endCase.GridParent)
        {
            Debug.LogWarning("PathFinding : Attention les cases ne viennent pas de la même grille");
            return null;
        }

        ResetCasesPreview(startCase, endCase);

        List<Case> openSet = new List<Case>();
        HashSet<Case> closedSet = new HashSet<Case>();
        openSet.Add(startCase);
        while (openSet.Count > 0)
        {
            Case currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == endCase)
            {
                // on atteint la endCase, ainsi on retrace le chemin grace au parent de chaque case
                return RetracePath(startCase, endCase);
            }
            foreach (Case adjacentCase in GridManager.GetAdjacentCases(currentNode))
            {
                if (adjacentCase == null || adjacentCase.state != CaseState.Empty || closedSet.Contains(adjacentCase))
                {
                    continue;
                }

                int newMovementCostToNeighbour = currentNode.gCost + GetScore(currentNode, adjacentCase);
                if (newMovementCostToNeighbour < adjacentCase.gCost || !openSet.Contains(adjacentCase))
                {
                    adjacentCase.gCost = newMovementCostToNeighbour;
                    adjacentCase.hCost = GetScore(adjacentCase, endCase);
                    // Si le chemin est bon, on indique de quelle case provient notre case ideal
                    // utile pour retracer le chemin après
                    adjacentCase.ParentCase = currentNode;
                    adjacentCase.Checked = true;
                    if (!openSet.Contains(adjacentCase))
                        openSet.Add(adjacentCase);
                }
            }

        }

        // Si on arrive la cest que aucun chemin n'a était trouvé
        return null;
    }

    /// <summary> Renvoi la list des cases qui offrent le meilleur chemin </summary>
    static Case[] RetracePath(Case StartNode, Case endNode)
    {
        List<Case> path = new List<Case>();
        Case currentNode = endNode;
        while (currentNode != StartNode)
        {
            currentNode.Checked = true;
            currentNode.ChangeMaterial(StartNode.GridParent.Data.caseNone);
            path.Add(currentNode);
            currentNode = currentNode.ParentCase;
        }
        path.Reverse();
        return path.ToArray();
    }
}
