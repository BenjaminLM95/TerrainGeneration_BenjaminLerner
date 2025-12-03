using UnityEngine;
using static MapGenerator;

public class RegionColors : MonoBehaviour
{
    public TerrainType[] islandTerrain = new TerrainType[8];
    public TerrainType[] volcanicTerrain = new TerrainType[8];
    public TerrainType[] canyonTerrain = new TerrainType[8];

    public TerrainType[] currentTerrain = new TerrainType[8]; 
       

    public void ChangeTerrainData(TerrainType[] _terrainType) 
    {
        for(int i = 0;  i < islandTerrain.Length; i++) 
        {
            currentTerrain[i].name = _terrainType[i].name;
            currentTerrain[i].height = _terrainType[i].height;
            currentTerrain[i].color = _terrainType[i].color;
        }

        Debug.Log("Getting Island Terrain"); 
    }

    public void ChangeIslandTerrain() 
    {
        ChangeTerrainData(islandTerrain);
    }

    public void ChangeVolcanicTerrain() 
    {
        ChangeTerrainData(volcanicTerrain);
    }

    public void ChangeCanyonTerrain() 
    {
        ChangeTerrainData(canyonTerrain); 
    }

}
