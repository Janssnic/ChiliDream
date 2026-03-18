using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

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
    public static string translateToBinary(float chiliData)
    {
        int plantGene = BitConverter.SingleToInt32Bits(chiliData);
        string binary = Convert.ToString(plantGene, 2).PadLeft(32, '0');
        //Debug.Log(chiliData);
        //Debug.Log(binary);
        return binary;
    }

    public static float translateTofloat(string chiliData)
    {
        int bitInt = Convert.ToInt32(chiliData, 2);
        float restoredFloat = BitConverter.Int32BitsToSingle(bitInt);

        //Debug.Log(restoredFloat);
        return restoredFloat;
    }

    public static string DavisOrderCrossover(float dna1, float dna2)
    {
        int p1 = BitConverter.SingleToInt32Bits(dna1);
        int p2 = BitConverter.SingleToInt32Bits(dna2);

        int pt1 = UnityEngine.Random.Range(0, 15);
        int pt2 = UnityEngine.Random.Range(16, 31);

        int mask = (-1 >> (32 - (pt2 - pt1))) << pt1;

        int childBits = (p1 & mask) | (p2 & ~mask);

        string newGene = Convert.ToString(childBits, 2).PadLeft(32, '0');
        return Mutate(newGene, 0.5f);
    }

    public static string Mutate(string dna, float mutationChance)
    {

        if (UnityEngine.Random.value > mutationChance)
        {
            return dna;
        }

        char[] bits = dna.ToCharArray();

        int index = UnityEngine.Random.Range(9, 32);

        if (bits[index] == '0')
        {
            bits[index] = '1';
        }
        else
        {
            bits[index] = '0';
        }


        return new string(bits);
    }

    public static ChiliData BinaryBreed(ChiliData pepper1, ChiliData pepper2)
    {

        //scoville gene breeding
        string scoville = DavisOrderCrossover(pepper1.scoville, pepper2.scoville);
        float hybridScovile = translateTofloat(scoville);
        Debug.Log(hybridScovile);
        //length breeding
        string lenght = DavisOrderCrossover(pepper1.length, pepper2.length);
        float hybridLength = translateTofloat(lenght);

        string width = DavisOrderCrossover(pepper1.width, pepper2.width);
        float hybridWidth = translateTofloat(width);

        string curve = DavisOrderCrossover(pepper1.curvature, pepper2.curvature);
        float hybridCurve = translateTofloat(curve);

        string texture = DavisOrderCrossover(pepper1.surfaceTexture, pepper2.surfaceTexture);
        float hybridTexture = translateTofloat(texture);

        string hue = DavisOrderCrossover(pepper1.hue, pepper2.hue);
        float hybridHue = translateTofloat(hue);

        string height = DavisOrderCrossover(pepper1.plantHeight, pepper2.plantHeight);
        float hybridHeight = translateTofloat(height);

        string depth = DavisOrderCrossover(pepper1.maxDepth, pepper2.maxDepth);
        float hybridDepth = translateTofloat(depth);

        string angle = DavisOrderCrossover(pepper1.branchAngle, pepper2.branchAngle);
        float hybridAngle = translateTofloat(angle);

        string yield = DavisOrderCrossover(pepper1.yieldChance, pepper2.yieldChance);
        float hybridYield = translateTofloat(yield);

        string sweet = DavisOrderCrossover(pepper1.sweetness, pepper2.sweetness);
        float hybridSweet = translateTofloat(sweet);

        string smoke = DavisOrderCrossover(pepper1.smokiness, pepper2.smokiness);
        float hybridSmoke = translateTofloat(smoke);

        string bitter = DavisOrderCrossover(pepper1.bitterness, pepper2.bitterness);
        float hybridBitter = translateTofloat(bitter);

        string fruit = DavisOrderCrossover(pepper1.fruitiness, pepper2.fruitiness);
        float hybridFruit = translateTofloat(fruit);

        return new ChiliData(
            "hybrid", hybridScovile, hybridLength, hybridWidth, hybridCurve, hybridTexture, hybridHue,
            hybridHeight, (int)hybridDepth, hybridAngle, hybridYield,
            hybridSweet, hybridFruit, hybridSmoke, hybridBitter,
            1.0f, // meshScale default
            Mathf.Max(pepper1.generation, pepper2.generation) + 1
        );
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
            current = BinaryBreed(current, current);
            //BinaryBreed(current, current);//remove TESTING ONLY
            family.Add(current);
        }
        return family;
    }
}