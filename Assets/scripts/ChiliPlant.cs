using UnityEngine;
using System.Collections.Generic;

public class ChiliPlant : MonoBehaviour
{
    public GameObject pepperPrefab; // Drag your Pepper Prefab here
    public ChiliData data;          // The DNA of this specific plant

    [Header("Visual Settings")]
    public float thicknessMultiplier = 0.1f;

    public void GeneratePlant(ChiliData dna, Vector3 Pos)
    {
        data = dna;

        // 1. Set the main object's position to the requested Pos
        transform.position = Pos;

        // 2. Clear old growth 
        // (Note: Destroying in a loop can be tricky, so we use a list or a while loop)
        while (transform.childCount > 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }

        // 3. Start the recursive growth using the specific Pos provided
        // We multiply 'up' by plantHeight to determine where the first split happens
        GrowBranch(Pos, Vector3.up * data.plantHeight, 0);
    }

    void GrowBranch(Vector3 startPos, Vector3 direction, int depth)
    {
        if (depth >= data.maxDepth) return;

        float fruitBuffer = (data.length * data.meshScale) * 0.05f;
        Vector3 adjustedDir = direction + (direction.normalized * fruitBuffer);

        Vector3 endPos = startPos + adjustedDir;

        // 1. Create the Branch Visual (Simple Cylinder/Line)
        GameObject branch = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        branch.transform.parent = this.transform;

        // Position and Scale the branch
        float dist = Vector3.Distance(startPos, endPos);
        branch.transform.position = startPos + (direction / 2);
        branch.transform.up = direction;
        // Branches get thinner as they go up
        float scale = (data.width * thicknessMultiplier) / (depth + 1);
        branch.transform.localScale = new Vector3(scale, dist / 2, scale);

        // Color the branch green
        branch.GetComponent<Renderer>().material.color = new Color(0.1f, 0.4f, 0.1f);

        // 2. Chance to Spawn a Pepper at this node
        if (Random.value < data.yieldChance)
        {
            SpawnPepperAtNode(endPos);
        }

        // 3. Recursive Splitting (The "Y" Shape)
        // Calculate new directions for the next two branches
        float varyAngle = data.branchAngle + Random.Range(-10f, 10f);
        Vector3 leftDir = Quaternion.Euler(0, 0, varyAngle) * direction * 0.75f;
        Vector3 rightDir = Quaternion.Euler(0, 0, -varyAngle) * direction * 0.75f;

        GrowBranch(endPos, leftDir, depth + 1);
        GrowBranch(endPos, rightDir, depth + 1);
    }

    void SpawnPepperAtNode(Vector3 pos)
    {
        GameObject newPepper = Instantiate(pepperPrefab, pos, Quaternion.identity, this.transform);

        // Use your existing procedural logic!
        ChiliProceduralMesh meshScript = newPepper.GetComponent<ChiliProceduralMesh>();
        if (meshScript != null)
        {
            meshScript.Generate(data);
        }
    }
}