using System.Linq;
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

        private Vector3[] _vertices;
        
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
            _vertices = _seaMesh.vertices.ToArray();
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
            public float Gap;

            public float Speed;
            public float Period;
            public float WaveSize;
            public float WrinkleFrequency;
            public float WrinkleSize;
            public float NoiseSize;
            public float NoiseScale;

            public int2 Size;
            public float Time;

            public NativeArray<Vector3> Vertices;
            public NativeArray<Vector3> VerticesCur;
            
            public void Execute(int i)
            {
                var loc = (((float3) Vertices[i]).xz / Size + new float2(0f, Time)) * NoiseSize;
                var n = noise.cnoise(loc) * NoiseScale;
                float3 cur = Vertices[i];
                float3 vert = cur;
                cur.y = (math.sin(vert.z * Period + Time * Speed) + math.sin(vert.z * Period * 0.4f + Time * Speed) + n) * WaveSize;
                cur.z = vert.z + math.sin((vert.x + Time * Gap) * WrinkleFrequency) * WrinkleSize + math.sin((vert.z * math.sin(Time) + vert.x * math.cos(Time)) * Period * 0.4f + Time * Speed);
                VerticesCur[i] = cur;
            }
        }

        private void Update()
        {
            var vertices = new NativeArray<Vector3>(_vertices, Allocator.Persistent);
            var verticesCur = new NativeArray<Vector3>(_vertices.Length, Allocator.Persistent);
            
            var job = new WaveJob
            {
                Size = new int2(Dimensions.x, Dimensions.y),
                Time = Time.time,
                Vertices = vertices,
                NoiseScale = NoiseScale,
                NoiseSize = NoiseSize,
                VerticesCur = verticesCur,
                Gap = Gap,
                Period = Period,
                Speed = Speed,
                WaveSize = WaveSize,
                WrinkleFrequency = WrinkleFrequency,
                WrinkleSize = WrinkleSize
            };

            job.Schedule(_vertices.Length, 1).Complete();

            _seaMesh.vertices = job.VerticesCur.ToArray();
            
            vertices.Dispose();
            verticesCur.Dispose();
            // Makes the lighting better :D
            _seaMesh.RecalculateNormals();
        }
    }
}
