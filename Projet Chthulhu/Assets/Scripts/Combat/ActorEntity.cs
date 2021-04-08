using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorEntity : MonoBehaviour
{
    [SerializeField] private ActorData baseStats;
    private int str, dex, spd, intl, agi, con, lck, mnt, pm_max, ap_max, mp_max;
    public int pm, ap, mp;
    public TileEntity currentTile;
    private CombatManager manager = null;
    
    private void Awake() {
        if (baseStats == null) {
            Debug.LogError("No base stat was set for " + this);
        }else{
            Constructor(baseStats);
        }
        
        //Security
        if (transform.GetComponentInParent<CombatManager>() == null) {
            Debug.LogError("Please set the Actor as child of Combat Manager");
        }
        manager = transform.GetComponentInParent<CombatManager>();
        manager.gameEntities.entities.Add(gameObject);
    }

    private void Constructor (ActorData data) {
        str = data.str;
        dex = data.dex;
        spd = data.spd;
        intl = data.intl;
        agi = data.agi;
        con = data.con;
        lck = data.lck;
        mnt = data.mnt;
        pm_max = (int)data.spd/10;
        ap_max = (int)(data.dex+data.str)/20;
        mp_max = (int)data.intl/10;
        pm = pm_max;
        ap = ap_max;
        mp = mp_max;
    }
    /// <summary>
    /// Set the actor to the closest point in the grid and move him to it
    /// </summary>
    public void SnapToGrid () {
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
        currentTile = closestTile;
        transform.position = closestTile.transform.position;
    }
}
