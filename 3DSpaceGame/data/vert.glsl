#version 440 core


uniform mat4 cam_view;
uniform mat4 cam_projection;

uniform mat4 obj_transform;

in vec3 v_pos;
in vec2 v_uv;

out vec2 uv;

void main() {
	uv = v_uv;
	gl_Position = cam_projection * cam_view * obj_transform * vec4(v_pos, 1.0);
}