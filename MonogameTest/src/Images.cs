using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.Graphics;

namespace MonogameTest;

public static class Images
{
    private static List<Texture2D> _orcWall;

    public static Texture2D OrcWall(int index)
    {
        return _orcWall[index];
    }

    private static Texture2D LoadTexture(GraphicsDevice graphicsDevice, string path)
    {
        using FileStream fileStream = File.OpenRead(path);
        return Texture2D.FromStream(graphicsDevice, fileStream);
    }

    public static void LoadImages(GraphicsDevice graphicsDevice)
    {
        _orcWall =
        [
            LoadTexture(graphicsDevice, "Content/assets/orc_0.png"),
            LoadTexture(graphicsDevice, "Content/assets/orc_1.png"),
            LoadTexture(graphicsDevice, "Content/assets/orc_2.png"),
            LoadTexture(graphicsDevice, "Content/assets/orc_3.png")
        ];
    }    
}