using UnityEngine;
using System.Collections.Generic;

public class ChiliPlant : MonoBehaviour
{
    public GameObject pepperPrefab; // Drag your Pepper Prefab here
    public ChiliData data;          // The DNA of this specific plant

    public GameObject labelPrefab;

    [Header("Visual Settings")]
    public float thicknessMultiplier = 0.1f;


    public void GeneratePlant(ChiliData dna, Vector3 Pos)
    {
        data = dna;
        transform.position = Pos;

        // 1. Clear old growth (This is what was deleting your labels!)
        while (transform.childCount > 0)
        {
            // DestroyImmediate is used here to ensure the count updates instantly
            DestroyImmediate(transform.GetChild(0).gameObject);
        }

        // 2. Start recursive growth
        GrowBranch(Pos, Vector3.up * data.plantHeight, 0);

        // 3. SPAWN LABEL LAST (So it doesn't get deleted by the loop above)
        if (labelPrefab != null)
        {
            // Placing it slightly in front (Z: -1) so it doesn't clip into the stem
            Vector3 labelPos = Pos + new Vector3(0, -0.5f, -1f);
            GameObject labelObj = Instantiate(labelPrefab, labelPos, Quaternion.identity, transform);

            // Use GetComponentInChildren if the script is on the Canvas but the text is a child
            ChiliLabel labelScript = labelObj.GetComponent<ChiliLabel>();
            if (labelScript != null)
            {
                labelScript.SetText(data);
            }
        }
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

        // Scale the pepper based on the plant's overall scale or a fixed ratio
        // This ensures the pepper isn't 5x larger than the branch it sits on
        newPepper.transform.localScale = Vector3.one * 0.5f;

        ChiliProceduralMesh meshScript = newPepper.GetComponent<ChiliProceduralMesh>();
        if (meshScript != null)
        {
            meshScript.Generate(data);
        }
    }
}