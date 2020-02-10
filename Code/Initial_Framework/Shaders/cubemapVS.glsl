#version 330 core
layout (location = 0) in vec3 aPos;

out vec3 TexCoords;

uniform mat4 ProjMat;
uniform mat4 ViewMat;
uniform mat4 ModelMat;

void main()
{
	TexCoords = aPos;
	vec4 pos = ProjMat * ViewMat * ModelMat * vec4(aPos, 1.0);
	gl_Position = pos.xyww;
}