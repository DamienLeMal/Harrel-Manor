using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;
using UnityEngine.UI;

public class SoundMenuVolume : MonoBehaviour
{
    [SerializeField] private AudioGroupMember master;
    [Serializable] private class AudioGroupMember {
        public AudioMixerGroup audioMixerGroup;
        public string exposedParam;
        public float volume {
            get {
                return GetVolume();
            }
            set {
                SetVolume(value);
            }
        }
        private float GetVolume() {
            bool result = audioMixerGroup.audioMixer.GetFloat(exposedParam,out float value);
            if (result) return value;
            return 0f;
        }
        private void SetVolume(float value) {
            audioMixerGroup.audioMixer.SetFloat(exposedParam,value);
        }
    }
    [SerializeField] private AudioGroupMember[] groupAmbiance;
    [SerializeField] private AudioGroupMember[] groupMusic;
    [SerializeField] private AudioGroupMember[] groupVoice;
    [SerializeField] private AudioGroupMember[] groupSFX;
    [SerializeField] private Slider[] audioSlider;

    private void Update() {
        master.volume = SliderValue(0);
        foreach (AudioGroupMember agm in groupAmbiance) {
            agm.volume = SliderValue(1);
        }
        foreach (AudioGroupMember agm in groupMusic) {
            agm.volume = SliderValue(2);
        }
        foreach (AudioGroupMember agm in groupVoice) {
            agm.volume = SliderValue(3);
        }
        foreach (AudioGroupMember agm in groupSFX) {
            agm.volume = SliderValue(4);
        }
    }

    private int SliderValue (int index) {
        if (audioSlider[index].value == -20) return int.MinValue;
        return (int)audioSlider[index].value;
    }
}