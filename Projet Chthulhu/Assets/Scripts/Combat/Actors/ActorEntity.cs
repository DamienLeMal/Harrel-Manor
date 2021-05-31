using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorEntity : MonoBehaviour
{
    [SerializeField] protected ActorData baseStats;
    [HideInInspector] public List<WeaponData> weaponInventory;
    [HideInInspector] public int str, dex, spd, intl, agi, con, lck, mnt, pm_max, ap_max, mp_max, hp_max, mnt_max;
    public int _pm, _ap, _mp, _hp;
    #region Points Definition
    public int pm {
        get {return _pm;}
        set {
            _pm = value;
            if (_pm > pm_max) _pm = pm_max;
        }
    }
    public int ap {
        get {return _ap;}
        set {
            _ap = value;
            if (_ap > ap_max) _ap = ap_max;
        }
    }
    public int mp {
        get {return _mp;}
        set {
            _mp = value;
            if (_mp > mp_max) _mp = mp_max;
        }
    }
    public int hp {
        get {return _hp;}
        set {
            _hp = value;
            if (_hp > hp_max) _hp = hp_max;
        }
    }

    #endregion
    [HideInInspector] public string entityName;
    [HideInInspector] public int level = 0;
    [HideInInspector] public TileEntity currentTile;
    protected CombatManager manager;
    public ActorUi ui;

    
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

    public virtual void Start() {
        foreach (WeaponData w in weaponInventory) {
            foreach (AttackData a in w.attacks) {
                Debug.Log("Initialise data");
                a.InitialiseData();
            }
        }
    }

    private void Constructor (ActorData data) {
        entityName = data.name;
        foreach (WeaponData wd in data.weaponInvetory) {
            weaponInventory.Add(wd);
        }
        str = data.str;
        dex = data.dex;
        spd = data.spd;
        intl = data.intl;
        agi = data.agi;
        con = data.con;
        lck = data.lck;
        mnt = data.mnt;
        mnt_max = mnt;
        NewMaxStat();
        pm = pm_max;
        ap = ap_max;
        mp = mp_max;
        hp = hp_max;
    }

    protected void NewMaxStat () {
        pm_max = (int)spd/10;
        ap_max = (int)(dex+str)/20;
        mp_max = (int)intl/10;
        hp_max = (int)con/5;
    }
    protected int GetRealLevel () {
        return (str + dex + spd + intl + agi + con + lck)/7;
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
    public IEnumerator MoveOneTile (List<TileEntity> path, bool firstMove) {
        if (path.Count > 0) {
            pm -= 1;
            Vector3 newPos = path[0].transform.position + new Vector3(0, GetComponent<MeshRenderer>().bounds.size.y / 2, 0);
            float speed = 50/spd;
            if (firstMove) {
                LeanTween.move(gameObject,newPos,speed).setEaseInSine();
            }else{
                LeanTween.move(gameObject,newPos,speed);
            }
            path.RemoveAt(0);
            yield return new WaitForSeconds(speed);
            StartCoroutine(MoveOneTile(path, false));
        }else{
            //end
            manager.playerState = PlayerState.Normal;
            manager.ResetActorsPositions();
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
        if (hp <= 0) {
            ActorDeath();
        }
        CombatEventSystem.current.TakeDamage(dmg,this);
        CombatEventSystem.current.DealDamage(dmg,attacker);
    }
    /// <summary>
    /// This Method should be overrided
    /// </summary>
    virtual protected void ActorDeath () { 
        SoundEventManager.current.ActorDeath();
    }
}
