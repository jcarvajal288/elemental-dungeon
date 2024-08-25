using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonogameTest.map;

public class Room() : Corridor {

    public Room(Vector2 topLeft, Vector2 bottomRight) : this() {
        TopLeft = topLeft;
        BottomRight = bottomRight;
        FloorTerrain = Terrain.OrcFloor;
        WallTerrain = Terrain.OrcWall;
        Grid = new Dictionary<Vector2, Tile>();
        Build();
    }
    
    private void Build() {
        HashSet<Vector2> positions = RoomDigger.GetTileRegion(TopLeft, BottomRight);
        foreach (Vector2 pos in positions) {
            Grid[pos] = Tile.CreateTileForTerrain(FloorTerrain);
        }

        List<Vector2> wallPositions = GetEdgePositions(positions.ToList());
        foreach (Vector2 pos in wallPositions) {
            Grid[pos] = Tile.CreateTileForTerrain(WallTerrain);
        }
    }

    private List<Vector2> GetEdgePositions(List<Vector2> positions) {
        List<Vector2> wallPositions = positions.Where(pos => {
            return (int)pos.X == (int)TopLeft.X || 
                   (int)pos.Y == (int)TopLeft.Y ||
                   (int)pos.X == (int)BottomRight.X ||
                   (int)pos.Y == (int)BottomRight.Y;
        }).ToList();
        return wallPositions;
    }

    public List<Vector2> GetTilePositions() {
        return Grid.Keys.ToList();
    }

    public void AddDoorwayForCorridor(HashSet<Vector2> corridorTiles) {
        List<Vector2> edgePositions = GetEdgePositions(Grid.Keys.ToList());
        foreach (Vector2 position in edgePositions.Intersect(corridorTiles)) {
            Grid[position] = Tile.CreateTileForTerrain(FloorTerrain);
        }
    }
}