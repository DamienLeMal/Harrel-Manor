using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateGrid : MonoBehaviour
{
    [SerializeField] private GameObject tile = null;
    [SerializeField] private MapData map = null;
    [SerializeField] private GameObject tileParent = null;
    private CombatManager manager;
    void Start()
    {
        manager = GetComponent<CombatManager>();
        Generate(map);
    }
    /// <summary>
    /// Generate a grid based on a level data
    /// </summary>
    /// <param name="data"></param>
    private void Generate (MapData data) {
        int[,] map = data.ReadSheetData(data.map);
        manager.grid = new TileEntity[(int)Mathf.Sqrt(map.Length),(int)Mathf.Sqrt(map.Length)];
        TileEntity[,] tileGrid = manager.grid;
        
        for (int i = 0; i<Mathf.Sqrt(map.Length); i++) {
            for (int j = 0; j < Mathf.Sqrt(map.Length); j++) {
                if (map[i, j] == 0) continue;
                MeshRenderer mesh = tile.GetComponentInChildren<MeshRenderer>();
                GameObject g = Instantiate(tile, new Vector3((float)i*mesh.bounds.size.x, 0.3f, -(float)j*mesh.bounds.size.x),Quaternion.identity,tileParent.transform);
                TileEntity t = g.GetComponent<TileEntity>();
                tileGrid[i,j] = t;
                t.coordinates = new Vector2Int(i,j);
                t.manager = manager;

                //Setting the base state of the tile
                switch (map[i,j])
                {
                    case 1 : 
                        t.tileState = TileState.Walk;
                        break;
                    case 2 : 
                        t.tileState = TileState.Block;
                        break;
                    case 3 :
                        t.tileState = TileState.Occupied;
                        break;
                    case 4 :
                        //Add door interface
                        t.tileState = TileState.Block;
                        break;
                }
            }
        }
        int[,] coord;
        foreach (TileEntity t in tileGrid) {
            if (t == null) { continue;}
            coord = new int[4,2] {
                {t.coordinates.x-1,t.coordinates.y},
                {t.coordinates.x+1,t.coordinates.y},
                {t.coordinates.x,t.coordinates.y-1},
                {t.coordinates.x,t.coordinates.y+1}};
            SetNeighbours(coord,t.directNeighbourTiles,tileGrid);
            SetNeighbours(coord,t.allNeighbourTiles,tileGrid);

            coord = new int[4,2] {
                {t.coordinates.x-1,t.coordinates.y+1},
                {t.coordinates.x+1,t.coordinates.y+1},
                {t.coordinates.x-1,t.coordinates.y-1},
                {t.coordinates.x+1,t.coordinates.y-1}};
            SetNeighbours(coord,t.allNeighbourTiles,tileGrid);
            t.gameObject.SetActive(false);
        }
    }
    private void SetNeighbours (int[,] coord, List<TileEntity> nList, TileEntity[,] tileGrid) {
        for (int i = 0; i < 4; i++) {
            if(coord[i,0] < tileGrid.GetLength(0) && coord[i,1] < tileGrid.GetLength(1) && coord[i,0] >= 0 && coord[i,1] >= 0) {
                if (tileGrid[coord[i,0],coord[i,1]] != null) {
                    nList.Add(tileGrid[coord[i,0],coord[i,1]]);
                } 
            }
        }
    }
}