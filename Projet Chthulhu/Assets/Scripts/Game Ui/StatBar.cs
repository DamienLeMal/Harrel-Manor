using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Stat {
    Health,
    MentalHealth,
    Ap,
    Mp,
    Pm
}
public class StatBar : MonoBehaviour
{
    [SerializeField] private Stat statToShow;
    [SerializeField] private Transform mask, spriteFull;
    [SerializeField] private Text text;
    [SerializeField] private float yPosEmpty, yPosFull;
    [SerializeField] private PlayerEntity player;
    private Vector3 spriteFullPos;
    private int statValue;
    private int maxStatValue;
    private bool finishedCoroutine = true;
    
    void Start()
    {
        spriteFullPos = spriteFull.position;
    }

    private void Update() {
        if (finishedCoroutine) {
            CheckStat();
        }
    }

    private void LateUpdate() {
        spriteFull.SetPositionAndRotation(spriteFullPos,Quaternion.identity);
    }

    private void CheckStat () {
        switch (statToShow) {
            case Stat.Health :
                maxStatValue = player.hp_max;
                if (statValue == player.hp) finishedCoroutine = true;
                if (statValue < player.hp) StartCoroutine(UpdateStat(1));
                if (statValue > player.hp) StartCoroutine(UpdateStat(-1));
                break;
            case Stat.MentalHealth :
                maxStatValue = player.mnt_max;
                if (statValue == player.mnt) finishedCoroutine = true;
                if (statValue < player.mnt) StartCoroutine(UpdateStat(1));
                if (statValue > player.mnt) StartCoroutine(UpdateStat(-1));
                break;
        }
    }

    IEnumerator UpdateStat (int addValue) {
        finishedCoroutine = false;
        statValue += addValue;
        text.text = statValue.ToString();
        yield return new WaitForSeconds(0.1f);
        CheckStat();
    }
}
