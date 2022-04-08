using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using Random = UnityEngine.Random;

public class ChunksGen : MonoBehaviour
{
    public Settings settings;
    public GameObject CHUNK;

    private BlockType[,,] _blocks; // store all blocks in all chunks
    private List<Chunk> _chunks = new List<Chunk>();
    private float _randomX, _randomZ;   // random parameter of Perlin Noise

    private void Start()
    {
        Random.InitState(settings.seed);
        _randomX = Random.value * 100;
        
        Random.InitState(Random.Range(0,100000));
        _randomZ = Random.value * 100;
        TerrainGen();
        BuildChunks();
    }

    private int TotalLength()
    {
        return 2 * settings.length * settings.viewDistance + settings.length;
    }
    private void TerrainGen()   // generate the terrain
    {
        _blocks = new BlockType[TotalLength(), settings.height, TotalLength()];    //store all blocks
        
        for (int x = 0; x < TotalLength(); x++)
        {
            for (int z = 0; z < TotalLength(); z++)
            {
                int y = GetY(x, z);
                
                _blocks[x, y, z] = settings.blockType;  // the surface
                GenTree(x, y, z);
                
                // bedrock layer
                for (int i = 0; i < y; i++)
                {
                    if (i < 5)
                    {
                        _blocks[x, i, z] = BlockType.Bedrock;
                    }
                    else
                    {
                        _blocks[x, i, z] = BlockType.Dirt;
                    }
                }
            }
        }
    }

    //use Perlin noise to generate the value of y
    private int GetY(int x, int z)
    {
        float xSample = (x * settings.xScale + _randomX) / settings.relief;
        float zSample = (z * settings.zScale + _randomZ) / settings.relief;  // z  not z + z
        float yNoise = Mathf.Clamp(Mathf.PerlinNoise(xSample, zSample), 0, 1);
        return (int)(yNoise * settings.heightMax);
    }

    private void GenTree(int x, int y, int z)
    {
        if (Random.Range(0, 100) == 0 && x > 2 && x < TotalLength() - 3 && z > 2 && z < TotalLength() - 3 && y < settings.height - 20)
        {
            int height = Random.Range(4, 10);
            int treeType = Random.Range((int)BlockType.AcaciaLog, (int)BlockType.SpruceLog);
            for (int i = 1; i < height; i++)
            {
                _blocks[x, y + i, z] = (BlockType)treeType;
                _blocks[x, y + height + 1, z] = (BlockType)(treeType + 18);
                for (int xL = -2; xL < 3; xL++)  // xl: the x of leaves 
                {
                    for (int zL = -2; zL < 3; zL++)
                    {
                        for (int yL = 0; yL < 4; yL++)
                        {
                            if(yL == 3 && (xL == -2 || xL == 2 || zL == -2 || zL == 2)) continue;
                            if(yL == 3)  _blocks[x + xL, y + height, z + zL] = (BlockType) (treeType + 18);
                            if(yL < 3 && xL == 0 && zL == 0) continue;
                            if (yL < 3 && (xL == -2 || xL == 2) && xL == zL && Random.value < 0.8f) continue;
                            _blocks[x + xL, y + height - 3 + yL, z + zL] = (BlockType)(treeType + 18);
                        }
                    }
                }
            }
        }
    }
    
    private void BuildChunks()
    {
        for (int id = 0; id < Math.Pow(2 * settings.viewDistance + 1, 2); id++)
        {
            int dx = (id / (2 * settings.viewDistance + 1)) * settings.length;
            int dz = (id % (2 * settings.viewDistance + 1)) * settings.length;
            Vector3 position = CHUNK.transform.position;
            GameObject chunkPre = Instantiate(CHUNK, new Vector3(position.x + dx, position.y, position.z + dz), Quaternion.identity);
            Chunk chunk = chunkPre.GetComponent<Chunk>();
            _chunks.Add(chunk);
            chunk.dx = dx;
            chunk.dz = dz;
            chunk.Blocks = _blocks;
            chunk.InitMesh();
        }
    }
}
