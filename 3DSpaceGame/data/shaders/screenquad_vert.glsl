#version 440 core

in vec2 vPos;
in vec2 texCoords;

out vec2 uv;

void main() {
	gl_Position = vec4(vPos.x, vPos.y, 0.0, 1.0);
	uv = texCoords;
}