#include "linalg.h"

// Color functions
HSV ToHSV(const RGB &rgb)
{
    ld_t R = rgb.r, G = rgb.g, B = rgb.b;
    ld_t M = max(max(R, G), B);
    ld_t m = min(min(R, G), B);
    ld_t V = M/255;
    ld_t S = (M > 0 ? 1 - M/m : 0);
    ld_t H = acos((R - .5*G - .5*B)/sqrt(R*R + G*G + B*B - R*G - R*B - G*B))*DEGPERPI;
    if (B > G)
    {
        H = 360 - H;
    }

    return HSV(H, S, V);
}

RGB ToRGB(const HSV &hsv)
{
    ld_t H = hsv.h, S = hsv.s, V = hsv.v;
    ld_t M = 255*V;
    ld_t m = M*(1-S);
    ld_t z = (M-m)*(1 - fabs(fmod(H/60, 2) - 1));
    byte_t R, G, B;
    if (H < 60)
    {
        R = M;
        G = z + m;
        B = m;
    }
    else if (H < 120)
    {
        R = z + m;
        G = M;
        B = m;
    }
    else if (H < 180)
    {
        R = m;
        G = M;
        B = z + m;
    }
    else if (H < 240)
    {
        R = m;
        G = z + m;
        B = M;
    }
    else if (H < 300)
    {
        R = z + m;
        G = m;
        B = M;
    }
    else
    {
        R = M;
        G = m;
        B = z + m;
    }
    return RGB(R, G, B);
}

RGBA alphaBlend(const RGBA &front, const RGBA &back)
{
    //ld_t alpha = front.a;
    //RGB blended = front.rgb * alpha + back.rgb * (1. - alpha);
    return front; // TODO: FIX this function
}
