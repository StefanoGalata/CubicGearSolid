using UnityEngine;
using UnityEditor;
 
 [CustomEditor(typeof(GameManager))]
public class DeletePlayerPrefsScript : Editor{
    public override void OnInspectorGUI(){
        base.OnInspectorGUI();
        if(GUILayout.Button("Delete Player Prefs"))
            PlayerPrefs.DeleteAll();
    }
}

