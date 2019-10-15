#version 440 core

#include "light.glsl"


uniform DirLight dirLight;
//uniform PointLight pointLight;
uniform vec3 cam_pos;

uniform Material material;

in vec2 uv;
in vec3 normal;
in vec3 fragPos;

out vec4 out_color;

void main() {
	vec3 color = vec3(0.);
	vec3 camdir = normalize(cam_pos - fragPos);

	color += CalcDirLight(dirLight, normal, camdir, material);
	//color += CalcPointLight(pointLight, normal, fragPos, camdir);

	out_color = vec4(color, 1.);
}


