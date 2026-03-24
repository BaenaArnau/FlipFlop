shader_type canvas_item;

uniform float progress : hint_range(0.0, 1.0) = 0.0;
uniform vec4 background_color = vec4(0.0, 0.0, 0.0, 1.0);

void vertex() {
	// Called for every vertex the material is visible on.
}

void fragment() {
	vec4 color = texture(TEXTURE, UV);
	
	vec4 bg_color = background_color;
	
	float alpha_progress = smoothstep(0.0, 1.0, progress);
	
	bg_color.a *= alpha_progress;
	
	COLOR = bg_color;
}

//void light() {
//	// Called for every pixel for every light affecting the material.
//	// Uncomment to replace the default light processing function with this one.
//}
