//UNITY_SHADER_NO_UPGRADE
#ifndef VOLUMETRICSCATTERING_INCLUDE
#define VOLUMETRICSCATTERING_INCLUDE

float4 clouds(UnityTexture3D _volumeTex, UnitySamplerState _volumeSampler, float _ditherValue, float2 _tile, float3 _L, float3 _ro, float3 _rd, float _time, float _hgf, float _absorbtion, float3 _scatter)
{
    float4 sl = float4(0., 0., 0., 1.);
    float tmin = -1., tmax = -1.;
    float st = 1.;
    //return vec4(st, st, st, 0.);
    float t = 0.01;
    t += st* .5* _ditherValue;

    for (int i = 0; i < 16; ++i)
    {
	float3 p = _ro + _rd * t;
	float dist = length(p);
	float4 den = SAMPLE_TEXTURE3D(_volumeTex, _volumeSampler, p+_time).rgba;
	float volume = SAMPLE_TEXTURE3D(_volumeTex, _volumeSampler, _tile.x*p + _time).g;
	den.r *= volume* volume;
	den.r *= exp(-0.1*dist);
	if (den.r > 0.01)
	{
	    float d = 0.5;
	    float transmittance = sl.a;
	    float stepL = 0.5*st;
	    for (int j = 0; j < 8; ++j)
	    {
		float3 pm = p + _L * d;

		float4 denm = SAMPLE_TEXTURE3D(_volumeTex, _volumeSampler, _tile.y*pm + _time).rgba;
		if (denm.r > 0.01)
		{
		    transmittance *= exp(-_absorbtion *stepL*denm.r);
		}
		if (transmittance < 0.01)
		    break;
		d += stepL;
	    }
	    float3 lum = (
		0.2
		+ _scatter
		)*den.r*_hgf*transmittance;
	    sl.rgb += sl.a*(0., lum - transmittance * lum) / max(0.01, den.r);

	    sl.a = transmittance;
	}
	t += st;
    }
    return sl;
}

float phaseHG(float _g, float _cosTeta)
{
    return (1. - _g * _g) / (4.*3.14*pow(max(0., 1. + _g * _g - 2.*_g*_cosTeta), 1.5));
}

void VolumetricScattering_float(UnityTexture3D _volumeTex, UnitySamplerState _volumeSampler, float _ditherValue, float2 _tile, float _time, float _absorbtion, float3 _lightDir, float3 _viewOrigin, float3 _viewDir, out float4 _scatterAndAbsorb)
{
    _scatterAndAbsorb = float4(0., 0., 0., 1.);
    float cosTeta = max(0., dot(_viewDir, _lightDir));
    float hgf = clamp(lerp(phaseHG(-0.3, cosTeta), phaseHG(0.6, cosTeta), .5), 0., 1.);
    _scatterAndAbsorb = clouds(_volumeTex, _volumeSampler, _ditherValue, _tile, _lightDir, _viewOrigin, _viewDir, _time, hgf, _absorbtion, 50.*float3(1., 1., 1.));
    //_scatterAndAbsorb = float4(hgf, hgf, hgf, 0.);
}

#endif