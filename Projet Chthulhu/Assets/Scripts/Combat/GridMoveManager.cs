using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMoveManager : MonoBehaviour
{
    private TileEntity[,] tileGrid = null;
    public Dictionary<TileEntity,int> tileHighlightRanges = new Dictionary<TileEntity, int>();
    private void Start() {
        tileGrid = GetComponent<CombatManager>().grid;
    }
    public void HighlightSurroundingTiles (TileEntity currentTile) {
        int range = currentTile.tileUser.GetComponent<ActorEntity>().mp;
        
        SetRangeValue(currentTile.neighbourTiles,range);
        foreach (KeyValuePair<TileEntity,int> d in tileHighlightRanges) {
            d.Key.GetComponentInChildren<MeshRenderer>().material.color= new Color(d.Value,0,0);
        }
    }
    public void StartPathFinding (TileEntity startTile, TileEntity endTile) {
        foreach (TileEntity t in PathFinding(startTile,endTile)) {
            t.GetComponentInChildren<MeshRenderer>().material.color= new Color(0,0,0);
        }
    }

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
}
