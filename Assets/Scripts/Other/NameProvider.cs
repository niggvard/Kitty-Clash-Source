using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


[System.Serializable]
public class NameList
{
    public List<string> names;
}

public class NameProvider : MonoBehaviour
{
    private static NameList nameList;

    private static void LoadNames()
    {
        if (nameList != null) return;

        TextAsset jsonFile = Resources.Load<TextAsset>("names");
        if (jsonFile == null)
        {
            nameList = new NameList { names = new List<string>() };
            return;
        }

        nameList = JsonUtility.FromJson<NameList>(jsonFile.text);
    }

    public static string GetRandomName()
    {
        if (nameList == null || nameList.names.Count == 0)
            LoadNames();

        int words = Random.Range(1, 4);
        StringBuilder name = new();
        for (int i = 0; i < words; i++)
        {
            name.Append(Randomizer.GetRandomFromList(nameList.names));
        }

        var fullName = name.ToString();
        if (fullName.Length > 14)
            return fullName.Substring(0, 14);

        return fullName;
    }
}
