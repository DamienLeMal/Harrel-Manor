using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiColorScheme : MonoBehaviour
{
    public static UiColorScheme current;
    private void Awake() {
        current = this;
    }

    public Color strColor, dexColor, spdColor, intlColor, agiColor, conColor, lckColor, apColor, mpColor, pmColor, hpColor, mntColor, dmgColor;

    public Color GetColor (PlayerStat stat) {
        switch (stat) {
            case PlayerStat.Str : return strColor;
            case PlayerStat.Dex : return dexColor;
            case PlayerStat.Spd : return spdColor;
            case PlayerStat.Intl : return intlColor;
            case PlayerStat.Agi : return agiColor;
            case PlayerStat.Con : return conColor;
            case PlayerStat.Lck : return lckColor;
            default : return Color.black;
        }
    }

    public string GetTagColor (Color clr) {
        string c = ColorUtility.ToHtmlStringRGB( clr );
        return "<color=#" + c + ">";
    }
}
