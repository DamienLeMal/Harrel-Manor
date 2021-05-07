using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorEntity : MonoBehaviour
{
    [SerializeField] private ActorData baseStats;
    [HideInInspector] public List<WeaponData> weaponInventory;
    [HideInInspector] public int str, dex, spd, intl, agi, con, lck, mnt, pm_max, ap_max, mp_max, hp_max, mnt_max;
    public int pm, ap, mp, hp;
    [HideInInspector] public TileEntity currentTile;
    public CombatManager manager = null;
    public ActorUi ui;
    
    private void Awake() {
        if (baseStats == null) {
            Debug.LogError("No base stat was set for " + this);
        }else{
            Constructor(baseStats);
        }
        foreach (WeaponData w in weaponInventory) {
            foreach (AttackData a in w.attacks) {
                a.InitialiseData();
            }
        }
        
        //Security
        if (transform.GetComponentInParent<CombatManager>() == null) {
            Debug.LogError("Please set the Actor as child of Combat Manager");
        }
        manager = transform.GetComponentInParent<CombatManager>();
        manager.gameEntities.entities.Add(gameObject);
    }

    private void Constructor (ActorData data) {
        weaponInventory = data.weaponInvetory;
        str = data.str;
        dex = data.dex;
        spd = data.spd;
        intl = data.intl;
        agi = data.agi;
        con = data.con;
        lck = data.lck;
        mnt = data.mnt;
        mnt_max = mnt;
        pm_max = (int)data.spd/10;
        ap_max = (int)(data.dex+data.str)/20;
        mp_max = (int)data.intl/10;
        hp_max = (int)data.con/5;
        pm = pm_max;
        ap = ap_max;
        mp = mp_max;
        hp = hp_max;
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
        closestTile.SetTileUser(this);
        currentTile = closestTile;
        transform.position = closestTile.transform.position + new Vector3(0, GetComponent<MeshRenderer>().bounds.size.y / 2, 0);
    }

    /// <summary>
    /// Move one tile at a time
    /// </summary>
    public IEnumerator MoveOneTile (List<TileEntity> path) {
        if (path.Count > 0) {
            pm -= 1;
            Vector3 newPos = path[0].transform.position + new Vector3(0, GetComponent<MeshRenderer>().bounds.size.y / 2, 0);
            LeanTween.move(gameObject,newPos,1f);
            path.RemoveAt(0);
            yield return new WaitForSeconds(1.1f);
            StartCoroutine(MoveOneTile(path));
        }else{
            //end
            GetComponent<CombatManager>().playerState = PlayerState.Normal;
            GetComponent<CombatManager>().ResetActorsPositions();
        }
    }

    public void TakeDammage (ActorEntity attacker, AttackData attack) {
        //Damage Calcul
        int bonusDmg;
        if (attack.rangedAttack) {
            bonusDmg = (int)(Random.Range(0,attacker.dex)/10);
        }else{
            bonusDmg = (int)(Random.Range(0,attacker.str)/10);
        }
        int dmg = attack.dmg + bonusDmg;

        hp -= dmg;
        ui.ShowDamageAmount(dmg);
        Debug.Log("Damage taken : " + dmg);
        if (hp <= 0) {
            ActorDeath();
        }
    }
    /// <summary>
    /// This Method should be overrided
    /// </summary>
    virtual protected void ActorDeath () { }
}
