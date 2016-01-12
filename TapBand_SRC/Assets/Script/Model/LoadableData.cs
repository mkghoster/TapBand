using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.Serialization;

[System.Serializable]
public abstract class LoadableData
{
    public void TryLoadFromAssets(string assetsPath)
    {
        string gameDataPath = ConstructDataPathAndFile(assetsPath);

        // Load GameData byte[]
#if UNITY_ANDROID && !UNITY_EDITOR
		IEnumerator a = GetByteArray(gameDataPath);
		while (a.MoveNext()) {}
#else
        byteArray = File.ReadAllBytes(gameDataPath);
#endif

        if (byteArray.Length > 0)
        {
            MemoryStream ms = new MemoryStream(byteArray);
            LoadData(ms);
        }
    }

    public abstract string GetFileName();
    protected abstract void LoadData(MemoryStream ms);

    private byte[] byteArray;

    private IEnumerator GetByteArray(string dataPath)
    {
        WWW www = new WWW(dataPath);
        while (!www.isDone)
        {
            yield return null;
        }

        byteArray = www.bytes;
    }

    public void SaveToFile(string assetsPath)
    {
        string gameDataPath = ConstructDataPathAndFile(assetsPath);

        BinaryFormatter bf = new BinaryFormatter();

        // Get GameState byte[]
        MemoryStream ms = new MemoryStream();
        bf.Serialize(ms, this);
        byte[] normalByteArray = ms.ToArray();

        // Save to file
        File.WriteAllBytes(gameDataPath, normalByteArray);
    }

    private string ConstructDataPathAndFile(string assetsPath)
    {
        string gameDataPath = System.IO.Path.Combine(assetsPath, "Data/");

#if UNITY_ANDROID && !UNITY_EDITOR
#else
        if (!Directory.Exists(gameDataPath))
        {
            Directory.CreateDirectory(gameDataPath);
        }
#endif

        gameDataPath += GetFileName();
        Debug.Log(gameDataPath);
#if UNITY_ANDROID && !UNITY_EDITOR
#else
        if (!File.Exists(gameDataPath))
        {
            FileStream created = File.Create(gameDataPath);
            created.Close();
        }
#endif
        return gameDataPath;
    }
}
