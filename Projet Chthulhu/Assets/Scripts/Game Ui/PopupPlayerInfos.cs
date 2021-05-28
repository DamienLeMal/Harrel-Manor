using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupPlayerInfos : MonoBehaviour
{
    private string text;
    [SerializeField] private PopupWindow popup;
    private void UpdateText () {
        text += UiColorScheme.current.GetTagColor(UiColorScheme.current.strColor) + CombatManager.current.player.str + " - Force</color>\n";
        text += UiColorScheme.current.GetTagColor(UiColorScheme.current.dexColor) + CombatManager.current.player.dex + " - Dextérité</color>\n";
        text += UiColorScheme.current.GetTagColor(UiColorScheme.current.spdColor) + CombatManager.current.player.spd + " - Vitesse</color>\n";
        text += UiColorScheme.current.GetTagColor(UiColorScheme.current.intlColor) + CombatManager.current.player.intl + " - Intelligence</color>\n";
        text += UiColorScheme.current.GetTagColor(UiColorScheme.current.agiColor) + CombatManager.current.player.agi + " - Agilité</color>\n";
        text += UiColorScheme.current.GetTagColor(UiColorScheme.current.conColor) + CombatManager.current.player.con + " - Constitution</color>\n";
        text += UiColorScheme.current.GetTagColor(UiColorScheme.current.lckColor) + CombatManager.current.player.lck + " - Chance</color>";
    }
    public void ShowInformations () {
        UpdateText();
        popup.ActivatePopup(text,"Statistiques",PopupType.Information);
    }
}
