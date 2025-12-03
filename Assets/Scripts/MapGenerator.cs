using UnityEngine;
using UnityEngine.UI; 


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
    private int regionIndex; 

    public Slider heightSlider;
    public float heightSliderValue; 

    private void Awake()
    {
        GeneratingNewMap(1);

        heightSlider.value = (meshHeightMultiplier + 100) / -400;
        heightSliderValue = heightSlider.value; 
    }

    private void Update()
    {
        if(heightSlider.value != heightSliderValue) 
        {
            meshHeightMultiplier = (heightSliderValue * -400) - 100;
            heightSliderValue = heightSlider.value;
            GenerateMap(); 
        }
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

                for (int i = 0; i < regionsColors.currentTerrain.Length; i++) 
                {
                    if(currentHeight <= regionsColors.currentTerrain[i].height) 
                    {
                        colorMap[y * mapChunkSize + x] = regionsColors.currentTerrain[i].color;
                        break;
                    }


                }
            }
        }


        PlaneTexture display = FindFirstObjectByType<PlaneTexture>();
              
        display.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, meshHeightMultiplier, meshHeightCurve, levelOfDetail), TextureGenerator.TextureFromColorMap(colorMap, mapChunkSize, mapChunkSize)); 
        
    }

    private void GeneratingNewMap(int num) 
    {
        switch (num) 
        {
            case 1:
                regionsColors.ChangeIslandTerrain();
                regionIndex = 1;
                break;
            case 2:
                regionsColors.ChangeVolcanicTerrain();
                regionIndex = 2;
                break;
            case 3:
                regionsColors.ChangeCanyonTerrain();
                regionIndex = 3;
                break;
            default:
                regionsColors.ChangeIslandTerrain();
                regionIndex = 1; 
                break;

        }

        seed++;
        offset = new Vector2(Random.Range(-100, 101), Random.Range(-100, 101)); 
        GenerateMap();

    }

    public void ChangeToIslandAction() 
    {
        GeneratingNewMap(1);
    }

    public void ChangeToVolcanic() 
    {
        GeneratingNewMap(2);
    }

    public void ChangeToCanyon() 
    {
        GeneratingNewMap(3);     
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
