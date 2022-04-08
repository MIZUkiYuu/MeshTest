using System;
using System.Collections.Generic;
using UnityEngine;

public enum TileType
{
    // full block tiles
    CubeSide, CubeTop, CubeDown,
    // flower
    FlowerSide
}

public class BlockMesh
{
    private int _side, _top, _down;
    private float _xPos, _yPos;
    private const float Tiles = 16.0f;   // 16 * 16 tiles in one block_texture.png
    private const float Width = 1 / 16.0f;   // the width of single tile
    private const float Crop = 0.001f;   // Avoid crop errors in Unity

    private BlockMesh(int side)
    {
        _side = _top = _down = side;    // The six faces are of the same texture
    }

    private BlockMesh(int side, int top)
    {
        _side = side;
        _top = _down = top; 
    }

    private BlockMesh(int side, int top, int down)
    {
        _side = side;
        _top = top;
        _down = down;
    }

    private Vector2[] CubeTilePos(int num)
    {
        _xPos = (int)(num / 16) / Tiles;
        _yPos = num % 16 / Tiles;
        return new Vector2[4]
        {
            new Vector2(_xPos + Width - Crop, _yPos + Width - Crop), new Vector2(_xPos + Crop, _yPos + Width - Crop),
            new Vector2(_xPos + Width - Crop, _yPos + Crop), new Vector2(_xPos + Crop, _yPos + Crop)
        };
    }
    
    public Vector2[] GetUVs(TileType to)
    {
        switch (to)
        {
            case TileType.CubeTop:
                return CubeTilePos(_top);

            case TileType.CubeDown:
                return CubeTilePos(_down);

            case TileType.CubeSide:
                return CubeTilePos(_side);

            case TileType.FlowerSide:
                break;
            
            default:
                throw new ArgumentOutOfRangeException(nameof(to), to, null);
        }
        return new Vector2[] { };
    }
    

    public static readonly Dictionary<BlockType, BlockMesh> BlockTilePos = new Dictionary<BlockType, BlockMesh>()
    {
        {BlockType.GrassBlock, new BlockMesh(0,1,2)},
        {BlockType.Dirt, new BlockMesh(2)},
        {BlockType.Grass, new BlockMesh(3)},
        {BlockType.TallGrass, new BlockMesh(4, 5)},
        {BlockType.LilyOfTheValley, new BlockMesh(6)},
        {BlockType.Dandelion, new BlockMesh(7)},
        {BlockType.Poppy, new BlockMesh(8)},
        {BlockType.Allium, new BlockMesh(9)},
        {BlockType.OxeyeDaisy, new BlockMesh(10)},
        {BlockType.WhiteTulip, new BlockMesh(11)},
        {BlockType.OrangeTulip, new BlockMesh(12)},
        {BlockType.PinkTulip, new BlockMesh(13)},
        {BlockType.Peony, new BlockMesh(14, 15)},
        {BlockType.Cobblestone, new BlockMesh(16)},
        {BlockType.Stone, new BlockMesh(17)},
        {BlockType.SmoothStone, new BlockMesh(18)},
        {BlockType.StoneBricks, new BlockMesh(19)},
        {BlockType.CrackedStoneBricks, new BlockMesh(20)},
        {BlockType.ChiseledStoneBricks, new BlockMesh(21)},
        {BlockType.Bedrock, new BlockMesh(22)},
        {BlockType.Obsidian, new BlockMesh(23)},
        {BlockType.Gravel, new BlockMesh(24)},
        {BlockType.Andesite, new BlockMesh(25)},
        {BlockType.PolishedAndesite, new BlockMesh(26)},
        {BlockType.Diorite, new BlockMesh(27)},
        {BlockType.PolishedDiorite, new BlockMesh(28)},
        {BlockType.Granite, new BlockMesh(29)},
        {BlockType.PolishedGranite, new BlockMesh(30)},
        {BlockType.Bricks, new BlockMesh(31)},
        {BlockType.CoalOre, new BlockMesh(32)},
        {BlockType.IronOre, new BlockMesh(33)},
        {BlockType.DiamondOre, new BlockMesh(34)},
        {BlockType.GoldOre, new BlockMesh(35)},
        {BlockType.RedstoneOre, new BlockMesh(36)},
        {BlockType.CoalBlock, new BlockMesh(37)},
        {BlockType.IronBlock, new BlockMesh(38)},
        {BlockType.DiamondBlock, new BlockMesh(39)},
        {BlockType.GoldBlock, new BlockMesh(40)},
        {BlockType.RedstoneBlock, new BlockMesh(41)},
        {BlockType.Sand, new BlockMesh(42)},
        {BlockType.AcaciaPlanks, new BlockMesh(43)},
        {BlockType.BirchPlanks, new BlockMesh(44)},
        {BlockType.DarkOakPlanks, new BlockMesh(45)},
        {BlockType.JunglePlanks, new BlockMesh(46)},
        {BlockType.OakPlanks, new BlockMesh(47)},
        {BlockType.SprucePlanks, new BlockMesh(48)},
        {BlockType.AcaciaLog, new BlockMesh(49, 55)},
        {BlockType.BirchLog, new BlockMesh(50, 56)},
        {BlockType.DarkOakLog, new BlockMesh(51, 57)},
        {BlockType.JungleLog, new BlockMesh(52, 58)},
        {BlockType.OakLog, new BlockMesh(53, 59)},
        {BlockType.SpruceLog, new BlockMesh(54, 60)},
        {BlockType.StrippedAcaciaLog, new BlockMesh(61, 67)},
        {BlockType.StrippedBirchLog, new BlockMesh(62, 68)},
        {BlockType.StrippedDarkOakLog, new BlockMesh(63, 69)},
        {BlockType.StrippedJungleLog, new BlockMesh(64, 70)},
        {BlockType.StrippedOakLog, new BlockMesh(65, 71)},
        {BlockType.StrippedSpruceLog, new BlockMesh(66, 72)},
        {BlockType.StrippedAcaciaWood, new BlockMesh(61)},
        {BlockType.StrippedBirchWood, new BlockMesh(62)},
        {BlockType.StrippedDarkOakWood, new BlockMesh(63)},
        {BlockType.StrippedJungleWood, new BlockMesh(64)},
        {BlockType.StrippedOakWood, new BlockMesh(65)},
        {BlockType.StrippedSpruceWood, new BlockMesh(66)},
        {BlockType.AcaciaLeaves, new BlockMesh(73)},
        {BlockType.BirchLeaves, new BlockMesh(74)},
        {BlockType.DarkOakLeaves, new BlockMesh(75)},
        {BlockType.JungleLeaves, new BlockMesh(76)},
        {BlockType.OakLeaves, new BlockMesh(77)},
        {BlockType.SpruceLeaves, new BlockMesh(78)},
        {BlockType.Glass, new BlockMesh(79)},
        {BlockType.BlackStainedGlass, new BlockMesh(80)},
        {BlockType.BlueStainedGlass, new BlockMesh(81)},
        {BlockType.BrownStainedGlass, new BlockMesh(82)},
        {BlockType.CyanStainedGlass, new BlockMesh(83)},
        {BlockType.GrayStainedGlass, new BlockMesh(84)},
        {BlockType.GreenStainedGlass, new BlockMesh(85)},
        {BlockType.LightBlueStainedGlass, new BlockMesh(86)},
        {BlockType.LightGrayStainedGlass, new BlockMesh(87)},
        {BlockType.LimeStainedGlass, new BlockMesh(88)},
        {BlockType.MagentaStainedGlass, new BlockMesh(89)},
        {BlockType.OrangeStainedGlass, new BlockMesh(90)},
        {BlockType.PinkStainedGlass, new BlockMesh(91)},
        {BlockType.PurpleStainedGlass, new BlockMesh(92)},
        {BlockType.RedStainedGlass, new BlockMesh(93)},
        {BlockType.WhiteStainedGlass, new BlockMesh(94)},
        {BlockType.YellowStainedGlass, new BlockMesh(95)},
        {BlockType.QuartzBlock, new BlockMesh(96)},
        {BlockType.SmoothQuartzBlock, new BlockMesh(97)},
        {BlockType.QuartzPillar, new BlockMesh(98, 99)},
        {BlockType.ChiseledQuartzBlock, new BlockMesh(100, 101)},
        {BlockType.QuartzBricks, new BlockMesh(102)},
        
    };
}