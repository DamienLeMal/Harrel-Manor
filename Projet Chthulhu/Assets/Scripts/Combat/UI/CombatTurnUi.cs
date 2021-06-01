using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatTurnUi : MonoBehaviour
{
    [SerializeField] private Text txt;

    private void Update() {
        if (CombatManager.current.turnManager.fightingEntities[CombatManager.current.turnManager.currentActorIndex] == CombatManager.current.player) {
            if (txt.text != "Tour du joueur") txt.text = "Tour du joueur";
            
        }else{
            if (txt.text != "Tour de l'ennemi") txt.text = "Tour de l'ennemi";
        }
    }
}
