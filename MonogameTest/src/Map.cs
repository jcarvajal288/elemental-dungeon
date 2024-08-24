using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonogameTest;

public class Map {
    private readonly List<List<Tile>> _grid;

    public Map(int width, int height) {
        Random rng = new();
        _grid = new List<List<Tile>>();
        for (int y = 0; y < height; y++) {
            _grid.Add(new List<Tile>());
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

    public void DigRoom(int x, int y, int width, int height) {
        for (int b = y; b < y + height; b++) {
            for (int a = x; a < x + width; a++) {
                _grid[b][a] = Tile.CreateOrcFloorTile();
            }
        }
    }
}