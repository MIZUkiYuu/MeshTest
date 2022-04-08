using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Settings")]
public class Settings : ScriptableObject
{
    public BlockType blockType = BlockType.GrassBlock;

    [Header("Chunk")]
    public int length = 16;
    public int height = 64;
    
    [Header("Region")]
    public int viewDistance = 10;
    
    [Header("Terrain Gen Parameter")]
    public int seed = 0;
    public float relief = 100;
    public int heightMax = 64;
    public float xScale = 1;
    public float zScale = 1;
    
}
