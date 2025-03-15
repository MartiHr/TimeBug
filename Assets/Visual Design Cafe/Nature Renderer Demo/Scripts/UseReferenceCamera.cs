using UnityEngine;
using VisualDesignCafe.Rendering.Instancing;

namespace NatureRendererDemo
{
    [ExecuteAlways]
    public class UseReferenceCamera : MonoBehaviour
    {
        [SerializeField]
        private Camera _referenceCamera;

        private Camera _camera;

        private int _delay = 1;

        private void Start()
        {
            _camera = GetComponent<Camera>();
        }

        private void Update()
        {
            _delay--;
            if( _delay <= 0 )
            {
                var cameraRenderer = RendererPool.GetCamera( CameraId.GetId( _camera ) );
                if( cameraRenderer != null && cameraRenderer.ReferenceCamera != _referenceCamera )
                    cameraRenderer.SetReferenceCamera( _referenceCamera );
            }
        }
    }
}