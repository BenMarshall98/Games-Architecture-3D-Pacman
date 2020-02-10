#version 330
 
in vec2 v_TexCoord;
uniform sampler2D texture;

out vec4 Color;
 
void main()
{
    Color = texture2D(texture, v_TexCoord);

	if (Color.a < 0.1)
	{
		discard;
	}
}