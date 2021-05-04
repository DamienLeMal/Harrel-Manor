using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "ScriptableObjects/Combat/Attack", order = 2)]
public class AttackData : SheetData
{
    public string attackName;
    public int apCost, mpCost, dmg;
    public bool rangedAttack;
    [SerializeField] private string positionPattern;
    private int[,] positionPatternArray;
    [SerializeField] private string damagePattern;
    private int[,] damagePatternArray;
    public List<Vector2Int> positionPatternCoord;
    public List<Vector2Int> damagePatternCoord;
    

    public void InitialiseData () {
        positionPatternArray = ReadSheetData(positionPattern);
        damagePatternArray = ReadSheetData(damagePattern);
        positionPatternCoord = ProcessData(positionPatternArray);
        damagePatternCoord = ProcessData(damagePatternArray);
    }

    public void Cost (ActorEntity attacker) {
        if (attacker.ap - apCost < 0) Debug.LogWarning("Ap under 0");
        attacker.ap -= apCost;
    }

    private List<Vector2Int> ProcessData (int[,] data) {
        List<Vector2Int> quarterCoords = new List<Vector2Int>();
        for (int i = 0; i<Mathf.Sqrt(data.Length); i++) {
            for (int j = 0; j < Mathf.Sqrt(data.Length); j++) {
                if (data[i,j] == 2) {
                    quarterCoords.Add(new Vector2Int(i,j));
                }
            }
        }
        List<Vector2Int> finalCoords = new List<Vector2Int>();
        //Add all the rotations
        foreach (Vector2Int v in quarterCoords) {
            finalCoords.Add(v);// angle 0
            finalCoords.Add(new Vector2Int(v.y,-v.x));// angle 90
            finalCoords.Add(new Vector2Int(-v.x,-v.y));// angle 180
            finalCoords.Add(new Vector2Int(-v.y,v.x));// angle 270
        }
        return finalCoords;
    }
}
