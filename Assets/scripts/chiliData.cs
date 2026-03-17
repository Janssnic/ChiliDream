using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChiliData
{
    public string name;
    public int generation;

    // Core Traits
    public float scoville;
    public float length;
    public float width;
    public float curvature;
    public float surfaceTexture;
    public float hue; // 0 to 1 for Unity's Color.HSVToRGB
    public float seed;


    public ChiliData(string n, float s, float l, float w, float c, float t, float h, int gen = 1)
    {
        name = n; scoville = s; length = l; width = w;
        curvature = c; surfaceTexture = t; hue = h; generation = gen;

        this.seed = (float)(new System.Random().NextDouble() * 10000.0);
    }

    public static ChiliData Breed(ChiliData p1, ChiliData p2)
{
    // Individual mutation check for physical traits
    float physMutate = (Random.value < 0.15f) ? Random.Range(0.8f, 1.2f) : 1.0f;

    // Separate Color mutation logic
    float baseHue = Mathf.Lerp(p1.hue, p2.hue, Random.value);
    float hueDrift = (Random.value < 0.05f) ? Random.Range(-0.1f, 0.1f) : 0f;
    // Mathf.Repeat ensures 1.1 becomes 0.1 (circular color spectrum)
    float finalHue = Mathf.Repeat(baseHue + hueDrift, 1.0f);

    return new ChiliData(
        "Hybrid",
        ((p1.scoville + p2.scoville) / 2) * physMutate,
        ((p1.length + p2.length) / 2) * physMutate,
        ((p1.width + p2.width) / 2) * physMutate,
        ((p1.curvature + p2.curvature) / 2) * physMutate,
        ((p1.surfaceTexture + p2.surfaceTexture) / 2) * physMutate,
        finalHue,
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
        // Breeding with self causes the traits to drift over time
        current = Breed(current, current);
        family.Add(current);
    }
    return family;
}
}