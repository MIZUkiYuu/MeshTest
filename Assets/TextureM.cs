using UnityEditor;
using UnityEngine;

public class TextureM
{
    public static Texture2D Load(BlockType blockType)
    {
        return AssetDatabase.LoadAssetAtPath<Texture2D>(BlockTile.BlockTilePath[blockType]);
    }
    
    public static Texture2D Load(string path)
    {
        return AssetDatabase.LoadAssetAtPath<Texture2D>(path);
    }

    public static int GetWidth(BlockType blockType)
    {
        return Load(blockType).width;
    }
    
    public static int GetWidth(string path)
    {
        return Load(path).width;
    }
}
