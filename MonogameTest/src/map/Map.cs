using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonogameTest.map;

public class Map {
    private readonly List<List<Tile>> _grid;
    private readonly Dictionary<int, Room> _rooms;

    public Map(int width, int height) {
        Random rng = new();
        _grid = [];
        _rooms = new Dictionary<int, Room>();
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
                Vector2 position = new(x * Tile.Size, y * Tile.Size);
                tile.Draw(spriteBatch, position);
            }
        }
    }

    public Tile GetTileAt(Vector2 position) {
        return _grid[(int)position.Y][(int)position.X];
    }

    public void SetTileAt(Vector2 position, Tile tile) {
        _grid[(int)position.Y][(int)position.X] = tile;
    }

    public int GetWidth() {
        return _grid[0].Count;
    }

    public int GetHeight() {
        return _grid.Count;
    }

    public void AddRoom(Vector2 topLeft,  Vector2 bottomRight) {
        int id = _rooms.Count;
        _rooms.Add(id, new Room(topLeft, bottomRight));
    }

    public int GetRoomIdForPosition(Vector2 position) {
        List<int> ids = _rooms.Keys.Where(key => _rooms[key].ContainsPosition(position)).ToList();
        return ids.Count switch {
            > 1 => throw new Exception("Multiple rooms found for position: " + position),
            1 => ids[0],
            _ => -1
        };
    }
}