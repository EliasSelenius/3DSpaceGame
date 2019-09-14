#version 440 core

struct PointLight {
	vec3 pos;
	vec3 color;
};

uniform PointLight plight;
uniform sampler2D main_texture;
uniform vec3 cam_pos;

in vec2 uv;
in vec3 normal;
in vec3 fragPos;

out vec4 out_color;

void main() {
	// Ambient light:
	vec3 ambient = plight.color * .1;
	
	// Diffuse light:
	vec3 dir = normalize(plight.pos - fragPos);
	float diffuseScaler = max(dot(dir, normal), 0.);
	vec3 diffuse = plight.color * diffuseScaler;

	// specular light:
	vec3 camDir = normalize(cam_pos - fragPos);
	vec3 reflectvector = reflect(-dir, normal);
	float specularScaler = pow(max(dot(reflectvector, camDir), 0.), 64.);
	vec3 specular = plight.color * specularScaler;

	out_color = vec4(ambient + diffuse + specular, 1.);
}