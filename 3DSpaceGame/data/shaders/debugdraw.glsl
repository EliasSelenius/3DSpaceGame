
#version 440 core 

#define ShaderType

#ifdef SHADER_VERT

uniform mat4 cam_view;
uniform mat4 cam_projection;

in vec3 pos;
void main() {
	gl_Position = cam_projection * cam_view * vec4(pos, 1.0);
}

#endif
#ifdef SHADER_FRAG

out vec4 color;
void main() {
	color = vec4(1.);
}

#endif
