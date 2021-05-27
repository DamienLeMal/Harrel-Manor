using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "ScriptableObjects/Combat/Attack", order = 3)]
public class AttackData : SheetData
{
    public string attackName;
    public string description;
    public int apCost, mpCost, mntCost, dmg;
    public bool rangedAttack;
    [SerializeField] private string positionPattern;
    private int[,] positionPatternArray;
    [SerializeField] private string damagePattern;
    private int[,] damagePatternArray;
    public List<Vector2Int> positionPatternCoord;
    public List<Vector2Int> damagePatternCoord;
    public Sprite attackIcon;
    public ParticleSystem attackParticle;
    

    public void InitialiseData () {
        positionPatternArray = ReadSheetData(positionPattern);
        damagePatternArray = ReadSheetData(damagePattern);
        positionPatternCoord = ProcessData(positionPatternArray);
        damagePatternCoord = ProcessData(damagePatternArray);
        ProcessText();
    }

    private void ProcessText () {
        attackName = "<B>" + name + "</B>";
        description = "";
        if (apCost != 0) description += "Coût AP : <color=green>" + apCost.ToString() + "</color>";
        if (mpCost != 0) description += "\nCoût MP : <color=purple>" + mpCost.ToString() + "</color>";
        if (mntCost != 0) description += "\nCoût Santé Mentale : <color=purple>" + mntCost.ToString() + "</color>";
        if (dmg != 0) description += "\nDégâts : <color=red>" + dmg.ToString() + "</color>";

    }
    public bool CheckCost (ActorEntity attacker) {
        return (attacker.ap > apCost && attacker.mp > mpCost && attacker.mnt > mntCost);
    }
    public void Cost (ActorEntity attacker) {
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
