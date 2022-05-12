using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Grid Data", menuName = "Level/Grid", order = 1)]
public class SO_Grid : ScriptableObject
{
    [Header("GRID PARAMETERS")]
    public int gridSizeX = 10; // Size of the grid in X
    public int gridSizeY = 10; // Size of the grid in Y 
    public int cellSize = 10; // Size of the cell in the world

    public Case[] Grid; // A table with one entry with x and y, with for each element the type Case


}
