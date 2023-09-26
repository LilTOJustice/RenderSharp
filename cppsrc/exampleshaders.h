#include <complex>
#include <cmath>

#include <iostream>

#include "shader.h"

constexpr int MAXITS = 100;
ld_t mandel(const std::complex<ld_t> &c)
{
    int its = 0;
    std::complex<ld_t> res = c;
    ld_t absolute = 0;
    while ((absolute = abs(res)) < 2 && its++ < MAXITS)
    {
        res = res * res + c;
    }

    return absolute;
}

ld_t multi(const std::complex<ld_t> &c, const ld_t exponent)
{
    int its = 0;
    std::complex<ld_t> res = c;
    ld_t absolute = 0;
    while ((absolute = abs(res)) < 2 && its++ < MAXITS)
    {
        res = std::pow(res, exponent) + c;
    }

    return absolute;
}

FragShader mandelbrot =
Frag()
{
    fVec2 st = fVec2{fragCoord} / res;
    st.x *= 1. * res.x/res.y;
    st = (st - fVec2{1.25, .5}) / .2;
    std::complex<ld_t> c(st.x, st.y);
    ld_t mandelOut = mandel(c);
    if (mandelOut < 2)
    {
        out = RGBA(0, 0, 0, 255);
    }
    else
    {
        out = RGBA(ToRGB(HSV(mandelOut * 5, 1, 1)), 255);
    }
};

FragShader multibrot =
Frag()
{
    fVec2 st = fVec2{fragCoord} / res;
    st.x *= 1. * res.x / res.y;
    st = (st - fVec2{1.25, .5}) / .2;
    std::complex<ld_t> c(st.x, st.y);
    ld_t exponent = 3 * cos(time) + 4;
    ld_t multiOut = multi(c, exponent);
    if (multiOut < 2)
    {
        out = RGBA(0, 0, 0, 255);
    }
    else
    {
        out = RGBA(ToRGB(HSV(multiOut * 5, 1, 1)), 255);
    }
};

FragShader rainbow =
Frag()
{
    out = in * fVec4{ToRGB(HSV(fmod(180 * time, 360), 1, 1))/255., 1.};
};
