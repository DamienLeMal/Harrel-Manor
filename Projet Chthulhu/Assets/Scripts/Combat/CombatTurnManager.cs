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
    private GridManager gridManager = null;
    public List<ActorEntity> fightingEntities;
    public int currentActorIndex = 0;

    private void Start() {
        manager = CombatManager.current;
        gridManager = GetComponent<GridManager>();
    }
    
    public void NewTurn() {
        manager.playerState = PlayerState.Normal;
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
        if (fightingEntities[currentActorIndex] != manager.player) {
            if (fightingEntities[currentActorIndex] != a) return;
        }
        manager.gridManager.ClearTileHighlight();
        currentActorIndex += 1;
        if (currentActorIndex > fightingEntities.Count - 1) {
            NewTurn();
            return;
        }
        StartTurn(fightingEntities[currentActorIndex]);
    }

    private void StartTurn (ActorEntity a) {
        if (a == manager.player) {
            manager.turn = Turn.PlayerTurn;
            SoundEventManager.current.PlayerTurn((float)manager.player.hp/(float)manager.player.hp_max);
        }else{
            Debug.Log("Start ennemy turn");
            manager.turn = Turn.EnnemyTurn;
            StartCoroutine(a.GetComponent<EnnemyBrain>().PlayTurn());
        }
    }

    public void TestCombatEnd () {
        if (fightingEntities.Count != 1) return;
        manager.turn = Turn.End;
        //Combat end, player win
        SoundEventManager.current.CombatEnd(true);
    }
}
