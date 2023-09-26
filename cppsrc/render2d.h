#pragma once

#include <atomic>
#include <memory>
#include <thread>
#include <vector>

#include "frame.h"
#include "scene2d.h"
#include "shader.h"

#define SCENETHINKPARAMLIST ld_t time, ld_t dt
#define SceneThink(CAPTUREPARAMS ...) [CAPTUREPARAMS](SCENETHINKPARAMLIST)

typedef std::function<void(SCENETHINKPARAMLIST)> SceneThinkFunc;

class Render2d
{
    Render2d(const Render2d &) = delete;
    Render2d& operator=(const Render2d &) = delete;

    class SceneInstance
    {
        public:
        SceneInstance(const Scene2d &scene);

        const Scene2d::Camera& getCamera() const;

        const std::vector<Scene2d::Actor>& getActors() const;

        RGB getBgColor() const;

        std::shared_ptr<Sprite> getBgSprite() const;

        const std::vector<FragShader>& getShaderQueue() const;

        // Coordinate space transforms
        // Out -> In
        Vec2 screenToWorld(const uVec2 &screenSize, const uVec2 &screenCoord) const;
        Vec2 worldToActor(const Scene2d::Actor &actor, const Vec2 &worldCoord) const;
        Vec2 screenToActor(const uVec2 &screenSize, const Scene2d::Actor &actor, const uVec2 &screenCoord) const;

        private:
        const Scene2d::Camera m_camera;
        std::vector<Scene2d::Actor> m_actors;
        const RGB m_bgColor;
        const std::shared_ptr<Sprite> m_bgSprite;
        const std::vector<FragShader> m_shaderQueue; // Functions like a queue
    };
    
    public:
    Render2d(ull_t xRes, ull_t yRes, const std::shared_ptr<Scene2d> &spScene, ull_t numThreads = std::thread::hardware_concurrency());
    Render2d(uVec2 res, const std::shared_ptr<Scene2d> &spScene, ull_t numThreads = std::thread::hardware_concurrency());

    std::shared_ptr<Frame> renderFrameNum(ull_t frameNum) const;

    std::shared_ptr<Movie> renderAll() const;
    
    std::shared_ptr<Scene2d> getScene() const;
    
    void setNumThreads(ull_t numThreads);

    void bindThinkFunc(const SceneThinkFunc &stf);

    void unbindThinkFunc();

    std::shared_ptr<Frame> render(ld_t time = 0, bool verbose = true) const;

    private:
    std::shared_ptr<Frame> render(const SceneInstance &scene, ld_t time = 0, bool verbose = true) const;
    static void threadRender(const Render2d &renderer
            , const std::vector<Render2d::SceneInstance> &sceneInstances
            , const std::shared_ptr<Movie> &spMovie
            , std::atomic_ullong &aFrameIndex);

    ull_t m_xRes, m_yRes;
    std::shared_ptr<Scene2d> m_spScene;
    ull_t m_numThreads;
    SceneThinkFunc m_sceneThinkFunc;
};
