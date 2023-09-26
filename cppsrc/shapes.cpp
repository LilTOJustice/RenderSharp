#include "shapes.h"

#define cimg_display 0
#define cimg_use_png
#include "CImg.h"

#include <iostream>

using namespace std;
using namespace cimg_library;

// Sprite
Sprite::Sprite(const uVec2 &size, const RGBA &color)
    : m_size{size}, m_pPixMap{new RGBA[size.x * size.y]}
{
    for (ull_t i = 0; i < size.y; i++)
    {
        for (ull_t j = 0; j < size.x; j++)
        {
            m_pPixMap[i * size.x + j] = color;
        }
    }
}

Sprite::Sprite(const string filename)
{
    CImg<byte_t> img(filename.c_str());

    m_size = uVec2{ull_t(img.width()), ull_t(img.height())};

    ull_t imgSize = m_size.x * m_size.y;
    ull_t spectrum = img.spectrum();
    m_pPixMap = new RGBA[imgSize];

    switch(spectrum)
    {
        case 1:
            for (ull_t i = 0; i < imgSize; i++)
            {
                m_pPixMap[i].r = img[i];
                m_pPixMap[i].g = 255;
                m_pPixMap[i].b = 255;
                m_pPixMap[i].a = 255; 
            }
            break;

        case 2:
            for (ull_t i = 0; i < imgSize; i++)
            {
                m_pPixMap[i].r = img[i];
                m_pPixMap[i].g = img[i + imgSize];
                m_pPixMap[i].b = 255;
                m_pPixMap[i].a = 255;
            }
            break;

        case 3:
            for (ull_t i = 0; i < imgSize; i++)
            {
                m_pPixMap[i].r = img[i];
                m_pPixMap[i].g = img[i + imgSize];
                m_pPixMap[i].b = img[i + 2 * imgSize];
                m_pPixMap[i].a = 255;
            }
            break;

        default:
            for (ull_t i = 0; i < imgSize; i++)
            {
                m_pPixMap[i].r = img[i];
                m_pPixMap[i].g = img[i + imgSize];
                m_pPixMap[i].b = img[i + 2 * imgSize];
                m_pPixMap[i].a = img[i + 3 * imgSize];
            }
    }
}

uVec2 Sprite::getSize() const
{
    return m_size;
}

ull_t Sprite::getWidth() const
{
    return m_size.x;
}

ull_t Sprite::getHeight() const
{
    return m_size.y;
}

const RGBA* Sprite::getPixMap() const
{
    return m_pPixMap;
}

RGBA* Sprite::getPixMap()
{
    return m_pPixMap;
}

Sprite::~Sprite()
{
    delete[] m_pPixMap;
}
