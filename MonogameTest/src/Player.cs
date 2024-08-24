using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonogameTest;

public class Player {
    private Vector2 _position;

    //public Vector2 Position => _position;

    public Player(Vector2 position) {
        _position = position;            
    }

    public void Draw(SpriteBatch spriteBatch) {
        Vector2 pixelPosition = new(_position.X * Tile.Size, _position.Y * Tile.Size);
        spriteBatch.Draw(Images.PlayerSpriteSet[PlayerSprite.HumanMale], pixelPosition, Color.White);
        spriteBatch.Draw(Images.PlayerSpriteSet[PlayerSprite.RobeBlackAndGold], pixelPosition, Color.White);
    }

    public void SendAction(PlayerAction action) {
        switch (action) {
            case PlayerAction.MoveLeft:
                _position.X -= 1;
                break;
            case PlayerAction.MoveRight:
                _position.X += 1;
                break;
            case PlayerAction.MoveUp:
                _position.Y -= 1;
                break;
            case PlayerAction.MoveDown:
                _position.Y += 1;
                break;
        };
    }
}