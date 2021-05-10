using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "ScriptableObjects/Combat/Dialogue", order = 2)]
public class ActorDialogue : ScriptableObject
{
    public string[] door;
    
    public string key_01;
    public string key_02;
    public string key_03;
}
