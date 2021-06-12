//UNITY_SHADER_NO_UPGRADE
#ifndef FLOWMAP_INCLUDE
#define FLOWMAP_INCLUDE

void FlowMap_float(
    float _blend_cycle, float _cycle_speed,
    float _offset, float _time,
    float2 _flow, float2 _flow_scale,
    float2 _base_uv,
    float _flow_speed,
    out float _blend_factor,
    out float2 _ouv1, out float2 _ouv2)
{
    // Compute cycle, phases
    float half_cycle = _blend_cycle * 0.5;

    float phase0 = fmod(_offset + _time * _cycle_speed, _blend_cycle);
    float phase1 = fmod(_offset + _time * _cycle_speed + half_cycle, _blend_cycle);

    // Blend factor to mix the two layers
    _blend_factor = abs(half_cycle - phase0) / half_cycle;

    // Offset by halfCycle to improve the animation for color (for normalmap not absolutely necessary)
    phase0 -= half_cycle;
    phase1 -= half_cycle;

    // Multiply with scale to make flow speed independent from the uv scaling
    _flow *= _flow_speed * _flow_scale;

    _ouv1 = _flow * phase0 + _base_uv;
    _ouv2 = _flow * phase1 + _base_uv;
}

#endif