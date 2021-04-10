using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMoveManager : MonoBehaviour
{
    private TileEntity[,] tileGrid = null;
    /// <summary>
    /// Highlight all tiles the player can interact with
    /// </summary>
    /// <typeparam name="TileEntity">Tiles that are interactable</typeparam>
    /// <typeparam name="int">Distance of the tile from the player</typeparam>
    /// <returns></returns>
    public Dictionary<TileEntity,int> tileHighlightRanges = new Dictionary<TileEntity, int>();
    private void Start() {
        tileGrid = GetComponent<CombatManager>().grid;
    }

    #region Higlight Surroundings
    public void ResetHighlight () {
        foreach (KeyValuePair<TileEntity,int> d in tileHighlightRanges) {
            d.Key.UpdateMaterial();
        }
    }
    public void HighlightSurroundingTiles (TileEntity currentTile) {
        int range = currentTile.tileUser.GetComponent<ActorEntity>().pm;
        
        SetRangeValue(currentTile.neighbourTiles,range);
        foreach (KeyValuePair<TileEntity,int> d in tileHighlightRanges) {
            d.Key.GetComponentInChildren<MeshRenderer>().material.color= new Color(d.Value,0,0);
        }
    }

    /// <summary>
    /// Associate every tile in range with a value decreasing in distance
    /// </summary>
    private void SetRangeValue (List<TileEntity> nTiles, int range) {
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
                    SetRangeValue(nt.neighbourTiles,range-1);
                } 
            }
        }
    }

    #endregion
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

            foreach (TileEntity neighbour in currentTile.neighbourTiles) {
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
}