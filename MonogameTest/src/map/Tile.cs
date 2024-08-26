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
            Terrain.BrownBrickWall => CreateBrownBrickWallTile(),
            Terrain.BrownStoneWall => CreateBrownStoneWallTile(),
            Terrain.CrystalWallLightBlue => CreateCrystalWallLightBlueTile(),
            Terrain.CrystalWallLightRed => CreateCrystalWallLightRedTile(),
            Terrain.LimestoneFloor => CreateLimestoneFloorTile(),
            Terrain.MosaicFloor => CreateMosaicFloorTile(),
            Terrain.OrcFloor => CreateOrcFloorTile(),
            Terrain.OrcWall => CreateOrcWallTile(),
            Terrain.ReliefWall => CreateReliefWallTile(),
            _ => throw new ArgumentOutOfRangeException(nameof(terrain), terrain, null)
        };
    }

    private static Tile CreateBrownBrickWallTile() {
        return new Tile(Terrain.BrownBrickWall, Rng.Next(0, Images.TerrainSpriteSet[Terrain.BrownBrickWall].Count));
    }

    private static Tile CreateBrownStoneWallTile() {
        return new Tile(Terrain.BrownStoneWall, Rng.Next(0, Images.TerrainSpriteSet[Terrain.BrownStoneWall].Count));
    }

    public static Tile CreateCrystalWallLightBlueTile() {
        return new Tile(Terrain.CrystalWallLightBlue, 0);
    }
    
    private static Tile CreateCrystalWallLightRedTile() {
        return new Tile(Terrain.CrystalWallLightRed, 0);
    }

    private static Tile CreateLimestoneFloorTile() {
        return new Tile(Terrain.LimestoneFloor, Rng.Next(0, Images.TerrainSpriteSet[Terrain.LimestoneFloor].Count));
    }

    private static Tile CreateMosaicFloorTile() {
        return new Tile(Terrain.MosaicFloor, Rng.Next(0, Images.TerrainSpriteSet[Terrain.MosaicFloor].Count));
    }

    private static Tile CreateOrcFloorTile() {
        return new Tile(Terrain.OrcFloor, Rng.Next(0, Images.TerrainSpriteSet[Terrain.OrcFloor].Count));
    }

    public static Tile CreateOrcWallTile() {
        return new Tile(Terrain.OrcWall, Rng.Next(0, Images.TerrainSpriteSet[Terrain.OrcWall].Count));
    }

    private static Tile CreateReliefWallTile() {
        return new Tile(Terrain.ReliefWall, Rng.Next(0, Images.TerrainSpriteSet[Terrain.ReliefWall].Count));
    }
}