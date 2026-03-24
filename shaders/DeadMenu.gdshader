shader_type canvas_item;

// Parámetros del shader
uniform float progress : hint_range(0.0, 1.0) = 0.0;
uniform vec4 background_color = vec4(0.0, 0.0, 0.0, 1.0);

void vertex() {
	// Called for every vertex the material is visible on.
}

void fragment() {
	// Obtener el color de la textura (ColorRect usa colores sólidos)
	vec4 color = texture(TEXTURE, UV);
	
	// Usar el color de fondo negro configurado
	vec4 bg_color = background_color;
	
	// Aplicar efecto de fade in con suavizado (ease-out cubic)
	float alpha_progress = smoothstep(0.0, 1.0, progress);
	
	// Aplicar el progreso de aparición al alpha del fondo
	bg_color.a *= alpha_progress;
	
	COLOR = bg_color;
}

//void light() {
//	// Called for every pixel for every light affecting the material.
//	// Uncomment to replace the default light processing function with this one.
//}
