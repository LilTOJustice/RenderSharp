#include "render2d.h"
#include "exampleshaders.h"

#include <iostream>

using namespace std;

int main()
{
    auto spScene = make_shared<Scene2d>(60, 2, RGB{0, 0, 255});
    Render2d r2d(uVec2{2560, 1440}, spScene);
    auto spSprite = make_shared<Sprite>();
    spScene->addActor(make_shared<Scene2d::Actor>(spSprite));
    r2d.renderAll()->output("output");
}
