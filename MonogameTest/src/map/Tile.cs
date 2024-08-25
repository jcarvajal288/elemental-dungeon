using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonogameTest.map;

public class Tile(Terrain terrain, int spriteIndex) {
    public const int Size = 32;
    private static readonly Random Rng = new();

    public void Draw(SpriteBatch spriteBatch, Vector2 position) {
        Vector2 pixelPosition = new(position.X * Size, position.Y * Size);
        spriteBatch.Draw(Images.TerrainSpriteSet[terrain][spriteIndex], pixelPosition, Color.White);
    }

    public bool IsWalkable() {
        return TerrainExtensions.WalkableTerrain.Contains(terrain);
    }

    
    // Tile Builders
    public static Tile CreateTileForTerrain(Terrain terrain) {
        return terrain switch {
            Terrain.OrcWall => CreateOrcWallTile(),
            Terrain.OrcFloor => CreateOrcFloorTile(),
            Terrain.BrownBrickWall => CreateBrownBrickWallTile(),
            Terrain.LimestoneFloor => CreateLimestoneFloorTile(),
            Terrain.BrownStoneWall => CreateBrownStoneWallTile(),
            Terrain.CrystalWallLightBlue => CreateCrystalWallLightBlueTile(),
            Terrain.CrystalWallLightRed => CreateCrystalWallLightRedTile(),
            _ => throw new ArgumentOutOfRangeException(nameof(terrain), terrain, null)
        };
    }

    public static Tile CreateOrcWallTile() {
        return new Tile(Terrain.OrcWall, Rng.Next(0, 4));
    }

    public static Tile CreateOrcFloorTile() {
        return new Tile(Terrain.OrcFloor, Rng.Next(0, 8));
    }

    private static Tile CreateBrownBrickWallTile() {
        throw new NotImplementedException();
    }

    private static Tile CreateLimestoneFloorTile() {
        throw new NotImplementedException();
    }

    private static Tile CreateBrownStoneWallTile() {
        throw new NotImplementedException();
    }

    public static Tile CreateCrystalWallLightBlueTile() {
        return new Tile(Terrain.CrystalWallLightBlue, 0);
    }
    
    private static Tile CreateCrystalWallLightRedTile() {
        throw new NotImplementedException();
    }
}