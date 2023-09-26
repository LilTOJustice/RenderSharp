#pragma once

#include <memory>
#include <unordered_set>
#include <vector>

#include "shapes.h"

class Scene2d
{

    public:
    class Camera
    {
        public:
        Camera(const Vec2 &center, ld_t zoom, ld_t rot);

        void setCenter(const Vec2 &center);

        void setZoom(ld_t zoom);
        
        void setRot(ld_t rot);
        
        void translate(const Vec2 &change);

        void scaleZoom(ld_t zoom);

        void rotate(ld_t radChange);

        const Vec2& getCenter() const;
        
        ld_t getZoom() const;
        
        ld_t getRot() const;

        private:
        Vec2 m_center;
        ld_t m_zoom;
        ld_t m_rot;
    };

    class Actor
    {
        public:
        Actor(const std::shared_ptr<Sprite> &spSprite = nullptr, Vec2 pos = {}, uVec2 size = {}, ld_t rot = 0);

        void setSprite(const std::shared_ptr<Sprite> &spSprite);

        void setPos(const Vec2 &pos);

        void setSize(const uVec2 &size);

        void setRot(ld_t rot);

        void translate(const Vec2 &change);

        void scale(const fVec2 &scale);
        void scale(ld_t scale);

        void rotate(ld_t radChange);

        std::shared_ptr<Sprite> getSprite() const;

        const Vec2& getPos() const;

        const uVec2& getSize() const;

        ull_t getWidth() const;
        
        ull_t getHeight() const;

        ld_t getRot() const;

        const std::vector<FragShader>& getShaderQueue() const;

        void queueShader(const FragShader &fragShader);
        
        void clearShaders();

        protected:
        std::shared_ptr<Sprite> m_spSprite;
        Vec2 m_pos;
        uVec2 m_size;
        ld_t m_rot;
        std::vector<FragShader> m_shaderQueue;
    };

    /* TODO: Implement lines using alphablend
    class Line : public Actor
    {
        public:
        void setSprite(const std::shared_ptr<Sprite> &spSprite) = delete;
        void setPos(const Vec2 &pos) = delete;
        void setSize(const uVec2 &size) = delete;
        void setRot(ld_t rot) = delete;
        void translate(const Vec2 &change) = delete;
        void scale(const fVec2 &scale) = delete;
        void rotate(ld_t radChange) = delete;
        const Vec2& getPos() const = delete;
        const uVec2& getSize() const = delete;
        ld_t getRot() const = delete;

        Line(const Vec2 &start, const Vec2 &end, const RGBA &color = RGBA{}, ull_t thickness = 0);

        void setColor(const RGBA &color);

        void setThickness(ull_t thickness);

        RGBA getColor() const;

        ull_t getThickness() const;

        private:
        RGBA m_color;
        ull_t m_thickness;
    };*/

    Scene2d(ull_t framerate, ld_t duration, const RGB &bgColor = RGB{0, 0, 0}, const std::shared_ptr<Sprite> &bgSprite = std::make_shared<Sprite>(uVec2{1, 1})); // For Movie rendering or single-frame rendering
    Scene2d(const std::shared_ptr<Sprite> &bgSprite = std::make_shared<Sprite>(uVec2{1, 1}));
    Scene2d(const RGB &bgColor = RGB{0, 0, 0}); // For single-frame rendering

    void addActor(const std::shared_ptr<Actor> &spActor);
    void addActor(const std::vector<std::shared_ptr<Actor>> &spActor);

    std::shared_ptr<Actor> addActor(const std::shared_ptr<Sprite> &spSprite, const Vec2 &pos = {}, const uVec2 &size = {}, ld_t rot = {});
    //std::shared_ptr<Line> addLine(const Vec2 &start, const Vec2 &end, const RGBA &color = RGBA{}, ull_t thickness = 0);

    bool removeActor(const std::shared_ptr<Actor> &spActor);

    ull_t getFps() const;

    Camera& getCamera();
    const Camera& getCamera() const;

    const std::vector<ld_t>& getTimeSeq() const;

    ld_t getDt() const;

    const std::unordered_set<std::shared_ptr<Actor>>& getActors() const;

    RGB getBgColor() const;

    std::shared_ptr<Sprite> getBgSprite() const;

    const std::vector<FragShader>& getShaderQueue() const;

    void queueShader(const FragShader &shader);

    void clearShaders();

    private:
    ull_t m_fps;
    Camera m_camera;
    std::vector<ld_t> m_timeSeq;
    ld_t m_dt;
    std::unordered_set<std::shared_ptr<Actor>> m_actors;
    RGB m_bgColor;
    std::shared_ptr<Sprite> m_bgSprite;
    std::vector<FragShader> m_shaderQueue; // Functions like a queue
};
