using UnityEngine;
using System.Collections.Generic;
using System.Reflection.Emit;

public class PepperLabManager : MonoBehaviour
{
    public GameObject pepperPrefab;
    public GameObject labelPrefab;
    public List<ChiliData> activePeppers = new List<ChiliData>();

    void Start()
    {
        // 1. Thai Bird's Eye (The "Wild Bush"): High yield, tiny fruit, very bushy.
        ChiliData jalapeno = new ChiliData("Jalapeno", 8000, 3.5f, 1.2f, 0.7f, 0.2f, 0.1f, 6.0f, 6, 35f, 1.2f, 0.6f, 0.5f, 0.2f, 0.1f, 1.1f, 1);
        ChiliData habanero = new ChiliData("Habanero", 220000, 2.0f, 1.8f, 0.9f, 0.3f, 0.2f, 4.5f, 5, 25f, 0.8f, 0.9f, 0.95f, 0.3f, 0.2f, 1.3f, 2);
        ChiliData cayenne = new ChiliData("Cayenne", 50000, 6.5f, 0.5f, 0.8f, 0.4f, 0.3f, 5.5f, 4, 40f, 1.5f, 0.3f, 0.4f, 0.6f, 0.2f, 1.0f, 1);
        ChiliData ghost = new ChiliData("Ghost Pepper", 1000000, 2.8f, 1.1f, 1.0f, 0.5f, 0.4f, 5.0f, 5, 30f, 0.6f, 0.4f, 0.7f, 0.8f, 0.4f, 1.5f, 3);
        ChiliData poblano = new ChiliData("Poblano", 1500, 4.0f, 2.5f, 0.3f, 0.1f, 0.05f, 6.5f, 6, 20f, 1.8f, 0.7f, 0.6f, 0.3f, 0.2f, 0.9f, 1);
        ChiliData scorpion = new ChiliData("Scorpion", 1500000, 2.5f, 1.5f, 1.2f, 0.6f, 0.9f, 4.8f, 4, 35f, 0.5f, 0.3f, 0.6f, 0.9f, 0.5f, 1.6f, 4);
        ChiliData banana = new ChiliData("Banana Pepper", 500, 5.5f, 1.5f, 0.6f, 0.2f, 0.1f, 6.0f, 5, 25f, 2.0f, 0.8f, 0.7f, 0.1f, 0.1f, 0.8f, 1);

        // Inside your Manager Start() or Spawn function:
        GameObject plantObj = new GameObject("MyFirstPlant");
        ChiliPlant plantScript = plantObj.AddComponent<ChiliPlant>();
        plantScript.pepperPrefab = pepperPrefab;

        ChiliData pepperHYBRID = ChiliData.Breed(scorpion, ghost);

        List<ChiliData> family = ChiliData.growGenerations(pepperHYBRID, 100);

        for (int i = 0; i < family.Count; i++)
        {
            // Space them 15 units apart along the X axis
            Vector3 spawnPos = new Vector3(i * 15.0f, 0, 0);

            GameObject newPlantObj = new GameObject("Plant_Gen_" + i);
            ChiliPlant script = newPlantObj.AddComponent<ChiliPlant>();
            script.pepperPrefab = pepperPrefab;
            script.labelPrefab = labelPrefab;

            script.GeneratePlant(family[i], spawnPos);
        }

    }

    void SpawnPepper(ChiliData data, Vector3 position)
    {
        GameObject obj = Instantiate(pepperPrefab, position, Quaternion.identity);
        obj.GetComponent<ChiliProceduralMesh>().Generate(data);
        obj.name = data.name + "_Gen" + data.generation;
    }
}