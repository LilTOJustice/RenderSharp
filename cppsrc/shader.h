#pragma once

#include <functional>

#include "linalg.h"

// Frag shaders
#define FRAGPARAMLIST const RGBA in, RGBA &out, const uVec2 &fragCoord, const uVec2 &res, const ld_t time
#define Frag(CAPTUREPARAMS ...) [CAPTUREPARAMS](FRAGPARAMLIST)

typedef std::function<void(FRAGPARAMLIST)> FragShader;

// This is how to write a shader
/*
FragShader Test =
Frag() // In between (), put variables in the definition scope that are to be captured,
{      // similar to uniforms in GLSL
    out = RGBA{0, 0, 0, 0};
};
*/
