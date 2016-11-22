using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(DefendorGame))]
[CanEditMultipleObjects()]
public class DefendorGameEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DefendorGame game = (DefendorGame)target;
        DrawDefaultInspector();

        if (GUILayout.Button("InstaKill All Cities"))
        {
            foreach (PlayerCity city in game.cities)
            {
                city.InstaKill();
            }
        }

        if (GUILayout.Button("Skip To Game Win"))
        {
            game.ForceGameWin();
        }
    }


}
