using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public Settings settings;

    public int dx, dz;
    public BlockType[,,] Blocks; // All blocks in loaded chunks : GroundGen.cs -> chunk.Blocks = _blocks;

    public void InitMesh()
    {
        Mesh mesh = new Mesh();

        List<Vector2> uvs = new List<Vector2>();
        List<int> triangles = new List<int>();
        List<Vector3> vertices = new List<Vector3>();

        int totalLength = 2 * settings.length * settings.viewDistance + settings.length;
        for (int x = dx; x < dx + settings.length; x++)
        {
            for (int y = 0; y < settings.height; y++)
            {
                for (int z = dz; z < dz + settings.length; z++)
                {
                    if (Blocks[x, y, z] != BlockType.Air)
                    {
                        Vector3 blockPos = new Vector3(x - dx, y, z - dz);
                        int faces = 0;
                    
                        //top
                        if(y == totalLength - 1 || Blocks[x, y + 1, z] == BlockType.Air)
                        {
                            vertices.Add(blockPos + new Vector3(1,1,1));
                            vertices.Add(blockPos + new Vector3(0,1,1));
                            vertices.Add(blockPos + new Vector3(1,1,0));
                            vertices.Add(blockPos + new Vector3(0,1,0));
                            uvs.AddRange(BlockMesh.BlockTilePos[Blocks[x,y,z]].GetUVs(TileType.CubeTop));
                            faces++;
                        }
                        //down
                        if (y == 0 || Blocks[x, y - 1, z] == BlockType.Air)
                        {
                            vertices.Add(blockPos + new Vector3(0,0,1));
                            vertices.Add(blockPos + new Vector3(1,0,1));
                            vertices.Add(blockPos + new Vector3(0,0,0));
                            vertices.Add(blockPos + new Vector3(1,0,0));
                            uvs.AddRange(BlockMesh.BlockTilePos[Blocks[x,y,z]].GetUVs(TileType.CubeDown));
                            faces++;
                        }
                        //front
                        if (z == totalLength - 1 || Blocks[x, y, z + 1] == BlockType.Air)
                        {
                            vertices.Add(blockPos + new Vector3(0,1,1));
                            vertices.Add(blockPos + new Vector3(1,1,1));
                            vertices.Add(blockPos + new Vector3(0,0,1));
                            vertices.Add(blockPos + new Vector3(1,0,1));
                            uvs.AddRange(BlockMesh.BlockTilePos[Blocks[x,y,z]].GetUVs(TileType.CubeSide));
                            faces++;
                        }
                        //back
                        if (z == 0 || Blocks[x, y, z - 1] == BlockType.Air)
                        {
                            vertices.Add(blockPos + new Vector3(1,1,0));
                            vertices.Add(blockPos + new Vector3(0,1,0));
                            vertices.Add(blockPos + new Vector3(1,0,0));
                            vertices.Add(blockPos + new Vector3(0,0,0));
                            uvs.AddRange(BlockMesh.BlockTilePos[Blocks[x,y,z]].GetUVs(TileType.CubeSide));
                            faces++;
                        }
                        //left
                        if (x == 0 || Blocks[x - 1, y, z] == BlockType.Air)
                        {
                            vertices.Add(blockPos + new Vector3(0,1,0));
                            vertices.Add(blockPos + new Vector3(0,1,1));
                            vertices.Add(blockPos + new Vector3(0,0,0));
                            vertices.Add(blockPos + new Vector3(0,0,1));
                            uvs.AddRange(BlockMesh.BlockTilePos[Blocks[x,y,z]].GetUVs(TileType.CubeSide));
                            faces++;
                        }
                        //right
                        if (x == totalLength - 1  || Blocks[x + 1, y, z] == BlockType.Air)
                        {
                            vertices.Add(blockPos + new Vector3(1,1,1));
                            vertices.Add(blockPos + new Vector3(1,1,0));
                            vertices.Add(blockPos + new Vector3(1,0,1));
                            vertices.Add(blockPos + new Vector3(1,0,0));
                            uvs.AddRange(BlockMesh.BlockTilePos[Blocks[x,y,z]].GetUVs(TileType.CubeSide));
                            faces++;
                        }

                        int tl = vertices.Count - 4 * faces;
                        for (int i = 0; i < faces; i++)
                        {
                            triangles.AddRange(new int[] { tl + i * 4, tl + i * 4 + 3, tl + i * 4 + 1, tl + i * 4, tl + i * 4 + 2, tl + i * 4 + 3 });
                        }
                    }
                }
            }
        }
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.RecalculateNormals();

       GetComponent<MeshFilter>().mesh = mesh;
       GetComponent<MeshCollider>().sharedMesh = mesh;
    }
}
