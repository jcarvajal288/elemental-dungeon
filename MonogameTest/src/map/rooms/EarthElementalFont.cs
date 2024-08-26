
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MonogameTest.map.rooms;

public class EarthElementalFont : Room {

    public EarthElementalFont(Vector2 topLeft, Vector2 bottomRight) {
        TopLeft = topLeft;
        BottomRight = bottomRight;
        WallTerrain = Terrain.BrownBrickWall;
        FloorTerrain = Terrain.LimestoneFloor;
        Grid = new Dictionary<Vector2, Tile>();
        Build();
    }
    
    protected new void FillRoom() {
        throw new System.NotImplementedException();
    }
}