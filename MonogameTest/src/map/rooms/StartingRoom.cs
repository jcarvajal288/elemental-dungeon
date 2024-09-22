using Microsoft.Xna.Framework;

namespace MonogameTest.map.rooms;

public class StartingRoom (
    Vector2 topLeft, 
    Vector2 bottomRight,
    int id
) : Room(
    topLeft, 
    bottomRight, 
    id,
    Terrain.MosaicFloor, 
    Terrain.ReliefWall
) {
    protected override void FillRoom() {
    }
}