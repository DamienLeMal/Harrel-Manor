using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatUiManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> combatCanvas = new List<GameObject>();
    [SerializeField] private GameObject canvasWeaponButtonParent;
    [SerializeField] private GameObject attackButtonPrefab;
    [SerializeField] private GameObject weaponButtonPrefab;
    [SerializeField] private TooltipUi attackTooltip;
    public void ToggleCombatUi(bool toggle) {
        foreach (GameObject go in combatCanvas) {
            if (go != combatCanvas[2])
            go.SetActive(toggle);
        }
    }

    public void GameOverScreen () {
        combatCanvas[2].SetActive(true);
    }
}
