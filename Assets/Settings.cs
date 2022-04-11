using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Settings")]
public class Settings : ScriptableObject
{
    public BlockType blockType = BlockType.GrassBlock;

    [Header("Chunk")]
    public int chunkLength = 16;
    public int chunkHeight = 64;
    
    [Header("Region")]
    public int viewDistance = 10;
    
    [Header("Terrain Gen Parameter")]
    public int seed = 0;
    public float relief = 100;
    public int terrainHeightMax = 64;
    public float xScale = 1;
    public float zScale = 1;
    
}
