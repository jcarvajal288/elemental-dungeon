using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.HighPerformance;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonogameTest.map.rooms;
using MonogameTest.player;

namespace MonogameTest.map;

public class Map {
    private readonly List<List<Tile>> _grid;
    private readonly Dictionary<int, Room> _rooms;
    private readonly List<Corridor> _corridors;
    private int[,] _roomConnectionMatrix;

    public Map(int width, int height) {
        Random rng = new();
        _grid = [];
        _rooms = new Dictionary<int, Room>();
        _corridors = [];
        for (int y = 0; y < height; y++) {
            _grid.Add([]);
            for (int x = 0; x < width; x++) {
                if (rng.Next(0, 50) == 0) {
                    _grid[y].Add(Tile.CreateCrystalWallLightBlueTile());
                } else {
                    _grid[y].Add(Tile.CreateOrcWallTile());
                }
            }
        }
    }

    public void Draw(SpriteBatch spriteBatch) {
        for (int y = 0; y < _grid.Count; y++) {
            for (int x = 0; x < _grid[y].Count; x++) {
                Tile tile = _grid[y][x];
                Vector2 position = new(x, y);
                tile.Draw(spriteBatch, position);
            }
        }

        foreach (Room room in _rooms.Values) {
            room.Draw(spriteBatch);
        }
        _corridors.ForEach(corridor => corridor.Draw(spriteBatch));
    }

    public Tile GetTileAt(Vector2 position) {
        List<Room> containingRooms = _rooms.Values.Where(room => room.ContainsPosition(position)).ToList();
        if (containingRooms.Count > 0) {
            return containingRooms.First().GetTileAt(position);
        }
        
        List<Corridor> containingCorridors = _corridors.Where(corr => corr.ContainsPosition(position)).ToList();
        if (containingCorridors.Count > 0) {
            return containingCorridors.First().GetTileAt(position);
        }
        
        return _grid[(int)position.Y][(int)position.X];
    }

    public int GetWidth() {
        return _grid[0].Count;
    }

    public int GetHeight() {
        return _grid.Count;
    }

    public void AddRoom(Room room) {
        _rooms.Add(room.GetId(), room);
    }

    public void AddCorridor(Corridor corridor) {
        _corridors.Add(corridor);
        CalculateRoomConnectionMatrix();
        CalculateElementalPowersForRooms();
    }

    public int GetRoomIdForPosition(Vector2 position) {
        List<int> ids = _rooms.Keys.Where(key => _rooms[key].ContainsPosition(position)).ToList();
        return ids.Count switch {
            > 1 => throw new Exception("Multiple rooms found for position: " + position),
            1 => ids[0],
            _ => -1
        };
    }

    public Room GetRoomForId(int playerRoomId) {
        return _rooms[playerRoomId];
    }

    public Room GetRoomForPosition(Vector2 position) {
        return GetRoomForId(GetRoomIdForPosition(position));
    }

    public Corridor GetCorridorForPosition(Vector2 position) {
        return _corridors.FirstOrDefault(corridor => corridor.ContainsPosition(position));
    }
    
    public bool IsValidDiggingPosition(Vector2 playerPosition, PlayerAction playerAction) {
        Room playerRoom = GetRoomForPosition(playerPosition);
        return playerAction switch {
            PlayerAction.DigLeft => playerRoom.GetEdgePositions().Contains(playerPosition with { X = playerPosition.X - 1 }),
            PlayerAction.DigRight => playerRoom.GetEdgePositions().Contains(playerPosition with { X = playerPosition.X + 1 }),
            PlayerAction.DigUp => playerRoom.GetEdgePositions().Contains(playerPosition with { Y = playerPosition.Y - 1 }),
            PlayerAction.DigDown => playerRoom.GetEdgePositions().Contains(playerPosition with { Y = playerPosition.Y + 1 }),
            _ => false
        };
    }


    public static HashSet<Vector2> GetTileRegion(Vector2 topLeft, Vector2 bottomRight) {
        HashSet<Vector2> roomTiles = new();
        for (int y = (int)topLeft.Y; y <= bottomRight.Y; y++) {
            for (int x = (int)topLeft.X; x <= bottomRight.X; x++) {
                roomTiles.Add(new Vector2(x, y));
            }
        }
        return roomTiles;
    }

    public int NextRoomId() {
        return _rooms.Count;
    }

    private void CalculateElementalPowersForRooms() {
        foreach (Room room in _rooms.Values) {
            room.EarthElementalPower = 0;
        }

        Span2D<int> roomSpan = _roomConnectionMatrix;
        foreach (int roomId in _rooms.Keys) {
            Room room = _rooms[roomId];
            room.SpreadPower(_rooms, roomSpan.GetRow(roomId).ToArray());
        }
    }

    private void CalculateRoomConnectionMatrix() {
        bool[,] adjacencyMatrix = new bool[_rooms.Count, _rooms.Count];
        for (int i = 0; i < adjacencyMatrix.GetLength(0); i++) {
            for (int j = 0; j < adjacencyMatrix.GetLength(1); j++) {
                adjacencyMatrix[i, j] = false;
            }
        }
        
        _corridors.ForEach(corridor => {
            Tuple<int, int> ids = corridor.GetConnectedRoomIds();
            adjacencyMatrix[ids.Item1, ids.Item2] = true;
            adjacencyMatrix[ids.Item2, ids.Item1] = true;
        });
        
        _roomConnectionMatrix = new int[_rooms.Count, _rooms.Count];
        for (int roomId = 0; roomId < _roomConnectionMatrix.GetLength(0); roomId++) {
            FindShortestPathsForRoomId(roomId, adjacencyMatrix);
        }
    }

    private void FindShortestPathsForRoomId(int roomId, bool[,] adjacencyMatrix) {
        int[] distances = new int[_roomConnectionMatrix.GetLength(0)];
        Array.Fill(distances, int.MaxValue);
        distances[roomId] = 0;
        
        bool[] visited = new bool[_roomConnectionMatrix.GetLength(0)];
        Array.Fill(visited, false);
        visited[roomId] = true;
        
        Queue<int> queue = new();        
        queue.Enqueue(roomId);

        int distance = 0;
        while (queue.Count > 0) { // Breadth First Search
            distance++;
            int v = queue.Dequeue();

            for (int i = 0; i < adjacencyMatrix.GetLength(0); i++) {
                if (adjacencyMatrix[v, i] && !visited[i]) {
                    visited[i] = true;
                    distances[i] = distance;
                    queue.Enqueue(i);
                }
            }
        }

        for (int i = 0; i < distances.GetLength(0); i++) {
            _roomConnectionMatrix[roomId, i] = distances[i];
        }
    }
}