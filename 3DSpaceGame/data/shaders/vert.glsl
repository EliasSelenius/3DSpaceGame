#version 440 core


uniform mat4 cam_view;
uniform mat4 cam_projection;

uniform mat4 obj_transform;

in vec3 v_pos;
in vec2 v_uv;
in vec3 v_normal;

out vec2 uv;
out vec3 normal;
out vec3 fragPos;

void main() {
	uv = v_uv;
	normal = v_normal;
	fragPos = (obj_transform * vec4(v_pos, 0.)).xyz;
	gl_Position = cam_projection * cam_view * obj_transform * vec4(v_pos, 1.0);
}