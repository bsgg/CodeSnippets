using UnityEngine;
using System.Collections;

namespace MinecraftTerrain
{
    public class MinecraftTerrainManager : MonoBehaviour
    {
        [SerializeField]  private TerrainGenerator m_Terrain;

        private void Awake()
        {
            m_Terrain = GetComponent<TerrainGenerator>();
        }

        private void Start()
        {
            m_Terrain.GenerateTerrain();
        }

        
        public void ResetTerrain()
        {
            m_Terrain.ResetTerrain();
        }
    }
}
