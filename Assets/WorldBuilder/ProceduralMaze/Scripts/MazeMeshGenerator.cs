using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Cr7_PCMaze
{
    public class MazeMeshGenerator : MonoBehaviour
    {
        float width;
        float height;

        float offsetX = 1.0f;

        public void InitInfo(float _width, float _height)
        {
            width = _width;
            height = _height;
        }

        public Mesh FromData(int[,] data)
        {
            Mesh maze = new Mesh();

            var newVertices = new List<Vector3>();
            var newUVs = new List<Vector2>();

            maze.subMeshCount = 2;
            var floorTriangles = new List<int>();
            var wallTriangles = new List<int>();

            int rMax = data.GetUpperBound(0);
            var cMax = data.GetUpperBound(1);
            var halfH = height * .5f;


            Quaternion rotation = Quaternion.Euler(0, 90, 0);

            for (int i = 0; i <= rMax; i++)
            {
                for (int j = 0; j <= cMax; j++)
                {
                    if (data[i, j] != 1)
                    {
                        // floor
                        AddCube(Matrix4x4.TRS(
                            new Vector3(i * width + .5f * width, j * width + .5f * width, height),
                            Quaternion.LookRotation(Vector3.up, Vector3.back),
                            new Vector3(width, 1, width)
                        ), ref newVertices, ref newUVs, ref floorTriangles);


                        // walls on sides next to blocked grid cells
                        if (j - 1 < 0 || data[i, j - 1] == 1)
                        {
                            AddCube(Matrix4x4.TRS(
                                new Vector3((i + .5f) * width, j * width, halfH),
                                // Quaternion.LookRotation(Vector3.back),
                                Quaternion.Euler(0, 0, 0),
                                new Vector3(width*offsetX, 1, height)
                            ), ref newVertices, ref newUVs, ref wallTriangles);
                        }


                        if (j + 1 > cMax || data[i, j + 1] == 1)
                        {
                            AddCube(Matrix4x4.TRS(
                                new Vector3((i + .5f) * width, (j + 1) * width, halfH),
                                // Quaternion.LookRotation(Vector3.forward),
                                Quaternion.Euler(0, 0, 0),
                                new Vector3(width*offsetX, 1, height)
                            ), ref newVertices, ref newUVs, ref wallTriangles);
                        }

                        if (i + 1 > rMax || data[i + 1, j] == 1)
                        {
                            AddCube(Matrix4x4.TRS(
                                new Vector3((i + 1) * width, (j + .5f) * width, halfH),
                                Quaternion.LookRotation(Vector3.back, Vector3.left),
                                // Quaternion.Euler(0, 0, -90),
                                new Vector3(width, 1, height)
                            ), ref newVertices, ref newUVs, ref wallTriangles);
                        }

                        if (i - 1 < 0 || data[i - 1, j] == 1)
                        {
                            AddCube(Matrix4x4.TRS(
                                new Vector3(i * width, (j + .5f) * width, halfH),
                                Quaternion.LookRotation(Vector3.forward, Vector3.right),
                                // Quaternion.Euler(0, 0, 90),
                                new Vector3(width, 1, height)
                            ), ref newVertices, ref newUVs, ref wallTriangles);
                        }
                    }
                }
            }

            //quard
            for (int i = 0; i <= rMax; i++)
            {
                for (int j = 0; j <= cMax; j++)
                {
                    //     if (data[i, j] != 1)
                    //     {
                    //         // floor
                    //         AddQuad(Matrix4x4.TRS(
                    //             new Vector3(j * width, i * width, 0),
                    //             Quaternion.LookRotation(Vector3.forward),
                    //             new Vector3(width,1, width)
                    //         ), ref newVertices, ref newUVs, ref floorTriangles);



                    //         // walls on sides next to blocked grid cells

                    //         if (i - 1 < 0 || data[i - 1, j] == 1)
                    //         {
                    //             AddQuad(Matrix4x4.TRS(
                    //                 new Vector3(j * width, (i - .5f) * width, halfH),
                    //                 Quaternion.LookRotation(Vector3.up),
                    //                 new Vector3(width, 1,height)
                    //             ), ref newVertices, ref newUVs, ref wallTriangles);
                    //         }

                    //         if (j + 1 > cMax || data[i, j + 1] == 1)
                    //         {
                    //             AddQuad(Matrix4x4.TRS(
                    //                 new Vector3((j + .5f) * width, i * width, halfH),
                    //                 Quaternion.LookRotation(Vector3.left),
                    //                 new Vector3(width,1, height)
                    //             ), ref newVertices, ref newUVs, ref wallTriangles);
                    //         }

                    //         if (j - 1 < 0 || data[i, j - 1] == 1)
                    //         {
                    //             AddQuad(Matrix4x4.TRS(
                    //                 new Vector3((j - .5f) * width, i * width, halfH),
                    //                 Quaternion.LookRotation(Vector3.right),
                    //                 new Vector3(width, 1,height)
                    //             ), ref newVertices, ref newUVs, ref wallTriangles);
                    //         }

                    //         if (i + 1 > rMax || data[i + 1, j] == 1)
                    //         {
                    //             AddQuad(Matrix4x4.TRS(
                    //                 new Vector3(j * width, (i + .5f) * width, halfH),
                    //                 Quaternion.LookRotation(Vector3.down),
                    //                 new Vector3(width,1, height)
                    //             ), ref newVertices, ref newUVs, ref wallTriangles);
                    //         }
                    //     }
                }
            }


            // with no offset
            for (int j = 0; j <= rMax; j++)
            {
                for (int i = 0; i <= cMax; i++)
                {
                    //     if (data[j, i] != 1)
                    //     {
                    //         // floor
                    //         AddCube(Matrix4x4.TRS(
                    //             new Vector3(j * width + .5f * width, 0, i * width + .5f * width),
                    //             Quaternion.LookRotation(Vector3.up),
                    //             new Vector3(width, width, 1)
                    //         ), ref newVertices, ref newUVs, ref floorTriangles);

                    //         // walls on sides next to blocked grid cells
                    //         // float randomWidthOffset = Random.value * .25f * width;
                    //         float randomWidthOffset = 0;

                    //         if (i - 1 < 0 || data[j, i - 1] == 1)
                    //         {
                    //             AddCube(Matrix4x4.TRS(
                    //                 new Vector3(j * width + .5f * width, halfH, i * width + randomWidthOffset),
                    //                 Quaternion.LookRotation(Vector3.forward),
                    //                 new Vector3(width, height, 1)
                    //             ), ref newVertices, ref newUVs, ref wallTriangles);
                    //         }
                    //         // randomWidthOffset = Random.value * .25f * width;
                    //         if (i + 1 > cMax || data[j, i + 1] == 1)
                    //         {
                    //             AddCube(Matrix4x4.TRS(
                    //                 new Vector3(j * width + .5f * width, halfH, (i + 1) * width + randomWidthOffset),
                    //                 Quaternion.LookRotation(Vector3.back),
                    //                 new Vector3(width, height, 1)
                    //             ), ref newVertices, ref newUVs, ref wallTriangles);

                    //         }

                    //         if (j + 1 > rMax || data[j + 1, i] == 1)
                    //         {
                    //             AddCube(Matrix4x4.TRS(
                    //                 new Vector3((j + 1) * width, halfH, i * width + .5f * width),
                    //                 Quaternion.LookRotation(Vector3.left),
                    //                 new Vector3(width, height, 1)
                    //             ), ref newVertices, ref newUVs, ref wallTriangles);
                    //         }

                    //         if (j - 1 < 0 || data[j - 1, i] == 1)
                    //         {
                    //             AddCube(Matrix4x4.TRS(
                    //                 new Vector3(j * width, halfH, i * width + .5f * width),
                    //                 Quaternion.LookRotation(Vector3.right),
                    //                 new Vector3(width, height, 1)
                    //             ), ref newVertices, ref newUVs, ref wallTriangles);

                    //         }

                    //     }
                }
            }

            maze.vertices = newVertices.ToArray();
            maze.uv = newUVs.ToArray();

            maze.SetTriangles(floorTriangles.ToArray(), 0);
            maze.SetTriangles(wallTriangles.ToArray(), 1);

            maze.RecalculateNormals();

            return maze;
        }

        public static void AddCube(Matrix4x4 matrix, ref List<Vector3> newVertices, ref List<Vector2> newUVs, ref List<int> newTriangles)
        {
            int index = newVertices.Count;
            var primitiveCube = GetPrimitiveShapeMesh.GetPrimitiveVertex(PrimitiveType.Cube);

            foreach (var v in primitiveCube.vertices)
            {
                newVertices.Add(matrix.MultiplyPoint3x4(v));
            }
            foreach (var u in primitiveCube.uv)
            {
                newUVs.Add(u);
            }
            foreach (var t in primitiveCube.triangles)
            {
                newTriangles.Add(index + t);
            }
        }

        private void AddQuad(Matrix4x4 matrix, ref List<Vector3> newVertices, ref List<Vector2> newUVs, ref List<int> newTriangles)
        {
            int index = newVertices.Count;

            // corners before transforming
            Vector3 vert1 = new Vector3(-.5f, -.5f, 0);
            Vector3 vert2 = new Vector3(-.5f, .5f, 0);
            Vector3 vert3 = new Vector3(.5f, .5f, 0);
            Vector3 vert4 = new Vector3(.5f, -.5f, 0);

            newVertices.Add(matrix.MultiplyPoint3x4(vert1));
            newVertices.Add(matrix.MultiplyPoint3x4(vert2));
            newVertices.Add(matrix.MultiplyPoint3x4(vert3));
            newVertices.Add(matrix.MultiplyPoint3x4(vert4));

            newUVs.Add(new Vector2(1, 0));
            newUVs.Add(new Vector2(1, 1));
            newUVs.Add(new Vector2(0, 1));
            newUVs.Add(new Vector2(0, 0));

            newTriangles.Add(index + 2);
            newTriangles.Add(index + 1);
            newTriangles.Add(index);

            newTriangles.Add(index + 3);
            newTriangles.Add(index + 2);
            newTriangles.Add(index);
        }
    }
}