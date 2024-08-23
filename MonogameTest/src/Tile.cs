namespace MonogameTest;

public class Tile
{
    private Terrain _terrain;
    public int SpriteIndex { get; }

    public Tile(Terrain terrain, int spriteIndex)
    {
        _terrain = terrain;
        SpriteIndex = spriteIndex;
    }
}