#include "render2d.h"

#include <chrono>
#include <iomanip>
#include <iostream>
#include <stdexcept>

#define NUMBARS 50

using namespace std;

// PrintBar helper
const string loadSeq = "|/-\\";
void printBar(ull_t frameIndex, ull_t numFrames, ull_t totalBars)
{
    static ull_t loadSeqInd = 0;
    ull_t numBars = (1. * frameIndex / numFrames) * totalBars;

    cout << "\r[";
    for (ull_t i = 0; i < numBars; i++)
    {
        cout << '|';
    }

    for (ull_t i = 0; i < totalBars - numBars; i++)
    {
        cout << ' ';
    }

    cout << "] " << frameIndex << '/' << numFrames 
         << " (" << fixed << setprecision(3) << 100.*frameIndex / numFrames << "%) "
         << (frameIndex == numFrames ? ' ' : loadSeq[(loadSeqInd++) % loadSeq.size()]);
    cout.flush();
}

// Render2d
Render2d::Render2d(ull_t xRes, ull_t yRes, const shared_ptr<Scene2d> &spScene, ull_t numThreads)
    : m_xRes{xRes}, m_yRes{yRes}, m_spScene{spScene}, m_numThreads{numThreads},
    m_sceneThinkFunc{SceneThink(){return;}}
{
    if (m_spScene == nullptr)
    {
        throw runtime_error("Invalid scene provided to renderer.");
    }

    if (xRes < 1 || yRes < 1)
    {
         throw runtime_error("Invalid image size.");
    }
}

Render2d::Render2d(uVec2 res, const shared_ptr<Scene2d> &spScene, ull_t numThreads)
    : Render2d{res.x, res.y, spScene, numThreads}
{}

shared_ptr<Frame> Render2d::render(ld_t time, bool verbose) const
{
    return render(*m_spScene, time, verbose);
}

shared_ptr<Frame> Render2d::render(const Render2d::SceneInstance &scene, ld_t time, bool verbose) const
{
    auto spOutput = make_shared<Frame>(m_xRes, m_yRes);
    uVec2 screenRes(m_xRes, m_yRes);

    if (verbose)
    {
        cout << "\nBeginning actor render (" << scene.getActors().size() << " total)...\n";
    }

    auto start = chrono::high_resolution_clock::now(), end = start;

    // Actor rendering
    RGBA bgColor{scene.getBgColor(), 255};
    auto spBgSprite = scene.getBgSprite();
    ll_t bgSpriteLeft = -(spBgSprite->getWidth() / 2);
    ll_t bgSpriteTop = spBgSprite->getHeight() / 2;
    RGBA *bgSpritePixMap = spBgSprite->getPixMap();
    for (ull_t i = 0; i < m_yRes; i++)
    {
        for (ull_t j = 0; j < m_xRes; j++)
        {
            Vec2 worldLoc = scene.screenToWorld(screenRes, uVec2{j, i});

            // Sample bgColor, then overwrite with bgSprite
            RGBA outColor = bgColor;
            
            if (spBgSprite->getWidth() != 0 && spBgSprite->getHeight() != 0)
            {
                Vec2 pixMapIndInt = worldLoc - Vec2{bgSpriteLeft, bgSpriteTop};
                uVec2 pixMapVInd = uVec2{ull_t(pixMapIndInt.x) % spBgSprite->getWidth()
                    , ull_t(-pixMapIndInt.y) % spBgSprite->getHeight()};
                ull_t pixMapInd = pixMapVInd.y * spBgSprite->getWidth() + pixMapVInd.x;
                outColor = alphaBlend(bgSpritePixMap[pixMapInd], outColor);
            }

            // Now sample from each actor, TODO: Make this more efficient
            for (const auto &actor : scene.getActors())
            {
                Vec2 actorLoc = scene.worldToActor(actor, worldLoc);

                // Now convert to top-left relative for sprite sample
                actorLoc -= Vec2{-ll_t(actor.getSize().x / 2), ll_t(actor.getSize().y / 2)};
                uVec2 actorTlLoc{ull_t(actorLoc.x), ull_t(-actorLoc.y)};

                // Did not intersect actor
                if (actorTlLoc.x >= actor.getWidth() || actorTlLoc.y >= actor.getHeight())
                {
                    continue;
                }

                // Now sample actor's sprite and blend it with previous color
                const auto spSprite = actor.getSprite();
                const RGBA* pActorPixMap = spSprite->getPixMap();
                uVec2 spriteInd = (fVec2{actorTlLoc}/actor.getSize()) * spSprite->getSize();
                RGBA sampledColor = pActorPixMap[spriteInd.y * spSprite->getSize().x + spriteInd.x];

                // Sample from shader passes
                for (const FragShader &fs : actor.getShaderQueue())
                {
                    fs(sampledColor, sampledColor, spriteInd, spSprite->getSize(), time);
                }

                outColor = alphaBlend(sampledColor, outColor);
            }

            (*spOutput)[(screenRes.y - i - 1) * m_xRes + j] = outColor;
        }
    }


    if (verbose)
    {
        end = chrono::high_resolution_clock::now();
        cout << "Done! (" << chrono::duration_cast<chrono::duration<double>>(end - start).count() << "s)\n";

        cout << "\nBeginning screen-space shader pass...\n";
        start = chrono::high_resolution_clock::now();
    }


    // Finally, screen-space shader pass
    for (const FragShader &fs : scene.getShaderQueue())
    {
        for (size_t i = 0; i < m_yRes; i++)
        {
            for (size_t j = 0; j < m_xRes; j++)
            {
                ull_t ind = (screenRes.y - i - 1) * m_xRes + j;
                RGBA color = {(*spOutput)[ind]};
                fs(color, color, uVec2{j, i}, screenRes, time);
                (*spOutput)[ind] = color;
            }
        }
    }

    if (verbose)
    {
        end = chrono::high_resolution_clock::now();
        cout << "Done! (" << chrono::duration_cast<chrono::duration<double>>(end - start).count()
            << "s)\n\nRender complete.\n";
    }

    return spOutput;
}

shared_ptr<Frame> Render2d::renderFrameNum(ull_t frameNum) const
{
    if (frameNum >= m_spScene->getTimeSeq().size())
    {
        throw runtime_error("Invalid frame num greater max.");
    }

    cout << "\nBeginning simulation... ";
    auto start = chrono::high_resolution_clock::now();

    vector<SceneInstance> sceneInstances;
    sceneInstances.reserve(m_spScene->getTimeSeq().size());
    ull_t curFrameNum = 1;
    for (; curFrameNum <= frameNum; curFrameNum++)
    {
        m_sceneThinkFunc(m_spScene->getTimeSeq()[curFrameNum], m_spScene->getDt());
    }


    auto end = chrono::high_resolution_clock::now();

    cout << "Done! (" << chrono::duration_cast<chrono::duration<double>>(end - start).count()
        << "s)\n";

    return render(m_spScene->getTimeSeq()[curFrameNum], true);
}

shared_ptr<Scene2d> Render2d::getScene() const
{
    return m_spScene;
}

void Render2d::threadRender(const Render2d &renderer, const vector<Render2d::SceneInstance> &sceneInstances, const shared_ptr<Movie> &spMovie, atomic_ullong &aFrameIndex)
{
    ull_t numFrames = spMovie->getNumFrames();
    for (ull_t frameInd = aFrameIndex++; frameInd < numFrames; frameInd = aFrameIndex++)
    {
        spMovie->writeFrame(renderer.render(sceneInstances[frameInd], renderer.getScene()->getTimeSeq()[frameInd], false), frameInd);
    }
}


shared_ptr<Movie> Render2d::renderAll() const
{
    if (m_spScene->getTimeSeq().empty())
    {
        throw runtime_error("Movie render attempted on scene with no time sequence. Did you use the correct scene constructor?");
    }

    if (m_numThreads <= 0)
    {
        throw runtime_error("Invalid number of threads given (" + to_string(m_numThreads) + ")!");
    }

    cout << "\nBeginning simulation... ";
    auto start = chrono::high_resolution_clock::now();

    vector<SceneInstance> sceneInstances;
    sceneInstances.reserve(m_spScene->getTimeSeq().size());
    for (ld_t time : m_spScene->getTimeSeq())
    {
        sceneInstances.emplace_back(*m_spScene);
        m_sceneThinkFunc(time, m_spScene->getDt());
    }
    sceneInstances.emplace_back(*m_spScene);

    auto end = chrono::high_resolution_clock::now();

    cout << "Done! (" << chrono::duration_cast<chrono::duration<double>>(end - start).count()
        << "s)\n";

    auto spMovie = make_shared<Movie>(m_xRes, m_yRes, m_spScene->getFps(), m_spScene->getTimeSeq().size());
    ull_t numFrames = spMovie->getNumFrames();
    cout << "\nBeginning render " << '(' << m_numThreads << " thread" << (m_numThreads > 1 ? "s" : "") << "): "
        << spMovie->getWidth() << 'x' << spMovie->getHeight() << " @ " << spMovie->getFps() << " -> "
        << numFrames << " frames (" << spMovie->getDuration() << "s)\n";

    start = chrono::high_resolution_clock::now();
    atomic_ullong aFrameIndex(0);
    vector<thread> renderThreads;

    for (ull_t i = 0; i < m_numThreads; i++)
    {
        renderThreads.emplace_back(threadRender, ref(*this), ref(sceneInstances), ref(spMovie), ref(aFrameIndex));
    }

    // Wait for completion
    while (aFrameIndex < numFrames)
    {
        printBar(aFrameIndex, numFrames, NUMBARS);
        this_thread::sleep_for(chrono::milliseconds(500));
    }

    for (auto &rt : renderThreads)
    {
        rt.join();
    }

    printBar(numFrames, numFrames, NUMBARS);

    end = chrono::high_resolution_clock::now();

    cout << "\nDone! (" << chrono::duration_cast<chrono::duration<double>>(end - start).count()
        << "s)\n\nRender complete.\n";

    return spMovie;
}


void Render2d::setNumThreads(ull_t numThreads)
{
    m_numThreads = numThreads;
}

void Render2d::bindThinkFunc(const SceneThinkFunc &stf)
{
    m_sceneThinkFunc = stf;
}

void Render2d::unbindThinkFunc()
{
    m_sceneThinkFunc = SceneThink(){return;};
}

// Render2d::SceneInstance
Render2d::SceneInstance::SceneInstance(const Scene2d &scene)
    : m_camera{scene.getCamera()}, m_actors{}, m_bgColor{scene.getBgColor()}, m_bgSprite{scene.getBgSprite()}, m_shaderQueue{scene.getShaderQueue()}
{
    m_actors.reserve(scene.getActors().size());
    for (const auto &spActor : scene.getActors())
    {
        m_actors.push_back(*spActor);
    }
}

const Scene2d::Camera& Render2d::SceneInstance::getCamera() const
{
    return m_camera;
}

const vector<Scene2d::Actor>& Render2d::SceneInstance::getActors() const
{
    return m_actors;
}

RGB Render2d::SceneInstance::getBgColor() const
{
    return m_bgColor;
}

shared_ptr<Sprite> Render2d::SceneInstance::getBgSprite() const
{
    return m_bgSprite;
}

const vector<FragShader>& Render2d::SceneInstance::getShaderQueue() const
{
    return m_shaderQueue;
}

// Coordinate space transforms
Vec2 Render2d::SceneInstance::screenToWorld(const uVec2 &screenSize, const uVec2 &screenCoord) const
{
    return (Vec2{ll_t(screenCoord.x - ll_t(screenSize.x / 2)),
            ll_t(screenCoord.y - ll_t(screenSize.y / 2))} / m_camera.getZoom() + m_camera.getCenter()).rot(m_camera.getRot());
}

Vec2 Render2d::SceneInstance::worldToActor(const Scene2d::Actor &actor, const Vec2 &worldCoord) const
{
    return (worldCoord - actor.getPos()).rot(-actor.getRot());
}

Vec2 Render2d::SceneInstance::screenToActor(const uVec2 &screenSize, const Scene2d::Actor &actor, const uVec2 &screenCoord) const
{
    return worldToActor(actor, screenToWorld(screenSize, screenCoord));
}
