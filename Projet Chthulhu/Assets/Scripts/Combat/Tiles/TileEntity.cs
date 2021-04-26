using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;

public enum TileState {
    Walk,
    Block,
    Occupied
}
public class TileEntity : MonoBehaviour
{
    public Vector2Int coordinates;
    public List<TileEntity> directNeighbourTiles = new List<TileEntity>();
    public List<TileEntity> allNeighbourTiles = new List<TileEntity>();
    public ActorEntity tileUser;
    public bool isWalkable;
    public TileState tileState;
    public CombatManager manager;
    private GridManager gManager;
    public TileCosmetic cosmetic;
    //PathFinding
    [HideInInspector] public int gCost;
    [HideInInspector] public int hCost;
    public int fCost { get { return gCost + hCost;}}
    [HideInInspector]public TileEntity previousInPath = null;

    private void Awake() {
        cosmetic = GetComponent<TileCosmetic>();
    }
    private void Start() {
        UpdateMaterial();
        gManager = manager.GetComponent<GridManager>();
    }

    public void SetTileUser (ActorEntity user) {
        tileUser = user;
        if (user != null) {
            tileState = TileState.Occupied;
        }else{
            tileState = TileState.Walk;
        }
        UpdateMaterial();
    }

    public void UpdateMaterial () {
        cosmetic.UpdateMaterial(tileState);
    }
#region Interactions
    private void OnMouseEnter() {
        TileEntity currentTile = manager.player.currentTile;
        gManager.ResetTileHighlight();
        if (manager.pStateAffectGrid.Contains(manager.playerState)) {
            if (gManager.tileHighlightRanges.TryGetValue(this,out int value)){
                //yes
                gManager.HighlightActionTiles(currentTile);
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
                gManager.HighlightActionTiles(currentTile);
            } 
        }
        
    }
    private void OnMouseDown() {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        if (manager.playerState == PlayerState.Moving) {
            manager.playerState = PlayerState.Locked;
            gManager.ResetTileHighlight();
            if (gManager.tileHighlightRanges.TryGetValue(this,out int value)){
                gManager.MoveAlongPath(manager.player.currentTile,this,manager.player.GetComponent<ActorEntity>());
            }
        }
        if (manager.playerState == PlayerState.Attacking && gManager.tileHighlightRanges.TryGetValue(this,out int val)) {
            gManager.LaunchAttach(this,manager.player, manager.activeButton.attack);
        }
    }
#endregion
}