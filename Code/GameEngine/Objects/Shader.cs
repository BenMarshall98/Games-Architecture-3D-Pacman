using OpenTK.Graphics.OpenGL;
using System;
using System.IO;

namespace Game_Engine.Objects
{
    public class Shader
    {
        int mShaderID;

        public Shader()
        {

        }

        public void Delete()
        {
            GL.DeleteProgram(mShaderID);
        }

        public int ShaderID
        {
            get { return mShaderID; }
        }

        public void LoadShader(string vertexName, string fragmentName)
        {
            mShaderID = GL.CreateProgram();

            int vertexShader, fragmentShader;
            LoadShader(vertexName, ShaderType.VertexShader, mShaderID, out vertexShader);
            LoadShader(fragmentName, ShaderType.FragmentShader, mShaderID, out fragmentShader);
            GL.LinkProgram(mShaderID);
            Console.WriteLine(GL.GetProgramInfoLog(mShaderID));

            GL.DetachShader(mShaderID, vertexShader);
            GL.DetachShader(mShaderID, fragmentShader);
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);
        }

        void LoadShader(String filename, ShaderType type, int program, out int address)
        {
            address = GL.CreateShader(type);
            using (StreamReader sr = new StreamReader(filename))
            {
                GL.ShaderSource(address, sr.ReadToEnd());
            }
            GL.CompileShader(address);
            GL.AttachShader(program, address);
            Console.WriteLine(GL.GetShaderInfoLog(address));
        }
    }
}
