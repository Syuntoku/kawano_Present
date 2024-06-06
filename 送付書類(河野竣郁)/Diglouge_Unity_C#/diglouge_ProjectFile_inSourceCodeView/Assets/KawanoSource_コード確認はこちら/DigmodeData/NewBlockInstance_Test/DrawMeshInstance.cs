using Battle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Syuntoku.Test
{
    /// <summary>
    /// •`‰æ‚ÌƒRƒXƒg‚ª‚‚¢‚½‚ßDrawMesh‚Å’S“–‚·‚é
    /// </summary>
    public class DrawMeshInstance : MonoBehaviour
    {

        [SerializeField] Mesh mesh;
        [SerializeField] Material material;
            Vector3[] pos = { new Vector3(0, 0, 0), new Vector3(1, 0, 0), new Vector3(2, 0, 0) };

        private void Start()
        {
            for (int i = 0; i < pos.Length; i++)
            {
                GameObject gameObject = new GameObject("test");
                gameObject.transform.position = pos[i];
            }
        }

        private void Update()
        {
            var matrix = new Matrix4x4[3];

            matrix[0] = Matrix4x4.TRS(pos[0], Quaternion.identity, Vector3.one);
            matrix[1] = Matrix4x4.TRS(pos[1], Quaternion.identity, Vector3.one);
            matrix[2] = Matrix4x4.TRS(pos[2], Quaternion.identity, Vector3.one);
            Graphics.DrawMeshInstanced(mesh, 0, material, matrix);

            if(Input.GetKey(KeyCode.W))
            {
                pos[0] += Vector3.up * 0.1f;
            }
        }

    }

}