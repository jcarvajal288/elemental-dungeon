using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonogameTest.map;

namespace MonogameTest.player;

public class Player(Vector2 position) {
    private Vector2 _position = position;

    public Vector2 Position => _position;

    public void Draw(SpriteBatch spriteBatch) {
        Vector2 pixelPosition = new(_position.X * Tile.Size, _position.Y * Tile.Size);
        spriteBatch.Draw(Images.PlayerSpriteSet[PlayerSprites.HumanMale], pixelPosition, Color.White);
        spriteBatch.Draw(Images.PlayerSpriteSet[PlayerSprites.RobeBlackAndGold], pixelPosition, Color.White);
    }

    private void TryMove(Vector2 newPosition, Map map) {
        if (map.GetTileAt(newPosition).IsWalkable()) {
            int roomId = map.GetRoomIdForPosition(newPosition);
            if (roomId == -1) {
                Console.Out.WriteLine(map.GetCorridorForPosition(newPosition).GetConnectedRoomIds());
            } else {
                Console.Out.WriteLine("Earth: {0}", map.GetRoomForId(roomId).EarthElementalPower);
            }
            _position = newPosition;
        }
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