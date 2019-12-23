using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshInverseUV : MonoBehaviour {

    Mesh mesh;
    float time = 0;
    bool sucess = false; 
	// Use this for initialization
	void Start ()
    {
       

    }
	
	// Update is called once per frame
	void Update ()
    {
        time += Time.deltaTime;

        if (time > 5 && !sucess)
        {
            mesh = GetComponent<MeshFilter>().mesh;
            Vector2[] uvs = mesh.uv;

            Vector3[] vertices = mesh.vertices;

            Vector3[] normals = mesh.normals;

            for (int i = 0; i < mesh.vertexCount; i++)
            {
                vertices[i] += normals[i] * 0.005f;
                //vertices[i] *= 1.1f;
                //uvs[i] *= 2.0f;
            }

            mesh.vertices = vertices;
            mesh.uv = uvs;

            int[] triangles = mesh.triangles;


            for (int i = 0; i < mesh.triangles.Length; i += 3)
            {
                int temp = triangles[i];
                triangles[i] = triangles[i + 1];
                triangles[i + 1] = temp;
            }


            mesh.triangles = triangles;
            sucess = true;
            //LogMesh();
        }
    }

    void LogMesh()
    {
        Debug.Log("Vertices : ");
        for (int i = 0; i < mesh.vertices.Length; i++)
        {
            Debug.Log(i + " - " + mesh.vertices[i]);
        }

        Debug.Log("Triangles : ");
        for (int i = 0; i < mesh.triangles.Length; i += 3)
        {
            Debug.Log(i / 3 + " - " + mesh.triangles[i] + ", "
                + mesh.triangles[i + 1] + ", " + mesh.triangles[i + 2]);
        }

        Debug.Log("UV : ");
        for (int i = 0; i < mesh.uv.Length; i++)
        {
            Debug.Log(i + " - " + mesh.uv[i]);
        }

        Debug.Log("Normals : ");
        for (int i = 0; i < mesh.normals.Length; i++)
        {
            Debug.Log(i + " - " + mesh.normals[i]);
        }
    }
}
