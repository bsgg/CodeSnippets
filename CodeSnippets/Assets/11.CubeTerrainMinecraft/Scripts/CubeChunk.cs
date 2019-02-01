using UnityEngine;
using System.Collections.Generic;

namespace CubeTerrain
{
    public class CubeChunk : MonoBehaviour
    {
        // Widht, lenght for the chuncs
        public const int Width = 16;
        public const int Length = 16;

        // The height for each chunk
        public const int Height = 64;

        private byte[] m_cubeChunkInfo;

        private Mesh m_meshChunk;

        private Material m_materialChunk;
        public Material MaterialChunk
        {
            set { m_materialChunk = value; }
            get { return m_materialChunk; }
        }

        private Vector2 m_positionChunk = Vector2.zero;
        public Vector2 PositionChunk
        {
            set { m_positionChunk = value; }
            get { return m_positionChunk; }
        }

        void Start()
        {
            InitializeChunkdData();
            SetupBoxChunk();

            // Add mesh renderer and mesh colider
            // MeshFilter allows to generate the mesh of the game object in procedural way
            gameObject.AddComponent<MeshFilter>().mesh = m_meshChunk;
            gameObject.AddComponent<MeshRenderer>().material = m_materialChunk;
            gameObject.AddComponent<MeshCollider>().sharedMesh = m_meshChunk;
            transform.position = new Vector3(m_positionChunk.x * Width, 0, m_positionChunk.y * Length);
        }


        private void InitializeChunkdData()
        {
            m_cubeChunkInfo = new byte[Width * Length * Height];

            // Generate the cube chunk, sets the uv and pseud-random perlin noise for the heigh
            for (int x = 0; x < Width; ++x)
            {
                for (int z = 0; z < Length; ++z)
                {
                    // Generate heights pseudo-random through perlin noise
                    float heightPerlin = Mathf.PerlinNoise(m_positionChunk.x + x / 16.0f, m_positionChunk.y + z / 16.0f) * 16;

                    for (int y = 0; y < Height; ++y)
                    {
                        // Gets coord from 3D space to 1D space
                        int index = x + y * (Width * Length) + z * Length;

                        byte value = (byte)0;

                        // Check heigh, every height under semi random heightPerlin is drawn
                        if (y < heightPerlin)
                        {
                            value = (byte)2;
                        }

                        m_cubeChunkInfo[index] = value;
                    }
                }
            }
        }


        private byte GetInfo(int a_x, int a_y, int a_z)
        {
            // Check boundaries
            if
            ((a_x < 0) || (a_x >= Width) ||
               (a_y < 0) || (a_y >= Height) ||
               (a_z < 0) || (a_z >= Length)
            )
            {
                return 0;
            }

            // Returns chunk info
            int index = a_x + a_y * (Width * Length) + a_z * Length;
            return m_cubeChunkInfo[index];
        }

        private void AddUVs(List<Vector2> listUvs, byte currentInfo)
        {
            int xCenter = currentInfo % 16;
            int yCenter = currentInfo / 16;
            float size = 1.0f / 16.0f;

            float min = size * 0.5f;
            float max = 1.0f - min;

            listUvs.Add(new Vector2((xCenter + max) * size, 1.0f - (yCenter + max) * size));
            listUvs.Add(new Vector2((xCenter + min) * size, 1.0f - (yCenter + max) * size));
            listUvs.Add(new Vector2((xCenter + min) * size, 1.0f - (yCenter + min) * size));
            listUvs.Add(new Vector2((xCenter + max) * size, 1.0f - (yCenter + min) * size));
        }


        private void UpdateIndices(List<int> lIndex, int id)
        {
            if (lIndex != null)
            {
                lIndex.Add(id);
                lIndex.Add(id + 1);
                lIndex.Add(id + 3);
                lIndex.Add(id + 2);
                lIndex.Add(id + 3);
                lIndex.Add(id + 1);
            }
        }

        /// <summary>
        /// Sets the visual elements of the cube chunk
        /// </summary>
        private void SetupBoxChunk()
        {
            // List of vertex
            List<Vector3> listVertices = new List<Vector3>();
            // List uvs
            List<Vector2> listUvs = new List<Vector2>();
            // List index of the chunk
            List<int> listIndex = new List<int>();

            // Mesh
            m_meshChunk = new Mesh();

            int auxIndex = 0;
            for (int y = 0; y < Height; ++y)
            {
                for (int x = 0; x < Width; ++x)
                {
                    for (int z = 0; z < Length; ++z)
                    {
                        // Gets current info
                        byte currentInfo = GetInfo(x, y, z);
                        if (currentInfo != 0)
                        {
                            // Generate left face
                            if (GetInfo(x - 1, y, z) == 0)
                            {
                                listVertices.Add(new Vector3(x - 0.5f, y + 0.0f, z - 0.5f));
                                listVertices.Add(new Vector3(x - 0.5f, y + 0.0f, z + 0.5f));
                                listVertices.Add(new Vector3(x - 0.5f, y + 1.0f, z + 0.5f));
                                listVertices.Add(new Vector3(x - 0.5f, y + 1.0f, z - 0.5f));

                                // Generate the uvs
                                AddUVs(listUvs, currentInfo);
                                // index for triangles
                                UpdateIndices(listIndex, auxIndex);
                                auxIndex += 4;
                            }
                            // Rigth face
                            if (GetInfo(x + 1, y, z) == 0)
                            {
                                listVertices.Add(new Vector3(x + 0.5f, y + 0.0f, z + 0.5f));
                                listVertices.Add(new Vector3(x + 0.5f, y + 0.0f, z - 0.5f));
                                listVertices.Add(new Vector3(x + 0.5f, y + 1.0f, z - 0.5f));
                                listVertices.Add(new Vector3(x + 0.5f, y + 1.0f, z + 0.5f));

                                // Generate the uvs
                                AddUVs(listUvs, currentInfo);
                                // index for triangles
                                UpdateIndices(listIndex, auxIndex);
                                auxIndex += 4;
                            }

                            // Top face
                            if (GetInfo(x, y - 1, z) == 0)
                            {
                                listVertices.Add(new Vector3(x - 0.5f, y + 0.0f, z - 0.5f));
                                listVertices.Add(new Vector3(x + 0.5f, y + 0.0f, z - 0.5f));
                                listVertices.Add(new Vector3(x + 0.5f, y + 0.0f, z + 0.5f));
                                listVertices.Add(new Vector3(x - 0.5f, y + 0.0f, z + 0.5f));
                                // Generate the uvs
                                AddUVs(listUvs, currentInfo);
                                // index for triangles
                                UpdateIndices(listIndex, auxIndex);
                                auxIndex += 4;
                            }

                            // Bottom face
                            if (GetInfo(x, y + 1, z) == 0)
                            {
                                listVertices.Add(new Vector3(x + 0.5f, y + 1.0f, z - 0.5f));
                                listVertices.Add(new Vector3(x - 0.5f, y + 1.0f, z - 0.5f));
                                listVertices.Add(new Vector3(x - 0.5f, y + 1.0f, z + 0.5f));
                                listVertices.Add(new Vector3(x + 0.5f, y + 1.0f, z + 0.5f));
                                // Generate the uvs
                                AddUVs(listUvs, currentInfo);
                                // index for triangles
                                UpdateIndices(listIndex, auxIndex);
                                auxIndex += 4;
                            }

                            // Front face
                            if (GetInfo(x, y, z - 1) == 0)
                            {
                                listVertices.Add(new Vector3(x + 0.5f, y + 0.0f, z - 0.5f));
                                listVertices.Add(new Vector3(x - 0.5f, y + 0.0f, z - 0.5f));
                                listVertices.Add(new Vector3(x - 0.5f, y + 1.0f, z - 0.5f));
                                listVertices.Add(new Vector3(x + 0.5f, y + 1.0f, z - 0.5f));
                                // Generate the uvs
                                AddUVs(listUvs, currentInfo);
                                // index for triangles
                                UpdateIndices(listIndex, auxIndex);
                                auxIndex += 4;
                            }

                            // Back face
                            if (GetInfo(x, y, z + 1) == 0)
                            {
                                listVertices.Add(new Vector3(x - 0.5f, y + 0.0f, z + 0.5f));
                                listVertices.Add(new Vector3(x + 0.5f, y + 0.0f, z + 0.5f));
                                listVertices.Add(new Vector3(x + 0.5f, y + 1.0f, z + 0.5f));
                                listVertices.Add(new Vector3(x - 0.5f, y + 1.0f, z + 0.5f));
                                // Generate the uvs
                                AddUVs(listUvs, currentInfo);
                                // index for triangles
                                UpdateIndices(listIndex, auxIndex);
                                auxIndex += 4;
                            }
                        }
                    }
                }
            }

            // Assign uv, vertices, triangles and normals
            m_meshChunk.vertices = listVertices.ToArray();
            m_meshChunk.uv = listUvs.ToArray();
            m_meshChunk.triangles = listIndex.ToArray();
            m_meshChunk.RecalculateNormals();
        }

    }
}
