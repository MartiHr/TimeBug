using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace NatureRendererDemo
{
    public class CompileShaders : MonoBehaviour
    {
        public PlayableDirector Director;

        public GameObject NatureRendererTerrain;
        public GameObject UnityTerrain;

#if UNITY_EDITOR
        private IEnumerator Start()
        {
            Director.Pause();
            Time.timeScale = 0f;

            // Store initial state
            bool allowAsyncCompilation = ShaderUtil.allowAsyncCompilation;

            // Force compilation of shaders and wait until compilation finished.
            ShaderUtil.allowAsyncCompilation = false;
            yield return null;

            ShaderUtil.allowAsyncCompilation = true;
            while( ShaderUtil.anythingCompiling )
                yield return null;

            if( UnityTerrain != null )
            {
                // Disable Nature Renderer and enable Unity's renderer to compile those shaders as well,
                // otherwise there will be a freeze/shader compilation in the middle of the timeline.
                NatureRendererTerrain.SetActive( false );
                UnityTerrain.SetActive( true );

                // Force compilation of shaders and wait until compilation finished.
                ShaderUtil.allowAsyncCompilation = false;
                yield return null;

                ShaderUtil.allowAsyncCompilation = true;
                while( ShaderUtil.anythingCompiling )
                    yield return null;

                // Restore Nature Renderer.
                NatureRendererTerrain.SetActive( true );
                UnityTerrain.SetActive( false );
            }

            // Restore initial state
            ShaderUtil.allowAsyncCompilation = allowAsyncCompilation;

            Time.timeScale = 1f;
            Director.Play();
        }
#endif
    }
}
