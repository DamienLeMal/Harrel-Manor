using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatUiManager : MonoBehaviour
{
    [SerializeField] private GameObject canvasButtonParent;
    [SerializeField] private GameObject attackButtonPrefab;
    [SerializeField] private GameObject weaponButtonPrefab;

    public WeaponButton ShowWeaponButton () {
        bool recycling = false;
        WeaponButton returnButton = null;
        foreach (Transform button in canvasButtonParent.transform) {
           if (!button.gameObject.activeSelf) {
                button.gameObject.SetActive(true);
                recycling = true;
                returnButton = button.GetComponent<WeaponButton>();
            }
        }
        if (!recycling) {
            GameObject button = Instantiate(weaponButtonPrefab,Vector3.zero,Quaternion.identity,canvasButtonParent.transform);
            button.transform.localScale = Vector3.one;
            returnButton = button.GetComponent<WeaponButton>();
        }
        return returnButton;
    }
    public void ShowAttackButton (AttackData attack, Transform parentTransform) {
        Debug.Log(parentTransform.gameObject);
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
    }

    private void PrepareButton (AttackData attack, GameObject button) {
        button.gameObject.name = attack.attackName;
        button.GetComponentInChildren<Text>().text = attack.attackName;
        button.GetComponent<CombatButton>().ButtonConstructor(GetComponent<CombatManager>(),PlayerState.Attacking,attack);
    }
}
