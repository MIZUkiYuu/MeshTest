using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ChunksGen : MonoBehaviour
{
    public GameObject CHUNK;

    private Settings _settings;
    public static BlockType[,,] Blocks; // store all blocks in all chunks
    private List<Chunk> _chunks = new List<Chunk>();
    private float _randomX, _randomZ;   // random parameter of Perlin Noise

    private void Start()
    {
        _settings = Resources.Load<Settings>("settings");
        
        Random.InitState(_settings.seed);
        _randomX = Random.value * 100;
        
        Random.InitState(Random.Range(0,100000));
        _randomZ = Random.value * 100;

        WorldGen();
        GenChunks();
    }

    private int TotalLength()
    {
        return 2 * _settings.chunkLength * _settings.viewDistance + _settings.chunkLength;
    }
    private void WorldGen()   // generate the terrain
    {
        Blocks = new BlockType[TotalLength(), _settings.chunkHeight, TotalLength()];    //store all blocks
        
        for (int x = 0; x < TotalLength(); x++)
        {
            for (int z = 0; z < TotalLength(); z++)
            {
                int y = GetYFromPerlinNoise(x, z);
                
                Blocks[x, y, z] = BlockType.GrassBlock;  // the surface

                for (int i = 0; i < _settings.chunkHeight; i++)
                {
                    if (i <= 4) Blocks[x, i, z] = BlockType.Bedrock;
                    if (4 < i && i < y) Blocks[x, i, z] = BlockType.Dirt;
                }
                Plant.Generation(x, y + 1, z);
            }
        }
    }

    //use Perlin noise to generate the value of y
    private int GetYFromPerlinNoise(int x, int z)
    {
        float xSample = (x * _settings.xScale + _randomX) / _settings.relief;
        float zSample = (z * _settings.zScale + _randomZ) / _settings.relief;
        float yNoise = Mathf.Clamp(Mathf.PerlinNoise(xSample, zSample), 0, 1);
        return (int)(yNoise * _settings.terrainHeightMax);
    }

    private void GenChunks()
    {
        for (int id = 0; id < Math.Pow(2 * _settings.viewDistance + 1, 2); id++)
        {
            int dx = (id / (2 * _settings.viewDistance + 1)) * _settings.chunkLength;
            int dz = (id % (2 * _settings.viewDistance + 1)) * _settings.chunkLength;
            Vector3 position = CHUNK.transform.position;
            GameObject chunkPre = Instantiate(CHUNK, new Vector3(position.x + dx, position.y, position.z + dz), Quaternion.identity);
            Chunk chunk = chunkPre.GetComponent<Chunk>();
            _chunks.Add(chunk);
            chunk.dx = dx;
            chunk.dz = dz;
            chunk.Blocks = Blocks;
            chunk.InitMesh();
        }
    }
}
