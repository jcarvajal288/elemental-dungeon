using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonogameTest;

public class Tile(Terrain terrain, int spriteIndex) {
    public const int Size = 32;
    private static readonly Random Rng = new();

    public void Draw(SpriteBatch spriteBatch, Vector2 position) {
        spriteBatch.Draw(Images.TerrainSpriteSet[terrain][spriteIndex], position, Color.White);
    }

    public bool IsWalkable() {
        return TerrainExtensions.WalkableTerrain.Contains(terrain);
    }

    
    // Tile Builders
    public static Tile CreateOrcWallTile() {
        return new Tile(Terrain.OrcWall, Rng.Next(0, 4));
    }

    public static Tile CreateOrcFloorTile() {
        return new Tile(Terrain.OrcFloor, Rng.Next(0, 8));
    }

    public static Tile CreateCrystalWallLightBlueTile() {
        return new Tile(Terrain.CrystalWallLightBlue, 0);
    }
}