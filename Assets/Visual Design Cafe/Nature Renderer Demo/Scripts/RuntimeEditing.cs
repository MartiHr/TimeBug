using UnityEngine;
using VisualDesignCafe.Rendering.Nature;

namespace NatureRendererDemo
{
    public class RuntimeEditing : MonoBehaviour
    {
        public Terrain Terrain;
        public float Duration;
        public Transform[] Waypoints;
        public Transform Target;
        public float Radius;

        private float _totalLength;
        private float _startTime;
        private Vector3 _erasePosition;

        private AnimationCurve _curveX;
        private AnimationCurve _curveY;
        private AnimationCurve _curveZ;

        private Vector3[] _cachedPositions;

        private void OnDrawGizmos()
        {
            if( _cachedPositions == null )
                CreateCurve();

            for( int i = 0; i < Waypoints.Length; i++ )
            {
                if( Waypoints[i].position != _cachedPositions[i] )
                {
                    CreateCurve();
                    break;
                }
            }

            if( _curveX != null )
            {
                float step = 1f / (_totalLength / 0.25f);
                for( float t = 0; t <= 1f; t += step )
                {
                    Gizmos.DrawLine(
                        new Vector3(
                            _curveX.Evaluate( t ),
                            _curveY.Evaluate( t ),
                            _curveZ.Evaluate( t ) ),
                        new Vector3(
                            _curveX.Evaluate( t + step ),
                            _curveY.Evaluate( t + step ),
                            _curveZ.Evaluate( t + step ) ) );
                }
            }
        }

        private void OnValidate()
        {
            CreateCurve();
        }

        private void OnEnable()
        {
            _startTime = Time.time;
            CreateCurve();
        }

        private void Update()
        {
            UpdatePosition();

            float resolution = Terrain.terrainData.size.x / (float) Terrain.terrainData.detailResolution;

            if( Vector3.Distance( Target.position, _erasePosition ) >= resolution )
            {
                _erasePosition = Target.position;
                RemoveDetails( Terrain, Target.position, Radius );
            }
        }

        private void UpdatePosition()
        {
            float time = Mathf.Clamp01( (Time.time - _startTime) / Duration );

            Target.position =
                new Vector3(
                    _curveX.Evaluate( time ),
                    _curveY.Evaluate( time ),
                    _curveZ.Evaluate( time ) );
        }

        private void CreateCurve()
        {
            if( Waypoints == null )
                return;

            _cachedPositions = new Vector3[Waypoints.Length];

            _totalLength = 0;

            for( int i = 0; i < Waypoints.Length; i++ )
                _cachedPositions[i] = Waypoints[i].position;

            for( int i = 0; i < Waypoints.Length - 1; i++ )
                _totalLength += Vector3.Distance( Waypoints[i].position, Waypoints[i + 1].position );

            _curveX = new AnimationCurve();
            _curveY = new AnimationCurve();
            _curveZ = new AnimationCurve();

            float position = 0;
            for( int i = 0; i < Waypoints.Length; i++ )
            {
                Vector3 current = Waypoints[i].position;
                float time = position / _totalLength;

                _curveX.AddKey( new Keyframe( time, current.x ) );
                _curveY.AddKey( new Keyframe( time, current.y ) );
                _curveZ.AddKey( new Keyframe( time, current.z ) );

                if( i < Waypoints.Length - 1 )
                    position += Vector3.Distance( current, Waypoints[i + 1].position );
            }

            _curveX.preWrapMode = WrapMode.ClampForever;
            _curveY.preWrapMode = WrapMode.ClampForever;
            _curveZ.preWrapMode = WrapMode.ClampForever;

            _curveX.postWrapMode = WrapMode.ClampForever;
            _curveY.postWrapMode = WrapMode.ClampForever;
            _curveZ.postWrapMode = WrapMode.ClampForever;

            for( int i = 0; i < Waypoints.Length - 1; i++ )
            {
                _curveX.SmoothTangents( i, 0 );
                _curveY.SmoothTangents( i, 0 );
                _curveZ.SmoothTangents( i, 0 );
            }
        }

        private void RemoveDetails( Terrain terrain, Vector3 point, float radius )
        {
            var terrainData = terrain.terrainData;

#if UNITY_EDITOR
            if( UnityEditor.EditorUtility.IsPersistent( terrainData ) )
            {
                throw new System.Exception(
                    "Can't edit a terrain asset. Clone the terrain to enable runtime editing." );
            }
#endif

            point -= terrain.GetPosition();
            var terrainRect =
                new TerrainRect( point.x - radius, point.z - radius, radius * 2f, radius * 2f );
            var detailRect =
                new DetailmapRect( terrainRect, terrainData, DetailmapRect.RoundingMethod.Cover );

            int[] layers =
                terrainData.GetSupportedLayers(
                    detailRect.X,
                    detailRect.Y,
                    detailRect.Width,
                    detailRect.Height );

            int[,] data = new int[detailRect.Width, detailRect.Height];
            for( int layer = 0; layer < layers.Length; layer++ )
                terrainData.SetDetailLayer( detailRect.X, detailRect.Y, layers[layer], data );
        }
    }
}
