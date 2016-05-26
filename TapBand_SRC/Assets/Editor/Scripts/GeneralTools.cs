using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections;

[ExecuteInEditMode]
public class GeneralTools : EditorWindow
{
    [MenuItem("PGI/General Tools")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(GeneralTools));
    }

    void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();

        DeletePlayerPrefsAndPersistenceButton();

        EditorGUILayout.EndHorizontal();
    }

    private void DeletePlayerPrefsAndPersistenceButton()
    {
        if (GUILayout.Button("Delete PlayerPrefs and Persistence"))
        {
            // delete playerprefs
            Debug.Log("Deleting PlayerPrefs");
            PlayerPrefs.DeleteAll();

            // delete persistend data

            DirectoryInfo dataDir = new DirectoryInfo(Application.persistentDataPath);
            Debug.Log("Deleting persistent data from " + dataDir.FullName);
            dataDir.Delete(true);
        }
    }
}
