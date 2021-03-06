using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackAnimation {
    Shot,
    ShotgunShot,
    Stab,
    Slash,
    Cast,
    HitKnuckle
}

[CreateAssetMenu(fileName = "Attack", menuName = "ScriptableObjects/Combat/Attack", order = 3)]
public class AttackData : SheetData
{
    [HideInInspector] public string attackName;
    [HideInInspector] public string description;
    public int apCost, mpCost, mntCost, dmg;
    public bool rangedAttack, heal;
    [SerializeField] private string positionPattern;
    private int[,] positionPatternArray;
    [SerializeField] private string damagePattern;
    private int[,] damagePatternArray;
    [HideInInspector] public List<Vector2Int> positionPatternCoord = new List<Vector2Int>();
    [HideInInspector] public List<Vector2Int> damagePatternCoord = new List<Vector2Int>();
    public Sprite attackIcon;
    public int attackParticleId;
    [SerializeField] private AttackAnimation animation;

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
        if (apCost != 0) description += UiColorScheme.current.GetTagColor(UiColorScheme.current.apColor) + "Coût AP : " + apCost.ToString() + "</color>";
        if (mpCost != 0) description += UiColorScheme.current.GetTagColor(UiColorScheme.current.mpColor) + "\nCoût MP : " + mpCost.ToString() + "</color>";
        if (mntCost != 0) description += UiColorScheme.current.GetTagColor(UiColorScheme.current.mntColor) + "\nCoût Santé Mentale : " + mntCost.ToString() + "</color>";
        if (dmg != 0 && !heal) description += UiColorScheme.current.GetTagColor(UiColorScheme.current.dmgColor) + "\nDégâts : " + dmg.ToString() + "</color>";
        if (dmg != 0 && heal) description += UiColorScheme.current.GetTagColor(UiColorScheme.current.hpColor) + "\nSoigne : " + dmg.ToString() + "</color>";
    }
    /// <summary>
    /// Return True if can attack
    /// </summary>
    public bool CheckCost (ActorEntity attacker) {
        return (attacker.ap >= apCost && attacker.mp >= mpCost && attacker.mnt >= mntCost);
    }
    public void Cost (ActorEntity attacker) {
        attacker.ap -= apCost;
        attacker.mp -= mpCost;
        attacker.mnt -= mntCost;
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

    public string GetAnimation () {
        switch (animation) {
            case AttackAnimation.Shot : return "isShotting";
            case AttackAnimation.ShotgunShot : return "isShottingPump";
            case AttackAnimation.Stab : return "isStabing";
            case AttackAnimation.Slash : return "isSlashing";
            case AttackAnimation.Cast : return "isCasting";
            case AttackAnimation.HitKnuckle : return "isHittingKnuckle";
            default : goto case AttackAnimation.Shot;
            
        }
    }
}
