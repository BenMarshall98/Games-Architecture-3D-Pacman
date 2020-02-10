//Based on https://learnopengl.com/Lighting/Basic-Lighting
#version 330
 
layout (location = 0) in vec3 aPos;
layout (location = 1) in vec2 aTexCoord;
layout (location = 2) in vec3 aNormal;

uniform mat4 ModelMat;
uniform mat4 ViewMat;
uniform mat4 ProjMat;

out vec2 vTexCoord;
out vec3 vPos;
out vec3 vNormal;

void main()
{
    gl_Position = ProjMat * ViewMat * ModelMat * vec4(aPos, 1.0);

    vTexCoord = aTexCoord;
	vPos = vec3(ModelMat * vec4(aPos, 1.0));
	vNormal = mat3(transpose(inverse(ModelMat))) * aNormal;
}