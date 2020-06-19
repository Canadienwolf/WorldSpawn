using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator_Noise : MonoBehaviour
{
    public int width = 1;
    public int height = 1;
    [Range(1, 100)]
    public float hillSize = 50;
    [Range(1, 50)]
    public float hillSteepess = 25;

    public float perlinZoom = 0.3f;
    public float steepness = 2f;

    Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;

    public void Start()
    {
        mesh = new Mesh();

        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = new Material(Shader.Find("Standard"));

        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        GenerateShape();
        UpdateMesh();

    }

    float MakeNoise(float x, float z)
    {
        float hillNoise = Mathf.PerlinNoise(x * (1 / hillSize), z * (1 / hillSize)) * hillSteepess;
        
        return 0;
    }

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
    }

    void GenerateShape()
    {
        vertices = new Vector3[(width + 1) * (height + 1)];

        for (int i = 0, z = 0; z <= height; z++)
        {
            for (int x = 0; x <= width; x++)
            {
                float y = MakeNoise(x, z);
                vertices[i] = new Vector3(x, y, z);
                i++;
            }
        }

        triangles = new int[width * height * 6];

        int vert = 0;
        int tris = 0;
        for (int z = 0; z < height; z++)
        {
            for (int i = 0; i < width; i++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + width + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + width + 1;
                triangles[tris + 5] = vert + width + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }
    }
}
