using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Stat {
    Health,
    MentalHealth
}
public class StatBar : MonoBehaviour
{
    [SerializeField] private Stat statToShow;
    [SerializeField] private Transform mask, spriteFull;
    [SerializeField] private float yPosEmpty, yPosFull;
    [SerializeField] private PlayerEntity player;
    private Vector3 spriteFullPos;
    
    void Start()
    {
        spriteFullPos = spriteFull.position;
    }


    private void LateUpdate() {
        spriteFull.SetPositionAndRotation(spriteFullPos,Quaternion.identity);
    }
}
