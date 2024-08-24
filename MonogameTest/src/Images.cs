using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.Graphics;

namespace MonogameTest;

public static class Images {
    public static readonly Dictionary<Terrain, List<Texture2D>> TerrainSpriteSet = new();
    public static readonly Dictionary<PlayerSprite, Texture2D> PlayerSpriteSet = [];

    private static Texture2D LoadTexture(GraphicsDevice graphicsDevice, string path) {
        using FileStream fileStream = File.OpenRead(path);
        return Texture2D.FromStream(graphicsDevice, fileStream);
    }

    public static void LoadImages(GraphicsDevice graphicsDevice) {
        TerrainSpriteSet.Add(Terrain.OrcWall, [
                LoadTexture(graphicsDevice, "Content/assets/orcWall/orc_0.png"),
                LoadTexture(graphicsDevice, "Content/assets/orcWall/orc_1.png"),
                LoadTexture(graphicsDevice, "Content/assets/orcWall/orc_2.png"),
                LoadTexture(graphicsDevice, "Content/assets/orcWall/orc_3.png")
            ]
        );
        TerrainSpriteSet.Add(Terrain.OrcFloor, [
                LoadTexture(graphicsDevice, "Content/assets/orcFloor/orc_0.png"),
                LoadTexture(graphicsDevice, "Content/assets/orcFloor/orc_1.png"),
                LoadTexture(graphicsDevice, "Content/assets/orcFloor/orc_2.png"),
                LoadTexture(graphicsDevice, "Content/assets/orcFloor/orc_3.png"),
                LoadTexture(graphicsDevice, "Content/assets/orcFloor/orc_4.png"),
                LoadTexture(graphicsDevice, "Content/assets/orcFloor/orc_5.png"),
                LoadTexture(graphicsDevice, "Content/assets/orcFloor/orc_6.png"),
                LoadTexture(graphicsDevice, "Content/assets/orcFloor/orc_7.png")
            ]
        );
        TerrainSpriteSet.Add(Terrain.CrystalWallLightBlue, [
                LoadTexture(graphicsDevice, "Content/assets/crystals/crystal_wall_lightblue.png")
            ]
        );
        PlayerSpriteSet.Add(PlayerSprite.HumanMale, LoadTexture(graphicsDevice, "Content/assets/player/human_male.png"));
        PlayerSpriteSet.Add(PlayerSprite.RobeBlackAndGold, LoadTexture(graphicsDevice, "Content/assets/player/robe_black_gold.png"));
    }
}