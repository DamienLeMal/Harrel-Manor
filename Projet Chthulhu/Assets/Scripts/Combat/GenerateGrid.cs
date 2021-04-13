using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateGrid : MonoBehaviour
{
    [SerializeField] private GameObject tile = null;
    [SerializeField] private MapData map = null;
    [SerializeField] private GameObject tileParent = null;
    void Start()
    {
        Generate(map);
    }
    /// <summary>
    /// Generate a grid based on a level data
    /// </summary>
    /// <param name="data"></param>
    private void Generate (MapData data) {
        int[,] map = data.ReadSheetData(data.map);
        GetComponent<CombatManager>().grid = new TileEntity[(int)Mathf.Sqrt(map.Length),(int)Mathf.Sqrt(map.Length)];
        TileEntity[,] tileGrid = GetComponent<CombatManager>().grid;
        
        for (int i = 0; i<Mathf.Sqrt(map.Length); i++) {
            for (int j = 0; j < Mathf.Sqrt(map.Length); j++) {
                if (map[i,j] != 0) {

                    GameObject g = Instantiate(tile, new Vector3((float)i*tile.GetComponentInChildren<MeshRenderer>().bounds.size.x, 0, -(float)j*tile.GetComponentInChildren<MeshRenderer>().bounds.size.x),Quaternion.identity);
                    g.transform.SetParent(tileParent.transform);
                    tileGrid[i,j] = g.GetComponent<TileEntity>();
                    g.GetComponent<TileEntity>().coordinates = new Vector2Int(i,j);
                    g.GetComponent<TileEntity>().manager = GetComponent<CombatManager>();

                    //Setting the base state of the tile
                    switch (map[i,j])
                    {
                        case 1 : 
                                g.GetComponent<TileEntity>().isWalkable = true;
                                g.GetComponent<TileEntity>().tileState = TileState.Walk;
                            break;
                        case 2 : 
                                g.GetComponent<TileEntity>().isWalkable = false;
                                g.GetComponent<TileEntity>().tileState = TileState.Block;
                            break;
                    }
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