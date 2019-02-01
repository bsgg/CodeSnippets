using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CubeTerrain
{
    public class TerrainGenerator : MonoBehaviour
    {
        public struct Point
        {
            public int x;
            public int y;
            public Point(int a_xPos, int a_yPos)
            {
                x = a_xPos;
                y = a_yPos;
            }
        };


        [SerializeField] private Material m_terrainMat;

        [SerializeField] private Transform m_terrainParent;

        /// <summary>
        /// Number chunks in x
        /// </summary>
        private int m_numberChunksX = 0;

        /// <summary>
        /// Number chunks in y
        /// </summary>
        private int m_numberChunksY = 0;

        public void GenerateTerrain()
        {
            m_numberChunksX = Random.Range(5, 10);
            m_numberChunksY = Random.Range(5, 10);

            // Generate chunks
            for (int x = 0; x < m_numberChunksX; x++)
            {
                for (int y = 0; y < m_numberChunksY; y++)
                {
                    CreateBoxChunk(new Point(x, y));
                }
            }
        }



        /// <summary>
        /// Creates the chunk box in the given point
        /// </summary>
        /// <param name="a_point">Point.</param>
        private void CreateBoxChunk(Point a_point)
        {
            string chunkName = "CubeChunk_" + a_point.x + "_" + a_point.y;

            GameObject newChunkGameObject = new GameObject(chunkName);

            CubeChunk newChunk = newChunkGameObject.AddComponent<CubeChunk>();

            newChunk.PositionChunk = new Vector2(a_point.x, a_point.y);
            newChunk.MaterialChunk = m_terrainMat;
            newChunk.transform.parent = m_terrainParent;
        }
    }
}
