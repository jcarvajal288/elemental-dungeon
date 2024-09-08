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
    
    protected override void FillRoom() {
        Vector2 center = TopLeft + (BottomRight - TopLeft) / 2f;
        Grid[center] = Tile.CreateCrystalWallLightRedTile();
        Vector2 up = center with { Y = center.Y - 1 };
        Vector2 down = center with { Y = center.Y + 1 };
        Vector2 left = center with { X = center.X - 1 };
        Vector2 right = center with { X = center.X + 1 };
        Grid[up] = Tile.CreateBrownStoneWallTile();
        Grid[down] = Tile.CreateBrownStoneWallTile();
        Grid[left] = Tile.CreateBrownStoneWallTile();
        Grid[right] = Tile.CreateBrownStoneWallTile();
    }
}