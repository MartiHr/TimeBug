namespace NatureRendererDemo
{
    using UnityEngine;
    using UnityEngine.Rendering;

    [ExecuteInEditMode]
    public class RenderPipelineLighting : MonoBehaviour
    {
        [Header( "Input" )]
        [SerializeField]
        private GameObject _legacyInputSystem;

        [SerializeField]
        private GameObject _newInputSystem;

        [Header( "Standard" )]
        [SerializeField]
        private GameObject _standardLighting;

        [SerializeField]
        private Material _standardSky;

        [SerializeField]
        private Material _standardTerrain;

        [SerializeField]
        private GameObject _standardVolume;

        [SerializeField]
        private Material _standardLineMaterial;

        [SerializeField]
        private Material _standardCameraIconMaterial;

        [SerializeField]
        private GameObject _standardPaintDecal;

        [Header( "Universal" )]
        [SerializeField]
        private GameObject _universalLighting;

        [SerializeField]
        private Material _universalSky;

        [SerializeField]
        private Material _universalTerrain;

        [SerializeField]
        private GameObject _universalVolume;

        [SerializeField]
        private Material _universalLineMaterial;

        [SerializeField]
        private Material _universalCameraIconMaterial;

        [SerializeField]
        private GameObject _universalPaintDecal;

        [Header( "HD" )]
        [SerializeField]
        private GameObject _highDefinitionLighting;

        [SerializeField]
        private Material _highDefinitionSky;

        [SerializeField]
        private GameObject _highDefinitionVolume;

        [SerializeField]
        private Material _highDefinitionTerrain;

        [SerializeField]
        private Material _highDefinitionLineMaterial;

        [SerializeField]
        private Material _highDefinitionCameraIconMaterial;

        [SerializeField]
        private GameObject _highDefinitionPaintDecal;

        private void OnValidate()
        {
            Awake();
        }

        private void Awake()
        {
#if UNITY_EDITOR

            if( Application.isPlaying )
            {
#if ENABLE_INPUT_SYSTEM
                _legacyInputSystem.SetActive( false );
                _newInputSystem.SetActive( true );
#else
                _legacyInputSystem.SetActive( true );
                _newInputSystem.SetActive( false );
#endif
            }

            var renderPipeline = QualitySettings.renderPipeline;
            if(renderPipeline == null)
                renderPipeline = GraphicsSettings.defaultRenderPipeline;

            var renderPipelineName = renderPipeline?.GetType().Name ?? "";

            if( _standardVolume != null )
                _standardVolume.SetActive( renderPipelineName == "" );

            if( _universalVolume != null )
                _universalVolume.SetActive( renderPipelineName == "UniversalRenderPipelineAsset" );

            if( _highDefinitionVolume != null )
                _highDefinitionVolume.SetActive( renderPipelineName == "HDRenderPipelineAsset" );

            if( _standardLighting != null )
                _standardLighting.SetActive( renderPipelineName == "" );

            if( _universalLighting != null )
                _universalLighting.SetActive( renderPipelineName == "UniversalRenderPipelineAsset" );

            if( _highDefinitionLighting != null )
                _highDefinitionLighting.SetActive( renderPipelineName == "HDRenderPipelineAsset" );

            if( _standardPaintDecal != null )
                _standardPaintDecal.SetActive( renderPipelineName == "" );

            if( _universalPaintDecal != null )
                _universalPaintDecal.SetActive( renderPipelineName == "UniversalRenderPipelineAsset" );

            if( _highDefinitionPaintDecal != null )
                _highDefinitionPaintDecal.SetActive( renderPipelineName == "HDRenderPipelineAsset" );

            switch( renderPipelineName )
            {
                case "":
                    RenderSettings.skybox = _standardSky;
                    SetTerrainMaterial( _standardTerrain );
                    SetLineMaterial( _standardLineMaterial );
                    SetCameraIconMaterial( _standardCameraIconMaterial );
                    break;
                case "UniversalRenderPipelineAsset":
                    RenderSettings.skybox = _universalSky;
                    SetTerrainMaterial( _universalTerrain );
                    SetLineMaterial( _universalLineMaterial );
                    SetCameraIconMaterial( _universalCameraIconMaterial );
                    break;
                case "HDRenderPipelineAsset":
                    RenderSettings.skybox = _highDefinitionSky;
                    SetTerrainMaterial( _highDefinitionTerrain );
                    SetLineMaterial( _highDefinitionLineMaterial );
                    SetCameraIconMaterial( _highDefinitionCameraIconMaterial );
                    break;
            }
#endif
        }

#if UNITY_EDITOR
        private void SetTerrainMaterial( Material material )
        {
            foreach( var terrain in FindObjectsByType<Terrain>(FindObjectsInactive.Include, FindObjectsSortMode.None) )
                terrain.materialTemplate = material;
        }

        private void SetLineMaterial( Material material )
        {
            if( material == null )
                return;

            foreach( var renderer in FindObjectsByType<LineRenderer>( FindObjectsInactive.Include, FindObjectsSortMode.None ) )
                renderer.sharedMaterial = material;
        }

        private void SetCameraIconMaterial( Material material )
        {
            if( material == null )
                return;

            foreach( var renderer in FindObjectsByType<ParticleSystemRenderer>( FindObjectsInactive.Include, FindObjectsSortMode.None ) )
                renderer.sharedMaterial = material;
        }
#endif
    }
}