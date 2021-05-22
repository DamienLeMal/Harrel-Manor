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

    #region Combat Buttons
    public WeaponButton ShowWeaponButton () {
        bool recycling = false;
        WeaponButton returnButton = null;
        foreach (Transform button in canvasWeaponButtonParent.transform) {
           if (!button.gameObject.activeSelf) {
                button.gameObject.SetActive(true);
                recycling = true;
                returnButton = button.GetComponent<WeaponButton>();
            }
        }
        if (!recycling) {
            GameObject button = Instantiate(weaponButtonPrefab,Vector3.zero,Quaternion.identity,canvasWeaponButtonParent.transform);
            button.transform.localScale = Vector3.one;
            returnButton = button.GetComponent<WeaponButton>();
        }
        return returnButton;
    }
    public void ShowAttackButton (AttackData attack, Transform parentTransform) {
        bool recycling = false;
        foreach (Transform button in parentTransform) {
            if (!button.gameObject.activeSelf) {
                button.gameObject.SetActive(true);
                button.GetComponent<Button>().onClick.RemoveAllListeners();
                PrepareButton(attack,button.gameObject);
                recycling = true;
            }
        }
        if (!recycling) {
            GameObject button = Instantiate(attackButtonPrefab,Vector3.zero,Quaternion.identity,parentTransform);
            button.transform.localScale = Vector3.one;
            PrepareButton(attack,button);
        }
        parentTransform.transform.parent.GetComponent<WeaponButton>().Invoke("Unactive",0.01f);
    }

    private void PrepareButton (AttackData attack, GameObject button) {
        button.gameObject.name = attack.attackName;
        button.GetComponentInChildren<Text>().text = attack.attackName;
        button.GetComponent<CombatButton>().ButtonConstructor(GetComponent<CombatManager>(),PlayerState.Attacking,attack,attackTooltip);
    }
    #endregion
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
