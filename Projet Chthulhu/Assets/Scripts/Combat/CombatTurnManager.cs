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
    private bool playerPriority;
    public List<ActorEntity> fightingEntities = new List<ActorEntity>();

    private void Awake() {
        manager = GetComponent<CombatManager>();
    }
    
    private void NewTurn() {
        manager.turn = Turn.Start;
        foreach (ActorEntity a in fightingEntities) {
            a.mp = a.mp_max;
            a.pm = a.pm_max;
            a.ap = a.ap_max;
        }
        if (playerPriority) {
            manager.turn = Turn.PlayerTurn;
        }else{
            manager.turn = Turn.EnnemyTurn;
            Debug.Log("Ennemy's Turn");
        }
    }

    public void EndTurn (ActorEntity a) {

    }
}
