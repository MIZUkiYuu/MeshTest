using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public Settings settings;

    public int dx, dz;
    public BlockType?[,,] Blocks; // store all blocks in CHUNK
    private static readonly int MainTex = Shader.PropertyToID("_MainTex");

    public void InitMesh()
    {
        Mesh mesh = new Mesh();

        List<Vector2> uvs = new List<Vector2>();
        List<int> triangles = new List<int>();
        List<Vector3> vertices = new List<Vector3>();

        int totalLength = settings.length * settings.viewDistance;
        for (int x = dx; x < dx + settings.length; x++)
        {
            for (int y = 0; y < settings.height; y++)
            {
                for (int z = dz; z < dz + settings.length; z++)
                {
                    if (Blocks[x, y, z].HasValue)
                    {
                        Vector3 blockPos = new Vector3(x - dx, y, z - dz);
                        int faces = 0;
                    
                        //top
                        if(y == totalLength - 1 || !Blocks[x, y + 1, z].HasValue)
                        {
                            vertices.Add(blockPos + new Vector3(1,1,1));
                            vertices.Add(blockPos + new Vector3(0,1,1));
                            vertices.Add(blockPos + new Vector3(1,1,0));
                            vertices.Add(blockPos + new Vector3(0,1,0));
                            uvs.AddRange(BlockTile.GetUVs(settings.blockType, TileOrientation.Top));
                            faces++;
                        }
                        //down
                        if (y == 0 || !Blocks[x, y - 1, z].HasValue)
                        {
                            vertices.Add(blockPos + new Vector3(0,0,1));
                            vertices.Add(blockPos + new Vector3(1,0,1));
                            vertices.Add(blockPos + new Vector3(0,0,0));
                            vertices.Add(blockPos + new Vector3(1,0,0));
                            uvs.AddRange(BlockTile.GetUVs(settings.blockType,TileOrientation.Down));
                            faces++;
                        }
                        //front
                        if (z == totalLength - 1 || !Blocks[x, y, z + 1].HasValue)
                        {
                            vertices.Add(blockPos + new Vector3(0,1,1));
                            vertices.Add(blockPos + new Vector3(1,1,1));
                            vertices.Add(blockPos + new Vector3(0,0,1));
                            vertices.Add(blockPos + new Vector3(1,0,1));
                            uvs.AddRange(BlockTile.GetUVs(settings.blockType));
                            faces++;
                        }
                        //back
                        if (z == 0 || !Blocks[x, y, z - 1].HasValue)
                        {
                            vertices.Add(blockPos + new Vector3(1,1,0));
                            vertices.Add(blockPos + new Vector3(0,1,0));
                            vertices.Add(blockPos + new Vector3(1,0,0));
                            vertices.Add(blockPos + new Vector3(0,0,0));
                            uvs.AddRange(BlockTile.GetUVs(settings.blockType));
                            faces++;
                        }
                        //left
                        if (x == 0 || !Blocks[x - 1, y, z].HasValue)
                        {
                            vertices.Add(blockPos + new Vector3(0,1,0));
                            vertices.Add(blockPos + new Vector3(0,1,1));
                            vertices.Add(blockPos + new Vector3(0,0,0));
                            vertices.Add(blockPos + new Vector3(0,0,1));
                            uvs.AddRange(BlockTile.GetUVs(settings.blockType));
                            faces++;
                        }
                        //right
                        if (x == totalLength - 1  || !Blocks[x + 1, y, z].HasValue)
                        {
                            vertices.Add(blockPos + new Vector3(1,1,1));
                            vertices.Add(blockPos + new Vector3(1,1,0));
                            vertices.Add(blockPos + new Vector3(1,0,1));
                            vertices.Add(blockPos + new Vector3(1,0,0));
                            uvs.AddRange(BlockTile.GetUVs(settings.blockType));
                            faces++;
                        }
                        
                        // for (int i = 0; i < faces; i++)
                        // {
                        //     triangles.AddRange(new int[] {i * 4, i * 4 + 3, i * 4 + 1, i * 4, i * 4 + 2, i * 4 + 3});
                        // }
                        
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

       GetComponent<MeshRenderer>().material.SetTexture(MainTex, TextureM.Load(settings.blockType));
       GetComponent<MeshFilter>().mesh = mesh;
       GetComponent<MeshCollider>().sharedMesh = mesh;
    }
}
