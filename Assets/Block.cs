using UnityEngine;

public static class Block
{
    public static void SetBlock(int x, int y, int z, BlockType blockType)
    {
        BlockType[,,] blocks = ChunksGen.Blocks;
        blocks[x, y, z] = blockType;
        ChunksGen.Blocks = blocks;
    }
    
    public static void SetBlock(int x, int y, int z, BlockType blockType, RangeInt blockMask)
    {
        BlockType[,,] blocks = ChunksGen.Blocks;
        if (blockMask.start < (int)blocks[x, y, z] && (int)blocks[x, y, z] < blockMask.end) return;
        blocks[x, y, z] = blockType;
        ChunksGen.Blocks = blocks;
    }
}

public enum BlockType
{
    Air,
    GrassBlock, Dirt,
    Grass, TallGrass,
    // flower
    LilyOfTheValley, Dandelion, Poppy, Allium, OxeyeDaisy, WhiteTulip, OrangeTulip, PinkTulip, Peony,
    // stone
    Cobblestone, Stone, SmoothStone, StoneBricks, CrackedStoneBricks, ChiseledStoneBricks,
    Bedrock, Obsidian, Gravel, Andesite, PolishedAndesite, Diorite, PolishedDiorite, Granite, PolishedGranite, Bricks,
    // ore
    CoalOre, IronOre, DiamondOre, GoldOre, RedstoneOre, CoalBlock, IronBlock, DiamondBlock, GoldBlock, RedstoneBlock,
    Sand,
    // wood
    AcaciaPlanks, BirchPlanks, DarkOakPlanks, JunglePlanks, OakPlanks, SprucePlanks,
    AcaciaLog, BirchLog, DarkOakLog, JungleLog, OakLog, SpruceLog,
    StrippedAcaciaLog, StrippedBirchLog, StrippedDarkOakLog, StrippedJungleLog, StrippedOakLog, StrippedSpruceLog,
    StrippedAcaciaWood, StrippedBirchWood, StrippedDarkOakWood, StrippedJungleWood, StrippedOakWood, StrippedSpruceWood,
    AcaciaLeaves, BirchLeaves, DarkOakLeaves, JungleLeaves, OakLeaves, SpruceLeaves,
    // glass
    Glass,
    BlackStainedGlass, BlueStainedGlass, BrownStainedGlass, CyanStainedGlass, GrayStainedGlass, GreenStainedGlass, 
    LightBlueStainedGlass, LightGrayStainedGlass, LimeStainedGlass, MagentaStainedGlass,OrangeStainedGlass, 
    PinkStainedGlass, PurpleStainedGlass, RedStainedGlass, WhiteStainedGlass, YellowStainedGlass, 
    // quartz
    QuartzBlock, SmoothQuartzBlock, QuartzPillar, ChiseledQuartzBlock, QuartzBricks
}

public enum Direction
{
   Top, Down, Front, Back, Right, Left
}