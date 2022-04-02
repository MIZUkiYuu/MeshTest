using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GroundGen : MonoBehaviour
{
    public GameObject block;
    public BlockType blockType;
    [Range(1,64)]public int range = 64;
    
    private Mesh _mesh;

    private static readonly int MainTex = Shader.PropertyToID("_MainTex");
    
    private void Start()
    {
        InitMesh();
        block.GetComponent<MeshRenderer>().material.SetTexture(MainTex, AssetDatabase.LoadAssetAtPath<Texture>(BlockTile.BlockTilePath[blockType]));
    }
    
    private void InitMesh()
    {
        _mesh = block.GetComponent<MeshFilter>().mesh;

        // BlockType[,,] blocks; 

        List<Vector2> uvs = new List<Vector2>();
        List<int> triangles = new List<int>();
        List<Vector3> vertices = new List<Vector3>();

        for (int x = 0; x < range; x++)
        {
            for (int z = 0; z < range; z++)
            {
                Vector3 blockPos = new Vector3(x, 1, z);
                int faces = 0;
                
                //top
                // if (blocks[x, 1, z] == BlockType.Air)
                {
                    vertices.Add(blockPos + new Vector3(1,1,1));
                    vertices.Add(blockPos + new Vector3(0,1,1));
                    vertices.Add(blockPos + new Vector3(1,1,0));
                    vertices.Add(blockPos + new Vector3(0,1,0));
                    uvs.AddRange(BlockTile.GetUVs(blockType, TileOrientation.Top));
                    faces++;
                }
                //down
                // if (blocks[x, 0, z] == BlockType.Air)
                {
                    vertices.Add(blockPos + new Vector3(0,0,1));
                    vertices.Add(blockPos + new Vector3(1,0,1));
                    vertices.Add(blockPos + new Vector3(0,0,0));
                    vertices.Add(blockPos + new Vector3(1,0,0));
                    uvs.AddRange(BlockTile.GetUVs(blockType,TileOrientation.Down));
                    faces++;
                }
                //front
                // if (blocks[x, 0, z + 1] == BlockType.Air)
                {
                    vertices.Add(blockPos + new Vector3(1,1,0));
                    vertices.Add(blockPos + new Vector3(0,1,0));
                    vertices.Add(blockPos + new Vector3(1,0,0));
                    vertices.Add(blockPos + new Vector3(0,0,0));
                    uvs.AddRange(BlockTile.GetUVs());
                    faces++;
                }
                //back
                // if (blocks[x, 0, z - 1] == BlockType.Air)
                {
                    vertices.Add(blockPos + new Vector3(0,1,1));
                    vertices.Add(blockPos + new Vector3(1,1,1));
                    vertices.Add(blockPos + new Vector3(0,0,1));
                    vertices.Add(blockPos + new Vector3(1,0,1));
                    uvs.AddRange(BlockTile.GetUVs());
                    faces++;
                }
                //left
                // if (blocks[x - 1, 0, z] == BlockType.Air)
                {
                    vertices.Add(blockPos + new Vector3(0,1,0));
                    vertices.Add(blockPos + new Vector3(0,1,1));
                    vertices.Add(blockPos + new Vector3(0,0,0));
                    vertices.Add(blockPos + new Vector3(0,0,1));
                    uvs.AddRange(BlockTile.GetUVs());
                    faces++;
                }
                //right
                // if (blocks[x + 1, 0, z] == BlockType.Air)
                {
                    vertices.Add(blockPos + new Vector3(1,1,1));
                    vertices.Add(blockPos + new Vector3(1,1,0));
                    vertices.Add(blockPos + new Vector3(1,0,1));
                    vertices.Add(blockPos + new Vector3(1,0,0));
                    uvs.AddRange(BlockTile.GetUVs());
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
        _mesh.vertices = vertices.ToArray();
        _mesh.triangles = triangles.ToArray();
        _mesh.uv = uvs.ToArray();
        _mesh.RecalculateNormals();

        block.GetComponent<MeshFilter>().mesh = _mesh;
        block.GetComponent<MeshCollider>().sharedMesh = _mesh;
    }
}
