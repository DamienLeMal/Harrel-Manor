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
    private GameObject tileUser;
    public bool isWalkable;
    public TileState tileState;

    private void Start() {
        switch (tileState) {
            case TileState.Walk : GetComponentInChildren<MeshRenderer>().material = pathM;
                break;
            case TileState.Block : GetComponentInChildren<MeshRenderer>().material = wallM;
                break;
        }
    }

    public void SetTileUser (GameObject user) {
        tileUser = user;
        if (user != null) {
            tileState = TileState.Block;
        }else{
            tileState = TileState.Walk;
        }
    }
    
}
