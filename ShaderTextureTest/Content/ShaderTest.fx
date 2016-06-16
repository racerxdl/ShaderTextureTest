#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

sampler TextureSampler : register(s0);
float time;
float2 resolution;

float4 main(float4 pos : SV_Position, float4 color : COLOR0, float2 texCoord : TEXCOORD0) : COLOR0
{
    float2 p = (pos.xy / resolution.xy) - 0.5;
    float sx = 0.2* (p.x + 0.5) * sin( 50.0 * p.x - 10.f * time);
    float dy = 1.f/ ( 50.f * abs(p.y  - sx) );
    dy += 1.f/ (60.f  * length(p - float2(p.x, 0.)));
    return float4( (p.x + 0.4) * dy, 0.9 * dy, dy, 1.0 );
}


technique Technique1
{
    pass Pass1
    {
        PixelShader = compile PS_SHADERMODEL main();
    }
}