using System;
using System.Collections.Generic;
using CommunityToolkit.HighPerformance.Enumerables;
using Microsoft.Xna.Framework;
using MonogameTest.map.rooms;

namespace MonogameTest.map;

public abstract class Room() : Corridor {
    public int EarthElementalPower { get; set; } = 0;
    
    protected Room(Vector2 topLeft, Vector2 bottomRight, int id, Terrain floorTerrain = Terrain.OrcFloor, Terrain wallTerrain = Terrain.OrcWall) : this() {
        TopLeft = topLeft;
        BottomRight = bottomRight;
        FloorTerrain = floorTerrain;
        WallTerrain = wallTerrain;
        Grid = new Dictionary<Vector2, Tile>();
        Id = id;
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

    public static Room CreateRoom(RoomType roomType, Vector2 topLeft, Vector2 bottomRight, int id) {
        return roomType switch {
            RoomType.EarthElementalFont => new EarthElementalFont(topLeft, bottomRight, id),
            RoomType.StartingRoom => new StartingRoom(topLeft, bottomRight, id),
            _ => throw new ArgumentOutOfRangeException(nameof(roomType), roomType, null)
        };
    }

    protected abstract void FillRoom();

    public abstract void SpreadPower(Dictionary<int, Room> rooms, int[] roomDistances);
}