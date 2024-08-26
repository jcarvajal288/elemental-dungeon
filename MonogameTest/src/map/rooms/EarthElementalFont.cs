
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MonogameTest.map.rooms;

public class EarthElementalFont(
    Vector2 topLeft, 
    Vector2 bottomRight
) : Room(
    topLeft, 
    bottomRight, 
    Terrain.LimestoneFloor, 
    Terrain.BrownBrickWall
) {
    protected new void FillRoom() {
        throw new System.NotImplementedException();
    }
}