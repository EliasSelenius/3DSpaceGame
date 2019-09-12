#version 440 core

uniform sampler2D main_texture;

in vec2 uv;

out vec4 out_color;

void main() {
	out_color = vec4(1.0);
}