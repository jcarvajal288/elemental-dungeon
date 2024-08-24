
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonogameTest.map;

public static class RoomDigger {

    private static Vector2 _digCenter;
    private static PlayerAction _digDirection;
    private static int _halfWidth;
    private static int _halfHeight;
    private static Vector2 _corridorTopLeft;
    private static Vector2 _corridorBottomRight;
    
    public static GameState CheckForNewDig(GameState gameState, PlayerAction playerAction, Vector2 playerPosition, Map map) {
        const int distanceFromPlayer = 5;
        _halfWidth = 2;
        _halfHeight = 2;
        return playerAction switch {
            PlayerAction.DigLeft => StartDigLeft(),
            PlayerAction.DigRight => StartDigRight(),
            PlayerAction.DigUp => StartDigUp(),
            PlayerAction.DigDown => StartDigDown(),
            _ => gameState
        };

        GameState StartDigLeft() {
            if (map.GetTileAt(playerPosition with { X = playerPosition.X - 1 }).IsWalkable()) {
                return GameState.Moving;
            }
            _digCenter = playerPosition with { X = playerPosition.X - distanceFromPlayer };
            _corridorTopLeft = _digCenter with { Y = _digCenter.Y - 1 };
            _corridorBottomRight = playerPosition with { X = playerPosition.X - 1, Y = playerPosition.Y + 1 };
            _digDirection = PlayerAction.DigLeft;
            return GameState.Digging;
        }

        GameState StartDigRight() {
            if (map.GetTileAt(playerPosition with { X = playerPosition.X + 1 }).IsWalkable()) {
                return GameState.Moving;
            }
            _digCenter = playerPosition with { X = playerPosition.X + distanceFromPlayer };
            _corridorTopLeft = playerPosition with { X = playerPosition.X + 1, Y = playerPosition.Y - 1 };
            _corridorBottomRight = _digCenter with { Y = _digCenter.Y + 1 };
            _digDirection = PlayerAction.DigRight;
            return GameState.Digging;
        }

        GameState StartDigUp() {
            if (map.GetTileAt(playerPosition with { Y = playerPosition.Y - 1 }).IsWalkable()) {
                return GameState.Moving;
            }
            _digCenter = playerPosition with { Y = playerPosition.Y - distanceFromPlayer };
            _corridorTopLeft = _digCenter with { X = _digCenter.X - 1 };
            _corridorBottomRight = playerPosition with { X = playerPosition.X + 1, Y = playerPosition.Y - 1 };
            _digDirection = PlayerAction.DigUp;
            return GameState.Digging;
        }

        GameState StartDigDown() {
            if (map.GetTileAt(playerPosition with { Y = playerPosition.Y + 1 }).IsWalkable()) {
                return GameState.Moving;
            }
            _digCenter = playerPosition with { Y = playerPosition.Y + distanceFromPlayer };
            _corridorTopLeft = playerPosition with { X = playerPosition.X - 1, Y = playerPosition.Y + 1 };
            _corridorBottomRight = _digCenter with { X = _digCenter.X + 1 };
            _digDirection = PlayerAction.DigDown;
            return GameState.Digging;
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

        for (int y = (int)_corridorTopLeft.Y; y <= _corridorBottomRight.Y; y++) {
            for (int x = (int)_corridorTopLeft.X; x <= _corridorBottomRight.X; x++) {
                Vector2 pixelPosition = new(x * Tile.Size, y * Tile.Size);
                spriteBatch.Draw(Images.UISpriteSet[UISprites.CursorGreen], pixelPosition, Color.White);
            }
        }
    }

    public static void AdjustBlueprint(PlayerAction playerAction) {
        const int minSize = 2;
        const int maxSize = 5;
        switch (playerAction) {
            case PlayerAction.DecreaseBlueprintWidth:
                if (_halfWidth > minSize) {
                    _halfWidth -= 1;
                }
                break;
            case PlayerAction.IncreaseBlueprintWidth:
                if (_halfWidth < maxSize) {
                    _halfWidth += 1;
                }
                break;
            case PlayerAction.IncreaseBlueprintHeight:
                if (_halfHeight < maxSize) {
                    _halfHeight += 1;
                }
                break;
            case PlayerAction.DecreaseBlueprintHeight:
                if (_halfHeight > minSize) {
                    _halfHeight -= 1;
                }
                break;
            case PlayerAction.MoveLeft:
                _digCenter = ValidateCenterMove(_digCenter with { X = _digCenter.X - 1 });
                break;
            case PlayerAction.MoveRight:
                _digCenter = ValidateCenterMove(_digCenter with { X = _digCenter.X + 1 });
                break;
            case PlayerAction.MoveUp:
                _digCenter = ValidateCenterMove(_digCenter with { Y = _digCenter.Y - 1 });
                break;
            case PlayerAction.MoveDown:
                _digCenter = ValidateCenterMove(_digCenter with { Y = _digCenter.Y + 1 });
                break;
        }

        Vector2 ValidateCenterMove(Vector2 newPosition) {
            Vector2 newRoomTopLeft = newPosition with { X = newPosition.X - _halfWidth, Y = newPosition.Y - _halfHeight };
            Vector2 newRoomBottomRight = newPosition with { X = newPosition.X + _halfWidth, Y = newPosition.Y + _halfHeight };
            if (_digDirection is PlayerAction.DigLeft or PlayerAction.DigRight) {
                if (newRoomTopLeft.Y > _corridorTopLeft.Y || newRoomBottomRight.Y < _corridorBottomRight.Y) {
                    return _digCenter;
                }
            } else if (_digDirection is PlayerAction.DigUp or PlayerAction.DigDown){
                if (newRoomTopLeft.X > _corridorTopLeft.X || newRoomBottomRight.X < _corridorBottomRight.X) {
                    return _digCenter;
                }
            }
            return newPosition;
        }
    }
}