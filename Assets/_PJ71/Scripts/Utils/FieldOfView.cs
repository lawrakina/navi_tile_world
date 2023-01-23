using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using NavySpade.pj46.Extensions;
using UnityEngine;

namespace NavySpade._PJ71.Utils
{
    [RequireComponent(typeof(MeshFilter))]
    public class FieldOfView : MonoBehaviour
    {
        [SerializeField] private float _viewRadius = 4;
        [SerializeField] [Range(0, 360)] private float _viewAngle = 120f;
        [SerializeField] private LayerMask _targetMask;
        [SerializeField] private LayerMask _obstacleMask;
        [SerializeField] private float _meshResolution = 10f;
        [SerializeField] private int _edgeResolveIterations = 1;
        [SerializeField] private float _edgeDstThreshold = 1f;
        [SerializeField] private float _findTargetsPeriod = 1f;
        [SerializeField] private int _maxTargets = 10;
        [SerializeField] private bool _isDraw = true;
        
        [field: Foldout("Debug")]
        [field: SerializeField]
        public Color RadiusColor { get; private set; } = Color.white;
        
        [field: Foldout("Debug")]
        [field: SerializeField]
        public Color FieldOfViewColor { get; private set; } = Color.yellow;
        
        [field: Foldout("Debug")]
        [field: SerializeField]
        public Color TargetColor { get; private set; } = Color.red;
        
        private MeshFilter _meshFilter;
        private Mesh _viewMesh;
        private WaitForSeconds _findTargetsDelay;
        private Collider[] _visibleColliders;

        public float ViewRadius
        {
            get => _viewRadius;
            set => _viewRadius = value;
        }
        
        public float ViewAngle => _viewAngle;

        public LayerMask TargetMask => _targetMask;
    
        public List<Collider> VisibleTargets { get; private set; }

        public event Action FoundTarget;
    
        private void Start()
        {
            if (_isDraw)
            {
                _meshFilter = GetComponent<MeshFilter>();
                _viewMesh = new Mesh();
                _viewMesh.name = "View Mesh";
                _meshFilter.mesh = _viewMesh;
            }
            
            _visibleColliders = new Collider[_maxTargets];
            VisibleTargets = new List<Collider>(_maxTargets);
            
            _findTargetsDelay = new WaitForSeconds(_findTargetsPeriod);
            StartCoroutine(FindTargetsWithDelay());
        }
    
        private IEnumerator FindTargetsWithDelay()
        {
            while (true)
            {
                yield return _findTargetsDelay;
                FindVisibleTargets();
            }
        }

        private void LateUpdate()
        {
            if(_isDraw)
                DrawFieldOfView();
        }

        private void FindVisibleTargets()
        {
            int hitsCount =  Physics.OverlapSphereNonAlloc(transform.position, _viewRadius, _visibleColliders, _targetMask);
            
            VisibleTargets.Clear();
            for (int i = 0; i < hitsCount; i++)
            {
                Transform target = _visibleColliders[i].transform;
                Vector3 dirToTarget = (target.position - transform.position).normalized;
                dirToTarget.y = 0;
                if (Vector3.Angle(transform.forward, dirToTarget) < _viewAngle / 2)
                {
                    float dstToTarget = Vector3.Distance(transform.position, target.position);
                    if (Physics.Raycast(transform.position, dirToTarget, out RaycastHit hit, dstToTarget, _obstacleMask))
                    {
                        Debug.Log(hit.collider, hit.collider);
                    }
                    else
                    {
                        VisibleTargets.Add(_visibleColliders[i]);   
                    }
                }
            }

            if (VisibleTargets.Count > 0)
            {
                FoundTarget?.Invoke();
            }
        }

        private void DrawFieldOfView()
        {
            int stepCount = Mathf.RoundToInt(_viewAngle * _meshResolution);
            float stepAngleSize = _viewAngle / stepCount;
            List<Vector3> viewPoints = new List<Vector3>();
            ViewCastInfo oldViewCast = new ViewCastInfo();
        
            for (int i = 0; i <= stepCount; i++)
            {
                float angle = transform.eulerAngles.y - _viewAngle / 2 + stepAngleSize * i;
                ViewCastInfo newViewCast = ViewCast(angle);

                if (i > 0)
                {
                    bool edgeDstThresholdExceeded = Mathf.Abs(oldViewCast.dst - newViewCast.dst) > _edgeDstThreshold;
                    if (oldViewCast.hit != newViewCast.hit ||
                        (oldViewCast.hit && newViewCast.hit && edgeDstThresholdExceeded))
                    {
                        EdgeInfo edge = FindEdge(oldViewCast, newViewCast);
                        if (edge.pointA != Vector3.zero)
                        {
                            viewPoints.Add(edge.pointA);
                        }

                        if (edge.pointB != Vector3.zero)
                        {
                            viewPoints.Add(edge.pointB);
                        }
                    }
                }
            
                viewPoints.Add(newViewCast.point);
                oldViewCast = newViewCast;
            }

            int vertexCount = viewPoints.Count + 1;
            Vector3[] vertices = new Vector3[vertexCount];
            int[] triangles = new int[(vertexCount - 2) * 3];

            vertices[0] = Vector3.zero;
            for (int i = 0; i < vertexCount - 1; i++)
            {
                vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);

                if (i < vertexCount - 2)
                {
                    triangles[i * 3] = 0;
                    triangles[i * 3 + 1] = i + 1;
                    triangles[i * 3 + 2] = i + 2;
                }
            }

            _viewMesh.Clear();

            _viewMesh.vertices = vertices;
            _viewMesh.triangles = triangles;
            _viewMesh.RecalculateNormals();
        }


        private EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast)
        {
            float minAngle = minViewCast.angle;
            float maxAngle = maxViewCast.angle;
            Vector3 minPoint = Vector3.zero;
            Vector3 maxPoint = Vector3.zero;

            for (int i = 0; i < _edgeResolveIterations; i++)
            {
                float angle = (minAngle + maxAngle) / 2;
                ViewCastInfo newViewCast = ViewCast(angle);

                bool edgeDstThresholdExceeded = Mathf.Abs(minViewCast.dst - newViewCast.dst) > _edgeDstThreshold;
                if (newViewCast.hit == minViewCast.hit && !edgeDstThresholdExceeded)
                {
                    minAngle = angle;
                    minPoint = newViewCast.point;
                }
                else
                {
                    maxAngle = angle;
                    maxPoint = newViewCast.point;
                }
            }

            return new EdgeInfo(minPoint, maxPoint);
        }


        private ViewCastInfo ViewCast(float globalAngle)
        {
            Vector3 dir = DirFromAngle(globalAngle, true);
            RaycastHit hit;

            if (Physics.Raycast(transform.position, dir, out hit, _viewRadius, _obstacleMask))
            {
                return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
            }
            else
            {
                return new ViewCastInfo(false, transform.position + dir * _viewRadius, _viewRadius, globalAngle);
            }
        }

        public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
        {
            if (!angleIsGlobal)
            {
                angleInDegrees += transform.eulerAngles.y;
            }

            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        }

        private struct ViewCastInfo
        {
            public bool hit;
            public Vector3 point;
            public float dst;
            public float angle;

            public ViewCastInfo(bool _hit, Vector3 _point, float _dst, float _angle)
            {
                hit = _hit;
                point = _point;
                dst = _dst;
                angle = _angle;
            }
        }

        private struct EdgeInfo
        {
            public Vector3 pointA;
            public Vector3 pointB;

            public EdgeInfo(Vector3 _pointA, Vector3 _pointB)
            {
                pointA = _pointA;
                pointB = _pointB;
            }
        }
    }
}
