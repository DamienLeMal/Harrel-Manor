using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorEntity : MonoBehaviour
{
    [SerializeField] private ActorData baseStats;
    private int str, dex, spd, intl, agi, con, lck, mnt;
    private CombatManager manager = null;
    private void Constructor (ActorData data) {
        str = data.str;
        dex = data.dex;
        spd = data.spd;
        intl = data.intl;
        agi = data.agi;
        con = data.con;
        lck = data.lck;
        mnt = data.mnt;
    }
    private void Awake() {
        Constructor(baseStats);
        //Security
        if (transform.GetComponentInParent<CombatManager>() == null) {
            Debug.LogError("Please set the Actor as child of Combat Manager");
        }
        manager = transform.GetComponentInParent<CombatManager>();
        manager.gameEntities.entities.Add(gameObject);
    }
    private void Start() {
        //Temp
        Invoke("SnapToGrid",2f);
    }

    /// <summary>
    /// Set the actor to the closest point in the grid and move him to it
    /// </summary>
    private void SnapToGrid () {
        float minDist = Mathf.Infinity;
        float a;
        TileEntity closestTile = null;
        foreach (TileEntity t in manager.grid) {
            if (t?.tileState != TileState.Walk || t == null) { continue; }
            a = (transform.position - t.transform.position).magnitude;
            if (a < minDist) {
                minDist = a;
                closestTile = t;
            }
        }
        //Apply
        closestTile.SetTileUser(gameObject);
        transform.position = closestTile.transform.position;
    }
}
