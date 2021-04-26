using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Turn {
    Start,
    PlayerTurn,
    EnnemyTurn,
    End
}
public class CombatTurnManager : MonoBehaviour
{
    private CombatManager manager = null;

    private void Awake() {
        manager = GetComponent<CombatManager>();
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
