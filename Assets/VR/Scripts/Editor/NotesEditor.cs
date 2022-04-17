using UnityEngine;
using UnityEditor;

public class NotesEditor : EditorWindow
{
    [MenuItem("Editors/Notes")]

    public static void ShowWindow()
    {
        GetWindow<NotesEditor>("Notes");
    }

    private void OnGUI()
    {
        
    }

}
