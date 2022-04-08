using System;
using System.Collections.Generic;
using UnityEngine;
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
    private void TerrainGen()   // generate the terrain
    {
        int totalLength = 2 * settings.length * settings.viewDistance + settings.length;
        _blocks = new BlockType[totalLength, settings.height, totalLength];    //store all blocks
        
        for (int x = 0; x < totalLength; x++)
        {
            for (int z = 0; z < totalLength; z++)
            {
                int y = GetY(x, z);
                _blocks[x, y, z] = settings.blockType;
                for (int i = 0; i < y; i++)
                {
                    _blocks[x, i, z] = BlockType.Dirt;
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
