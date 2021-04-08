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
    private GridMoveManager gManager;
    //PathFinding
    [HideInInspector] public int gCost;
    [HideInInspector] public int hCost;
    public int fCost { get { return gCost + hCost;}}
    [HideInInspector]public TileEntity previousInPath = null;

    private void Start() {
        UpdateMaterial();
        gManager = manager.GetComponent<GridMoveManager>();
    }

    public void SetTileUser (GameObject user) {
        tileUser = user;
        if (user != null) {
            tileState = TileState.Block;
        }else{
            tileState = TileState.Walk;
        }
    }

    public void UpdateMaterial () {
        switch (tileState) {
            case TileState.Walk : 
                GetComponentInChildren<MeshRenderer>().material = pathM;
                break;
            case TileState.Block : 
                GetComponentInChildren<MeshRenderer>().material = wallM;
                break;
        }
    }
#region Interactions
    private void OnMouseEnter() {
        if (manager.playerState == PlayerState.Moving) {
            TileEntity currentTile = manager.player.GetComponent<PlayerEntity>().currentTile;
            if (gManager.tileHighlightRanges.TryGetValue(this,out int value)){
                //yes
                gManager.HighlightSurroundingTiles(currentTile);
                gManager.StartPathFinding(currentTile,this);
            }else{
                //no
                gManager.HighlightSurroundingTiles(currentTile);
            }
        }
        
    }
    private void OnMouseDown() {
        if (manager.playerState == PlayerState.Moving) {
            manager.playerState = PlayerState.Locked;
            gManager.ResetHighlight();
            if (gManager.tileHighlightRanges.TryGetValue(this,out int value)){
                gManager.MoveAlongPath(manager.player.GetComponent<PlayerEntity>().currentTile,this,manager.player.GetComponent<ActorEntity>());
            }
        }
    }
#endregion
}
