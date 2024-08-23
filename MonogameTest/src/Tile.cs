using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonogameTest;

public class Tile
{
    private static Random rng = new();
    
    private Terrain _terrain;
    private int _spriteIndex;

    public Tile(Terrain terrain, int spriteIndex)
    {
        _terrain = terrain;
        _spriteIndex = spriteIndex;
    }

    public static Tile CreateOrcWallTile()
    {
        return new Tile(Terrain.OrcWall, rng.Next(0, 4));
    }
    
    public static Tile CreateOrcFloorTile()
    {
        return new Tile(Terrain.OrcFloor, rng.Next(0, 8));
    }
    
    public static Tile CreateCrystalWallLightBlueTile()
    {
        return new Tile(Terrain.CrystalWallLightBlue, 0);
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 position)
    {
        spriteBatch.Draw(Images.SpriteSet[_terrain][_spriteIndex], position, Color.White);
    }
}