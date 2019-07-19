using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Canoe.Helpers.Waves
{
    public class WaveBehaviour : MonoBehaviour
    {
        [Header("Sea Parameters")]
        public Vector2Int Dimensions;
        public float Gap;

        [Header("Wave Parameters")]
        public float Speed;
        public float Period;
        public float WaveSize;
        public float WrinkleFrequency;
        public float WrinkleSize;
        public float NoiseSize;
        public float NoiseScale;
        public Vector2 NoiseMovementVector;

        private NativeArray<Vector3> _vertices;
        private NativeArray<Vector3> _newVerticesJobArray;
        private Vector3[] _newVertices;
        private bool _visible;
        
        // Mesh instance
        private Mesh _seaMesh;

        private void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            _seaMesh = GenerateMesh(Dimensions, Gap);
            GetComponent<MeshFilter>().sharedMesh = _seaMesh;
            _vertices = new NativeArray<Vector3>(_seaMesh.vertices, Allocator.Persistent);
            _newVerticesJobArray = new NativeArray<Vector3>(_seaMesh.vertices.Length, Allocator.Persistent);
            _newVertices = new Vector3[_vertices.Length];
            _visible = GetComponent<Renderer>().isVisible;
        }

        private static Mesh GenerateMesh(Vector2Int dimensions, float gap)
        {
            var mesh =  new Mesh();
            var vertices = new Vector3[dimensions.x * dimensions.y * 6];
            var triangles = new int[vertices.Length];
            var ind = 0;
            for (var y = 0; y < dimensions.y; y++)
            {
                for (var x = 0; x < dimensions.x; x++)
                {
                    var pos = new Vector3(x - dimensions.x / 2f, 0, y - dimensions.y / 2f);

                    //Triangle 1
                    vertices[ind] = pos * gap;
                    vertices[ind + 1] = (pos + Vector3.forward) * gap;
                    vertices[ind + 2] = (pos + Vector3.right) * gap;
                    triangles[ind] = ind;
                    triangles[ind + 1] = ind + 1;
                    triangles[ind + 2] = ind + 2;
                    ind += 3;

                    //Triangle 2
                    vertices[ind] = (pos + Vector3.forward) * gap;
                    vertices[ind + 1] = (pos + Vector3.right + Vector3.forward) * gap;
                    vertices[ind + 2] = (pos + Vector3.right) * gap;
                    triangles[ind] = ind;
                    triangles[ind + 1] = ind + 1;
                    triangles[ind + 2] = ind + 2;
                    ind += 3;
                }
            }
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();
            return mesh;
        }
        
        [BurstCompile]
        private struct WaveJob : IJobParallelFor
        {
            public float3 Position;
            
            public float Gap;

            public float Speed;
            public float Period;
            public float WaveSize;
            public float WrinkleFrequency;
            public float WrinkleSize;
            public float NoiseSize;
            public float NoiseScale;
            public float2 NoiseMovementVector;

            public int2 Size;
            public float Time;

            public NativeArray<Vector3> Vertices;
            public NativeArray<Vector3> VerticesCur;
            
            public void Execute(int i)
            {
                float3 cur = Vertices[i];
                float2 location = cur.xz + Position.xz;
                cur.y = GetHeight(location, Size, Time, NoiseSize, NoiseScale, Period, Speed, WaveSize, NoiseMovementVector);
                cur.z = cur.z + math.sin((location.x + Time * Gap) * WrinkleFrequency) * WrinkleSize +
                        math.sin((location.y / Size.y + location.x * math.cos(Time)) * Period * 0.4f +
                                 Time * Speed);
                VerticesCur[i] = cur;
            }

            public static float GetHeight(float2 location, int2 size, float time, float noiseSize, float noiseScale,
                float period, float speed, float waveSize, float2 noiseVector)
            {
                var noiseValue = noise.cnoise((location / size + noiseVector * time) * noiseSize) * noiseScale;
                return (math.sin(location.y * period + time * speed) +
                        math.sin(location.y * period * 0.4f + time * speed) + noiseValue) * waveSize;
            }
        }

        public float GetHeight(Vector3 location)
        {
            return WaveJob.GetHeight(new float2(location.x, location.z), new int2(Dimensions.x, Dimensions.y),
                Time.time, NoiseSize, NoiseScale, Period, Speed, WaveSize, NoiseMovementVector);
        }

        private void Update()
        {
            if (!_visible) return;
            var job = new WaveJob
            {
                Size = new int2(Dimensions.x, Dimensions.y),
                Time = Time.time,
                Vertices = _vertices,
                NoiseScale = NoiseScale,
                NoiseSize = NoiseSize,
                VerticesCur = _newVerticesJobArray,
                Gap = Gap,
                Period = Period,
                Speed = Speed,
                WaveSize = WaveSize,
                WrinkleFrequency = WrinkleFrequency,
                WrinkleSize = WrinkleSize,
                Position = transform.position,
                NoiseMovementVector = NoiseMovementVector
            };

            job.Schedule(_vertices.Length, 1).Complete();

            job.VerticesCur.CopyTo(_newVertices);
            _seaMesh.vertices = _newVertices;
            
            // Makes the lighting better :D
            _seaMesh.RecalculateNormals();
        }

        private void OnBecameVisible()
        {
            _visible = true;
        }

        private void OnBecameInvisible()
        {
            _visible = false;
        }

        private void OnDestroy()
        {
            _vertices.Dispose();
            _newVerticesJobArray.Dispose();
        }
    }
}
