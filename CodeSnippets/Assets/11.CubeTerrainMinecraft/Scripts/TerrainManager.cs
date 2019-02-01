using UnityEngine;

namespace CubeTerrain
{
    public class TerrainManager : MonoBehaviour
    {
        [SerializeField] private TerrainGenerator m_Terrain;

        private void Awake()
        {
            m_Terrain = GetComponent<TerrainGenerator>();
        }

        private void Start()
        {
            m_Terrain.GenerateTerrain();
        }

    }
}
