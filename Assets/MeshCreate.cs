using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MeshCreate : MonoBehaviour
{
    private MeshFilter _meshFilter;
    private Mesh _mesh;
    private MeshRenderer _renderer;
    private Texture _texture;

    private static readonly int MainTex = Shader.PropertyToID("_MainTex");

    private void Start()
    {
        _renderer = gameObject.AddComponent<MeshRenderer>();
        _meshFilter = gameObject.AddComponent<MeshFilter>();
        
        _mesh = new Mesh();
        _meshFilter.mesh = _mesh;
        _renderer.material.SetTexture(MainTex, AssetDatabase.LoadAssetAtPath<Texture>("Assets/grass_block.png"));
    
        InitMesh();
    }

    private void InitMesh()
    {
        List<Vector3> vertices = new List<Vector3>()
        {
            //top
            new Vector3(1,1,1),
            new Vector3(0,1,1),
            new Vector3(1,1,0),
            new Vector3(0,1,0),
            
            //down
            new Vector3(0,0,1),
            new Vector3(1,0,1),
            new Vector3(0,0,0),
            new Vector3(1,0,0),
            
            //front
            new Vector3(1, 1, 0),
            new Vector3(0, 1, 0),
            new Vector3(1, 0, 0),
            new Vector3(0, 0, 0),
            
            //back
            new Vector3(0,1,1),
            new Vector3(1,1,1),
            new Vector3(0,0,1),
            new Vector3(1,0,1),
            
            //left
            new Vector3(0, 1, 0),
            new Vector3(0, 1, 1),
            new Vector3(0, 0, 0),
            new Vector3(0, 0, 1),
            
            //right
            new Vector3(1, 1, 1),
            new Vector3(1, 1, 0),
            new Vector3(1, 0, 1),
            new Vector3(1, 0, 0),
        };

        
        //set the triangles of mesh
        int[] triangles = new int[vertices.Count / 4 * 6];
        for (int i = 0; i < vertices.Count / 4; i++)
        {
            triangles[i * 6] = i * 4;
            triangles[i * 6 + 1] = i * 4 + 3;
            triangles[i * 6 + 2] = i * 4 + 1;
            triangles[i * 6 + 3] = i * 4;
            triangles[i * 6 + 4] = i * 4 + 2;
            triangles[i * 6 + 5] = i * 4 + 3;
        }
        // //top
            // 0,3,1,0,2,3,
            // //front
            // 4,7,5,4,6,7,
            // //left
            // 8,11,9,8,10,11,
            // //right
            // 12,15,13,12,14,15


        //set the uv of mesh
        Vector2[] uvs = new Vector2[vertices.Count];
        const float t = 1 / 2f;
        uvs[0] = new Vector2(t, 1.0f);
        uvs[1] = new Vector2(0, 1.0f);
        uvs[2] = new Vector2(t, t);
        uvs[3] = new Vector2(0, t);
        for (int i = 4; i < uvs.Length; i+=4)
        {
            uvs[i] = new Vector2(t, t);
            uvs[i + 1] = new Vector2(0, t);
            uvs[i + 2] = new Vector2(t, 0);
            uvs[i + 3] = new Vector2(0, 0);
        }

        _mesh.vertices = vertices.ToArray();
        _mesh.triangles = triangles;
        _mesh.uv = uvs;
        _mesh.RecalculateNormals();
    }
}
