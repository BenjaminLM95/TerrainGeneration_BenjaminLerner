using UnityEngine;


public class MapGenerator : MonoBehaviour
{
    const int mapChunkSize = 241;
    [Range(0,6)]
    public int levelOfDetail;
        
    public float noiseScale;

    public int octaves;
    [Range(0,1)]
    public float persistance;
    public float lacunarity;

    public int seed;    
    public Vector2 offset;

    public float meshHeightMultiplier;
    public AnimationCurve meshHeightCurve; 

    public bool autoUpdate;    

    public RegionColors regionsColors; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenerateMap(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateMap() 
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapChunkSize, mapChunkSize, seed, noiseScale, octaves, persistance, lacunarity, offset);

        Color[] colorMap = new Color[mapChunkSize * mapChunkSize];

        for (int y = 0; y < mapChunkSize; y++) 
        {
            for(int x = 0; x < mapChunkSize; x++) 
            {
                float currentHeight = noiseMap[x, y];

                for (int i = 0; i < regionsColors.regions.Length; i++) 
                {
                    if(currentHeight <= regionsColors.regions[i].height) 
                    {
                        colorMap[y * mapChunkSize + x] = regionsColors.regions[i].color;
                        break;
                    }


                }
            }
        }


        PlaneTexture display = FindFirstObjectByType<PlaneTexture>();
              
        display.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, meshHeightMultiplier, meshHeightCurve, levelOfDetail), TextureGenerator.TextureFromColorMap(colorMap, mapChunkSize, mapChunkSize)); 
        
    }

    private void OnValidate()
    {
        
        if(lacunarity < 1) 
        {
            lacunarity = 1;
        }
        if(octaves < 0) 
        {
            octaves = 0;
        }

    }

    [System.Serializable] 
    public struct TerrainType 
    {
        public string name; 
        public float height;
        public Color color; 
    }

}
