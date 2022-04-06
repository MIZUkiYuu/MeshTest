using System.Collections.Generic;
using UnityEngine;

public class BlockTile
{
    public static Vector2[] GetUVs(BlockType blockType,TileOrientation to)
    {
        float t = 1 / 2.0f;
        float d = 0.001f;   // Avoid crop rendering errors in Unity
        if (TextureM.GetWidth(blockType) == 16)
        {
            return GetUVs(blockType);
        }

        return to switch
        {
            TileOrientation.Top => new Vector2[4]
            {
                new Vector2(t - d, 1.0f - d), new Vector2(0 + d, 1.0f - d), new Vector2(t - d, t + d), new Vector2(0 + d, t + d)
            },
            TileOrientation.Down => new Vector2[4]
            {
                new Vector2(1.0f -d, t - d), new Vector2(t + d, t - d), new Vector2(1.0f - d, 0 + d), new Vector2(t + d, 0 + d)
            },
            _ => new Vector2[4] {new Vector2(t - d, t - d), new Vector2(0 + d, t - d), new Vector2(t - d, 0 +d), new Vector2(0 +d, 0 + d)}
        };
    }

    public static Vector2[] GetUVs(BlockType blockType)
    {
        float t = 1 / 2.0f;
        float d = 0.001f;
        if (TextureM.GetWidth(blockType) == 16)
        {
            t = 1.0f;
        }
        return new Vector2[4] {new Vector2(t - d, t - d), new Vector2(0 + d, t - d), new Vector2(t - d, 0 + d), new Vector2(0 + d, 0 + d)};
    }

    public static readonly Dictionary<BlockType, string> BlockTilePath = new Dictionary<BlockType, string>()
    {
        {BlockType.Air, ""},
        {BlockType.GrassBlock, "Assets/grass_block.png"},
        {BlockType.BirchPlanks, "Assets/birch_planks.png"},
        {BlockType.Dirt, "Assets/dirt.png"},

    };
}

public enum TileOrientation
{
    Top, Down
}
