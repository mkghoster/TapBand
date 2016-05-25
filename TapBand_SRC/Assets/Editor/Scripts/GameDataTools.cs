using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections;
using System.Net;

[ExecuteInEditMode]
public class GameDataTools : EditorWindow
{
    private const string dataRawPath = "Assets/Data/";
    private const string gameDataDirectory = "Assets/StreamingAssets/";

    private string documentId;
    private string gameDataDownloadFileName;
    private string gameDataConvertFileName;
    private static byte[] fileAsBinary;
    
    [MenuItem("PGI/GameData Tools")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(GameDataTools));
    }

    void OnEnable()
    {
        RestoreDefaultValues();
    }

    void OnGUI()
    {
        RestoreDefaultValuesGUI();

        EditorGUILayout.Space();

        DownloadGameDataGUI();

        EditorGUILayout.Space();

        ConvertToResourceFileGUI();

        EditorGUILayout.Space();
        GUILayout.Label("Warning: values reset to defaults if window is closed!");
    }

    private void RestoreDefaultValuesGUI()
    {
        if (GUILayout.Button("Restore default values"))
        {
            RestoreDefaultValues();
        }
    }

    private void RestoreDefaultValues()
    {
        documentId = "insert document id here";//@"Insert source URL here";
        gameDataDownloadFileName = "GameData.xlsx";
        gameDataConvertFileName = "GameData.xlsx";
    }

    private void DownloadGameDataGUI()
    {
        GUILayout.Label("Download GameData", EditorStyles.boldLabel);

        documentId = EditorGUILayout.TextField("Remote URL", documentId);
        gameDataDownloadFileName = EditorGUILayout.TextField("Local filename", gameDataDownloadFileName);

        if (GUILayout.Button("Download"))
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);
            using (var client = new WebClient())
            {
                client.DownloadFile(ConvertURL(), dataRawPath + gameDataDownloadFileName);
            }
        }
    }

    //works only with sharable link from google drive
    private string ConvertURL()
    {

        //https://docs.google.com/spreadsheets/d/1kPvlgNTmOn7k-ipQl_bzPssV2TQZ7dhUi0MMM54TLCA/export?format=xlsx&id=1kPvlgNTmOn7k-ipQl_bzPssV2TQZ7dhUi0MMM54TLCA
        return "https://docs.google.com/spreadsheets/d/" + documentId + "/export?format=xlsx&id=" + documentId;

        //else if (sourceURL.Contains ("dropbox"))
        //{
        //	
        //} 

    }

    private void ConvertToResourceFileGUI()
    {
        GUILayout.Label("Convert to resource file", EditorStyles.boldLabel);
        gameDataConvertFileName = EditorGUILayout.TextField("Convert from", gameDataConvertFileName);

        if (GUILayout.Button("Convert"))
        {
            EditorUtility.DisplayProgressBar("Please wait", "Loading and converting game data...", 0.2f);

            GameData gameData = new GameData();

            try
            {
                IGameDataLoader rawGameDataLoader =
                    new RawGameDataLoader(dataRawPath + gameDataConvertFileName);
                gameData = rawGameDataLoader.LoadGameData();
            }
            catch (System.Exception e)
            {
                Debug.LogError("Exception during loading: " + e.Message + ". StackTrace: " + e.StackTrace);
                EditorUtility.ClearProgressBar();
                return;
            }

            EditorUtility.DisplayProgressBar("Please wait", "Loading and converting game data...", 0.8f);

            try
            {
                gameData.SaveToFile(gameDataDirectory);
            }
            catch (System.Exception e)
            {
                Debug.LogError("Exception during writing binary file. Please restart Unity and run Convert again! Details: " + e.Message + ". StackTrace: " + e.StackTrace);
                EditorUtility.ClearProgressBar();
                return;
            }
            AssetDatabase.Refresh();

            EditorUtility.ClearProgressBar();
            Debug.Log("GameData converted to " + Application.streamingAssetsPath +
                       GameData.GAME_DATA_PATH + ".TimeStamp: " +
                       System.DateTime.Now.ToLongTimeString());
        }
    }

    private IEnumerator LoadGameDataWithLoader()
    {
        return null;
    }
}
