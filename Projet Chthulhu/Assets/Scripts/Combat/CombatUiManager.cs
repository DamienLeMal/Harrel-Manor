using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatUiManager : MonoBehaviour
{
    [SerializeField] private GameObject canvasButtonParent;
    [SerializeField] private GameObject buttonPrefab;
    public void ShowAttackButton (AttackData attack) {
        bool recycling = false;
        foreach (Transform button in canvasButtonParent.transform) {
            if (!button.gameObject.activeSelf) {
                button.gameObject.SetActive(true);
                button.GetComponent<Button>().onClick.RemoveAllListeners();
                PrepareButton(attack,button.gameObject);
                recycling = true;
            }
        }
        if (!recycling) {
            GameObject button = Instantiate(buttonPrefab,Vector3.zero,Quaternion.identity);
            button.transform.SetParent(canvasButtonParent.transform);
            button.transform.localScale = Vector3.one;
            PrepareButton(attack,button);
        }
    }

    private void PrepareButton (AttackData attack, GameObject button) {
        button.gameObject.name = attack.attackName;
        button.GetComponentInChildren<Text>().text = attack.attackName;
        button.GetComponent<CombatButton>().ButtonConstructor(GetComponent<CombatManager>(),PlayerState.Attacking,attack);
    }
}