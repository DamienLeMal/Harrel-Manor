using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    private CombatManager manager = null;
    private TileEntity[,] tileGrid = null;
    /// <summary>
    /// Highlight all tiles the player can interact with
    /// </summary>
    /// <typeparam name="TileEntity">Tiles that are interactable</typeparam>
    /// <typeparam name="int">Distance of the tile from the player</typeparam>
    /// <returns></returns>
    public Dictionary<TileEntity,int> tileHighlightRanges;
    private List<TileEntity> tempHighlightedTiles;
    private void Start() {
        manager = GetComponent<CombatManager>();
        tileGrid = manager.grid;
    }


    #region Higlight Surroundings
    public void ClearTileHighlight () {
        if (tileHighlightRanges != null) {
            tileHighlightRanges.Clear();
        }
        if (tempHighlightedTiles != null) {
            tempHighlightedTiles.Clear();
        }
    }
    public void ResetHighlight () {
        if (tileHighlightRanges != null) {
            foreach (KeyValuePair<TileEntity,int> d in tileHighlightRanges) {
                d.Key.UpdateMaterial();
            }
        }
        if (tempHighlightedTiles != null) {
            foreach (TileEntity d in tempHighlightedTiles) {
                d.UpdateMaterial();
            }
        }
    }
    public void HighlightActionTiles (TileEntity currentTile) {
        tileHighlightRanges = new Dictionary<TileEntity, int>();
        
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
            d.Key.GetComponentInChildren<MeshRenderer>().material.color= new Color(d.Value,0,0);
        }
    }


    /// <summary>
    /// Associate every tile in range with a value decreasing in distance
    /// </summary>
    private void SetMoveRangeValue (List<TileEntity> nTiles, int range) {
        foreach (TileEntity nt in nTiles) {
            if (nt.tileState == TileState.Walk) {
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
        List<TileEntity> tileList = GetPattern(manager.player.GetComponent<PlayerEntity>().currentTile,atk.positionPatternCoord);
        foreach (TileEntity t in tileList) {
            if (!tileHighlightRanges.ContainsKey(t)) {
                tileHighlightRanges.Add(t,1);
            }
        }
    }

    #endregion
#region Move
    
    #region PathFinding
    //Show the shortest path to destination
    public void StartPathFinding (TileEntity startTile, TileEntity endTile) {
        foreach (TileEntity t in PathFinding(startTile,endTile)) {
            t.GetComponentInChildren<MeshRenderer>().material.color= new Color(0,0,0);
        }
    }
    
    /// <summary>
    /// Move the Actor along the shortest path
    /// </summary>
    public void MoveAlongPath (TileEntity startTile, TileEntity endTile, ActorEntity actor) {
        List<TileEntity> path = PathFinding(startTile,endTile);
        StartCoroutine(MoveOneTile(path,actor));
    }
    /// <summary>
    /// Move one tile at a time
    /// </summary>
    IEnumerator MoveOneTile (List<TileEntity> path, ActorEntity actor) {
        if (path.Count > 0) {
            actor.pm -= 1;
            LeanTween.move(actor.gameObject,path[0].transform.position,1f);
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

    public void ShowAttackPattern (TileEntity startTile) {
        tempHighlightedTiles = GetPattern(startTile, manager.activeButton.attack.damagePatternCoord);
        foreach (TileEntity t in tempHighlightedTiles) {
            t.GetComponentInChildren<MeshRenderer>().material.color= new Color(0,0,0);
        }
    }

    public void LaunchAttach (TileEntity targetTile) {
        foreach (TileEntity t in tempHighlightedTiles) {
            t.GetComponentInChildren<MeshRenderer>().material.color= new Color(0,0,5);
            
        }
        //Hard code because problems :(
        targetTile.GetComponentInChildren<MeshRenderer>().material.color= new Color(0,0,5);
        Invoke("ResetHighlight",0.2f);
        Invoke("HighlightTiles",0.2f);
    }

    private List<TileEntity> GetPattern (TileEntity startTile, List<Vector2Int> pattern) {
        int startPosX = startTile.coordinates.x;
        int startPosY = startTile.coordinates.y;
        TileEntity[,] grid = manager.grid;
        List<TileEntity> potentialTargets = new List<TileEntity>();
        //Convert pattern to tiles
        foreach (Vector2Int v in pattern) {
            if ((startPosX + v.x) >= grid.GetLength(0) || (startPosY + v.y) >= grid.GetLength(1)) { continue; }
            if ((startPosX + v.x) < 0 || (startPosY + v.y) < 0) { continue; }
            TileEntity t = grid[startPosX + v.x, startPosY + v.y];
            if (t == null) { continue; }
            if (t.tileState != TileState.Walk) { continue; }
            potentialTargets.Add(grid[startPosX + v.x, startPosY + v.y]);
        }
        //Clear tiles that are blocked in the path
        List<TileEntity> finalTargets = new List<TileEntity>();
        for (int i = potentialTargets.Count - 1; i >= 0; i--) {
            bool a;
            if (true/*tileGrid[12,7] == potentialTargets[i]*/) {
                a = CheckTileAccess(grid[startPosX, startPosY],potentialTargets[i]);
            }
            if (a) {
                finalTargets.Add(potentialTargets[i]);
            }
        }
        return finalTargets;
    }

        //get closest neighbor from source to end tile unil you reach it and 
        //if there's a wall (check if a tile dist is == to wall /!\) remove the tile because it's not accessible
    #region legacy CheckTileAccess
    //private bool CheckTileAccess (TileEntity currentTile, TileEntity targetTile) {
    //    TileEntity closestTile = null;
    //    float dist = Mathf.Infinity;
    //    //Debug.Log("-----Start------------------------------------");
    //    foreach (TileEntity n in currentTile.directNeighbourTiles) {
    //        float nDist = (targetTile.transform.position - n.transform.position).magnitude;
    //        //Debug.Log("Block ? " + (n.tileState == TileState.Block) + " | Dist : " + nDist + " | Coord : " + n.coordinates);
    //        if (nDist < dist) {
    //            dist = nDist;
    //            closestTile = n;
    //        }else if (nDist == dist) {
    //            
    //            //if (closestTile.tileState == TileState.Block) {
    //            //    closestTile = n;
    //            //}
    //        }
    //    }
    //    //Debug.Log("Chosen : Block ? " + (closestTile.tileState == TileState.Block) + " | Dist : " + dist + "\n------END---------------------------------");
    //    if (closestTile.tileState == TileState.Block) {
    //        return false;
    //    }else if (closestTile == targetTile) {
    //        return true;
    //    }else{
    //        //Debug.Log("----Next---------------------------------");
    //        return CheckTileAccess(closestTile, targetTile);
    //    }
    //}
    #endregion
    private bool CheckTileAccess (TileEntity currentTile, TileEntity targetTile) {
        TileEntity closestTile = null;
        float dist = Mathf.Infinity;
        //Debug.Log("-----Start------------------------------------");
        foreach (TileEntity n in targetTile.directNeighbourTiles) {
            float nDist = (currentTile.transform.position - n.transform.position).magnitude;
            //Debug.Log("Block ? " + (n.tileState == TileState.Block) + " | Dist : " + nDist + " | Coord : " + n.coordinates);
            if (nDist < dist) {
                dist = nDist;
                closestTile = n;
            }else if (nDist == dist) {
                //if (closestTile.tileState == TileState.Block) {
                //    closestTile = n;
                //}
            }
        }
        //Debug.Log("Chosen : Block ? " + (closestTile.tileState == TileState.Block) + " | Dist : " + dist + "\n------END---------------------------------");
        if (closestTile == currentTile) {
            return true;
        }else if (closestTile.tileState == TileState.Block) {
            return false;
        }else{
            //Debug.Log("----Next---------------------------------");
            return CheckTileAccess(currentTile, closestTile);
        }
    }
#endregion
}