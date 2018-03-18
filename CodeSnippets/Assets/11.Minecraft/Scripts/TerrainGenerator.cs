using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MinecraftTerrain
{
    public class TerrainGenerator : MonoBehaviour
    {
        /// <summary>
        /// Point struct
        /// </summary>
        public struct Point
        {
            public int x;
            public int y;
            public Point(int xPos, int yPos)
            {
                this.x = xPos;
                this.y = yPos;
            }
        };

        /// <summary>
        /// Material for the all terrain
        /// </summary>
        public Material TerrainMaterial;

        /// <summary>
        /// Root object
        /// </summary>
        public Transform RootTerrain;

        /// <summary>
        /// Number chunks in x
        /// </summary>
        public int NumberChunksX = 10;

        /// <summary>
        /// Number chunks in y
        /// </summary>
        public int NumberChunksY = 10;

        public void GenerateTerrain()
        {
            NumberChunksX = Random.Range(5, 10);
            NumberChunksY = Random.Range(5, 10);

            // Generate chunks
            for (int x = 0; x < NumberChunksX; x++)
            {
                for (int y = 0; y < NumberChunksY; y++)
                {
                    createBoxChunk(new Point(x, y));
                }
            }
        }

        public void ResetTerrain()
        {
            for (int i = 0; i < this.RootTerrain.childCount; i++)
            {
                Destroy(this.RootTerrain.transform.GetChild(i).gameObject);
            }
            GenerateTerrain();

        }

        /// <summary>
        /// Creates the box chunk in the given position
        /// </summary>
        /// <param name="point">Point.</param>
        private void createBoxChunk(Point point)
        {
            string nameChunk = "CubeChunk_" + point.x + "_" + point.y;
            CubeChunk chunk = new GameObject(nameChunk).AddComponent<CubeChunk>();
            chunk.PositionChunk = new Vector2(point.x, point.y);
            chunk.MaterialChunk = this.TerrainMaterial;
            chunk.transform.parent = this.RootTerrain;
        }
    }
}
