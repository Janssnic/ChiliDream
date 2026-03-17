using UnityEngine;
using System.Collections.Generic;

public class PepperLabManager : MonoBehaviour
{
    public GameObject pepperPrefab;
    public List<ChiliData> activePeppers = new List<ChiliData>();

    void Start()
    {
        // 1. Thai Bird's Eye (The "Wild Bush"): High yield, tiny fruit, very bushy.
        ChiliData thai = new ChiliData("Thai", 75000, 2.5f, 0.4f, 0.6f, 0.1f, 0.05f, 3.5f, 5, 20f, 0.95f);

        // 2. Bell Pepper (The "Heavyweight"): Zero heat, massive fruit, short/thick plant, low yield.
        ChiliData bell = new ChiliData("Bell", 0, 9f, 8.5f, 0.05f, 0.05f, 0.33f, 1.5f, 2, 45f, 0.25f);

        // 3. Ghost Pepper (The "Wicked"): Extreme heat, medium fruit, very rough texture, wide branching.
        ChiliData ghost = new ChiliData("Bhut Jolokia", 1041427, 7.5f, 3.0f, 0.4f, 0.95f, 0.08f, 2.2f, 4, 55f, 0.15f);

        // 4. Jalapeño (The "Reliable"): Classic look, corking texture, medium plant.
        ChiliData jalapeno = new ChiliData("Jalapeno", 5000, 6f, 2.5f, 0.2f, 0.3f, 0.4f, 2.5f, 3, 35f, 0.65f);

        // 5. Habanero (The "Lantern"): High heat, short/stubby fruit, orange hue, very dense bush.
        ChiliData habanero = new ChiliData("Habanero", 300000, 4f, 3.5f, 0.1f, 0.6f, 0.12f, 1.8f, 4, 40f, 0.7f);

        // 6. Cayenne (The "Long Boy"): Thin, very curly, bright red, tall spindly plant.
        ChiliData cayenne = new ChiliData("Cayenne", 40000, 12f, 1.2f, 0.9f, 0.2f, 0.0f, 3.0f, 3, 15f, 0.5f);

        // 7. Carolina Reaper (The "Gnarled"): World's hottest, iconic "stinger" tail, super lumpy.
        ChiliData reaper = new ChiliData("Reaper", 2200000, 5f, 4.5f, 0.7f, 1.0f, 0.02f, 2.0f, 4, 60f, 0.1f);

        // 8. Peter Pepper (The "Weirdo"): Medium heat, very high curvature (contorted), rare growth.
        ChiliData peter = new ChiliData("Peter Pepper", 20000, 8f, 2.5f, 1.5f, 0.4f, 0.03f, 2.0f, 3, 30f, 0.4f);

        // Inside your Manager Start() or Spawn function:
        GameObject plantObj = new GameObject("MyFirstPlant");
        ChiliPlant plantScript = plantObj.AddComponent<ChiliPlant>();
        plantScript.pepperPrefab = pepperPrefab;

        ChiliData pepperHYBRID = ChiliData.Breed(bell, reaper);

        List<ChiliData> family = ChiliData.growGenerations(pepperHYBRID, 100);

        for (int i = 0; i < family.Count; i++)
        {
            // Space them 15 units apart along the X axis
            Vector3 spawnPos = new Vector3(i * 15.0f, 0, 0);

            GameObject newPlantObj = new GameObject("Plant_Gen_" + i);
            ChiliPlant script = newPlantObj.AddComponent<ChiliPlant>();
            script.pepperPrefab = pepperPrefab;

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