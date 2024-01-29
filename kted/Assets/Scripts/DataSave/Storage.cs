using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using DataSave.Surrogates;
using UnityEngine;

public class Storage
{
    private string filePath;
    private BinaryFormatter _formatter;

    public Storage()
    {
        var directory = Application.persistentDataPath + "/saves";
        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);
        filePath = directory + "/GameSave.save";
        InitBinaryFormatter();
    }
    private void InitBinaryFormatter()
    {
        _formatter = new BinaryFormatter();
        var selector = new SurrogateSelector();

        var v3Surrogate = new Vector3SerializationSurrogate();
        
        selector.AddSurrogate(typeof(Vector3), new StreamingContext(StreamingContextStates.All), v3Surrogate);

        _formatter.SurrogateSelector = selector;
    }
    public object Load(object saveDataByDefault)
    {
        if (!File.Exists(filePath))
        {
            if (saveDataByDefault != null)
            {
                Save(saveDataByDefault);
            }

            return saveDataByDefault;
        }

        var file = File.Open(filePath, FileMode.Open);
        var savedData = _formatter.Deserialize(file);
        file.Close();
        return savedData;
    }

    public void Save(object saveData)
    {
        var file = File.Create(filePath);
        _formatter.Serialize(file, saveData);
        file.Close();
    }
}
