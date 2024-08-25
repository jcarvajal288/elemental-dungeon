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
            Terrain.CobbleStoneFloor => CreateCobbleStoneFloorTile(),
            Terrain.CrystalWallLightBlue => CreateCrystalWallLightBlueTile(),
            Terrain.CrystalWallLightRed => CreateCrystalWallLightRedTile(),
            Terrain.GreyBrickWall => CreateGreyBrickWallTile(),
            Terrain.LimestoneFloor => CreateLimestoneFloorTile(),
            Terrain.OrcFloor => CreateOrcFloorTile(),
            Terrain.OrcWall => CreateOrcWallTile(),
            _ => throw new ArgumentOutOfRangeException(nameof(terrain), terrain, null)
        };
    }

    private static Tile CreateBrownBrickWallTile() {
        throw new NotImplementedException();
    }

    private static Tile CreateBrownStoneWallTile() {
        throw new NotImplementedException();
    }

    private static Tile CreateCobbleStoneFloorTile() {
        return new Tile(Terrain.CobbleStoneFloor, Rng.Next(0, Images.TerrainSpriteSet[Terrain.CobbleStoneFloor].Count));
    }

    public static Tile CreateCrystalWallLightBlueTile() {
        return new Tile(Terrain.CrystalWallLightBlue, 0);
    }
    
    private static Tile CreateCrystalWallLightRedTile() {
        throw new NotImplementedException();
    }
    
    private static Tile CreateGreyBrickWallTile() {
        return new Tile(Terrain.GreyBrickWall, Rng.Next(0, Images.TerrainSpriteSet[Terrain.GreyBrickWall].Count));
    }

    private static Tile CreateLimestoneFloorTile() {
        throw new NotImplementedException();
    }

    public static Tile CreateOrcFloorTile() {
        return new Tile(Terrain.OrcFloor, Rng.Next(0, Images.TerrainSpriteSet[Terrain.OrcFloor].Count));
    }

    public static Tile CreateOrcWallTile() {
        return new Tile(Terrain.OrcWall, Rng.Next(0, Images.TerrainSpriteSet[Terrain.OrcWall].Count));
    }
}