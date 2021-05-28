using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum PlayerStat {
    Str,
    Dex,
    Spd,
    Intl,
    Agi,
    Con,
    Lck
}
public class PopupLevelUpButton : PopupButton
{
    [SerializeField] private PlayerStat stat;
    private PlayerEntity player;
    
    private void Start() {
        player = GameObject.FindObjectOfType<PlayerEntity>();
        GetComponent<Image>().color = UiColorScheme.current.GetColor(stat);
    }
    public override void ButtonAction ()
    {
        base.ButtonAction();
        player.UpgradeStat(stat);
    }
}
