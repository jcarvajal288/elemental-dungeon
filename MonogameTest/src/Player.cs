using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonogameTest;

public class Player {
    private readonly Vector2 _position;

    public Vector2 Position => _position;

    public Player(Vector2 position) {
        _position = position;            
    }

    public void Draw(SpriteBatch spriteBatch) {
        Vector2 pixelPosition = new(_position.X * Tile.Size, _position.Y * Tile.Size);
        spriteBatch.Draw(Images.PlayerSpriteSet[PlayerSprite.HumanMale], pixelPosition, Color.White);
        spriteBatch.Draw(Images.PlayerSpriteSet[PlayerSprite.RobeBlackAndGold], pixelPosition, Color.White);
    }
}