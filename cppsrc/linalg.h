#pragma once

#include <cmath>

// Forward declared structs
template<typename T>
struct T_Vec2;

template<typename T>
struct T_Vec3;

template<typename T>
struct T_Vec4;

// Types
typedef unsigned long long ull_t;
typedef long long ll_t;
typedef long double ld_t;
typedef unsigned char byte_t;

// Consts
constexpr ld_t MAXBYTE = 255.;

// Math types
typedef T_Vec2<ll_t> Vec2;
typedef T_Vec3<ll_t> Vec3;
typedef T_Vec4<ll_t> Vec4;
typedef T_Vec2<ld_t> fVec2;
typedef T_Vec3<ld_t> fVec3;
typedef T_Vec4<ld_t> fVec4;
typedef T_Vec2<ull_t> uVec2;
typedef T_Vec3<ull_t> uVec3;
typedef T_Vec4<ull_t> uVec4;
typedef T_Vec3<ld_t> Rot;

// Color types
typedef T_Vec3<byte_t> RGB;
typedef T_Vec4<byte_t> RGBA;
typedef T_Vec3<ld_t> HSV;

template<typename T>
class T_Vec2
{
    public:
    T x, y;

    T_Vec2()
        : x{}, y{}
    {}

    T_Vec2(const T &x, const T &y)
        : x{x}, y{y}
    {}
    
    template<typename O>
    T_Vec2(const T_Vec2<O> &other)
        : x{T(other.x)}
        , y{T(other.y)}
    {}

    T_Vec2<T>& operator=(const T_Vec2<T> &other)
    {
        x = other.x;
        y = other.y;
        return *this;
    }

    template<typename O>
    T_Vec2<T>& operator=(const T_Vec2<O> &other)
    {
        x = T(other.x);
        y = T(other.y);
        return *this;
    }

    T_Vec2<T>& operator=(const T_Vec3<T> &other)
    {
        *this = other.xy;
        return *this;
    }

    T_Vec2<T>& operator=(const T_Vec4<T> &other)
    {
        *this = other.xy;
        return *this;
    }

    template<typename O>
    auto operator+(const T_Vec2<O> &other) const
    {
        return T_Vec2<decltype(x + other.x)>{x + other.x, y + other.y};
    }

    template<typename O>
    auto operator-(const T_Vec2<O> &other) const
    {
        return T_Vec2<decltype(x - other.x)>{x - other.x, y - other.y};
    }

    template<typename O>
    auto operator*(const T_Vec2<O> &other) const
    {
        return T_Vec2<decltype(x * other.x)>{x * other.x, y * other.y};
    }

    template<typename O>
    auto operator/(const T_Vec2<O> &other) const
    {
        return T_Vec2<decltype(x / other.x)>{x / other.x, y / other.y};
    }

    template<typename O>
    auto operator*(O scalar) const
    {
        return T_Vec2<decltype(x * scalar)>{x * scalar, y * scalar};
    }

    template<typename O>
    auto operator/(O scalar) const
    {
        return T_Vec2<decltype(x / scalar)>{x / scalar, y / scalar};
    }

    template<typename O>
    T_Vec2<T>& operator+=(const T_Vec2<O> &other)
    {
        return *this = *this + other;
    }

    template<typename O>
    T_Vec2<T>& operator-=(const T_Vec2<O> &other)
    {
        return *this = *this - other;
    }

    template<typename O>
    T_Vec2<T>& operator*=(const T_Vec2<O> &other)
    {
        return *this = *this * other;
    }

    template<typename O>
    T_Vec2<T>& operator/=(const T_Vec2<O> &other)
    {
        return *this = *this / other;
    }

    template<typename O>
    T_Vec2<T>& operator*=(O scalar)
    {
        return *this = *this * scalar;
    }

    template<typename O>
    T_Vec2<T>& operator/=(O scalar)
    {
        return *this = *this / scalar;
    }

    template<typename O>
    bool operator==(const T_Vec2<O> &other) const
    {
        return x == other.x && y == other.y;
    }

    template<typename O>
    bool operator!=(const T_Vec2<O> &other) const
    {
        return !(*this == other);
    }

    ld_t mag() const
    {
        return hypot(x, y);
    }

    T_Vec2<ld_t> norm() const
    {
        return (*this)/mag();
    }

    T_Vec2<ld_t> rot(ld_t radians) const
    {
        return T_Vec2<ld_t>{x * cos(radians) - y * sin(radians), x * sin(radians) + y * cos(radians)};
    }
};

template<typename T>
class T_Vec3
{
    public:
    T x, y, z;
    T &r, &g, &b;
    T &h, &s, &v;
    T_Vec2<T&> xy, xz, yz;

    T_Vec3()
        : x{}, y{}, z{}
        , r{x}, g{y}, b{z}
        , h{x}, s{y}, v{z}
        , xy{T_Vec2<T&>{x, y}}
        , xz{T_Vec2<T&>{x, z}}
        , yz{T_Vec2<T&>{y, z}}
    {}

    T_Vec3(const T &_x, const T &_y, const T &_z)
        : x{_x}, y{_y}, z{_z}
        , r{x}, g{y}, b{z}
        , h{x}, s{y}, v{z}
        , xy{T_Vec2<T&>{x, y}}
        , xz{T_Vec2<T&>{x, z}}
        , yz{T_Vec2<T&>{y, z}}
    {}

    template<typename O>
    T_Vec3(const T_Vec3<O> &other)
        : x{T(other.x)}
        , y{T(other.y)}
        , z{T(other.z)}
        , r{x}
        , g{y}
        , b{z}
        , h{x}
        , s{y}
        , v{z}
        , xy{T_Vec2<T&>{x, y}}
        , xz{T_Vec2<T&>{x, z}}
        , yz{T_Vec2<T&>{y, z}}
    {}

    T_Vec3<T>& operator=(const T_Vec3<T> &other)
    {
        x = other.x;
        y = other.y;
        z = other.z;
        return *this;
    }

    template<typename O>
    T_Vec3<T>& operator=(const T_Vec3<O> &other)
    {
        x = T(other.x);
        y = T(other.y);
        z = T(other.z);
        return *this;
    }

    T_Vec3<T>& operator=(const T_Vec4<T> &other)
    {
        *this = other.xyz;
        return *this;
    }

    T_Vec3<T>& operator=(const T_Vec2<T> &other)
    {
        x = other.x;
        y = other.y;
        return *this;
    }

    template<typename O>
    auto operator+(const T_Vec3<O> &other) const
    {
        return T_Vec3<decltype(x + other.x)>{x + other.x, y + other.y, z + other.z};
    }

    template<typename O>
    auto operator-(const T_Vec3<O> &other) const
    {
        return T_Vec3<decltype(x - other.x)>{x - other.x, y - other.y, z - other.z};
    }

    template<typename O>
    auto operator*(const T_Vec3<O> &other) const
    {
        return T_Vec3<decltype(x * other.x)>{x * other.x, y * other.y, z * other.z};
    }

    template<typename O>
    auto operator/(const T_Vec3<O> &other) const
    {
        return T_Vec3<decltype(x / other.x)>{x / other.x, y / other.y, z / other.z};
    }

    template<typename O>
    auto operator*(O scalar) const
    {
        return T_Vec3<decltype(x * scalar)>{x * scalar, y * scalar, z * scalar};
    }

    template<typename O>
    auto operator/(O scalar) const
    {
        return T_Vec3<decltype(x / scalar)>{x / scalar, y / scalar, z / scalar};
    }

    template<typename O>
    T_Vec3<T>& operator+=(const T_Vec3<O> &other)
    {
        return *this = *this + other;
    }

    template<typename O>
    T_Vec3<T>& operator-=(const T_Vec3<O> &other)
    {
        return *this = *this - other;
    }

    template<typename O>
    T_Vec3<T>& operator*=(const T_Vec3<O> &other)
    {
        return *this = *this * other;
    }

    template<typename O>
    T_Vec3<T>& operator/=(const T_Vec3<O> &other)
    {
        return *this = *this / other;
    }

    template<typename O>
    T_Vec3<T>& operator*=(O scalar)
    {
        return *this = *this * scalar;
    }

    template<typename O>
    T_Vec3<T>& operator/=(O scalar)
    {
        return *this = *this / scalar;
    }

    template<typename O>
    bool operator==(const T_Vec3<O> &other) const
    {
        return x == other.x && y == other.y && z == other.z;
    }

    template<typename O>
    bool operator!=(const T_Vec3<O> &other) const
    {
        return !(*this == other);
    }

    ld_t mag() const
    {
        return sqrt(x * x + y * y + z * z);
    }

    T_Vec3<ld_t> norm() const
    {
        return (*this)/mag();
    }
};

template<typename T>
class T_Vec4
{
    public:
    T x, y, z, w;
    T &r, &g, &b, &a;
    T_Vec2<T&> xy, xz, xw, yz, yw, zw;
    T_Vec3<T&> xyz, xyw, xzw, yzw, rgb;

    T_Vec4()
        : x{}, y{}, z{},  w{}
        , r{x}, g{y}, b{z}, a{w}
        , xy{T_Vec2<T&>{x, y}}
        , xz{T_Vec2<T&>{x, z}}
        , xw{T_Vec2<T&>{x, w}}
        , yz{T_Vec2<T&>{y, z}}
        , yw{T_Vec2<T&>{y, w}}
        , zw{T_Vec2<T&>{z, w}}
        , xyz{T_Vec3<T&>{x, y, z}}
        , xyw{T_Vec3<T&>{x, y, w}}
        , xzw{T_Vec3<T&>{x, z, w}}
        , yzw{T_Vec3<T&>{y, z, w}}
        , rgb{T_Vec3<T&>{x, y, z}}
    {}

    T_Vec4(const T &_x, const T &_y, const T &_z, const T &_w)
        : x{_x}, y{_y}, z{_z},  w{_w}
        , r{x}, g{y}, b{z}, a{w}
        , xy{T_Vec2<T&>{x, y}}
        , xz{T_Vec2<T&>{x, z}}
        , xw{T_Vec2<T&>{x, w}}
        , yz{T_Vec2<T&>{y, z}}
        , yw{T_Vec2<T&>{y, w}}
        , zw{T_Vec2<T&>{z, w}}
        , xyz{T_Vec3<T&>{x, y, z}}
        , xyw{T_Vec3<T&>{x, y, w}}
        , xzw{T_Vec3<T&>{x, z, w}}
        , yzw{T_Vec3<T&>{y, z, w}}
        , rgb{T_Vec3<T&>{x, y, z}}
    {}

    template<typename O>
    T_Vec4(const T_Vec4<O> &other)
        : x{T(other.x)}
        , y{T(other.y)}
        , z{T(other.z)}
        , w{T(other.w)}
        , r{x}
        , g{y}
        , b{z}
        , a{w}
        , xy{T_Vec2<T&>{x, y}}
        , xz{T_Vec2<T&>{x, z}}
        , xw{T_Vec2<T&>{x, w}}
        , yz{T_Vec2<T&>{y, z}}
        , yw{T_Vec2<T&>{y, w}}
        , zw{T_Vec2<T&>{z, w}}
        , xyz{T_Vec3<T&>{x, y, z}}
        , xyw{T_Vec3<T&>{x, y, w}}
        , xzw{T_Vec3<T&>{x, z, w}}
        , yzw{T_Vec3<T&>{y, z, w}}
        , rgb{T_Vec3<T&>{x, y, z}}
    {}
 
    T_Vec4(const T_Vec3<T> &other, const T &_w)
        : x{T(other.x)}
        , y{T(other.y)}
        , z{T(other.z)}
        , w{T(_w)}
        , r{x}
        , g{y}
        , b{z}
        , a{w}
        , xy{T_Vec2<T&>{x, y}}
        , xz{T_Vec2<T&>{x, z}}
        , xw{T_Vec2<T&>{x, w}}
        , yz{T_Vec2<T&>{y, z}}
        , yw{T_Vec2<T&>{y, w}}
        , zw{T_Vec2<T&>{z, w}}
        , xyz{T_Vec3<T&>{x, y, z}}
        , xyw{T_Vec3<T&>{x, y, w}}
        , xzw{T_Vec3<T&>{x, z, w}}
        , yzw{T_Vec3<T&>{y, z, w}}
        , rgb{T_Vec3<T&>{x, y, z}}
    {}

    T_Vec4<T>& operator=(const T_Vec4<T> &other)
    {
        x = other.x;
        y = other.y;
        z = other.z;
        w = other.w;
        return *this;
    }

    template<typename O>
    T_Vec4<T>& operator=(const T_Vec4<O> &other)
    {
        x = T(other.x);
        y = T(other.y);
        z = T(other.z);
        w = T(other.w);
        return *this;
    }

    T_Vec4<T>& operator=(const T_Vec3<T> &other)
    {
        x = other.x;
        y = other.y;
        z = other.z;
        return *this;
    }

    T_Vec4<T>& operator=(const T_Vec2<T> &other)
    {
        x = other.x;
        y = other.y;
        return *this;
    }

    template<typename O>
    auto operator+(const T_Vec4<O> &other) const
    {
        return T_Vec4<decltype(x + other.x)>{x + other.x, y + other.y, z + other.z, w + other.w};
    }

    template<typename O>
    auto operator-(const T_Vec4<O> &other) const
    {
        return T_Vec4<decltype(x - other.x)>{x - other.x, y - other.y, z - other.z, w - other.w};
    }

    template<typename O>
    auto operator*(const T_Vec4<O> &other) const
    {
        return T_Vec4<decltype(x * other.x)>{x * other.x, y * other.y, z * other.z, w * other.w};
    }

    template<typename O>
    auto operator/(const T_Vec4<O> &other) const
    {
        return T_Vec4<decltype(x / other.x)>{x / other.x, y / other.y, z / other.z, w / other.w};
    }

    template<typename O>
    auto operator*(O scalar) const
    {
        return T_Vec4<decltype(x * scalar)>{x * scalar, y * scalar, z * scalar, w * scalar};
    }

    template<typename O>
    auto operator/(O scalar) const
    {
        return T_Vec4<decltype(x / scalar)>{x / scalar, y / scalar, z / scalar, w / scalar};
    }

    template<typename O>
    T_Vec4<T>& operator+=(const T_Vec4<O> &other)
    {
        return *this = *this + other;
    }

    template<typename O>
    T_Vec4<T>& operator-=(const T_Vec4<O> &other)
    {
        return *this = *this - other;
    }

    template<typename O>
    T_Vec4<T>& operator*=(const T_Vec4<O> &other)
    {
        return *this = *this * other;
    }

    template<typename O>
    T_Vec4<T>& operator/=(const T_Vec4<O> &other)
    {
        return *this = *this / other;
    }

    template<typename O>
    T_Vec4<T>& operator*=(O scalar)
    {
        return *this = *this * scalar;
    }

    template<typename O>
    T_Vec4<T>& operator/=(O scalar)
    {
        return *this = *this / scalar;
    }

    template<typename O>
    bool operator==(const T_Vec4<O> &other) const
    {
        return x == other.x && y == other.y && z == other.z && w == other.w;
    }

    template<typename O>
    bool operator!=(const T_Vec4<O> &other) const
    {
        return !(*this == other);
    }

    ld_t mag() const
    {
        return sqrt(x * x + y * y + z * z + w * w);
    }

    T_Vec4<ld_t> Norm() const
    {
        return (*this)/mag();
    }
};


// Color functions / Helpers
template<typename T>
T max(T first, T second)
{
    return (first > second ? first : second);
}

template<typename T>
T min(T first, T second)
{
    return (first < second ? first : second);
}

const ld_t DEGPERPI = 180 / 3.14159265;
HSV ToHSV(const RGB &rgb);
RGB ToRGB(const HSV &hsv);
RGBA alphaBlend(const RGBA &front, const RGBA &back);
