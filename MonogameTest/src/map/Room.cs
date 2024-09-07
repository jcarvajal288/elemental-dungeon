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
        HashSet<Vector2> positions = Map.GetTileRegion(TopLeft, BottomRight);
        foreach (Vector2 pos in positions) {
            Grid[pos] = Tile.CreateTileForTerrain(FloorTerrain);
        }

        List<Vector2> wallPositions = GetEdgePositions();
        foreach (Vector2 pos in wallPositions) {
            Grid[pos] = Tile.CreateTileForTerrain(WallTerrain);
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