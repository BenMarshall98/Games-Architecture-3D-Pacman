//Based on https://learnopengl.com/Lighting/Basic-Lighting

#version 330
 
in vec2 vTexCoord;
in vec3 vPos;
in vec3 vNormal;

uniform sampler2D sTexture;
uniform vec3 sLightPos;
uniform vec3 sViewPos;
uniform vec3 sLightColor;

out vec4 Color;
 
void main()
{
	// ambient
	float ambientStrength = 0.1;
	vec3 ambient = ambientStrength * sLightColor;

	// diffuse
	vec3 norm = normalize(vNormal);
	vec3 lightDir = normalize(sLightPos - vPos);
	float diff = max(dot(norm, lightDir), 0.0);
	vec3 diffuse = diff * sLightColor;

	// specular
	float specularStrength = 0.5;
	vec3 viewDir = normalize(sViewPos - vPos);
	vec3 reflectDir = reflect(-lightDir, norm);
	float spec = pow(max(dot(viewDir, reflectDir), 0.0), 32);
	vec3 specular = specularStrength * spec * sLightColor;

	vec3 result = (ambient + diffuse + specular) * texture2D(sTexture, vTexCoord).xyz;
    Color = vec4(result, 1.0);
}