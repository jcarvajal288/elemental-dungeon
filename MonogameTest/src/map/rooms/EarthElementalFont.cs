using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.HighPerformance.Enumerables;
using Microsoft.Xna.Framework;

namespace MonogameTest.map.rooms;

public class EarthElementalFont(
    Vector2 topLeft, 
    Vector2 bottomRight,
    int id
) : Room(
    topLeft, 
    bottomRight, 
    id,
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

    public override void SpreadPower(Dictionary<int, Room> rooms, int[] roomDistances) {
        EarthElementalPower += 1;
        Enumerable.Range(0, roomDistances.Length)
            .Where(id => roomDistances[id] == 1).ToList()
            .ForEach(id => {
                rooms[id].EarthElementalPower += 1;
            });
    }
}