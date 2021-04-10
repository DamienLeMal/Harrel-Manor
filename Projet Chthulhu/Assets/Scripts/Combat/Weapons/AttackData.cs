using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "ScriptableObjects/Combat/Attack", order = 2)]
public class AttackData : SheetData
{
    string attackName;
    int apCost, mpCost;
    string positionPattern;
    private int[,] positionPatternArray;
    string damagePattern;
    private int[,] damagePatternArray;
    private List<Vector2> positionPatternCoord;
    private List<Vector2> damagePatternCoord;
    

    public void InitialiseData () {
        positionPatternArray = ReadSheetData(positionPattern);
        damagePatternArray = ReadSheetData(damagePattern);
        positionPatternCoord = ProcessData(positionPatternArray);
        damagePatternCoord = ProcessData(damagePatternArray);
    }

    private List<Vector2> ProcessData (int[,] data) {
        List<Vector2> quarterCoords = new List<Vector2>();
        for (int i = 0; i<Mathf.Sqrt(data.Length); i++) {
            for (int j = 0; j < Mathf.Sqrt(data.Length); j++) {
                if (data[i,j] == 2) {
                    quarterCoords.Add(new Vector2(i,j));
                }
            }
        }
        List<Vector2> finalCoords = new List<Vector2>();
        //Add all the rotations
        foreach (Vector2 v in quarterCoords) {
            finalCoords.Add(v);// angle 0
            finalCoords.Add(new Vector2(v.y,-v.x));// angle 90
            finalCoords.Add(new Vector2(-v.x,-v.y));// angle 180
            finalCoords.Add(new Vector2(-v.y,v.x));// angle 270
        }
        return finalCoords;
    }

    public void GetPattern (int startPosX, int startPosY, TileEntity[,] grid, List<Vector2> pattern) {
        List<TileEntity> potentialTargets = new List<TileEntity>();
        //Convert pattern to tiles
        foreach (Vector2 v in pattern) {
            TileEntity t = grid[startPosX + (int)v.x, startPosY + (int)v.y];
            if (t == null) { continue; }
            if (t.tileState != TileState.Walk) { continue; }
            potentialTargets.Add(grid[startPosX + (int)v.x, startPosY + (int)v.y]);
        }
        //Clear tiles that are blocked in the path

    }

    private void CheckTileAccess () {
        //get closest neighbor from source to end tile unil you reach it and 
        //if there's a wall (check if a tile dist is == to wall /!\) remove the tile because it's not accessible
    }
}
