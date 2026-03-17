using UnityEngine;
using System.Collections.Generic;

public class PepperLabManager : MonoBehaviour
{
    public GameObject pepperPrefab; 
    public List<ChiliData> activePeppers = new List<ChiliData>();

    void Start()
    {
        ChiliData bell = new ChiliData("Bell", 0, 5, 4, 0, 0.1f, 0.33f); // Green
        ChiliData habanero = new ChiliData("Hab", 300000, 3, 2, 0.2f, 0.8f, 0.05f); // Orange/Red
        ChiliData thai = new ChiliData("Thai Bird Eye", 100000f, 3.0f, 0.5f, 0.8f, 0.2f, 0.0f, 1);
        ChiliData ghost = new ChiliData("Bhut Jolokia", 1041427f, 7.5f, 3.0f, 0.3f, 0.9f, 0.0f, 1);

        //SpawnPepper(ghost, new Vector3(-4, 0, 0));
        //SpawnPepper(thai, new Vector3(4, 0, 0));


        ChiliData hybrid = ChiliData.Breed(bell, ghost);

        List<ChiliData> familyTree = ChiliData. growGenerations(hybrid, 100);

        // Spawn them in a row
        for (int i = 0; i < familyTree.Count; i++)
        {
            Vector3 pos = new Vector3(i * 3.0f, 0, 0);
            SpawnPepper(familyTree[i], pos);
        }
        //SpawnPepper(hybrid, new Vector3(0, -3, 0));
    }

    void SpawnPepper(ChiliData data, Vector3 position)
    {
        GameObject obj = Instantiate(pepperPrefab, position, Quaternion.identity);
        obj.GetComponent<ChiliProceduralMesh>().Generate(data);
        obj.name = data.name + "_Gen" + data.generation;
    }
}