using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// using System.Text.Json;

public class JsonReader : MonoBehaviour
{
    public TextAsset jsonFile;

    private void Start()
    {
        string jsonText = jsonFile.text;

        MyClass myClass = new MyClass();

        // 将JSON数据覆盖到现有对象上
        JsonUtility.FromJsonOverwrite(jsonText, myClass);

        Debug.Log("Name: " + myClass.name);
        Debug.Log("Age: " + myClass.age);
        Debug.Log("Inventory Count: " + myClass.inventory.Length);

        foreach (string item in myClass.inventory)
        {
            Debug.Log("Item: " + item);
        }
    }

    [System.Serializable]
    public class MyClass
    {
        public string name;
        public int age;
        public string[] inventory;
    }
}
