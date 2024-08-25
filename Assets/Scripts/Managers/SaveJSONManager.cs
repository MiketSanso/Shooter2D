using UnityEngine;
using System.IO;

public class SaveJSONManager : MonoBehaviour
{
    public static SaveJSONManager saveJSONManager;
    public MyData data;

    void Start()
    {
        // Сериализация в JSON
        string json = JsonUtility.ToJson(data);
        Debug.Log(json);

        // Десериализация из JSON
        MyData loadedData = JsonUtility.FromJson<MyData>(json);
        Debug.Log(loadedData.saveName + ", " + loadedData.age);

        // Сохранение в файл
        File.WriteAllText(Application.persistentDataPath + "/" + data.saveName + ".json", json);
    }

    public void LoadSave()
    {
        string loadedJson = File.ReadAllText(Application.persistentDataPath + "/data.json");
        MyData loadedFromFile = JsonUtility.FromJson<MyData>(loadedJson);
        Debug.Log(loadedFromFile.saveName + ", " + loadedFromFile.age);
    }

    [System.Serializable]
    public class MyData
    {
        public string saveName;
        public int age;
    }
}