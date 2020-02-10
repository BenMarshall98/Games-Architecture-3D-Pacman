using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Game_Engine.Objects
{
    public class Geometry
    {
        List<float> vertices = new List<float>();
        List<float> textures = new List<float>();
        List<float> normals = new List<float>();
        List<int> indices = new List<int>();

        // Graphics
        private int[] mVBO_IDs = new int[3];
        private int vao_Handle;
        private int mEBO;

        public Geometry()
        {
        }

        public void LoadObject(string filename)
        {
            if(filename.EndsWith(".txt"))
            {
                LoadTXT(filename);
            }
            else if (filename.EndsWith(".obj"))
            {
                LoadOBJ(filename);
            }
        }

        private void LoadOBJ(string filename)
        {
            string line;

            try
            {
                FileStream fin = File.OpenRead(filename);
                StreamReader reader = new StreamReader(fin);

                List<string> vertexLine = new List<string>();
                List<string> faceLine = new List<string>();
                List<string> normalLine = new List<string>();
                List<string> textureLine = new List<string>();

                while (!reader.EndOfStream)
                {
                    line = reader.ReadLine();

                    string[] lineSplit = line.Split(new char[] { ' ' }, 2);

                    if (lineSplit[0] == "v")
                    {
                        vertexLine.Add(lineSplit[1]);
                    }
                    else if (lineSplit[0] == "vt")
                    {
                        textureLine.Add(lineSplit[1]);
                    }
                    else if (lineSplit[0] == "vn")
                    {
                        normalLine.Add(lineSplit[1]);
                    }
                    else if (lineSplit[0] == "f")
                    {

                        faceLine.AddRange(lineSplit[1].Split(' '));
                    }
                }

                List<string> optimisedFaceList = new List<string>();

                for (int i = 0; i < faceLine.Count; i++)
                {
                    bool inList = false;
                    for (int j = 0; j < optimisedFaceList.Count; j++)
                    {
                        if (faceLine[i] == optimisedFaceList[j])
                        {
                            indices.Add(j);
                            inList = true;
                            break;
                        }
                    }
                    if (!inList)
                    {
                        indices.Add(optimisedFaceList.Count);
                        optimisedFaceList.Add(faceLine[i]);
                    }
                }

                for (int i = 0; i < optimisedFaceList.Count; i++)
                {
                    string[] faceNumbers = optimisedFaceList[i].Split('/');

                    int vertexIndex = int.Parse(faceNumbers[0]) - 1;
                    int textureIndex = int.Parse(faceNumbers[1]) - 1;
                    int normalIndex = int.Parse(faceNumbers[2]) - 1;

                    {
                        string[] vertexNumbers = vertexLine[vertexIndex].Split(' ');
                        vertices.Add(float.Parse(vertexNumbers[0]));
                        vertices.Add(float.Parse(vertexNumbers[1]));
                        vertices.Add(float.Parse(vertexNumbers[2]));
                    }

                    {
                        string[] textureNumbers = textureLine[textureIndex].Split(' ');
                        textures.Add(float.Parse(textureNumbers[0]));
                        textures.Add(float.Parse(textureNumbers[1]));
                    }

                    {
                        string[] normalNumbers = normalLine[normalIndex].Split(' ');
                        normals.Add(float.Parse(normalNumbers[0]));
                        normals.Add(float.Parse(normalNumbers[1]));
                        normals.Add(float.Parse(normalNumbers[2]));
                    }
                }

                GL.GenVertexArrays(1, out vao_Handle);
                GL.BindVertexArray(vao_Handle);
                GL.GenBuffers(3, mVBO_IDs);
                GL.GenBuffers(1, out mEBO);

                GL.BindBuffer(BufferTarget.ElementArrayBuffer, mEBO);
                GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(indices.Count * sizeof(int)), indices.ToArray<int>(), BufferUsageHint.StaticDraw);

                GL.BindBuffer(BufferTarget.ArrayBuffer, mVBO_IDs[0]);
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertices.Count * sizeof(float)), vertices.ToArray<float>(), BufferUsageHint.StaticDraw);

                // Positions
                GL.EnableVertexAttribArray(0);
                GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

                GL.BindBuffer(BufferTarget.ArrayBuffer, mVBO_IDs[1]);
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(textures.Count * sizeof(float)), textures.ToArray<float>(), BufferUsageHint.StaticDraw);

                // Textures
                GL.EnableVertexAttribArray(1);
                GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 2 * sizeof(float), 0);

                GL.BindBuffer(BufferTarget.ArrayBuffer, mVBO_IDs[2]);
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(normals.Count * sizeof(float)), normals.ToArray<float>(), BufferUsageHint.StaticDraw);

                // Normals
                GL.EnableVertexAttribArray(2);
                GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

                GL.BindVertexArray(0);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private void LoadTXT(string filename)
        {
            string line;

            try
            {
                FileStream fin = File.OpenRead(filename);
                StreamReader sr = new StreamReader(fin);

                GL.GenVertexArrays(1, out vao_Handle);
                GL.BindVertexArray(vao_Handle);
                GL.GenBuffers(2, mVBO_IDs);

                while (!sr.EndOfStream)
                {
                    line = sr.ReadLine();
                    string[] values = line.Split(',');

                    if (values[0].StartsWith("NUM_OF_TRIANGLES"))
                    {
                        int numberOfTriangles = int.Parse(values[0].Remove(0, "NUM_OF_TRIANGLES".Length));
                        for (int i = 0; i < numberOfTriangles * 3; i++)
                        {
                            indices.Add(i);
                        }
                        continue;
                    }
                    if (values[0].StartsWith("//") || values.Length < 5) continue;

                    vertices.Add(float.Parse(values[0]));
                    vertices.Add(float.Parse(values[1]));
                    vertices.Add(float.Parse(values[2]));
                    vertices.Add(float.Parse(values[3]));
                    vertices.Add(float.Parse(values[4]));
                }

                GL.GenBuffers(1, out mEBO);

                GL.BindBuffer(BufferTarget.ElementArrayBuffer, mEBO);
                GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(indices.Count * sizeof(int)), indices.ToArray<int>(), BufferUsageHint.StaticDraw);

                GL.BindBuffer(BufferTarget.ArrayBuffer, mVBO_IDs[0]);
                GL.BufferData<float>(BufferTarget.ArrayBuffer, (IntPtr)(vertices.Count * 4), vertices.ToArray<float>(), BufferUsageHint.StaticDraw);

                // Positions
                GL.EnableVertexAttribArray(0);
                GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * 4, 0);

                // Tex Coords
                GL.EnableVertexAttribArray(1);
                GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 5 * 4, 3 * 4);

                GL.BindVertexArray(0);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void Render()
        {
            GL.BindVertexArray(vao_Handle);
            GL.DrawElements(PrimitiveType.Triangles, indices.Count, DrawElementsType.UnsignedInt, 0);
        }

        public void Delete()
        {
            GL.DeleteBuffers(3, mVBO_IDs);
            GL.DeleteBuffer(mEBO);
            GL.DeleteVertexArray(vao_Handle);
        }
    }
}
