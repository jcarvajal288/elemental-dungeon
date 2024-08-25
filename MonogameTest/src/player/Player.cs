using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonogameTest.map;

namespace MonogameTest;

public class Player {
    private Vector2 _position;

    public Vector2 Position => _position;

    public Player(Vector2 position) {
        _position = position;            
    }

    public void Draw(SpriteBatch spriteBatch) {
        Vector2 pixelPosition = new(_position.X * Tile.Size, _position.Y * Tile.Size);
        spriteBatch.Draw(Images.PlayerSpriteSet[PlayerSprites.HumanMale], pixelPosition, Color.White);
        spriteBatch.Draw(Images.PlayerSpriteSet[PlayerSprites.RobeBlackAndGold], pixelPosition, Color.White);
    }

    private void TryMove(Vector2 newPosition, Map map) {
        if (map.GetTileAt(newPosition).IsWalkable()) {
            _position = newPosition;
        }
        Console.WriteLine(map.GetRoomIdForPosition(_position));
    }

    public void SendAction(PlayerAction action, Map map) {
        switch (action) {
            case PlayerAction.MoveLeft:
                TryMove(new Vector2(_position.X - 1, _position.Y), map);
                break;
            case PlayerAction.MoveRight:
                TryMove(new Vector2(_position.X + 1, _position.Y), map);
                break;
            case PlayerAction.MoveUp:
                TryMove(new Vector2(_position.X, _position.Y - 1), map);
                break;
            case PlayerAction.MoveDown:
                TryMove(new Vector2(_position.X, _position.Y + 1), map);
                break;
        }
    }
}