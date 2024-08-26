using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.Graphics;

namespace MonogameTest;

public static class Images {
    public static readonly Dictionary<Terrain, List<Texture2D>> TerrainSpriteSet = new();
    public static readonly Dictionary<PlayerSprites, Texture2D> PlayerSpriteSet = [];
    public static readonly Dictionary<UISprites, Texture2D> UiSpriteSet = [];

    private static Texture2D LoadTexture(GraphicsDevice graphicsDevice, string path) {
        using FileStream fileStream = File.OpenRead(path);
        return Texture2D.FromStream(graphicsDevice, fileStream);
    }

    public static void LoadImages(GraphicsDevice graphicsDevice) {
        TerrainSpriteSet.Add(Terrain.BrownBrickWall, [
                LoadTexture(graphicsDevice, "Content/assets/terrain/brownBrickWall/brick_brown_0.png"),
                LoadTexture(graphicsDevice, "Content/assets/terrain/brownBrickWall/brick_brown_1.png"),
                LoadTexture(graphicsDevice, "Content/assets/terrain/brownBrickWall/brick_brown_2.png"),
                LoadTexture(graphicsDevice, "Content/assets/terrain/brownBrickWall/brick_brown_4.png"),
                LoadTexture(graphicsDevice, "Content/assets/terrain/brownBrickWall/brick_brown_5.png"),
            ]
        );
        TerrainSpriteSet.Add(Terrain.BrownStoneWall, [
                LoadTexture(graphicsDevice, "Content/assets/terrain/brownStoneWall/stone_2_brown0.png")
            ]
        );
        TerrainSpriteSet.Add(Terrain.CrystalWallLightBlue, [
                LoadTexture(graphicsDevice, "Content/assets/terrain/crystals/crystal_wall_lightblue.png")
            ]
        );
        TerrainSpriteSet.Add(Terrain.CrystalWallLightRed, [
                LoadTexture(graphicsDevice, "Content/assets/terrain/crystals/crystal_wall_lightred.png")
            ]
        );
        TerrainSpriteSet.Add(Terrain.LimestoneFloor, [
                LoadTexture(graphicsDevice, "Content/assets/terrain/limestoneFloor/limestone_0.png"),
                LoadTexture(graphicsDevice, "Content/assets/terrain/limestoneFloor/limestone_1.png"),
                LoadTexture(graphicsDevice, "Content/assets/terrain/limestoneFloor/limestone_2.png"),
                LoadTexture(graphicsDevice, "Content/assets/terrain/limestoneFloor/limestone_3.png"),
                LoadTexture(graphicsDevice, "Content/assets/terrain/limestoneFloor/limestone_4.png"),
                LoadTexture(graphicsDevice, "Content/assets/terrain/limestoneFloor/limestone_5.png"),
                LoadTexture(graphicsDevice, "Content/assets/terrain/limestoneFloor/limestone_6.png"),
                LoadTexture(graphicsDevice, "Content/assets/terrain/limestoneFloor/limestone_7.png"),
                LoadTexture(graphicsDevice, "Content/assets/terrain/limestoneFloor/limestone_8.png"),
                LoadTexture(graphicsDevice, "Content/assets/terrain/limestoneFloor/limestone_9.png"),
            ]
        );
        TerrainSpriteSet.Add(Terrain.MosaicFloor, [
                LoadTexture(graphicsDevice, "Content/assets/terrain/mosaicFloor/mosaic_0.png"),
                LoadTexture(graphicsDevice, "Content/assets/terrain/mosaicFloor/mosaic_1.png"),
                LoadTexture(graphicsDevice, "Content/assets/terrain/mosaicFloor/mosaic_2.png"),
                LoadTexture(graphicsDevice, "Content/assets/terrain/mosaicFloor/mosaic_3.png"),
                LoadTexture(graphicsDevice, "Content/assets/terrain/mosaicFloor/mosaic_4.png"),
                LoadTexture(graphicsDevice, "Content/assets/terrain/mosaicFloor/mosaic_5.png"),
                LoadTexture(graphicsDevice, "Content/assets/terrain/mosaicFloor/mosaic_6.png"),
                LoadTexture(graphicsDevice, "Content/assets/terrain/mosaicFloor/mosaic_7.png"),
                LoadTexture(graphicsDevice, "Content/assets/terrain/mosaicFloor/mosaic_8.png"),
                LoadTexture(graphicsDevice, "Content/assets/terrain/mosaicFloor/mosaic_9.png"),
                LoadTexture(graphicsDevice, "Content/assets/terrain/mosaicFloor/mosaic_10.png"),
                LoadTexture(graphicsDevice, "Content/assets/terrain/mosaicFloor/mosaic_11.png"),
                LoadTexture(graphicsDevice, "Content/assets/terrain/mosaicFloor/mosaic_12.png"),
                LoadTexture(graphicsDevice, "Content/assets/terrain/mosaicFloor/mosaic_13.png"),
                LoadTexture(graphicsDevice, "Content/assets/terrain/mosaicFloor/mosaic_14.png"),
                LoadTexture(graphicsDevice, "Content/assets/terrain/mosaicFloor/mosaic_15.png"),
            ]
        );
        TerrainSpriteSet.Add(Terrain.OrcFloor, [
                LoadTexture(graphicsDevice, "Content/assets/terrain/orcFloor/orc_0.png"),
                LoadTexture(graphicsDevice, "Content/assets/terrain/orcFloor/orc_1.png"),
                LoadTexture(graphicsDevice, "Content/assets/terrain/orcFloor/orc_2.png"),
                LoadTexture(graphicsDevice, "Content/assets/terrain/orcFloor/orc_3.png"),
                LoadTexture(graphicsDevice, "Content/assets/terrain/orcFloor/orc_4.png"),
                LoadTexture(graphicsDevice, "Content/assets/terrain/orcFloor/orc_5.png"),
                LoadTexture(graphicsDevice, "Content/assets/terrain/orcFloor/orc_6.png"),
                LoadTexture(graphicsDevice, "Content/assets/terrain/orcFloor/orc_7.png")
            ]
        );
        TerrainSpriteSet.Add(Terrain.OrcWall, [
                LoadTexture(graphicsDevice, "Content/assets/terrain/orcWall/orc_0.png"),
                LoadTexture(graphicsDevice, "Content/assets/terrain/orcWall/orc_1.png"),
                LoadTexture(graphicsDevice, "Content/assets/terrain/orcWall/orc_2.png"),
                LoadTexture(graphicsDevice, "Content/assets/terrain/orcWall/orc_3.png")
            ]
        );
        TerrainSpriteSet.Add(Terrain.ReliefWall, [
                LoadTexture(graphicsDevice, "Content/assets/terrain/reliefWall/relief_0.png"),
                LoadTexture(graphicsDevice, "Content/assets/terrain/reliefWall/relief_1.png"),
                LoadTexture(graphicsDevice, "Content/assets/terrain/reliefWall/relief_2.png"),
                LoadTexture(graphicsDevice, "Content/assets/terrain/reliefWall/relief_3.png"),
            ]
        );
        
        PlayerSpriteSet.Add(PlayerSprites.HumanMale, LoadTexture(graphicsDevice, "Content/assets/player/human_male.png"));
        PlayerSpriteSet.Add(PlayerSprites.RobeBlackAndGold, LoadTexture(graphicsDevice, "Content/assets/player/robe_black_gold.png"));
        UiSpriteSet.Add(UISprites.CursorGreen, LoadTexture(graphicsDevice, "Content/assets/ui/cursor_green.png"));
        UiSpriteSet.Add(UISprites.CursorRed, LoadTexture(graphicsDevice, "Content/assets/ui/cursor_red.png"));
    }
}