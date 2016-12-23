using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(PlayerCity))]
[CanEditMultipleObjects()]
public class CityEditor : Editor {

    public override void OnInspectorGUI()
    { 
        PlayerCity city = (PlayerCity)target;
        DrawDefaultInspector();

        if (GUILayout.Button("InstaKill City"))
        {
            city.InstaKill();
        }

        if (GUILayout.Button("Deal 24% Damage"))
        {
            city.TakeDamage(city.cityMaxHP * 0.24f);
        }

    }
	

}
