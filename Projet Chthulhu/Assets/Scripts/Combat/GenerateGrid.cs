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
        int[] map = ReadMapData(data.map);
        for (int i =0; i<21; i++) {
            for (int j = 0; j < 21; j++) {
                if (map[i] != 0) {
                    GameObject g = Instantiate(tile, new Vector3((float)i*tile.GetComponentInChildren<MeshRenderer>().bounds.size.x, 0, (float)j*tile.GetComponentInChildren<MeshRenderer>().bounds.size.x),Quaternion.identity);
                    switch (map[i])
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

    private int[] ReadMapData (string data) {
        int[] map = new int[data.Length];
        for (int i = 0; i < data.Length; i++) {
            char c = data[i];
            map[i] = c - '0';
        }
        return map;
    }
}
