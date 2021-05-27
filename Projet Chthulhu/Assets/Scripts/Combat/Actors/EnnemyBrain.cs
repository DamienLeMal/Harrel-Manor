using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public enum CombatEvents {
    DealGreat,
    DealSmall,
    Miss,
    TakeGreat,
    TakeSmall,
    Dodge
}
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
    [SerializeField] private EnnemyState state = EnnemyState.Regular;
    private bool attackEnded;
    private TileEntity tileToAttack;

    private void Start() {
        entity = GetComponent<EnnemyEntity>();
        manager = CombatManager.current;
        gridManager = manager.GetComponent<GridManager>();
        CombatEventSystem.current.onDealDamage += ChangeStateDamageGiven;
        CombatEventSystem.current.onTakeDamage += ChangeStateDamageTaken;
    }
    //Get Move range value
    //Foreach tiles, evaluate potential damage output and survivability
    //For same values, choose the one with less cost (move range value distance)

    //Play Turn

#region Take Decisions
    public IEnumerator PlayTurn() {

        //Start
        EvaluateTiles();

        //Is it better to Attack Player ?    
        if (tileScore[GetMoveToTile()][Score.Attack] <= tileScore[entity.currentTile][Score.Attack] && tileScore[entity.currentTile][Score.Attack] > 0) {
            Debug.Log("Better to attack first");
            //Yes
            StartCoroutine(AttackLoop());
            yield return new WaitUntil(()=>attackEnded == true);
        }else{
            //No
            TileEntity goToTile = GetMoveToTile();
            MoveToTile(goToTile);
            Debug.Log("Better to move to " + goToTile.coordinates + " first");
            //Wait until he's arrived
            yield return new WaitUntil(()=>entity.currentTile == goToTile);

            EvaluateTiles();

            if (GetBestScore(Score.Attack) > 0) {/// Warning : doesn't care if the tile we're on can attack
                StartCoroutine(AttackLoop());
                yield return new WaitUntil(()=>attackEnded == true);
            }
        }

        if (entity.pm > 0) {
            //Go to safest tile
            TileEntity safeTile = GetBestTile(Score.Defense);
            MoveToTile(safeTile);
            yield return new WaitUntil(()=>entity.currentTile == safeTile);
        }
        //End
        manager.turnManager.EndTurn(entity);
    }

    private TileEntity GetMoveToTile () {
        switch (state) {
            case EnnemyState.Regular :
                //Go to average tile
                return GetBestAverageTile();
            case EnnemyState.Defensive :
                //Go to safest attack tile
                return GetBestDefensiveAttackTile();
            case EnnemyState.Aggressive :
                //Go to best attack tile
                return GetBestTile(Score.Attack);
            default :
                return null;
        }
    }

    IEnumerator AttackLoop () {
        EvaluateTiles();
        if (tileScore[entity.currentTile][Score.Attack] == 0) Debug.LogWarning("The ennemy can't attack from this tile !");
        attackEnded = false;
        AttackScore(entity.currentTile,bestAttack[entity.currentTile],entity,manager.player);
        TileEntity attackedTile = tileToAttack;
        if (bestAttack[entity.currentTile].CheckCost(entity)) {
            attackEnded = true;
            yield break;
        }
        gridManager.ShowAttackPattern(attackedTile,entity,bestAttack[entity.currentTile]);
        gridManager.LaunchAttach(attackedTile,entity,bestAttack[entity.currentTile]);
        yield return new WaitForSeconds(1f);
        EvaluateTiles();
        StartCoroutine(AttackLoop());
    }

    private void MoveToTile(TileEntity targetTile) {
        gridManager.MoveAlongPath(targetTile,entity);
    }
    //0 :
    private Dictionary<(CombatEvents,EnnemyState),(int,EnnemyState)> stateChangeData = new Dictionary<(CombatEvents, EnnemyState), (int, EnnemyState)>
    {
        {(CombatEvents.DealGreat,EnnemyState.Defensive),(25,EnnemyState.Aggressive)},
        {(CombatEvents.Miss,EnnemyState.Defensive),(25,EnnemyState.Regular)},
        {(CombatEvents.Dodge,EnnemyState.Defensive),(50,EnnemyState.Regular)},
        {(CombatEvents.DealSmall,EnnemyState.Defensive),(25,EnnemyState.Regular)},

        {(CombatEvents.TakeGreat,EnnemyState.Regular),(75,EnnemyState.Defensive)},
        {(CombatEvents.Dodge,EnnemyState.Regular),(25,EnnemyState.Defensive)},
        {(CombatEvents.Miss,EnnemyState.Regular),(50,EnnemyState.Aggressive)},
        {(CombatEvents.DealSmall,EnnemyState.Regular),(25,EnnemyState.Aggressive)},

        {(CombatEvents.TakeGreat,EnnemyState.Aggressive),(25,EnnemyState.Defensive)},
        {(CombatEvents.TakeSmall,EnnemyState.Aggressive),(25,EnnemyState.Regular)},
        {(CombatEvents.DealGreat,EnnemyState.Aggressive),(25,EnnemyState.Regular)},
    };

    private void ChangeStateDamageTaken (int damageAmount) {
        (int,EnnemyState) eventParam;
        //Dodged
        if (damageAmount == 0) {
            if (!stateChangeData.ContainsKey((CombatEvents.Dodge,state))) return;
            eventParam = stateChangeData[(CombatEvents.Dodge,state)];
        }else
        //Small amount
        if (damageAmount < entity.hp_max/2) {
            if (!stateChangeData.ContainsKey((CombatEvents.TakeSmall,state))) return;
            eventParam = stateChangeData[(CombatEvents.TakeSmall,state)];
        }
        //Big amount
        else{
            if (!stateChangeData.ContainsKey((CombatEvents.TakeGreat,state))) return;
            eventParam = stateChangeData[(CombatEvents.TakeGreat,state)];
        }
        RandomChangeState(eventParam.Item1,eventParam.Item2);
    }
    private void ChangeStateDamageGiven (int damageAmount) {
        (int,EnnemyState) eventParam;
        //Missed
        if (damageAmount == 0) {
            if (!stateChangeData.ContainsKey((CombatEvents.Miss,state))) return;
            eventParam = stateChangeData[(CombatEvents.Miss,state)];
        }else
        //Small amount
        if (damageAmount < entity.hp_max/2) {
            if (!stateChangeData.ContainsKey((CombatEvents.DealSmall,state))) return;
            eventParam = stateChangeData[(CombatEvents.DealSmall,state)];
        }
        //Big amount
        else{
            if (!stateChangeData.ContainsKey((CombatEvents.DealGreat,state))) return;
            eventParam = stateChangeData[(CombatEvents.DealGreat,state)];
        }
        RandomChangeState(eventParam.Item1,eventParam.Item2);
    }

    private void RandomChangeState(int chances, EnnemyState result) {
        int dice = UnityEngine.Random.Range(0,100);
        if (dice <= chances) {
            state = result;
        }
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

            if (attack.rangedAttack) {
                newScore = (actor.dex/10 + attack.dmg + actor.dex/5 - (int)Vector3.Distance(tileToScore.transform.position,target.currentTile.transform.position));//Dex + potential damage - dist
            }else{
                newScore = (actor.dex/10 + attack.dmg + actor.str/5 - (int)Vector3.Distance(tileToScore.transform.position,target.currentTile.transform.position));//Dex + potential damage - dist
            }
            if (newScore <= score) continue;
            score = newScore;
            tileToAttack = t.Key;
            
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
                bestAtk = t.Value[Score.Attack];
            }
        }

        float average = (bestAtk+bestDef)/2;

        foreach (KeyValuePair<TileEntity, Dictionary<Score, int>> t in tileScore) {
            int s = (t.Value[Score.Attack]+t.Value[Score.Defense])/2;
            if(t.Value[Score.Attack] == 0) continue;
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