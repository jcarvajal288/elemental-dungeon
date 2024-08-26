using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace MonogameTest.map.rooms;

public class Room() : Corridor {
    public Room(Vector2 topLeft, Vector2 bottomRight, Terrain floorTerrain = Terrain.OrcFloor, Terrain wallTerrain = Terrain.OrcWall) : this() {
        TopLeft = topLeft;
        BottomRight = bottomRight;
        FloorTerrain = floorTerrain;
        WallTerrain = wallTerrain;
        Grid = new Dictionary<Vector2, Tile>();
        Build();
    }

    protected void Build() {
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

    public void AddDoorwayForCorridor(List<Vector2> corridorFloorTiles) {
        List<Vector2> edgePositions = GetEdgePositions(Grid.Keys.ToList());
        List<Vector2> doorwayPositions = edgePositions.Where(AdjacentToAnyCorridorFloor).ToList();
        foreach (Vector2 position in doorwayPositions) {
            Grid[position] = Tile.CreateTileForTerrain(FloorTerrain);
        }

        bool AdjacentToAnyCorridorFloor(Vector2 pos) {
            return corridorFloorTiles.Exists(floor => 
                Math.Abs((int)floor.X - (int)pos.X) == 1 && (int)floor.Y == (int)pos.Y || 
                Math.Abs((int)floor.Y - (int)pos.Y) == 1 && (int)floor.X == (int)pos.X
            );
        }
    }


    protected void FillRoom() {
    }
}