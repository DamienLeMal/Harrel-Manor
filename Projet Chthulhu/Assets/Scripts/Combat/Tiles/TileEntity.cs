using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileState {
    Walk,
    Block
}
public class TileEntity : MonoBehaviour
{
    [SerializeField] private Material pathM = null;
    [SerializeField] private Material wallM = null;
    public Vector2 coordinates;
    public List<TileEntity> neighbourTiles = new List<TileEntity>();
    public GameObject tileUser;
    public bool isWalkable;
    public TileState tileState;
    public CombatManager manager;
    //PathFinding
    [HideInInspector] public int gCost;
    [HideInInspector] public int hCost;
    public int fCost { get { return gCost + hCost;}}
    [HideInInspector]public TileEntity previousInPath = null;

    private void Start() {
        UpdateMaterial();
    }

    public void SetTileUser (GameObject user) {
        tileUser = user;
        if (user != null) {
            tileState = TileState.Block;
        }else{
            tileState = TileState.Walk;
        }
    }

    private void UpdateMaterial () {
        switch (tileState) {
            case TileState.Walk : 
                GetComponentInChildren<MeshRenderer>().material = pathM;
                break;
            case TileState.Block : 
                GetComponentInChildren<MeshRenderer>().material = wallM;
                break;
        }
    }

    private void OnMouseEnter() {
        if (manager.GetComponent<GridMoveManager>().tileHighlightRanges.TryGetValue(this,out int value)){
            //yes
            manager.GetComponent<GridMoveManager>().HighlightSurroundingTiles(manager.player.GetComponent<PlayerEntity>().currentTile);
            manager.GetComponent<GridMoveManager>().StartPathFinding(manager.player.GetComponent<PlayerEntity>().currentTile,this);
        }
    }

    private void OnMouseExit() {
    }
}
