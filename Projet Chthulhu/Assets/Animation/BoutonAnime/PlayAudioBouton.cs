using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioBouton : MonoBehaviour
{
    [SerializeField] private AudioSource audioSourceHover;
    [SerializeField] private AudioSource audioSourceClick;
    // Start is called before the first frame update
    void Start()
    {
        //audioSourceHover = GetComponent<AudioSource>();
        //audioSourceClick = GetComponent<AudioSource>();
        if (audioSourceHover == null) Debug.LogError("There's no audio source on this game object >.<*");
        if (audioSourceClick == null) Debug.LogError("There's no audio source on this game object >.<*");
    }

    public void PlaySoundHover ()
    {
        audioSourceHover.Play();
    }

    public void PlaySoundClick()
    {
        audioSourceClick.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }/*
    private void OnEnable()
    {
        audioData.Play();
    }*/


}
