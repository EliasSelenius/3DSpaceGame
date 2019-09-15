﻿#version 440 core


//======DATASTRUCTURES========

struct PointLight {
	vec3 pos;
	vec3 color;

	float constant;
	float linear;
	float quadratic;
};

struct DirLight {
	vec3 dir;
	vec3 color;
};

struct Material {
	vec3 ambient;
	vec3 diffuse;
	vec3 specular;
	float shininess;
};

//=============================

vec3 CalcDirLight(DirLight l, vec3 n, vec3 viewDir);
vec3 CalcPointLight(PointLight l, vec3 n, vec3 fp, vec3 viewDir);

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

	color += CalcDirLight(dirLight, normal, camdir);
	//color += CalcPointLight(pointLight, normal, fragPos, camdir);

	out_color = vec4(color, 1.);
}


vec3 CalcDirLight(DirLight l, vec3 n, vec3 viewDir) {
	vec3 lightDir = normalize(-l.dir);
	// diffuse
	float diffscale = max(dot(n, lightDir), 0.);
	// specular
	float specscale = pow(max(dot(viewDir, reflect(-lightDir, n)), 0.), material.shininess * 128.);

	vec3 ambient = l.color * material.ambient;
	vec3 diffuse = l.color * (material.diffuse * diffscale);
	vec3 specular = l.color * (material.specular * specscale);

	return (ambient + diffuse + specular);
}

vec3 CalcPointLight(PointLight l, vec3 n, vec3 fp, vec3 viewDir) {
	vec3 lightDir = normalize(l.pos - fp);
	// diffuse
	float diffscale = max(dot(n, lightDir), 0.);
	// specular
	float specscale = pow(max(dot(viewDir, reflect(-lightDir, n)), 0.), material.shininess * 128.);
	// attenuation
	float lightdist = length(l.pos - fp);
	float attenuation = 1. / (l.constant + l.linear * lightdist + l.quadratic * (lightdist * lightdist));

	vec3 ambient = l.color * material.ambient * attenuation;
	vec3 diffuse = l.color * material.diffuse * diffscale * attenuation;
	vec3 specular = l.color * material.specular * specscale * attenuation;
	return ambient + diffuse + specular;
}


/*

	// Ambient light:
	vec3 ambient = light.color * material.ambient;
	
	// Diffuse light:
	vec3 dir = normalize(-light.dir);
	float diffuseScaler = max(dot(dir, normal), 0.);
	vec3 diffuse = light.color * (diffuseScaler * material.diffuse);

	// specular light:
	vec3 camDir = normalize(cam_pos - fragPos);
	vec3 reflectvector = reflect(-dir, normal);
	float specularScaler = pow(max(dot(reflectvector, camDir), 0.), material.shininess * 128.);
	vec3 specular = light.color * (specularScaler * material.specular);

	out_color = vec4(ambient + diffuse + specular, 1.);

*/