using System.Xml.Serialization;
using System.IO;
using UnityEngine;
using System.Collections.Generic;

public class SaveManager : MonoBehaviour
{
    public static SaveManager saveMg;
    public XMLFile activeSave;

    private void Awake()
    {
        saveMg = this;
        Load();
    }

    public void Save()
    {
        string dataPath = Application.dataPath;
        var fSerializer = new XmlSerializer(typeof(XMLFile));
        var fStream = new FileStream(dataPath + "/" + activeSave.saveName + ".save", FileMode.Create);
        fSerializer.Serialize(fStream, activeSave);
        fStream.Close();
    }

    public void Load()
    {
        string dataPath = Application.dataPath;
        if (System.IO.File.Exists(dataPath + "/" + activeSave.saveName + ".save"))
        {
            var fSerializer = new XmlSerializer(typeof(XMLFile));
            var fStream = new FileStream(dataPath + "/" + activeSave.saveName + ".save", FileMode.Open);
            activeSave = fSerializer.Deserialize(fStream) as XMLFile;
            fStream.Close();
        }
    }

    [System.Serializable]
    public class XMLFile
    {
        public string saveName = "ActiveSave";

        //InventoryCellsInMainCell
        public List<string> idInventoryItemsInMainCell;
        public List<int> xCellIndexItems, yCellIndexItems, countObjectsInPack;

        //InventoryCellsInAmmunitionCell
        public List<string> idInventoryItemsInAmmunitionCell;
        public List<int> indexAmmunitionCell;


    }
}
