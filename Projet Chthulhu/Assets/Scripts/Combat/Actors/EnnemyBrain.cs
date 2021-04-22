using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyBrain : MonoBehaviour
{
    private Dictionary<TileEntity,int> tilesInRange;
    private EnnemyEntity entity;
    private GridManager gridManager;
    private CombatManager manager;
    private Dictionary<TileEntity,int[]> tileScore;

    private void Start() {
        entity = GetComponent<EnnemyEntity>();
        gridManager = transform.GetComponentInParent<GridManager>();
        manager = gridManager.GetComponent<CombatManager>();
    }
    //Get Move range value
    //Foreach tiles, evaluate potential damage output and survivability
    //For same values, choose the one with less cost (move range value distance)

    public void EvaluateTiles () {
        tilesInRange = gridManager.EnnemyGetMoveRange(entity.currentTile.directNeighbourTiles,entity.pm);
        tilesInRange.Add(entity.currentTile,-1);
        tileScore = new Dictionary<TileEntity, int[]>();
        //Evaluation
        foreach (KeyValuePair<TileEntity,int> t in tilesInRange) {
            //x is pm cost, lowest is best
            int x = t.Value;
            
            //y is potential damage output, highest is best
            int y = 0;
            foreach (WeaponData w in GetComponent<EnnemyEntity>().weaponInventory) {
                foreach (AttackData a in w.attacks) {
                    y += AttackScore(t.Key,a,entity,manager.player);
                }
            }

            //z is defense score, lowest is best
            int z = 0;
            foreach (WeaponData w in manager.player.weaponInventory) {
                foreach (AttackData a in w.attacks) {
                    z += AttackScore(t.Key,a,manager.player,entity);
                }
            }

            tileScore.Add(t.Key,new int[] {x,y,z});
        }

        //Choose Tile based on comportement
        GetBestTile(tileScore,0,true).GetComponentInChildren<MeshRenderer>().material.color = new Color(5,0,0);
        GetBestTile(tileScore,1,false).GetComponentInChildren<MeshRenderer>().material.color = new Color(0,5,0);
        GetBestTile(tileScore,2,true).GetComponentInChildren<MeshRenderer>().material.color = new Color(0,0,5);
        GetBestAverageTile(tileScore).GetComponentInChildren<MeshRenderer>().material.color = new Color(0,5,5);
    }

    /// <summary>
    /// Loop through all tiles the ennemy can attack and set a value based on its chances to successfully hit the player
    /// </summary>
    private int AttackScore (TileEntity tileToScore, AttackData attack, ActorEntity actor, ActorEntity target) {
        int score = 0;
        Dictionary<TileEntity,int> pattern = gridManager.GetPattern(tileToScore,attack.positionPatternCoord);
        foreach (KeyValuePair<TileEntity,int> t in pattern) {
            Dictionary<TileEntity,int> damagePattern = gridManager.GetPattern(t.Key,attack.damagePatternCoord);
            foreach (KeyValuePair<TileEntity,int> tile in damagePattern) {
                if (tile.Key.tileUser != target) { continue; }
                if (attack.rangedAttack) {
                    score += (actor.dex/10 + attack.dmg + actor.dex/5 - t.Value);//Dex + potential damage - dist
                }else{
                    score += (actor.dex/10 + attack.dmg + actor.str/5 - t.Value);//Dex + potential damage - dist
                }
            }
        }
        return score;
    }

    /// <summary>
    /// Loop through all tiles the player can attack and set a value based on distance to the ennemy
    /// </summary>
    private int DefenseScore (TileEntity tileToScore, AttackData playerAttack) {
        int score = 0;
        Dictionary<TileEntity,int> pattern = gridManager.GetPattern(tileToScore,playerAttack.positionPatternCoord);
        foreach (KeyValuePair<TileEntity,int> t in pattern) {
            Dictionary<TileEntity,int> damagePattern = gridManager.GetPattern(t.Key,playerAttack.damagePatternCoord);
            foreach (KeyValuePair<TileEntity,int> tile in damagePattern) {
                if (tile.Key != tileToScore) { continue; }
                score = 100/t.Value;
            }
        }
        Debug.Log("-----score : " + score);
        return score;
    }
    private TileEntity GetBestTile (Dictionary<TileEntity,int[]> tileScore, int index, bool lowest) {
        TileEntity selectedTile = null;
        int score;
        int dist = 9000;
        if (lowest) {
            score = 9000;
        }else{
            score = 0;
        }
        
        foreach (KeyValuePair<TileEntity,int[]> t in tileScore) {
            if (lowest) {
                if (t.Value[index] < score) {
                    score = t.Value[index];
                    selectedTile = t.Key;
                    dist = t.Value[0];
                }else if (t.Value[index] == score) {
                    if (t.Value[0] < dist) {
                        dist = t.Value[0];
                        selectedTile = t.Key;
                        score = t.Value[index];
                    }
                }
            }else{
                if (t.Value[index] > score) {
                    score = t.Value[index];
                    selectedTile = t.Key;
                    dist = t.Value[0];
                }else if (t.Value[index] == score) {
                    if (t.Value[0] < dist) {
                        dist = t.Value[0];
                        selectedTile = t.Key;
                        score = t.Value[index];
                    }
                }
            }
        }
        Debug.Log(selectedTile.coordinates);
        Debug.Log("index : " + index + " | score : " + score + " bool : " + lowest);
        return selectedTile;
    }

    private TileEntity GetBestAverageTile (Dictionary<TileEntity,int[]> tileScore) {
        TileEntity selectedTile = null;
        int score = 9000;
        int dist = 9000;
        int bestDef = 9000;
        int bestAtk = 0;

        foreach (KeyValuePair<TileEntity,int[]> t in tileScore) {
            if (t.Value[2] < bestDef) {
                bestDef = t.Value[2];
            }
            if (t.Value[1] > bestAtk) {
                bestDef = t.Value[1];
            }
        }
        float average = (bestAtk+bestDef)/2;

        foreach (KeyValuePair<TileEntity,int[]> t in tileScore) {
            int s = (t.Value[1]+t.Value[2])/2;
            if (Mathf.Abs(average - s) < score) {
                score = (int)(average - s);
                selectedTile = t.Key;
                dist = t.Value[0];
            }else if (Mathf.Abs(average - s) == score) {
                if (t.Value[0] < dist) {
                    dist = t.Value[0];
                    selectedTile = t.Key;
                    score = (int)(average - s);
                }
            }
        }
        Debug.Log(selectedTile.coordinates);
        Debug.Log("average : " + average + " | score : " + score);
        return selectedTile;
    }
}
