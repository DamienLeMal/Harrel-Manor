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
    public List<ActorEntity> fightingEntities;
    private int currentActorIndex = 0;

    private void Awake() {
        manager = GetComponent<CombatManager>();
    }
    
    public void NewTurn() {
        Debug.Log("Very new turn");
        manager.turn = Turn.Start;
        foreach (ActorEntity a in fightingEntities) {
            a.mp = a.mp_max;
            a.pm = a.pm_max;
            a.ap = a.ap_max;
        }
        currentActorIndex = 0;
        StartTurn(fightingEntities[currentActorIndex]);
    }

    public void EndTurn (ActorEntity a) {
        Debug.Log(a + " end of turn");
        currentActorIndex += 1;
        if (currentActorIndex > fightingEntities.Count - 1) {
            NewTurn();
            return;
        }
        StartTurn(fightingEntities[currentActorIndex]);
    }

    private void StartTurn (ActorEntity a) {
        Debug.Log(a + " turn");
        if (a == manager.player) {
            manager.turn = Turn.PlayerTurn;
        }else{
            manager.turn = Turn.EnnemyTurn;
            StartCoroutine(a.GetComponent<EnnemyBrain>().PlayTurn());
        }
    }

    public void TestCombatEnd () {
        if (fightingEntities.Count != 1) return;
        //manager.turn = Turn.End;
        //Combat end, player win
    }
}
