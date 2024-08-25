using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.Graphics;

namespace MonogameTest;

public static class Images {
    public static readonly Dictionary<Terrain, List<Texture2D>> TerrainSpriteSet = new();
    public static readonly Dictionary<PlayerSprites, Texture2D> PlayerSpriteSet = [];
    public static readonly Dictionary<UISprites, Texture2D> UISpriteSet = [];

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
        TerrainSpriteSet.Add(Terrain.CobbleStoneFloor, [
                LoadTexture(graphicsDevice, "Content/assets/terrain/cobbleStoneFloor/cobble_blood_1_new.png"),
                LoadTexture(graphicsDevice, "Content/assets/terrain/cobbleStoneFloor/cobble_blood_2_new.png"),
                LoadTexture(graphicsDevice, "Content/assets/terrain/cobbleStoneFloor/cobble_blood_3_new.png"),
                LoadTexture(graphicsDevice, "Content/assets/terrain/cobbleStoneFloor/cobble_blood_4_new.png"),
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
        TerrainSpriteSet.Add(Terrain.GreyBrickWall, [
                LoadTexture(graphicsDevice, "Content/assets/terrain/greyBrickWall/brick_gray_0.png"),
                LoadTexture(graphicsDevice, "Content/assets/terrain/greyBrickWall/brick_gray_1.png"),
                LoadTexture(graphicsDevice, "Content/assets/terrain/greyBrickWall/brick_gray_2.png"),
                LoadTexture(graphicsDevice, "Content/assets/terrain/greyBrickWall/brick_gray_3.png"),
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
        
        PlayerSpriteSet.Add(PlayerSprites.HumanMale, LoadTexture(graphicsDevice, "Content/assets/player/human_male.png"));
        PlayerSpriteSet.Add(PlayerSprites.RobeBlackAndGold, LoadTexture(graphicsDevice, "Content/assets/player/robe_black_gold.png"));
        UISpriteSet.Add(UISprites.CursorGreen, LoadTexture(graphicsDevice, "Content/assets/ui/cursor_green.png"));
        UISpriteSet.Add(UISprites.CursorRed, LoadTexture(graphicsDevice, "Content/assets/ui/cursor_red.png"));
    }
}