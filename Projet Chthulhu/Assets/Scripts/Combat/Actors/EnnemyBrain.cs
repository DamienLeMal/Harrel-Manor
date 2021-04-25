using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnnemyState {
    Regular,
    Defensive,
    Aggressive
}
public enum Score {
    Movement,
    Attack,
    Defense
}
public class EnnemyBrain : MonoBehaviour
{
    private Dictionary<TileEntity,int> tilesInRange;
    private EnnemyEntity entity;
    private GridManager gridManager;
    private CombatManager manager;
    private Dictionary<TileEntity,Dictionary<Score,int>> tileScore;
    private Dictionary<TileEntity,AttackData> bestAttack;
    private EnnemyState state = EnnemyState.Regular;

    private void Start() {
        entity = GetComponent<EnnemyEntity>();
        gridManager = transform.GetComponentInParent<GridManager>();
        manager = gridManager.GetComponent<CombatManager>();
    }
    //Get Move range value
    //Foreach tiles, evaluate potential damage output and survivability
    //For same values, choose the one with less cost (move range value distance)

    //Play Turn

#region Take Decisions
    public void PlayTurn() {
        //Start
        EvaluateTiles();
        //Can I Attack Player ?
        int bestAtkScore = GetBestScore(Score.Attack);
        Debug.Log("bestAtkScore : " + bestAtkScore);
        if (bestAtkScore > 0) {
            //Yes
            AttackLoop();
        }else{
            //No
            switch (state) {
                case EnnemyState.Regular :
                    Debug.Log("RegularTile");
                    //Go to average tile
                    TileEntity RegularTile = GetBestAverageTile();
                    Debug.Log("Go to tile : " + RegularTile.coordinates);
                    break;
                case EnnemyState.Defensive :
                    Debug.Log("DefensiveTile");
                    //Go to safest attack tile
                    TileEntity DefensiveTile = GetBestDefensiveAttackTile();
                    Debug.Log("Go to tile : " + DefensiveTile.coordinates);
                    break;
                case EnnemyState.Aggressive :
                    Debug.Log("AggressiveTile");
                    //Go to best attack tile
                    TileEntity AttackTile = GetBestTile(Score.Attack);
                    Debug.Log("Go to tile : " + AttackTile.coordinates);
                    break;
            }
            EvaluateTiles();
        }
        bestAtkScore = GetBestScore(Score.Attack);
        Debug.Log("bestAtkScore : " + bestAtkScore);
        if (bestAtkScore > 0) {
            AttackLoop();//Will do nothing if not enough ap
        }
        if (entity.pm > 0) {
            //Go to safest tile
            TileEntity SafeTile = GetBestTile(Score.Defense);
            Debug.Log("Go to tile : " + SafeTile.coordinates);
        }
        //End   
    }

    private void AttackLoop () {
        TileEntity attackedTile = GetBestTile(Score.Attack);
        int newAp = entity.ap - bestAttack[attackedTile].apCost;
        Debug.Log("New Ap : " + newAp);
        if (newAp < 0) return ;
        Debug.Log("Launch Attack on " + attackedTile.coordinates + " with " + bestAttack[attackedTile].name);
        //TEMPORARY IT WILL NOT BE DONE HERE AFTER
        entity.ap = newAp;
        foreach (WeaponData w in entity.weaponInventory) {
            foreach (AttackData a in w.attacks) {
                if (entity.ap < a.apCost) return;
            }
        }
        EvaluateTiles();
        AttackLoop();
    }
#endregion
#region Calculations
    private void EvaluateTiles () {
        tilesInRange = gridManager.EnnemyGetMoveRange(entity.currentTile.directNeighbourTiles,entity.pm);
        tilesInRange.Add(entity.currentTile,-1);
        tileScore = new Dictionary<TileEntity, Dictionary<Score, int>>();
        bestAttack = new Dictionary<TileEntity, AttackData>();
        //Evaluation
        foreach (KeyValuePair<TileEntity,int> t in tilesInRange) {
            //x is pm cost, lowest is best
            int x = entity.pm - t.Value;
            
            //y is potential damage output, highest is best
            int y = 0;
            int bestY = 0;
            foreach (WeaponData w in entity.weaponInventory) {
                foreach (AttackData a in w.attacks) {
                    int yScore = AttackScore(t.Key,a,entity,manager.player);
                    y += yScore;
                    if (yScore <= bestY) continue;
                    bestY = yScore;
                    bestAttack[t.Key] = a;
                }
            }

            //z is defense score, lowest is best
            int z = 0;
            foreach (WeaponData w in manager.player.weaponInventory) {
                foreach (AttackData a in w.attacks) {
                    //The defense score is set on the attack score of the player
                    z += AttackScore(t.Key,a,manager.player,entity);
                }
            }
            tileScore.Add(t.Key,new Dictionary<Score, int> {[Score.Movement] = x,[Score.Attack] = y,[Score.Defense] = z});
        }

        //Choose Tile based on comportement
        ////GetBestTile(tileScore,1,false).GetComponentInChildren<MeshRenderer>().material.color = new Color(0,5,0);//agressive tile
        ////GetBestTile(tileScore,2,true).GetComponentInChildren<MeshRenderer>().material.color = new Color(0,0,5);//defensive tile
        ////GetBestAverageTile(tileScore).GetComponentInChildren<MeshRenderer>().material.color = new Color(0,5,5);//balanced tile
    }

    /// <summary>
    /// Loop through all tiles the ennemy can attack and set a value based on its chances to successfully hit the player
    /// </summary>
    private int AttackScore (TileEntity tileToScore, AttackData attack, ActorEntity actor, ActorEntity target) {
        int score = 0;
        Dictionary<TileEntity,int> pattern =  gridManager.GetPattern(actor.currentTile,attack.positionPatternCoord);
        foreach (KeyValuePair<TileEntity,int> t in pattern) {
            Dictionary<TileEntity,int> damagePattern = gridManager.GetPattern(t.Key,attack.damagePatternCoord);
            foreach (KeyValuePair<TileEntity,int> tile in damagePattern) {
                if (tile.Key.tileUser != target) continue;
                if (attack.rangedAttack) {
                    score += (actor.dex/10 + attack.dmg + actor.dex/5 - t.Value);//Dex + potential damage - dist
                }else{
                    score += (actor.dex/10 + attack.dmg + actor.str/5 - t.Value);//Dex + potential damage - dist
                }
            }
        }
        return score;
    }
#endregion
#region Exploit the Results
    /// <summary>
    /// Get the tile with the lowest or highest value on one parameter
    /// </summary>
    private TileEntity GetBestTile (Score index) {
        TileEntity selectedTile = null;
        bool searchLowest = (index == Score.Defense);
        int score;
        int dist;
        if (searchLowest) {
            score = 9000;
            dist = -1;
        }else{
            score = 0;
            dist = 9000;
        }
        foreach (KeyValuePair<TileEntity, Dictionary<Score, int>> t in tileScore) {
            if (searchLowest) {
                if (t.Value[index] > score) continue;
            }else{
                if (t.Value[index] < score) continue;
            }
            if (t.Value[index] == score && t.Value[Score.Movement] >= dist && index != Score.Defense) continue;
            if (t.Value[index] == score && t.Value[Score.Movement] <= dist && index == Score.Defense) continue;
            score = t.Value[index];
            selectedTile = t.Key;
            dist = t.Value[Score.Movement];
        }
        return selectedTile;
    }

    /// <returns>Tile with the best average values</returns>
    private TileEntity GetBestAverageTile () {
        TileEntity selectedTile = null;
        int score = 9000;
        int dist = 9000;
        int bestDef = 9000;
        int bestAtk = 0;

        foreach (KeyValuePair<TileEntity, Dictionary<Score, int>> t in tileScore) {
            if (t.Value[Score.Defense] < bestDef) {
                bestDef = t.Value[Score.Defense];
            }
            if (t.Value[Score.Attack] > bestAtk) {
                bestDef = t.Value[Score.Attack];
            }
        }
        float average = (bestAtk+bestDef)/2;

        foreach (KeyValuePair<TileEntity, Dictionary<Score, int>> t in tileScore) {
            int s = (t.Value[Score.Attack]+t.Value[Score.Defense])/2;
            if (Mathf.Abs(average - s) >= score) continue;
            if (Mathf.Abs(average - s) == score && t.Value[0] >= dist) continue;
            score = (int)(average - s);
            selectedTile = t.Key;
            dist = t.Value[Score.Movement];
        }
        return selectedTile;
    }

    private TileEntity GetBestDefensiveAttackTile () {
        TileEntity selectedTile = null;
        int score = 9000;
        foreach (KeyValuePair<TileEntity, Dictionary<Score, int>> t in tileScore) {
            if (t.Value[Score.Defense] < score) continue;
            if (t.Value[Score.Attack] == 0) continue;
            selectedTile = t.Key;
            score = t.Value[Score.Defense];
        }
        return selectedTile;
    }

    private int GetBestScore (Score index) {
        return tileScore[GetBestTile(index)][index];
    }
#endregion

}