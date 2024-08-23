using Microsoft.Xna.Framework.Graphics;

namespace MonogameTest;

public class Tile
{
    private Terrain _terrain;
    public int _spriteIndex;

    public Tile(Terrain terrain, int spriteIndex)
    {
        _terrain = terrain;
        _spriteIndex = spriteIndex;
    }

    public Texture2D getSprite()
    {
        return Images.SpriteSet[_terrain][_spriteIndex];
    }
}