#include "scene2d.h"

#include <stdexcept>

using namespace std;

// Scene2d
Scene2d::Scene2d(ull_t fps, ld_t duration, const RGB &bgColor, const shared_ptr<Sprite> &bgSprite)
    : m_fps{fps}, m_camera{Vec2{0, 0}, 1, 0}, m_bgColor{bgColor}, m_bgSprite{bgSprite}
{
    if (fps > 0)
    {
        m_dt = 1. / fps;
        for (ull_t i = 0; i < duration*fps; i ++)
        {
            m_timeSeq.push_back(i * m_dt);
        }
    }
}

Scene2d::Scene2d(const std::shared_ptr<Sprite> &bgSprite)
    : m_camera{Vec2{0, 0}, 1, 0}, m_bgSprite{bgSprite}
{}

Scene2d::Scene2d(const RGB &bgColor)
    : m_camera{Vec2{0, 0}, 1, 0}, m_bgColor{bgColor}
{}

void Scene2d::addActor(const shared_ptr<Actor> &spActor)
{
    m_actors.insert(spActor);
}

void Scene2d::addActor(const std::vector<std::shared_ptr<Actor>> &spActors)
{
    for (const auto &spActor : spActors)
    {
        m_actors.insert(spActor);
    }
}

shared_ptr<Scene2d::Actor> Scene2d::addActor(const shared_ptr<Sprite> &spSprite, const Vec2 &pos, const uVec2 &size, ld_t rot)
{
    auto spActor = make_shared<Actor>(spSprite, pos, size, rot);
    m_actors.insert(spActor);
    return spActor;
}

/*
shared_ptr<Scene2d::Line> Scene2d::addLine(const Vec2 &start, const Vec2 &end, const RGBA &color, ull_t thickness)
{
    auto spLine = make_shared<Line>(start, end, color, thickness);
    m_actors.insert(spLine);
    return spLine;
}*/

bool Scene2d::removeActor(const std::shared_ptr<Actor> &spActor)
{
    return m_actors.erase(spActor);
}

ull_t Scene2d::getFps() const
{
    return m_fps;
}

Scene2d::Camera& Scene2d::getCamera()
{
    return m_camera;
}

const Scene2d::Camera& Scene2d::getCamera() const
{
    return m_camera;
}

const vector<ld_t>& Scene2d::getTimeSeq() const
{
    return m_timeSeq;
}

ld_t Scene2d::getDt() const
{
    return m_dt;
}

const unordered_set<shared_ptr<Scene2d::Actor>>& Scene2d::getActors() const
{
    return m_actors;
}

RGB Scene2d::getBgColor() const
{
    return m_bgColor;
}

shared_ptr<Sprite> Scene2d::getBgSprite() const
{
    return m_bgSprite;
}

const vector<FragShader>& Scene2d::getShaderQueue() const
{
    return m_shaderQueue;
}

void Scene2d::queueShader(const FragShader &shader)
{
    m_shaderQueue.push_back(shader);
}

void Scene2d::clearShaders()
{
    m_shaderQueue.clear();
}

// Scene2d::Camera
Scene2d::Camera::Camera(const Vec2 &center, ld_t zoom, ld_t rot)
    : m_center{center}, m_zoom{zoom}, m_rot{rot}
{}

void Scene2d::Camera::setCenter(const Vec2 &center)
{
    m_center = center;
}

void Scene2d::Camera::setZoom(ld_t zoom)
{
    m_zoom = zoom;
}

void Scene2d::Camera::setRot(ld_t rot)
{
    m_rot = rot;
}

void Scene2d::Camera::translate(const Vec2 &change)
{
    m_center += change;
}

void Scene2d::Camera::scaleZoom(ld_t zoomScale)
{
    m_zoom *= zoomScale;
}

void Scene2d::Camera::rotate(ld_t radChange)
{
    m_rot += radChange;
}

const Vec2& Scene2d::Camera::getCenter() const
{
    return m_center;
}

ld_t Scene2d::Camera::getZoom() const
{
    return m_zoom;
}

ld_t Scene2d::Camera::getRot() const
{
    return m_rot;
}

// Scene2d::Actor
Scene2d::Actor::Actor(const shared_ptr<Sprite> &spSprite, Vec2 pos, uVec2 size, ld_t rot)
    : m_spSprite{spSprite}, m_pos{pos}, m_size{size}, m_rot{rot}
{
    if (!m_spSprite)
    {
        throw runtime_error("Actor passed invalid sprite.");
    }

    if (m_size == uVec2{0, 0})
    {
        m_size = uVec2{m_spSprite->getWidth(), m_spSprite->getHeight()};
    }
}

void Scene2d::Actor::setSprite(const shared_ptr<Sprite> &spSprite)
{
    m_spSprite = spSprite;
}

void Scene2d::Actor::setPos(const Vec2 &pos)
{
    m_pos = pos;
}

void Scene2d::Actor::setSize(const uVec2 &size)
{
    m_size = size;
}

void Scene2d::Actor::setRot(ld_t rot)
{
    m_rot = rot;
}

void Scene2d::Actor::translate(const Vec2 &change)
{
    m_pos += change;
}

void Scene2d::Actor::scale(const fVec2 &scale)
{
    m_size = fVec2{m_size} * scale;
}

void Scene2d::Actor::scale(ld_t scale)
{
    m_size = fVec2{m_size} * scale;
}

void Scene2d::Actor::rotate(ld_t radChange)
{
    m_rot += radChange;
}

shared_ptr<Sprite> Scene2d::Actor::getSprite() const
{
    return m_spSprite;
}

const Vec2& Scene2d::Actor::getPos() const
{
    return m_pos;
}

const uVec2& Scene2d::Actor::getSize() const
{
    return m_size;
}

ull_t Scene2d::Actor::getWidth() const
{
    return m_size.x;
}
        
ull_t Scene2d::Actor::getHeight() const
{
    return m_size.y;
}

ld_t Scene2d::Actor::getRot() const
{
    return m_rot;
}

const vector<FragShader>& Scene2d::Actor::getShaderQueue() const
{
    return m_shaderQueue;
}
        
void Scene2d::Actor::queueShader(const FragShader &fragShader)
{
    m_shaderQueue.push_back(fragShader);
}

void Scene2d::Actor::clearShaders()
{
    m_shaderQueue.clear();
}

/* TODO: Implement lines using alphablend
// Scene2d::Line
Scene2d::Line::Line(const Vec2 &start, const Vec2 &end, const RGBA &color, ull_t thickness)
    : Actor(make_shared<Sprite>(uVec2{end - start}, color), Vec2((start + (end - start)) / 2))
{
    m_color = color;
    m_thickness = thickness;
    RGBA empty = {0, 0, 0, 0};

    // TODO: Line drawing
    for (ull_t i = 0; i < m_size.y; i++)
    {
        for (ull_t j = 0; j < m_size.x; j++)
        {
            if (i < thickness || j < thickness || i > m_size.y - thickness - 1 || j > m_size.x - thickness - 1)
            {
                m_spSprite->getPixMap()[i * m_size.x + j] = color;
            }
            else
            {
                m_spSprite->getPixMap()[i * m_size.x + j] = empty; 
            }
        }
    }
}

void Scene2d::Line::setColor(const RGBA &color)
{
    m_color = color;
}

void Scene2d::Line::setThickness(ull_t thickness)
{
    m_thickness = thickness;
}

RGBA Scene2d::Line::getColor() const
{
    return m_color;
}

ull_t Scene2d::Line::getThickness() const
{
    return m_thickness;
}*/
