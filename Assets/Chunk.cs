using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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
                        int faces = 0;
                        // grass / flower
                        if (BlockFromMesh(x, y, z).CrossTileType)
                        {
                            vertices.AddRange(GetCrossVertices(x, y, z));
                            uvs.AddRange(GetUVs(x, y, z));
                            uvs.AddRange(GetUVs(x, y, z));
                            faces += 2;
                            triangles.AddRange(GetTriangles(faces, vertices.Count - 4 * faces));
                            continue;
                        }

                        //top
                        if (y == settings.height - 1 || !BlockFromMesh(x, y + 1, z).OpaqueDown)
                        {
                            vertices.AddRange(GetVertices(x, y, z, Direction.Top));
                            uvs.AddRange(GetUVs(x, y, z, TileType.CubeTop));
                            faces++;
                        }

                        //down
                        if (y == 0 || !BlockFromMesh(x, y - 1, z).OpaqueTop)
                        {
                            vertices.AddRange(GetVertices(x, y, z, Direction.Down));
                            uvs.AddRange(GetUVs(x, y, z, TileType.CubeDown));
                            faces++;
                        }

                        //front
                        if (z == totalLength - 1 || !BlockFromMesh(x, y, z + 1).OpaqueSide)
                        {
                            vertices.AddRange(GetVertices(x, y, z, Direction.Front));
                            uvs.AddRange(GetUVs(x, y, z));
                            faces++;
                        }

                        //back
                        if (z == 0 || !BlockFromMesh(x, y, z - 1).OpaqueSide)
                        {
                            vertices.AddRange(GetVertices(x, y, z, Direction.Back));
                            uvs.AddRange(GetUVs(x, y, z));
                            faces++;
                        }

                        //right
                        if (x == totalLength - 1 || !BlockFromMesh(x + 1, y, z).OpaqueSide)
                        {
                            vertices.AddRange(GetVertices(x, y, z, Direction.Right));
                            uvs.AddRange(GetUVs(x, y, z));
                            faces++;
                        }

                        //left
                        if (x == 0 || !BlockFromMesh(x - 1, y, z).OpaqueSide)
                        {
                            vertices.AddRange(GetVertices(x, y, z, Direction.Left));
                            uvs.AddRange(GetUVs(x, y, z));
                            faces++;
                        }

                        triangles.AddRange(GetTriangles(faces, vertices.Count - 4 * faces));
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

    private BlockMesh BlockFromMesh(int x, int y, int z)
    {
        return BlockMesh.BlockTilePos[Blocks[x, y, z]];
    }
    
    private Vector3[] GetVertices(int x, int y, int z, Direction direction)
    {
        return BlockFromMesh(x, y, z).CubeVertices(new Vector3(x - dx, y, z - dz), direction);
    }
    
    private Vector3[] GetCrossVertices(int x, int y, int z)
    {
        return BlockFromMesh(x, y, z)
            .CrossVertices(new Vector3(x - dx + Random.Range(-0.3f, 0.3f), y, z - dz + Random.Range(-0.3f, 0.3f)));
    }
    
    private Vector2[] GetUVs(int x, int y, int z, TileType tileType = TileType.CubeSide)
    {
        return BlockFromMesh(x, y, z).UVs(tileType);
    }

    private int[] GetTriangles(int i, int nums)
    {
        return BlockMesh.Triangles(i, nums);
    }

}
