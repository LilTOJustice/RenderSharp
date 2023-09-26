#include "frame.h"

#include <chrono>
#include <filesystem>
#include <iostream>

using namespace std;
using namespace cimg_library;

// Frame
Frame::Frame(ull_t width, ull_t height)
    : m_width{width}, m_height{height}, m_colorStride{width*height}, m_size{m_colorStride * 4},
    m_aspectRatio{(ld_t)width/(ld_t)height}, m_pImage{new byte_t[m_size]}
{}

void Frame::output(string filename) const
{
    string fullname = filename + ".png";
    cout << "\nExporting frame as image: " << fullname << "...\n";
    auto start = chrono::high_resolution_clock::now();
    CImg<byte_t> out(m_pImage, m_width, m_height, 1, 3);
    out.save_png(fullname.c_str());
    cout << "Done! (" << chrono::duration_cast<chrono::duration<double>>(chrono::high_resolution_clock::now()-start).count() << "s)\n";
}

Frame::RGBARef Frame::operator[](ull_t index)
{
    return Frame::RGBARef{
        m_pImage[index],                                                // r
        m_pImage[index + m_colorStride],                                // g
        m_pImage[index + m_colorStride + m_colorStride],                // b
        m_pImage[index + m_colorStride + m_colorStride + m_colorStride] // a
    };
}

ull_t Frame::getHeight() const
{
    return m_height;
}

ull_t Frame::getWidth() const
{
    return m_width;
}

ld_t Frame::getAspect() const
{
    return m_aspectRatio;
}

Frame::~Frame()
{
    delete[] m_pImage;
}

// Movie
ull_t Movie::m_nextId = 0;
Movie::Movie(ull_t width, ull_t height, ull_t fps, ull_t numFrames)
    : m_width{width}, m_height{height}, m_fps{fps}, m_numFrames{numFrames}, m_movieId{m_nextId++}, m_colorStride{width*height},
    m_imgSize{m_colorStride * 4}, m_aspectRatio{(ld_t)width/(ld_t)height}, m_duration{(ld_t)(numFrames)/fps},
    m_tempDir{"temp" + to_string(m_movieId)}
{
    filesystem::create_directory(m_tempDir);
}

void Movie::output(string filename) const
{
    string fullname = filename + ".mp4";
    cout << "\nExporting movie: " << fullname << "...\n";

    auto start = chrono::high_resolution_clock::now();

    string cmd = "ffmpeg -y -v -8 -framerate " + to_string(m_fps)
        + " -f image2 -i temp" + to_string(m_movieId)
        + "/%d.bmp -c h264 -pix_fmt yuv420p -b:v 32768k "
        + fullname;

    cout << cmd << "\n\n";

    if (system(cmd.c_str()))
    {
        cout << "Error outputting file!\n";
    }
    
    cout << "Done! (" << chrono::duration_cast<chrono::duration<double>>(chrono::high_resolution_clock::now()-start).count() << "s)\n";
}

void Movie::writeFrame(const shared_ptr<Frame> &spFrame, ull_t frameIndex)
{
    if (frameIndex >= m_numFrames)
    {
        throw runtime_error("Invalid frame index recieved!");
    }

    string filename = m_tempDir + "/" + to_string(frameIndex) + ".bmp";
    CImg<byte_t>(spFrame->m_pImage, m_width, m_height, 1, 3).save_bmp(filename.c_str());
}

ull_t Movie::getHeight() const
{
    return m_height;
}

ull_t Movie::getWidth() const
{
    return m_width;
}

ull_t Movie::getFps() const
{
    return m_fps;
}

ull_t Movie::getNumFrames() const
{
    return m_numFrames;
}

ld_t Movie::getAspect() const
{
    return m_aspectRatio;
}

ld_t Movie::getDuration() const
{
    return m_duration;
}

Movie::~Movie()
{
    filesystem::remove_all(m_tempDir);
}
