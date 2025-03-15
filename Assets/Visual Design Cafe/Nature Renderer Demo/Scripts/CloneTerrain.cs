using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NatureRendererDemo
{
    public class CloneTerrain : MonoBehaviour
    {
        public Terrain Terrain;

        private void Awake()
        {
            CloneTerrainData();
        }

        private void CloneTerrainData()
        {
            var copy = TerrainData.Instantiate( Terrain.terrainData );
            Terrain.terrainData = copy;
            Terrain.GetComponent<TerrainCollider>().terrainData = copy;
        }
    }
}
