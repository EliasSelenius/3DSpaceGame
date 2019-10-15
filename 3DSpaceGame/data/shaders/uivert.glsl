#version 440 core

uniform vec4 rectTransform;

layout (location = 0) in vec2 pos;

void main() {
	gl_Position = vec4(rectTransform.xy + (pos * rectTransform.zw), 0.0, 1.0);
}
