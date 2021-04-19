using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyBrain : MonoBehaviour
{
    private Dictionary<TileEntity,int> tilesInRange;
    private EnnemyEntity entity;
    private GridManager gridManager;
    private Dictionary<TileEntity,Vector3Int> tileScore;

    private void Start() {
        entity = GetComponent<EnnemyEntity>();
        gridManager = transform.GetComponentInParent<GridManager>();
    }
    //Get Move range value
    //Foreach tiles, evaluate potential damage output and survivability
    //For same values, choose the one with less cost (move range value distance)

    private void EvaluateTiles () {
        tilesInRange = gridManager.EnnemyGetMoveRange(entity.currentTile.directNeighbourTiles,entity.pm);

        //Evaluation
        foreach (KeyValuePair<TileEntity,int> t in tilesInRange) {
            //x is pm cost, lowest is best
            int x = entity.pm - t.Value;


            
            //y is potential damage output
            //GET BEST ATTACK METHOD
                //get attack position pattern -> get attack pattern
                //for all these tiles if player not on them -> 0
                //If player on one of them, score = damage - x and hihest is best
                //Highest score is this tile damage score

            //z is defense score
            //get attack position pattern -> get attack pattern but for the player this time
                //this tile score is the distance from the player attack range and -1 if it's not in the Dictionary
        }

        //Choose Tile based on comportement
    }
}
