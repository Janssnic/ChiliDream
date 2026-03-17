using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChiliData
{
    public string name;
    public int generation;

    // Pepper Stats
    public float scoville;
    public float length;
    public float width;
    public float curvature;
    public float surfaceTexture;
    public float hue;
    public float seed;

    //taste stats
    public float sweetness;
    public float fruitiness;
    public float smokiness;
    public float bitterness;

    // Plant Stats
    public float plantHeight;
    public int maxDepth;
    public float branchAngle;
    public float yieldChance;

    public float meshScale;

    public ChiliData(string n, float s, float l, float w, float c, float t, float h,
                 float pHeight, int pDepth, float pAngle, float pYield,
                 float sweet, float fruit, float smoke, float bitter,
                 float mScale = 1.0f, int gen = 1)
    {
        name = n; scoville = s; length = l; width = w;
        curvature = c; surfaceTexture = t; hue = h;

        plantHeight = pHeight;
        maxDepth = pDepth;
        branchAngle = pAngle;
        yieldChance = pYield;
        meshScale = mScale;

        sweetness = sweet;
        fruitiness = fruit;
        smokiness = smoke;
        bitterness = bitter;

        generation = gen;
        seed = (float)(new System.Random().NextDouble() * 10000.0);
    }

    public static ChiliData Breed(ChiliData p1, ChiliData p2)
    {
        // 1. Physical Mutation Factor (15% chance to drift)
        float mutate = (Random.value < 0.15f) ? Random.Range(0.8f, 1.2f) : 1.0f;

        float cScoville = ((p1.scoville + p2.scoville) / 2) * mutate;
        float cLen = ((p1.length + p2.length) / 2) * mutate;
        float cWid = ((p1.width + p2.width) / 2) * mutate;
        float cCurve = ((p1.curvature + p2.curvature) / 2) * mutate;
        float cTex = ((p1.surfaceTexture + p2.surfaceTexture) / 2) * mutate;

        // 2. Flavor Mutation (Sweet, Fruit, Smoke, Bitter)
        // We average the parents and then apply the mutation factor
        float cSweet = Mathf.Clamp01(((p1.sweetness + p2.sweetness) / 2) * mutate);
        float cFruit = Mathf.Clamp01(((p1.fruitiness + p2.fruitiness) / 2) * mutate);
        float cSmoke = Mathf.Clamp01(((p1.smokiness + p2.smokiness) / 2) * mutate);
        float cBitter = Mathf.Clamp01(((p1.bitterness + p2.bitterness) / 2) * mutate);

        // Rare "Flavor Burst" - 0.5% chance to max out one trait
        if (Random.value < 0.005f)
        {
            float roll = Random.value;
            if (roll < 0.25f) cSweet = 1.0f;
            else if (roll < 0.5f) cFruit = 1.0f;
            else if (roll < 0.75f) cSmoke = 1.0f;
            else cBitter = 1.0f;
        }

        // color mutations
        float baseHue = Mathf.Lerp(p1.hue, p2.hue, Random.value);
        float hueDrift = (Random.value < 0.05f) ? Random.Range(-0.1f, 0.1f) : 0f;
        float cHue = Mathf.Repeat(baseHue + hueDrift, 1.0f);

        // plant mutation
        float cHeight = ((p1.plantHeight + p2.plantHeight) / 2) * mutate;
        float cAngle = ((p1.branchAngle + p2.branchAngle) / 2) * mutate;
        float cYield = Mathf.Clamp01(((p1.yieldChance + p2.yieldChance) / 2) * mutate);

        // Depth logic
        int cDepth = (Random.value < 0.5f) ? p1.maxDepth : p2.maxDepth;
        if (Random.value < 0.05f) cDepth++;
        else if (Random.value < 0.05f) cDepth--;
        cDepth = Mathf.Clamp(cDepth, 1, 6);

        // Create the new plant with updated flavor params
        return new ChiliData(
            "Hybrid", cScoville, cLen, cWid, cCurve, cTex, cHue,
            cHeight, cDepth, cAngle, cYield,
            cSweet, cFruit, cSmoke, cBitter, // The new flavor genes
            1.0f, // meshScale default
            Mathf.Max(p1.generation, p2.generation) + 1
        );
    }
    public static List<ChiliData> growGenerations(ChiliData plant, int generations)
    {
        List<ChiliData> family = new List<ChiliData>();
        ChiliData current = plant;
        family.Add(current);
        for (int i = 0; i < generations - 1; i++)
        {
            current = Breed(current, current);
            family.Add(current);
        }
        return family;
    }
}