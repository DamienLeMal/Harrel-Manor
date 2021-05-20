using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundMusicManager : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup[] bus = new AudioMixerGroup[2];
    [SerializeField] private AudioClip[] musics = new AudioClip[11];
    [SerializeField] private AudioSource[] source = new AudioSource[5];
    private AudioSource mainMusicPlaying;

    private void Start() {
        SoundEventManager.current.onGamemodeChange += GameModeMusic;
        SoundEventManager.current.onCombatEnd += CombatEndMusic;
        SoundEventManager.current.onPlayerTurnHpHigh += CombatLowHpMusicOff;
        SoundEventManager.current.onPlayerTurnHpLow += CombatLowHpMusicOn;
        SoundEventManager.current.onPause += CombatPauseMusic;
        SoundEventManager.current.onUnpause += CombatUnpauseMusic;
        SoundEventManager.current.onPause += ExplorationPauseMusic;
        SoundEventManager.current.onUnpause += ExplorationUnpauseMusic;
        SoundEventManager.current.onEnnemyInSight += ExplorationLayer2On;
        SoundEventManager.current.onEnnemyLooseSight += ExplorationLayer2Off;
        SetExplorationMusic();
    }

    private void GameModeMusic() {
        if (CombatManager.current.combatOn) {
            StartCoroutine(PlayCombatMusic());
        }else{
            SetExplorationMusic();
        }
    }
#region Combat Mode
    private void SetCombatMusic () {
        source[0].outputAudioMixerGroup = bus[0];
        source[0].clip = musics[0];//c-layer 1
        source[1].outputAudioMixerGroup = bus[0];
        source[1].clip = musics[1];//c-layer 1 Game Over
        source[2].outputAudioMixerGroup = bus[1];
        source[2].clip = musics[11];//c-layer 1 Pause
        source[3].outputAudioMixerGroup = bus[0];
        source[3].clip = musics[3];//c-layer 1 Low Hp
        source[4].outputAudioMixerGroup = bus[0];
        source[4].clip = musics[4];//c-layer 1 Victory
        source[3].outputAudioMixerGroup = bus[0];
        source[5].clip = musics[2];//c-layer 1 Intro
    }

    IEnumerator PlayCombatMusic () {
        //Intro -> c-layer 1 100%
        //      -> c-layer 1 low hp 0%
        source[5].PlayOneShot(source[5].clip);
        StartCoroutine(StartFade(source[5],0.5f,1));//Intro fade In
        StartCoroutine(StartFade(mainMusicPlaying,0.444f,0));//Outro fade In

        yield return new WaitForSeconds(0.444f);
        SetCombatMusic();
        PlayMusic(0,true,1,0.222f);
        PlayMusic(2,false,0);//Pause
        PlayMusic(3,false,0);//Low Hp
        
        mainMusicPlaying = source[0];
    }

    private void CombatEndMusic (bool playerWin) {
        if (playerWin) {
            PlayCombatWin();
        }else{
            PlayCombatGameOver();
        }
    }

    private void PlayCombatWin () {
        //Current Music fade out 222 -> Music win
        StartCoroutine(StartFade(source[0],0.222f,0));
        StartCoroutine(StartFade(source[3],0.222f,0));
        source[4].PlayOneShot(source[4].clip);
    }

    private void PlayCombatGameOver () {
        //Current Music fade out 222 -> Music gameOver
        StartCoroutine(StartFade(source[0],0.222f,0));
        StartCoroutine(StartFade(source[3],0.222f,0));
        source[1].PlayOneShot(source[1].clip);
    }

    private void CombatPauseMusic () {
        if (mainMusicPlaying.clip != musics[0]) return;
        //bus -3db -> low pass
        StartCoroutine(SetLowPassFilter(1500,1f));
        StartCoroutine(StartFade(source[2],0.5f,0.75f));
    }

    private void CombatUnpauseMusic () {
        if (mainMusicPlaying.clip != musics[0]) return;
        //bus -3db -> low pass
        StartCoroutine(StartFade(source[2],0.5f,0f));
        StartCoroutine(SetLowPassFilter(22000,1f));
    }

    private void CombatLowHpMusicOn () {
        StartCoroutine(StartFade(source[3],0.5f,1));
    }
    private void CombatLowHpMusicOff () {
        StartCoroutine(StartFade(source[3],0.5f,0));
    }

#endregion
#region Exploration Mode

    private void SetExplorationMusic () {
        source[0].outputAudioMixerGroup = bus[0];
        source[0].clip = musics[5];//e-layer 1
        source[1].outputAudioMixerGroup = bus[1];
        source[1].clip = musics[6];//e-layer 1 Pause
        source[2].outputAudioMixerGroup = bus[0];
        source[2].clip = musics[7];//e-layer 2
        source[3].outputAudioMixerGroup = bus[1];
        source[3].clip = musics[8];//e-layer 2 Pause
        source[4].outputAudioMixerGroup = bus[0];
        source[4].clip = musics[9];//e-layer 2 Stealth
        source[5].outputAudioMixerGroup = bus[0];
        source[5].clip = musics[2];//c-layer 1 Intro
        PlayExplorationMusic();
    }
    private void PlayExplorationMusic () {
        
        PlayMusic(0,true,1,0.222f);//e-layer 1
        PlayMusic(1,false,0);//e-layer 1 Pause
        PlayMusic(2,false,0);//e-layer 2
        PlayMusic(3,false,0);//e-layer 2 Pause
        PlayMusic(4,false,0);//e-layer 2 Stealth
        mainMusicPlaying = source[0];
    }

    private void ExplorationPauseMusic () {
        if (mainMusicPlaying.clip != musics[5] && mainMusicPlaying.clip != musics[7] && mainMusicPlaying.clip != musics[9]) return;
        mainMusicPlaying.outputAudioMixerGroup.audioMixer.SetFloat("MusicLowPass",666);
        StartCoroutine(StartFade(mainMusicPlaying,0.5f,0.2f));

        if (mainMusicPlaying.clip == musics[5]){
            //e-layer 1
            StartCoroutine(StartFade(source[1],0.5f,0.2f));
        }
        if (mainMusicPlaying.clip == musics[7] || mainMusicPlaying.clip == musics[9]) {
            //e-layer 2 or stealth
            StartCoroutine(StartFade(source[3],0.5f,0.2f));
        }
    }

    private void ExplorationUnpauseMusic () {
        if (mainMusicPlaying.clip != musics[5] && mainMusicPlaying.clip != musics[7] && mainMusicPlaying.clip != musics[9]) return;
        mainMusicPlaying.outputAudioMixerGroup.audioMixer.SetFloat("MusicLowPass",22000);
        StartCoroutine(StartFade(mainMusicPlaying,0.5f,1));
        StartCoroutine(StartFade(source[1],0.5f,0));
        StartCoroutine(StartFade(source[3],0.5f,0));
    }

    private void ExplorationLayer2On () {
        if (mainMusicPlaying == source[2]) return;
        StartCoroutine(StartFade(mainMusicPlaying,0.5f,0));
        StartCoroutine(StartFade(source[2],0.5f,1));
        mainMusicPlaying = source[2];
    }
    private void ExplorationLayer2Off () {
        StartCoroutine(StartFade(mainMusicPlaying,2f,0));
        StartCoroutine(StartFade(source[0],2f,1));
        mainMusicPlaying = source[0];
    }

    //private void ExplorationStealthMusicOn () {
    //    StartCoroutine(StartFade(mainMusicPlaying,0.5f,0));
    //    StartCoroutine(StartFade(source[3],0.5f,0));
    //}

#endregion


#region Effects

    private void PlayMusic(int sourceId, bool fadeIn, float volumeAmount, float fadeInDuration = 0f) {
        if (fadeIn) {
            source[sourceId].volume = 0;
            StartCoroutine(StartFade(source[0],fadeInDuration,volumeAmount));
        }else{
            source[sourceId].volume = volumeAmount;
        }
        source[sourceId].Play();
    }
    private IEnumerator SetLowPassFilter (float targetValue, float duration) {
        float currentTime = 0;
        float start;
        source[0].outputAudioMixerGroup.audioMixer.GetFloat("MusicLowPass",out start);
        if (targetValue < start) {
            source[0].outputAudioMixerGroup.audioMixer.SetFloat("MusicLowPass",15000);
            start = 15000;
        }
        

        while (currentTime < duration)
        {
            currentTime += Time.unscaledDeltaTime;
            source[0].outputAudioMixerGroup.audioMixer.SetFloat("MusicLowPass",Mathf.Lerp(start, targetValue, currentTime / duration));
            yield return null;
        }
        yield break;
    }
    private static IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume) {
        float currentTime = 0;
        float start = audioSource.volume;

        while (currentTime < duration)
        {
            currentTime += Time.unscaledDeltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        yield break;
    }
}
#endregion
