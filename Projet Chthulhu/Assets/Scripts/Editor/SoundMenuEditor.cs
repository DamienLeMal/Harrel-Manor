using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SoundMenuVolume))]
public class SoundMenuEditor : Editor
{
    public override void OnInspectorGUI()
    {
        SoundMenuVolume soundMenu = (SoundMenuVolume)target;
        base.OnInspectorGUI();
    }
}
