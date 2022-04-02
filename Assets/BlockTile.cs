using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockTile
{
    public static Vector2[] GetUVs(BlockType blockType,TileOrientation to)
    {
        const float t = 1 / 2.0f;
        Texture texture = AssetDatabase.LoadAssetAtPath<Texture2D>(BlockTilePath[blockType]);
        if (texture.width == 32 && to == TileOrientation.Top)
        {
            return new Vector2[4] {new Vector2(t, 1.0f), new Vector2(0, 1.0f), new Vector2(t, t), new Vector2(0, t)};
        }

        if (texture.width == 32 && to == TileOrientation.Down)
        {
            return new Vector2[4] {new Vector2(1.0f, t), new Vector2(t, t), new Vector2(1.0f, 0), new Vector2(t, 0)};
        }
        //side, or same 6 faces
        if (texture.width == 32)
        {
            return new Vector2[4] {new Vector2(t, t), new Vector2(0, t), new Vector2(t, 0), new Vector2(0, 0)};
        }
        return new Vector2[4] {new Vector2(1, 1), new Vector2(0, 1), new Vector2(1, 0), new Vector2(0, 0)};
    }

    public static Vector2[] GetUVs()
    {
        return new Vector2[4] {new Vector2(1, 1), new Vector2(0, 1), new Vector2(1, 0), new Vector2(0, 0)};
    }

    public static Dictionary<BlockType, string> BlockTilePath = new Dictionary<BlockType, string>()
    {
        {BlockType.Air, ""},
        {BlockType.GrassBlock, "Assets/grass_block.png"},
        {BlockType.BirchPlanks, "Assets/birch_planks.png"}

    };
}

public enum TileOrientation
{
    Top, Down, Side
}
