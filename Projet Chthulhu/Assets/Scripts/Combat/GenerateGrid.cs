using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateGrid : MonoBehaviour
{
    [SerializeField] private GameObject tile = null;
    [SerializeField] private Material walk = null;
    [SerializeField] private Material wall = null;
    [SerializeField] private MapData map = null;
    void Start()
    {
        Generate(map);
    }

    private void Generate (MapData data) {
        int[,] map = ReadMapData(data.map);
        for (int i = 0; i<Mathf.Sqrt(map.Length); i++) {
            for (int j = 0; j < Mathf.Sqrt(map.Length); j++) {
                if (map[i,j] != 0) {
                    GameObject g = Instantiate(tile, new Vector3((float)i*tile.GetComponentInChildren<MeshRenderer>().bounds.size.x, 0, -(float)j*tile.GetComponentInChildren<MeshRenderer>().bounds.size.x),Quaternion.identity);
                    switch (map[i,j])
                    {
                        case 1 : g.GetComponentInChildren<MeshRenderer>().material = walk;
                            break;
                        case 2 : g.GetComponentInChildren<MeshRenderer>().material = wall;
                            break;
                    }
                }
            }
        }
    }

    private int[,] ReadMapData (string data) {
        
        string[] correctData = data.Split(',');
        int size = correctData.Length;
        int[] mapOne = new int[size];
        for (int i = 0; i < size; i++) {
            mapOne[i] = int.Parse(correctData[i]);
        }

        int[,] map = new int[(int)Mathf.Sqrt(size),(int)Mathf.Sqrt(size)];
        int count = 0;
        int rowCount = 0;
        foreach (int i in mapOne) {
            map[count,rowCount] = i;
            count++;
            if (count == (int)Mathf.Sqrt(size)) {
                count = 0;
                rowCount++;
            }
        }
        return map;
    }
}
