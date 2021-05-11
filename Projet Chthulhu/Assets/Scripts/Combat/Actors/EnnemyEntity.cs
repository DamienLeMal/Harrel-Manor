using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyEntity : ActorEntity
{

    private void Start() {
        level = (str + dex + spd + intl + agi + con + lck)/7;
        Debug.Log(level);
    }


    /// <summary>
    /// Make the ennemy disapear
    /// </summary>
    override protected void ActorDeath () {
        Destroy(gameObject);
        manager.turnManager.fightingEntities.Remove(this);
        manager.ResetActorsPositions();
        manager.turnManager.TestCombatEnd();
    }
}
