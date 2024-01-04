using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    GameData gameData = new();
    GenerateCube generateCube;
    ClickDetection clickDetection;
    // Start is called before the first frame update
    void Start()
    {
        generateCube = GetComponent<GenerateCube>();
        clickDetection = GetComponent<ClickDetection>();
    }

   public void SaveToJson()
    {
        gameData.size = generateCube.UIsize;
        gameData.positions.Clear();
        gameData.rotations.Clear();

        for (int i = 0; i < clickDetection.cubes.Count; i++)
        {
            gameData.positions.Add(clickDetection.cubes[i].position);
            gameData.rotations.Add(clickDetection.cubes[i].rotation.eulerAngles);
        }

        string gameDataJson = JsonUtility.ToJson(gameData);
        string filePath = Application.persistentDataPath + "/GameData.json";
        Debug.Log(filePath);
        System.IO.File.WriteAllText(filePath, gameDataJson);
    }

    public void LoadFromJson()
    {
        string filePath = Application.persistentDataPath + "/GameData.json";
        string gameDataJson = System.IO.File.ReadAllText(filePath);

        gameData = JsonUtility.FromJson<GameData>(gameDataJson);

        generateCube.UIsize = gameData.size;
        generateCube.Reload(gameData.size);

        for(int i = 0; i < clickDetection.cubes.Count; i++)
        {
            clickDetection.cubes[i].SetPositionAndRotation(gameData.positions[i], Quaternion.Euler(gameData.rotations[i]));
        }

    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(100, 10, 70, 30), "Save"))
        {
            SaveToJson();
        }

        if (GUI.Button(new Rect(200, 10, 70, 30), "Load"))
        {
            LoadFromJson();
        }
    }
}

public class GameData
{
    public int size;
    public List<Vector3> positions = new();
    public List<Vector3> rotations = new();
}
