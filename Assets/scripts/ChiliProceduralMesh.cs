using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ChiliProceduralMesh : MonoBehaviour
{
    public void Generate(ChiliData data)
    {
        Mesh mesh = new Mesh();
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Color> colors = new List<Color>();

        int segments = 100;
        float length = data.length * 0.15f;
        float widthBase = data.width * 0.12f;

        // Pushes the 'center' of the fan to the top third to prevent spikes
        vertices.Add(new Vector3(0, -length * 0.4f, 0));
        colors.Add(Color.HSVToRGB(data.hue, 0.85f, 0.9f));

        // Unique seed for every pepper name so they aren't identical
        float seed = data.seed;

        for (int i = 0; i <= segments; i++)
        {
            float angle = i * (360f / segments) * Mathf.Deg2Rad;
            float cosA = Mathf.Cos(angle);
            float sinA = Mathf.Sin(angle);

            // 1. ORGANIC NOISE (The "Ghost" Factor)
            // We sample noise in a circle. High texture = high frequency and jaggedness.
            float noiseFreq = Mathf.Lerp(1.5f, 6.0f, data.surfaceTexture);
            float n = Mathf.PerlinNoise((cosA + 1) * noiseFreq + seed, (sinA + 1) * noiseFreq + seed);

            // This "sharpens" the noise for rough peppers
            float noiseEffect = Mathf.Pow(n, Mathf.Lerp(1f, 3f, data.surfaceTexture));
            float lump = noiseEffect * (data.surfaceTexture * 0.3f);

            // 2. DIMENSIONING
            float x = cosA * (widthBase + lump);
            float y = sinA * (length + lump);

            // 3. THE "THAI" POINTY TIP
            // If it's a thin pepper, we aggressively pull the bottom to a point.
            if (y < 0)
            {
                float taperPower = Mathf.Clamp01(1.2f - (data.width * 0.2f));
                float distFromTop = Mathf.Abs(y / length);
                x *= Mathf.Lerp(1f, 1f - taperPower, distFromTop);

                // Curvature
                x += (y * y) * (data.curvature * 0.7f);
            }
            
            float finalX = x * data.meshScale;
            float finalY = (y - length) * data.meshScale;

            vertices.Add(new Vector3(finalX, finalY, 0));
            colors.Add(Color.HSVToRGB(data.hue, 0.85f, 0.9f));

            if (i > 0)
            {
                triangles.Add(0); triangles.Add(i + 1); triangles.Add(i);
            }
        }

        // --- PART 2: THE ADAPTIVE STEM ---
        int stemStart = vertices.Count;
        float sW = 0.02f + (data.width * 0.015f);
        float sH = 0.2f + (data.length * 0.04f);
        float sL = data.curvature * 0.15f;

        vertices.Add(new Vector3(-sW, 0, 0));
        vertices.Add(new Vector3(sW, 0, 0));
        vertices.Add(new Vector3(sW + sL, sH, 0));
        vertices.Add(new Vector3(-sW + sL, sH, 0));

        for (int i = 0; i < 4; i++) colors.Add(new Color(0.15f, 0.45f, 0.15f));

        triangles.Add(stemStart); triangles.Add(stemStart + 2); triangles.Add(stemStart + 1);
        triangles.Add(stemStart); triangles.Add(stemStart + 3); triangles.Add(stemStart + 2);

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.colors = colors.ToArray();
        mesh.RecalculateNormals();
        GetComponent<MeshFilter>().mesh = mesh;
    }
}