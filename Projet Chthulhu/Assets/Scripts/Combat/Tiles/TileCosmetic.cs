using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TileCosmetic : MonoBehaviour
{
    [SerializeField] Material wallM = null;
    [SerializeField] Material pathM = null;
    [SerializeField] Texture lineT = null;
    [SerializeField] Texture angleT = null;
    [SerializeField] Texture singleT = null;
    [SerializeField] Texture occupiedT = null;
    private TileEntity currentTile;
    private MeshRenderer tileRenderer;
    // Start is called before the first frame update
    private void Awake() {
        tileRenderer = GetComponentInChildren<MeshRenderer>();
        
        currentTile = GetComponent<TileEntity>();
    }
    private void Start() {
        if (currentTile.tileState == TileState.Walk) {
            tileRenderer.material = pathM;
        }else{
            tileRenderer.material = wallM;
        }
        UpdateMaterial(currentTile.tileState);
    }

    public void UpdateMaterial (TileState state) {
        if (state != TileState.Walk && state != TileState.Occupied) return;
        tileRenderer.material = pathM;
        //if (currentTile.tileUser == null) return;
        if (currentTile.tileState != TileState.Occupied) return;
        SetSprite(occupiedT,0);
    }

    public void ChangeTextureColor (Color clr) {
        tileRenderer.material.color = clr*0.5f;
    }

    public void TraceLine (TileEntity connectedTile1, Color clr, TileEntity connectedTile2 = null) {
        ChangeTextureColor(clr);
        int value = GetCase(connectedTile1,connectedTile2);
        currentTile.transform.Rotate(new Vector3(0,0,0),Space.World);
        switch (value) {
            case 1 ://A
                SetSprite(singleT,180);
                break;
            case 2 ://B
                SetSprite(singleT,90);
                break;
            case 3 ://BC
                SetSprite(angleT,180);
                break;
            case 4 ://C
                SetSprite(singleT,0);
                break;
            case 5 ://AC
                SetSprite(lineT,0);
                break;
            case 6 ://AB
                SetSprite(angleT,90);
                break;
            case 8 ://D
                SetSprite(singleT,-90);
                break;
            case 9 ://CD
                SetSprite(angleT,-90);
                break;
            case 10 ://BD
                SetSprite(lineT,90);
                break;
            case 12 ://AD
                SetSprite(angleT,0);
                break;
        }
    }
    private void SetSprite (Texture t, int rotation) {
        tileRenderer.material.mainTexture = t;
        LeanTween.rotate(currentTile.gameObject,new Vector3(0,rotation,0),0f);
    }
    
    private int GetCase (TileEntity connectedTile1, TileEntity connectedTile2) {
        int value = 0;
        if (connectedTile1 != null) {
            value += FindTilePosition(connectedTile1.coordinates - currentTile.coordinates);
        }
        if (connectedTile2 != null) {
            value += FindTilePosition(connectedTile2.coordinates - currentTile.coordinates);
        }

        return value;
    }

    private int FindTilePosition (Vector2Int coord) {
        if (coord == new Vector2Int(0,1)) return 1;
        if (coord == new Vector2Int(1,0)) return 2;
        if (coord == new Vector2Int(0,-1)) return 4;
        if (coord == new Vector2Int(-1,0)) return 8;
        Debug.LogError("A Direct Neighbor Tile wasn't found");
        return 0;
    }
}
