using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorBrain : MonoBehaviour
{
    [SerializeField] private ActorData baseStats;
    private int str, dex, spd, intl, agi, con, lck, mnt;
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
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
