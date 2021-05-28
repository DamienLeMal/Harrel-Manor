using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackUiManager : MonoBehaviour
{
    [SerializeField] private List<CombatButton> attackButton = new List<CombatButton>();

    public void LinkAttackAndButton (WeaponData w) {
        foreach (CombatButton b in attackButton) {
            b.gameObject.SetActive(false);
            b.name = "-";
        }
        for (int i = 0; i < w.attacks.Count; i++) {
            attackButton[i].attack = w.attacks[i];
            attackButton[i].gameObject.SetActive(true);
            attackButton[i].name = w.attacks[i].name;
            attackButton[i].GetComponent<Image>().sprite = w.attacks[i].attackIcon;
            attackButton[i].GetComponent<ToolTipReceiver>().SetToolTipText(w.attacks[i].attackName,w.attacks[i].description);
        }
    }
}
