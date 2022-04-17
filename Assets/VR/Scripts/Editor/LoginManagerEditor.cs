using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LoginManager))]

public class LoginManagerEditor : Editor
{    

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        EditorGUILayout.HelpBox("This script is responsible fo connecting to photon servers", MessageType.Info);

        LoginManager loginManager = (LoginManager)target;

        if(GUILayout.Button("Connect Anonymously"))
        {
            loginManager.ConnectAnonymously();
        }
        
    }

}
