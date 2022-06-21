using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Grid Data", menuName = "Level/Grid", order = 1)]
public class SO_Grid : ScriptableObject
{
    [Header("GRID PARAMETERS")]
    [Range(1,100)]
    public int gridSizeX = 10; // Size of the grid in X
    [Range(1,100)]
    public int gridSizeY = 10; // Size of the grid in Y 
    [Range(1,5)]
    public int cellSize = 3; // Size of the cell in the world
    public GameObject CasePrefab; // Prefab d'une cellule
    //public CaseInfo[] Grid; // A table with one entry with x and y, with for each element the type Case
    
    [Header("CASE MATERIALS")]
    public Material caseDefault;
    public Material caseHighlight;
    public Material caseLocked;
    public Material caseNone;
    public Material caseSelected;
    public Material caseOverwatch;
    public Material caseSpawner;
    public Material caseCharacter;
    public Material caseCharacterSelected;



}