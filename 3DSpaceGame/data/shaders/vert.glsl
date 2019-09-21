#version 440 core


uniform mat4 cam_view;
uniform mat4 cam_projection;

uniform mat4 obj_transform;

layout (location = 0) in vec3 v_pos;
layout (location = 1) in vec2 v_uv;
layout (location = 2) in vec3 v_normal;

out vec2 uv;
out vec3 normal;
out vec3 fragPos;

void main() {
	uv = v_uv;
	normal = v_normal;
	fragPos = (obj_transform * vec4(v_pos, 0.)).xyz;
	gl_Position = cam_projection * cam_view * obj_transform * vec4(v_pos, 1.0);
}