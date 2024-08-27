using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using MonogameTest.map.rooms;

namespace MonogameTest.map;

public abstract class Room() : Corridor {
    protected Room(Vector2 topLeft, Vector2 bottomRight, Terrain floorTerrain = Terrain.OrcFloor, Terrain wallTerrain = Terrain.OrcWall) : this() {
        TopLeft = topLeft;
        BottomRight = bottomRight;
        FloorTerrain = floorTerrain;
        WallTerrain = wallTerrain;
        Grid = new Dictionary<Vector2, Tile>();
        BuildWallsAndFloor();
        // ReSharper disable once VirtualMemberCallInConstructor
        FillRoom();
    }

    private void BuildWallsAndFloor() {
        HashSet<Vector2> positions = RoomDigger.GetTileRegion(TopLeft, BottomRight);
        foreach (Vector2 pos in positions) {
            Grid[pos] = Tile.CreateTileForTerrain(FloorTerrain);
        }

        List<Vector2> wallPositions = GetEdgePositions();
        foreach (Vector2 pos in wallPositions) {
            Grid[pos] = Tile.CreateTileForTerrain(WallTerrain);
        }
    }

    public List<Vector2> GetEdgePositions() {
        List<Vector2> wallPositions = GetTilePositions().Where(pos => {
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
        List<Vector2> edgePositions = GetEdgePositions();
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

    public static Room CreateRoom(RoomType roomType, Vector2 topLeft, Vector2 bottomRight) {
        return roomType switch {
            RoomType.EarthElementalFont => new EarthElementalFont(topLeft, bottomRight),
            RoomType.StartingRoom => new StartingRoom(topLeft, bottomRight),
            _ => throw new ArgumentOutOfRangeException(nameof(roomType), roomType, null)
        };
    }

    protected abstract void FillRoom();
}