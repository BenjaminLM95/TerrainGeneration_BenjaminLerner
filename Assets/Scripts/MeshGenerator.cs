using UnityEngine;

public static class MeshGenerator 
{
    // Method to generate a terrain mesh from a height map
    public static MeshData GenerateTerrainMesh(float[,] heightMap, float heightMultiplier, AnimationCurve heightCurve, int levelOfDetail) 
    {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);
        float topLeftX = (width - 1) / 2f;
        float topLeftZ = (height - 1) / 2f;

        // Calculate how much to simplify the mesh based on the level of detail
        int meshSimplificationIncrement;
        
        if(levelOfDetail == 0) 
        {
            meshSimplificationIncrement = 1;
        }
        else 
        {
            meshSimplificationIncrement = levelOfDetail * 2;
        }
                
        int verticesPerLine = (width - 1) / meshSimplificationIncrement + 1;

        // Initialize a MeshData object to hold the mesh's vertices, triangles, and UV coordinates
        MeshData meshData = new MeshData(verticesPerLine, verticesPerLine);
        int vertexIndex = 0;

        for(int z = 0; z < height; z+= meshSimplificationIncrement) 
        {
            for (int x = 0; x < width; x+= meshSimplificationIncrement)  
            {
                // Calculate the position of (x, z), adjusting the height using the height map
                // Height is modified by the heightMultiplier and the heightCurve for more control

                meshData.vertices[vertexIndex] = new Vector3(topLeftX + x, heightCurve.Evaluate(heightMap[x, z]) * heightMultiplier, topLeftZ + z); 
                meshData.uvs[vertexIndex] = new Vector2(x/(float)width, z/(float)height);

                if(x < width - 1 && z < height - 1) 
                {
                    meshData.AddTriangles(vertexIndex, vertexIndex + verticesPerLine + 1, vertexIndex + verticesPerLine);
                    meshData.AddTriangles(vertexIndex + verticesPerLine + 1, vertexIndex, vertexIndex + 1); 
                }

                vertexIndex++;
            }
        }

        return meshData; 
    }
}

public class MeshData 
{
    public Vector3[] vertices;
    public int[] triangles;
    public Vector2[] uvs; 

    int triangleIndex;

    public MeshData(int meshWidth, int meshHeight) 
    {
        vertices = new Vector3[meshWidth * meshHeight];
        uvs = new Vector2[meshWidth * meshHeight];
        triangles = new int[(meshWidth - 1) * (meshHeight - 1) * 6];
    }

    public void AddTriangles(int a, int b, int c) 
    {
        triangles[triangleIndex] = a;
        triangles[triangleIndex + 1] = b;
        triangles[triangleIndex + 2] = c;
        triangleIndex += 3; 
    }

    public Mesh CreateMesh() 
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
        return mesh; 
    }
}
