using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum TileState {
    Walk,
    Block
}
public class TileEntity : MonoBehaviour
{
    [SerializeField] private Material pathM = null;
    [SerializeField] private Material wallM = null;
    public Vector2Int coordinates;
    public List<TileEntity> directNeighbourTiles = new List<TileEntity>();
    public List<TileEntity> allNeighbourTiles = new List<TileEntity>();
    public GameObject tileUser;
    public bool isWalkable;
    public TileState tileState;
    public CombatManager manager;
    private GridManager gManager;
    //PathFinding
    [HideInInspector] public int gCost;
    [HideInInspector] public int hCost;
    public int fCost { get { return gCost + hCost;}}
    [HideInInspector]public TileEntity previousInPath = null;

    private void Start() {
        UpdateMaterial();
        gManager = manager.GetComponent<GridManager>();
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
        TileEntity currentTile = manager.player.GetComponent<PlayerEntity>().currentTile;
        gManager.ResetHighlight();
        if (manager.pStateAffectGrid.Contains(manager.playerState)) {
            if (gManager.tileHighlightRanges.TryGetValue(this,out int value)){
                //yes
                gManager.HighlightSurroundingTiles(currentTile);
                switch (manager.playerState) {
                    case PlayerState.Moving :
                        gManager.StartPathFinding(currentTile,this);
                        break;
                    case PlayerState.Attacking :
                        gManager.ShowAttackPattern(this);
                        //Hard code because problems :(
                        GetComponentInChildren<MeshRenderer>().material.color= new Color(0,0,0);
                        break;
                }
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
        if (manager.playerState == PlayerState.Attacking && gManager.tileHighlightRanges.TryGetValue(this,out int val)) {
            gManager.LaunchAttach(this);
        }
    }
#endregion
}
