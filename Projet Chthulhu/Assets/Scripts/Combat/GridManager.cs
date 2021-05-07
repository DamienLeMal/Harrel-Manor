using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    private CombatManager manager = null;
    private TileEntity[,] tileGrid = null;
    /// <summary>
    /// Refers to the tiles the program is processing right now, they are potentialy lighten up
    /// </summary>
    /// <typeparam name="TileEntity">Tiles that are interactable</typeparam>
    /// <typeparam name="int">Distance of the tile from the player</typeparam>
    /// <returns></returns>
    public Dictionary<TileEntity,int> tileHighlightRanges;
    private Dictionary<TileEntity,int> tileHighlightAttack;
    private void Start() {
        manager = GetComponent<CombatManager>();
        tileGrid = manager.grid;
    }


    #region Higlight Surroundings
    public void ClearTileHighlight () {
        if (tileHighlightRanges != null) {
            tileHighlightRanges.Clear();
        }
        if (tileHighlightAttack != null) {
            tileHighlightAttack.Clear();
        }
    }
    public void ResetTileHighlight () {
        if (tileHighlightRanges != null) {
            foreach (KeyValuePair<TileEntity,int> d in tileHighlightRanges) {
                d.Key.UpdateMaterial();
            }
        }
        if (tileHighlightAttack != null) {
            foreach (KeyValuePair<TileEntity,int> d in tileHighlightAttack) {
                d.Key.UpdateMaterial();
            }
        }
    }
    public void HighlightActionTiles () {
        tileHighlightRanges = new Dictionary<TileEntity, int>();
        TileEntity currentTile = manager.player.currentTile;
        switch (manager.playerState) {
            case PlayerState.Moving :
                SetMoveRangeValue(currentTile.directNeighbourTiles,currentTile.tileUser.GetComponent<ActorEntity>().pm);
                break;
            case PlayerState.Attacking :
                SetAttackRangeValue(manager.activeButton.attack);
                break;
        }
        HighlightTiles();
    }

    private void HighlightTiles () {
        foreach (KeyValuePair<TileEntity,int> d in tileHighlightRanges) {
            d.Key.cosmetic.ChangeTextureColor(new Color(d.Value,0,0));
        }
    }
    public Dictionary<TileEntity,int> EnnemyGetMoveRange (ActorEntity ennemyEntity, int range) {
        tileHighlightRanges = new Dictionary<TileEntity, int>();
        SetMoveRangeValue(new List<TileEntity>() {ennemyEntity.currentTile},range+1,true);//Range +1 since we also want the starting tile in the dictionnary
        return tileHighlightRanges;
    }

    /// <summary>
    /// Associate every tile in range with a value decreasing in distance
    /// </summary>
    private void SetMoveRangeValue (List<TileEntity> nTiles, int range, bool ennemyGetMoveRange = false) {
        foreach (TileEntity nt in nTiles) {
            if (nt.tileState == TileState.Walk || ennemyGetMoveRange) {
                if (tileHighlightRanges.TryGetValue(nt,out int value)){
                    if (range > value) {
                        tileHighlightRanges.Remove(nt);
                    }else{
                        continue;
                    }
                }
                tileHighlightRanges.Add(nt,range);
                if (range > 1) {
                    SetMoveRangeValue(nt.directNeighbourTiles,range-1);
                } 
            }
        }
    }
    private void SetAttackRangeValue (AttackData atk) {
        tileHighlightRanges = GetPattern(manager.player.currentTile,atk.positionPatternCoord,manager.player);
    }

    #endregion
#region Move
    
    #region PathFinding
    //Show the shortest path to destination
    public void StartPathFinding (TileEntity startTile, TileEntity endTile) {
        TileEntity prev = null;
        TileEntity current = manager.player.currentTile;
        bool first = true;
        foreach (TileEntity next in PathFinding(startTile,endTile)) {
            if (!first) {
                current.cosmetic.TraceLine(prev,next);
            }else{
                manager.player.currentTile.cosmetic.TraceLine(next);
                tileHighlightRanges.Add(manager.player.currentTile,0);
            }
            prev = current;
            current = next;
            first = false;
        }
        if (!first) {
            current.cosmetic.TraceLine(prev);
        }
    }
    
    /// <summary>
    /// Move the Actor along the shortest path
    /// </summary>
    public void MoveAlongPath (TileEntity endTile, ActorEntity actor) {
        List<TileEntity> path = PathFinding(actor.currentTile,endTile);
        StartCoroutine(MoveOneTile(path,actor));
    }
    /// <summary>
    /// Move one tile at a time
    /// </summary>
    IEnumerator MoveOneTile (List<TileEntity> path, ActorEntity actor) {
        if (path.Count > 0) {
            actor.pm -= 1;
            Vector3 newPos = path[0].transform.position + new Vector3(0, actor.GetComponent<MeshRenderer>().bounds.size.y / 2, 0);
            LeanTween.move(actor.gameObject,newPos,1f);
            path.RemoveAt(0);
            yield return new WaitForSeconds(1.1f);
            StartCoroutine(MoveOneTile(path,actor));
        }else{
            //end
            GetComponent<CombatManager>().playerState = PlayerState.Normal;
            GetComponent<CombatManager>().ResetActorsPositions();
        }
    }
    //Main Pathfinding method
    private List<TileEntity> PathFinding (TileEntity startTile, TileEntity endTile) {
        List<TileEntity> openSet = new List<TileEntity>();
        HashSet<TileEntity> closedSet = new HashSet<TileEntity>();

        openSet.Add(startTile);

        while (openSet.Count > 0) {
            TileEntity currentTile = openSet[0];
            for (int i = 1; i < openSet.Count; i++) {
                if (openSet[i].fCost < currentTile.fCost || openSet[i].fCost == currentTile.fCost && openSet[i].hCost < currentTile.hCost) {
                    currentTile = openSet[i];
                }
            }

            openSet.Remove(currentTile);
            closedSet.Add(currentTile);

            if (currentTile == endTile) {
                return RetracePath(startTile,endTile);
            }

            foreach (TileEntity neighbour in currentTile.directNeighbourTiles) {
                if (closedSet.Contains(neighbour) || neighbour.tileState != TileState.Walk) {
                    continue;
                }

                int newMovementCostToNeighbour = currentTile.gCost + GetDistance(currentTile,neighbour);
                if (newMovementCostToNeighbour <  neighbour.gCost || !openSet.Contains(neighbour)) {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, endTile);
                    neighbour.previousInPath = currentTile;

                    if (!openSet.Contains(neighbour)) {
                        openSet.Add(neighbour);
                    }
                }
            }
        }
        Debug.LogWarning("Pathfinding Error");
        return null;
    }

    private List<TileEntity> RetracePath (TileEntity startTile, TileEntity endTile) {
        TileEntity currentTile = endTile;
        List<TileEntity> path = new List<TileEntity>();
        while (currentTile != startTile) {
            path.Add(currentTile);
            currentTile = currentTile.previousInPath;
        }

        path.Reverse();
        return path;
    }

    private int GetDistance(TileEntity zoneA, TileEntity zoneB) {
        return (int) (zoneA.transform.position - zoneB.transform.position).magnitude;
    }
    #endregion
#endregion

#region Attack

    /// <summary>
    /// Get all tiles from the attack damage pattern and color them
    /// </summary>
    public void ShowAttackPattern (TileEntity startTile, ActorEntity attacker, AttackData attack) {
        tileHighlightAttack = new Dictionary<TileEntity, int>();
        tileHighlightAttack = GetPattern(startTile, attack.damagePatternCoord,attacker);
        if (tileHighlightAttack.TryGetValue(attacker.currentTile, out int value)) { 
            tileHighlightAttack.Remove(attacker.currentTile); 
        }
        foreach (KeyValuePair<TileEntity,int> t in tileHighlightAttack) {
            t.Key.cosmetic.ChangeTextureColor(new Color(0,0,0));
        }
    }
    /// <summary>
    /// Get all tiles from the attack damage pattern and attack them
    /// </summary>
    public void LaunchAttach (TileEntity targetTile, ActorEntity attacker, AttackData attack) {
        ShowAttackPattern(targetTile,attacker,attack);
        List<TileEntity> tileToAttack = new List<TileEntity>();
        foreach (KeyValuePair<TileEntity,int> t in tileHighlightAttack) {
            t.Key.cosmetic.ChangeTextureColor(new Color(0,0,5));
            if (t.Key.tileUser != null) {
                //Calcul precision
                bool missed;
                float rand1 = Random.Range(0,attacker.dex)+attacker.lck/10;
                float rand2 = Random.Range(0,t.Value*15);
                float rand3 = Random.Range(0,100)-t.Key.tileUser.agi/10;
                missed = rand1+rand2 < rand3;
                if (!missed) {
                    //Damage Calcul
                    
                    tileToAttack.Add(t.Key);
                }else{
                    t.Key.tileUser.ui.ShowDamageAmount(0);
                }
            }
        }
        foreach (TileEntity t in tileToAttack) {
            t.tileUser.TakeDammage(attacker, attack);
        }

        attack.Cost(attacker);

        bool canAttack = false;
        foreach (WeaponData w in attacker.weaponInventory) {
            foreach (AttackData a in w.attacks) {
                if (a.apCost <= attacker.ap && a.mpCost <= attacker.mp) continue;
                canAttack = true;
            }
        }
        if (canAttack) {
            ResetTileHighlight();
            manager.playerState = PlayerState.Normal;
            HighlightActionTiles();
        }

        //Hard code because problems :(
        targetTile.cosmetic.ChangeTextureColor(new Color(0,0,5));
        Invoke("ResetTileHighlight",0.2f);
    }
    /// <summary>
    /// Use a list of coordinates to return a list of corresponding tiles in the level
    /// </summary>
    public Dictionary<TileEntity,int> GetPattern (TileEntity startTile, List<Vector2Int> pattern, ActorEntity attacker) {
        int startPosX = startTile.coordinates.x;
        int startPosY = startTile.coordinates.y;
        TileEntity[,] grid = manager.grid;
        List<TileEntity> potentialTargets = new List<TileEntity>();
        //Convert pattern to tiles
        foreach (Vector2Int v in pattern) {
            if ((startPosX + v.x) >= grid.GetLength(0) || (startPosY + v.y) >= grid.GetLength(1)) continue;
            if ((startPosX + v.x) < 0 || (startPosY + v.y) < 0) continue;
            TileEntity t = grid[startPosX + v.x, startPosY + v.y];
            if (t == null) continue;
            if (t.tileState != TileState.Walk && t.tileState != TileState.Occupied) continue;
            potentialTargets.Add(grid[startPosX + v.x, startPosY + v.y]);
        }
        //Clear tiles that are blocked in the path
        Dictionary<TileEntity,int> finalTargets = new Dictionary<TileEntity, int>();
        for (int i = potentialTargets.Count - 1; i >= 0; i--) {
            (bool,int) tupleAccess;
            if (potentialTargets[i] == attacker) continue;
            tupleAccess = CheckTileAccess(grid[startPosX, startPosY],potentialTargets[i]);
            if (!tupleAccess.Item1 || finalTargets.TryGetValue(potentialTargets[i],out int value)) continue;
            if (potentialTargets[i].tileState == TileState.Occupied && potentialTargets[i].tileUser == null) continue;
            finalTargets.Add(potentialTargets[i],tupleAccess.Item2);
        }
        return finalTargets;
    }

    private (bool,int) CheckTileAccess (TileEntity currentTile, TileEntity targetTile, int tileDistance = 0) {
        TileEntity closestTile = null;
        float dist = Mathf.Infinity;
        tileDistance += 1;
        foreach (TileEntity n in targetTile.directNeighbourTiles) {
            float nDist = (currentTile.transform.position - n.transform.position).magnitude;
            if (nDist < dist) {
                dist = nDist;
                closestTile = n;
            }
        }
        if (closestTile == currentTile) {
            return (true,tileDistance);
        }else if (closestTile.tileState != TileState.Walk && closestTile.tileState != TileState.Occupied) {
            return (false,tileDistance);
        }else{
            return CheckTileAccess(currentTile, closestTile, tileDistance);
        }
    }
#endregion
}