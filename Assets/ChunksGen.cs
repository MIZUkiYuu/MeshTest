using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ChunksGen : MonoBehaviour
{
    public GameObject CHUNK;
    public static BlockType[,,] Blocks; // store all blocks in all chunks
    public static List<int> SurfaceYPos = new();
    public static int SurfaceYMin = 64;
    public static int SurfaceYMax = 0;
    public Transform chunksList;
    
    private Settings _settings;
    [SerializeField]private List<SolidBlocksMesh> chunks = new();
    private float _randomX, _randomZ;   // random parameter of Perlin Noise
    private float _relief;

    private void Start() {
        _settings = Resources.Load<Settings>("settings");
        
        Random.InitState(_settings.seed);
        _randomX = Random.value * 100;
        
        Random.InitState(Random.Range(0,100000));
        _randomZ = Random.value * 100;

        _relief = _settings.relief;

        WorldGen();
        GenChunks();
    }

    private int TotalLength() {
        return 2 * _settings.chunkLength * _settings.viewDistance + _settings.chunkLength;
    }
    
    private void WorldGen() {   // generate the terrain                                                         
        Blocks = new BlockType[TotalLength(), _settings.chunkHeight, TotalLength()]; //store all blocks
        
        for (int x = 0; x < TotalLength(); x++) {
            for (int z = 0; z < TotalLength(); z++) {
                int y = GetYFromPerlinNoise(x, z) < 5 ? 5 : GetYFromPerlinNoise(x, z);
                
                Blocks[x, y, z] = BlockType.GrassBlock;  // the surface

                SurfaceYPos.Add(y);
                SurfaceYMin = y < SurfaceYMin ? y : SurfaceYMin;
                SurfaceYMax = y > SurfaceYMax ? y : SurfaceYMax;
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
    private int GetYFromPerlinNoise(int x, int z) {
        float xSample = (x * _settings.xScale + _randomX) / _relief;
        float zSample = (z * _settings.zScale + _randomZ) / _relief;
        float yNoise = Math.Clamp(Mathf.PerlinNoise(xSample, zSample), 0, 1);
        yNoise = Astroid(yNoise);
        return (int)(yNoise * _settings.terrainHeightMax);
    }

    private void GenChunks() {
        for (int id = 0; id < Math.Pow(2 * _settings.viewDistance + 1, 2); id++) {
            int numX = (id / (2 * _settings.viewDistance + 1));
            int numZ = (id % (2 * _settings.viewDistance + 1));
            
            Vector3Int position = Vector3Int.zero;
            Vector3 chunkPos = CHUNK.transform.position;
            position.x = (int)chunkPos.x;
            position.y = (int)chunkPos.y;
            position.z = (int)chunkPos.z;
            
            GameObject chunkPre = Instantiate(CHUNK, new Vector3Int(position.x + numX * _settings.chunkLength, 
            position.y, position.z + numZ * _settings.chunkLength), Quaternion.identity, chunksList);
            chunkPre.name = $"Chunk({numX}, {numZ})";   // rename chunk -> Chunk(0, 0)
            SolidBlocksMesh solidBlocksMesh = chunkPre.GetComponentInChildren<SolidBlocksMesh>();
            
            chunks.Add(solidBlocksMesh);    //add chunk to chunk list
            //generate chunk
            solidBlocksMesh.dx = numX * _settings.chunkLength;
            solidBlocksMesh.dz = numZ * _settings.chunkLength;
            solidBlocksMesh.Blocks = Blocks;
            solidBlocksMesh.RefreshMesh();
        }
    }

    private static float Astroid(float x) {
        return Mathf.Pow(1 - Mathf.Pow(1 - x, 1.0f / 2), 2);    //y^(1/2) + x^(1/2) = 1 
    }
}
