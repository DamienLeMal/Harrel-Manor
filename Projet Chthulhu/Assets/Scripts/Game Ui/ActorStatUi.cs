using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActorStatUi : MonoBehaviour
{
    private PlayerEntity player;
    private Text txt;
    private Slider slider;
    [SerializeField] private Stat stat;

    private void Start() {
        txt = GetComponent<Text>();
        slider = GetComponent<Slider>();
        player = CombatManager.current.player;
    }

    // Update is called once per frame
    void Update()
    {
        if (txt != null) SetText();
        if (slider != null) SetSliderValue();
    }

    private void SetText () {
        switch (stat) {
            case Stat.Ap :
                txt.text = player.ap.ToString();
                break;
            case Stat.Mp :
                txt.text = player.mp.ToString();
                break;
            case Stat.Pm :
                txt.text = player.pm.ToString();
                break;
        }
    }

    private void SetSliderValue () {
        switch (stat) {
            case Stat.Health :
                LerpSlider((float)player.hp/(float)player.hp_max);
                break;
            case Stat.MentalHealth :
                LerpSlider((float)player.mnt/(float)player.mnt_max);
                break;
        }
    }

    private void LerpSlider (float value) {
        bool superior = slider.value > value;
        if (superior) {
            if (slider.value <= value) return;
            slider.value -= value * Time.unscaledDeltaTime;
        }else{
            if (slider.value >= value) return;
            slider.value += value * Time.unscaledDeltaTime;
        }
        
    }
}
