#version 440 core 

#define ShaderType

#ifdef SHADER_VERT

uniform mat4 cam_view;
uniform mat4 cam_projection;

layout (location = 0) in vec3 v_pos;

out vec3 texCoords;

void main() {
	vec4 pos = cam_projection * mat4(mat3(cam_view)) * vec4(v_pos, 1.0);

	gl_Position = pos.xyww;
	texCoords = v_pos;
}

#endif
#ifdef SHADER_FRAG

uniform samplerCube skybox;

in vec3 texCoords;

out vec4 color;
void main() {
	color = texture(skybox, texCoords);
}

#endif
