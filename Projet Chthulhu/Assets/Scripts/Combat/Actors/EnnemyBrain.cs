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
    private bool attackEnded;

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
    public IEnumerator PlayTurn() {
        //Start
        EvaluateTiles();
        //Can I Attack Player ?
        int bestAtkScore = GetBestScore(Score.Attack);
        if (bestAtkScore > 0) {
            //Yes
            StartCoroutine(AttackLoop());
            yield return new WaitUntil(()=>attackEnded == true);
        }else{
            //No
            switch (state) {
                case EnnemyState.Regular :
                    //Go to average tile
                    TileEntity regularTile = GetBestAverageTile();
                    Debug.Log("Go to regular tile : " + regularTile.coordinates);
                    break;
                case EnnemyState.Defensive :
                    //Go to safest attack tile
                    TileEntity defensiveTile = GetBestDefensiveAttackTile();
                    Debug.Log("Go to defensive tile : " + defensiveTile.coordinates);
                    break;
                case EnnemyState.Aggressive :
                    //Go to best attack tile
                    TileEntity attackTile = GetBestTile(Score.Attack);
                    Debug.Log("Go to aggressive tile : " + attackTile.coordinates);
                    break;
            }
            EvaluateTiles();
        }
        bestAtkScore = GetBestScore(Score.Attack);
        if (bestAtkScore > 0) {
            StartCoroutine(AttackLoop());//Will do nothing if not enough ap
            yield return new WaitUntil(()=>attackEnded == true);
        }
        if (entity.pm > 0) {
            //Go to safest tile
            TileEntity safeTile = GetBestTile(Score.Defense);
            MoveToTile(safeTile);
            Debug.Log("Go to safe tile : " + safeTile.coordinates);
        }
        //End
        manager.GetComponent<CombatTurnManager>().EndTurn(entity);
    }

    IEnumerator AttackLoop () {
        attackEnded = false;
        TileEntity attackedTile = GetBestTile(Score.Attack);
        int newAp = entity.ap - bestAttack[attackedTile].apCost;
        Debug.Log("New Ap : " + newAp);
        if (newAp < 0) {
            attackEnded = true;
            yield break;
        }
        Debug.Log("Launch Attack on " + attackedTile.coordinates + " with " + bestAttack[attackedTile].name);
        gridManager.ShowAttackPattern(attackedTile,entity,bestAttack[attackedTile]);
        gridManager.LaunchAttach(attackedTile,entity,bestAttack[attackedTile]);
        //TEMPORARY IT WILL NOT BE DONE HERE AFTER
        entity.ap = newAp;
        //foreach (WeaponData w in entity.weaponInventory) {
        //    foreach (AttackData a in w.attacks) {
        //        if (entity.ap < a.apCost) yield break;
        //    }
        //}
        yield return new WaitForSeconds(1f);
        EvaluateTiles();
        StartCoroutine(AttackLoop());
    }

    private void MoveToTile (TileEntity targetTile) {
        gridManager.MoveAlongPath(targetTile,entity);
    }
#endregion
#region Calculations
    private void EvaluateTiles () {
        tilesInRange = gridManager.EnnemyGetMoveRange(entity,entity.pm);
        tileScore = new Dictionary<TileEntity, Dictionary<Score, int>>();
        bestAttack = new Dictionary<TileEntity, AttackData>();
        //Evaluation
        foreach (KeyValuePair<TileEntity,int> t in tilesInRange) {
            //x is pm cost, lowest is best
            int x = entity.pm - t.Value + 1;
            
            //y is potential damage output, highest is best
            int y = 0;
            foreach (WeaponData w in entity.weaponInventory) {
                foreach (AttackData a in w.attacks) {
                    int yScore = AttackScore(t.Key,a,entity,manager.player);
                    if (yScore <= y) continue;
                    y = yScore;
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
    }

    public void TestColorEvaluation () {
        EvaluateTiles();
        foreach (KeyValuePair<TileEntity, Dictionary<Score, int>> t in tileScore) {
            t.Key.SetText("Defense : " + t.Value[Score.Defense].ToString()+"\nAttack : "+ t.Value[Score.Attack].ToString() + "\nDistance : " + t.Value[Score.Movement].ToString());
        }
        GetBestAverageTile().cosmetic.ChangeTextureColor(new Color(0,0,1));//Blue
        GetBestDefensiveAttackTile().cosmetic.ChangeTextureColor(new Color(0,1,0));//Green
        GetBestTile(Score.Attack).cosmetic.ChangeTextureColor(new Color(1,0,0));//Red
        GetBestTile(Score.Defense).cosmetic.ChangeTextureColor(new Color(1,1,0));//Red+Green
    }

    /// <summary>
    /// Loop through all tiles the ennemy can attack and set a value based on its chances to successfully hit it's target
    /// </summary>
    private int AttackScore (TileEntity tileToScore, AttackData attack, ActorEntity actor, ActorEntity target) {
        int score = 0;
        int newScore;
        Dictionary<TileEntity,int> pattern = gridManager.GetPattern(tileToScore,attack.positionPatternCoord,actor);

        foreach (KeyValuePair<TileEntity,int> t in pattern) {
            
            Dictionary<TileEntity,int> damagePattern = gridManager.GetPattern(t.Key,attack.damagePatternCoord,actor);

            if (!damagePattern.TryGetValue(target.currentTile, out int value)) continue;

            if (tileToScore.coordinates == new Vector2Int(9,10) && actor == entity) {
                Debug.Log("ié soui là");
            }

            if (attack.rangedAttack) {
                newScore = (actor.dex/10 + attack.dmg + actor.dex/5 - (int)Vector3.Distance(tileToScore.transform.position,target.currentTile.transform.position));//Dex + potential damage - dist
            }else{
                newScore = (actor.dex/10 + attack.dmg + actor.str/5 - (int)Vector3.Distance(tileToScore.transform.position,target.currentTile.transform.position));//Dex + potential damage - dist
            }
            if (newScore <= score) continue;
            score = newScore;
            
        }
        return score;
    }
    private int AttackScorePOVPlayer (TileEntity tileToScore, AttackData attack, ActorEntity actor, ActorEntity target) {
        int score = 0;
        Dictionary<TileEntity,int> pattern = gridManager.GetPattern(actor.currentTile,attack.positionPatternCoord,actor);
        foreach (KeyValuePair<TileEntity,int> t in pattern) {
            Dictionary<TileEntity,int> damagePattern = gridManager.GetPattern(t.Key,attack.damagePatternCoord,actor);
            foreach (KeyValuePair<TileEntity,int> tile in damagePattern) {
                if (tile.Key != target.currentTile) continue;
                if (attack.rangedAttack) {
                    score += (actor.dex/10 + attack.dmg + actor.dex/5 - (int)Vector3.Distance(tileToScore.transform.position,target.currentTile.transform.position));//Dex + potential damage - dist
                }else{
                    score += (actor.dex/10 + attack.dmg + actor.str/5 - (int)Vector3.Distance(tileToScore.transform.position,target.currentTile.transform.position));//Dex + potential damage - dist
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
            if (searchLowest) {
                dist = (int)Vector3.Distance(t.Key.transform.position,manager.player.transform.position);
            }else{
               dist = t.Value[Score.Movement]; 
            }
            
            
        }
        return selectedTile??GetDefaultTile();
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
        return selectedTile??GetDefaultTile();
    }

    private TileEntity GetBestDefensiveAttackTile () {
        TileEntity selectedTile = null;
        int score = 9000;
        foreach (KeyValuePair<TileEntity, Dictionary<Score, int>> t in tileScore) {
            if (t.Value[Score.Defense] >= score) continue;
            if (t.Value[Score.Attack] == 0) continue;
            selectedTile = t.Key;
            score = t.Value[Score.Defense];
        }
        return selectedTile??GetDefaultTile();
    }

    /// <summary>
    /// Return the Best Score for the given value
    /// </summary>
    private int GetBestScore (Score index) {
        return tileScore[GetBestTile(index)][index];
    }

    private TileEntity GetDefaultTile () {
        //Regular and aggressive will get closer to the player
        //Defensive will back 1 tile away from player
        switch (state) {
            case EnnemyState.Regular :
                goto default;
            case EnnemyState.Defensive :
                return GetFarthestNeighborFromTarget(manager.player);
            case EnnemyState.Aggressive :
                goto default;
            default :
                return GetClosestTileToTarget(manager.player);
        }
    }

    private TileEntity GetClosestTileToTarget (ActorEntity target) {
        float score = 9000f;
        float currentDist;
        TileEntity selectedTile = null;
        foreach (KeyValuePair<TileEntity, Dictionary<Score, int>> t in tileScore) {
            currentDist = Vector3.Distance(entity.currentTile.transform.position,target.currentTile.transform.position);
            if (score <= currentDist) continue;
            score = currentDist;
            selectedTile = t.Key;
        }
        return selectedTile;
    }

    private TileEntity GetFarthestNeighborFromTarget (ActorEntity target) {
        float score = 0;
        float currentDist;
        TileEntity selectedTile = null;
        foreach (TileEntity t in entity.currentTile.directNeighbourTiles) {
            currentDist = Vector3.Distance(entity.currentTile.transform.position,target.currentTile.transform.position);
            if (score >= currentDist) continue;
            score = currentDist;
            selectedTile = t;
        }
        return selectedTile;
    }
#endregion

}