using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;

using UnityEngine.UI;

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

    //Test
    public Text text;
    public void SetText (string txt) {
        text.text = txt;
    }

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
        gManager.ResetTileHighlight();
        if (manager.pStateAffectGrid.Contains(manager.playerState)) {
            if (gManager.tileHighlightRanges.TryGetValue(this,out int value)){
                //yes
                gManager.HighlightActionTiles();
                switch (manager.playerState) {
                    case PlayerState.Moving :
                        gManager.StartPathFinding(manager.player.currentTile,this);
                        break;
                    case PlayerState.Attacking :
                        gManager.ShowAttackPattern(this, manager.player, manager.activeButton.attack);
                        //Hard code because problems :(
                        cosmetic.ChangeTextureColor(new Color(0,0,0));
                        break;
                }
            }else{
                //no
                gManager.HighlightActionTiles();
            } 
        }
        
    }
    private void OnMouseDown() {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        if (manager.playerState == PlayerState.Moving) {
            manager.playerState = PlayerState.Locked;
            gManager.ResetTileHighlight();
            if (gManager.tileHighlightRanges.TryGetValue(this,out int value)){
                gManager.MoveAlongPath(this,manager.player);
            }
        }
        if (manager.playerState == PlayerState.Attacking && gManager.tileHighlightRanges.TryGetValue(this,out int val)) {
            gManager.LaunchAttach(this,manager.player, manager.activeButton.attack);
            gManager.Invoke("HighlightTiles",0.2f);
        }
    }
#endregion
}