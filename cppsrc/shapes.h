#pragma once

#include <string>
#include <vector>

#include "linalg.h"
#include "shader.h"

class Solid
{

};

class Sprite
{
    public:
    Sprite(const uVec2 &size, const RGBA &color = RGBA{});
    Sprite(const std::string filename);

    uVec2 getSize() const;
    ull_t getWidth() const;
    ull_t getHeight() const;
    const RGBA* getPixMap() const;
    RGBA* getPixMap();

    ~Sprite();

    private:
    uVec2 m_size;
    RGBA* m_pPixMap;
};

