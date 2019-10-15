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


vec3 CalcDirLight(DirLight l, vec3 n, vec3 viewDir, Material material) {
	vec3 lightDir = normalize(-l.dir);
	// diffuse
	float diffscale = max(dot(n, lightDir), 0.);
	// specular
	vec3 halfwaydir = normalize(lightDir + viewDir);
	float specscale = pow(max(dot(n, halfwaydir), 0.), material.shininess * 128. * 3.);

	vec3 ambient = l.color * material.ambient;
	vec3 diffuse = l.color * (material.diffuse * diffscale);
	vec3 specular = l.color * (material.specular * specscale);

	return (ambient + diffuse + specular);
}

// note this only uses Phong and not blinn-phong
vec3 CalcPointLight(PointLight l, vec3 n, vec3 fp, vec3 viewDir, Material material) {
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