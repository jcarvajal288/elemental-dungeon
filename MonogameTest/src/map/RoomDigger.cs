
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonogameTest.map;

public static class RoomDigger {

    private static Vector2 _digCenter;
    private static int _halfWidth;
    private static int _halfHeight;
    
    public static GameState CheckForNewDig(GameState gameState, PlayerAction playerAction, Vector2 playerPosition) {
        const int distanceFromPlayer = 5;
        _halfWidth = 2;
        _halfHeight = 2;
        switch (playerAction) {
            case PlayerAction.DigLeft:
                _digCenter = playerPosition with { X = playerPosition.X - distanceFromPlayer };
                return GameState.Digging;
            case PlayerAction.DigRight:
                _digCenter = playerPosition with { X = playerPosition.X + distanceFromPlayer };
                return GameState.Digging;
            case PlayerAction.DigUp:
                _digCenter = playerPosition with { Y = playerPosition.Y - distanceFromPlayer };
                return GameState.Digging;
            case PlayerAction.DigDown:
                _digCenter = playerPosition with { Y = playerPosition.Y + distanceFromPlayer };
                return GameState.Digging;
            default:
                return gameState;
        }
    }

    public static void DrawRoomBlueprint(SpriteBatch spriteBatch) {
        Vector2 topLeft = _digCenter with { X = _digCenter.X - _halfWidth, Y = _digCenter.Y - _halfHeight };
        int width = _halfWidth * 2 + 1;
        int height = _halfHeight * 2 + 1;
        for (int y = (int)topLeft.Y; y < topLeft.Y + height; y++) {
            for (int x = (int)topLeft.X; x < topLeft.X + width; x++) {
                Vector2 pixelPosition = new(x * Tile.Size, y * Tile.Size);
                spriteBatch.Draw(Images.UISpriteSet[UISprites.CursorGreen], pixelPosition, Color.White);
            }
        }
    }
}