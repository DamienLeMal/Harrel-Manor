using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CombatManager))]
public class TileTypeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        CombatManager manager = (CombatManager)target;

        base.OnInspectorGUI();
        
        if (GUILayout.Button("Color tiles by type")) {
            foreach (TileEntity t in manager.grid) {
                if (t == null) continue;
                switch (t.tileState) {
                    case TileState.Walk :
                        t.GetComponentInChildren<MeshRenderer>().material = manager.walkMaterial;
                        break;
                    case TileState.Block :
                        t.GetComponentInChildren<MeshRenderer>().material = manager.blockMaterial;
                        break;
                    case TileState.Occupied :
                        t.GetComponentInChildren<MeshRenderer>().material = manager.occupiedMaterial;
                        break;
                }
            }
        }
    }
}
