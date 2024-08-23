using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonogameTest;

public class Map
{
    private const int TileSize = 32;
    private readonly List<List<Tile>> _grid;

    public Map(int width, int height)
    {
        Random rng = new Random();
        _grid = new List<List<Tile>>();
        for (int y = 0; y < height; y++)
        {
            _grid.Add(new List<Tile>());
            for (int x = 0; x < width; x++)
            {
                if (rng.Next(0, 20) == 0)
                {
                    _grid[y].Add(new Tile(Terrain.CrystalLightBlue, 0));     
                }
                else
                {
                    _grid[y].Add(new Tile(Terrain.OrcWall, rng.Next(0, 4)));     
                }
            }
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        for (int y = 0; y < _grid.Count; y++)
        {
            for (int x = 0; x < _grid[y].Count; x++)
            {
                Tile tile = _grid[y][x];
                Vector2 position = new Vector2(x * TileSize, y * TileSize);
                spriteBatch.Draw(tile.getSprite(), position, Color.White);
            }
        }

    }
}