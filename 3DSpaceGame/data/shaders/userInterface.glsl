#version 440 core 

#define ShaderType

#ifdef SHADER_VERT

uniform vec4 rectTransform;

layout (location = 0) in vec2 pos;

void main() {
	gl_Position = vec4(rectTransform.xy + (pos * rectTransform.zw), 0.0, 1.0);
}

#endif
#ifdef SHADER_FRAG

out vec4 color;
void main() {
	color = vec4(1.);
}

#endif
