
using System;
using System.Collections.Generic;
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

    public static GameState AdjustBlueprint(PlayerAction playerAction, Map map) {
        const int minSize = 2;
        const int maxSize = 5;
        switch (playerAction) {
            case PlayerAction.DecreaseBlueprintWidth:
                if (_halfWidth > minSize && RoomAndCorridorAreValid(_digCenter, _halfWidth - 1, _halfWidth)) {
                    _halfWidth -= 1;
                }
                return GameState.Digging;
            case PlayerAction.IncreaseBlueprintWidth:
                if (_halfWidth < maxSize && RoomAndCorridorAreValid(_digCenter, _halfWidth + 1, _halfWidth)) {
                    _halfWidth += 1;
                }
                return GameState.Digging;
            case PlayerAction.IncreaseBlueprintHeight:
                if (_halfHeight < maxSize && RoomAndCorridorAreValid(_digCenter, _halfWidth, _halfHeight + 1)) {
                    _halfHeight += 1;
                }
                return GameState.Digging;
            case PlayerAction.DecreaseBlueprintHeight:
                if (_halfHeight > minSize && RoomAndCorridorAreValid(_digCenter, _halfWidth, _halfHeight - 1)) {
                    _halfHeight -= 1;
                }
                return GameState.Digging;
            case PlayerAction.MoveLeft:
                ValidateCenterMove(_digCenter with { X = _digCenter.X - 1 });
                return GameState.Digging;
            case PlayerAction.MoveRight:
                ValidateCenterMove(_digCenter with { X = _digCenter.X + 1 });
                return GameState.Digging;
            case PlayerAction.MoveUp:
                ValidateCenterMove(_digCenter with { Y = _digCenter.Y - 1 });
                return GameState.Digging;
            case PlayerAction.MoveDown:
                ValidateCenterMove(_digCenter with { Y = _digCenter.Y + 1 });
                return GameState.Digging;
            case PlayerAction.SubmitRoomBlueprint:
                DigRoom(map);
                return GameState.Moving;
            default:
                return GameState.Digging;
        }

        void ValidateCenterMove(Vector2 newCenterPosition) {
            if (!RoomAndCorridorAreValid(newCenterPosition, _halfWidth, _halfHeight)) return;
            _digCenter = newCenterPosition;
            switch (_digDirection) {
                case PlayerAction.DigLeft:
                    _corridorTopLeft = _corridorTopLeft with { X = _digCenter.X};
                    break;
                case PlayerAction.DigRight:
                    _corridorBottomRight = _corridorBottomRight with { X = _digCenter.X };
                    break;
                case PlayerAction.DigUp:
                    _corridorTopLeft = _corridorTopLeft with { Y = _digCenter.Y };
                    break;
                case PlayerAction.DigDown:
                    _corridorBottomRight = _corridorBottomRight with { Y = _digCenter.Y };
                    break;
            }
        }
    }

    private static void DigRoom(Map map) {
        Vector2 roomTopLeft = _digCenter with { X = _digCenter.X - _halfWidth, Y = _digCenter.Y - _halfHeight };
        int width = _halfWidth * 2;
        int height = _halfHeight * 2;
        Vector2 roomBottomRight = roomTopLeft with { X = roomTopLeft.X + width, Y = roomTopLeft.Y + height };
        HashSet<Vector2> roomTiles = GetTileRegion(roomTopLeft, roomBottomRight);
        HashSet<Vector2> corridorTiles = GetTileRegion(_corridorTopLeft, _corridorBottomRight);
        roomTiles.UnionWith(corridorTiles);
        foreach (Vector2 position in roomTiles) {
            map.SetTileAt(position, Tile.CreateOrcFloorTile());
        }
    }

    private static bool RoomAndCorridorAreValid(Vector2 newPosition, int newHalfWidth, int newHalfHeight) {
        Vector2 newRoomTopLeft = newPosition with { X = newPosition.X - newHalfWidth, Y = newPosition.Y - newHalfHeight };
        Vector2 newRoomBottomRight = newPosition with { X = newPosition.X + newHalfWidth, Y = newPosition.Y + newHalfHeight };
        if (_digDirection is PlayerAction.DigLeft or PlayerAction.DigRight) {
            if (newRoomTopLeft.Y > _corridorTopLeft.Y || newRoomBottomRight.Y < _corridorBottomRight.Y) {
                return false;
            } 
        } else if (_digDirection is PlayerAction.DigUp or PlayerAction.DigDown){
            if (newRoomTopLeft.X > _corridorTopLeft.X || newRoomBottomRight.X < _corridorBottomRight.X) {
                return false;
            }
        } 
        if (!CorridorIsLongEnough(newRoomTopLeft, newRoomBottomRight)) {
            return false;
        }
        return true;
    }

    private static bool CorridorIsLongEnough(Vector2 newRoomTopLeft, Vector2 newRoomBottomRight) {
        HashSet<Vector2> roomTiles = GetTileRegion(newRoomTopLeft, newRoomBottomRight);
        HashSet<Vector2> corridorTiles = GetTileRegion(_corridorTopLeft, _corridorBottomRight);

        corridorTiles.ExceptWith(roomTiles);
        return corridorTiles.Count >= 6;
    }

    private static HashSet<Vector2> GetTileRegion(Vector2 topLeft, Vector2 bottomRight) {
        HashSet<Vector2> roomTiles = new();
        for (int y = (int)topLeft.Y; y <= bottomRight.Y; y++) {
            for (int x = (int)topLeft.X; x <= bottomRight.X; x++) {
                roomTiles.Add(new (x, y));
            }
        }
        return roomTiles;
    }
}