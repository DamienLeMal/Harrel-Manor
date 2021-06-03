using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEndInteractable : MonoBehaviour, IClicked
{
    public void OnClickAction () {
        CombatManager.current.GameOver();
    }
}
