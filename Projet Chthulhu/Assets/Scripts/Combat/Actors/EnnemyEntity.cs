using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyEntity : ActorEntity
{
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