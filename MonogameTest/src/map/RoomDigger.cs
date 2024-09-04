using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonogameTest.map.rooms;

namespace MonogameTest.map;

public static class RoomDigger {

    private static Vector2 _digCenter;
    private static PlayerAction _digDirection;
    private static Room _playerRoom;
    private static int _halfWidth;
    private static int _halfHeight;
    private static readonly int CorridorHalfWidth = 2;
    
    private static Vector2 _corridorTopLeft;
    private static Vector2 _corridorBottomRight;
    
    private static bool _isDigValid;
    private static int _mapWidth;
    private static int _mapHeight;
    
    public static Vector2 DigCenter => _digCenter;
    
    public static bool IsNewDigValid(GameState gameState, PlayerAction playerAction, Vector2 playerPosition, Map map, int playerRoomId) {
        const int distanceFromPlayer = 6;
        _halfWidth = 3;
        _halfHeight = 3;
        _mapWidth = map.GetWidth();
        _mapHeight = map.GetHeight();
        _playerRoom = map.GetRoomForId(playerRoomId);
        Vector2 digOrigin = playerPosition;
        return playerAction switch {
            PlayerAction.DigLeft => StartDigLeft(),
            PlayerAction.DigRight => StartDigRight(),
            PlayerAction.DigUp => StartDigUp(),
            PlayerAction.DigDown => StartDigDown(),
            _ => false
        };

        bool StartDigLeft() {
            if (!_playerRoom.GetEdgePositions().Contains(playerPosition with { X = playerPosition.X - 1 })) {
                return false;
            }

            // adjust corridor position if player is in the corner of their room
            if ((int)digOrigin.Y - (int)_playerRoom.GetTopLeft().Y == 1) {
                digOrigin = digOrigin with { Y = digOrigin.Y + 1 };
            } else if ((int)_playerRoom.GetBottomRight().Y - (int)digOrigin.Y == 1) {
                digOrigin = digOrigin with { Y = digOrigin.Y - 1 };
            }
            
            _digCenter = digOrigin with { X = digOrigin.X - distanceFromPlayer };
            _corridorTopLeft = _digCenter with { Y = _digCenter.Y - CorridorHalfWidth };
            _corridorBottomRight = digOrigin with { X = digOrigin.X - 1, Y = digOrigin.Y + CorridorHalfWidth };
            _digDirection = PlayerAction.DigLeft;
            ValidateDig(map);
            return true;
        }

        bool StartDigRight() {
            if (!_playerRoom.GetEdgePositions().Contains(digOrigin with { X = digOrigin.X + 1 })) {
                return false;
            }
            
            // adjust corridor position if player is in the corner of their room
            if ((int)digOrigin.Y - (int)_playerRoom.GetTopLeft().Y == 1) {
                digOrigin = digOrigin with { Y = digOrigin.Y + 1 };
            } else if ((int)_playerRoom.GetBottomRight().Y - (int)digOrigin.Y == 1) {
                digOrigin = digOrigin with { Y = digOrigin.Y - 1 };
            }
            
            _digCenter = digOrigin with { X = digOrigin.X + distanceFromPlayer };
            _corridorTopLeft = digOrigin with { X = digOrigin.X + 1, Y = digOrigin.Y - CorridorHalfWidth };
            _corridorBottomRight = _digCenter with { Y = _digCenter.Y + CorridorHalfWidth };
            _digDirection = PlayerAction.DigRight;
            ValidateDig(map);
            return true;
        }

        bool StartDigUp() {
            if (!_playerRoom.GetEdgePositions().Contains(digOrigin with { Y = digOrigin.Y - 1 })) {
                return false;
            }
            
            // adjust corridor position if player is in the corner of their room
            if ((int)digOrigin.X - (int)_playerRoom.GetTopLeft().X == 1) {
                digOrigin = digOrigin with { X = digOrigin.X + 1 };
            } else if ((int)_playerRoom.GetBottomRight().X - (int)digOrigin.X == 1) {
                digOrigin = digOrigin with { X = digOrigin.X - 1 };
            }
            
            _digCenter = digOrigin with { Y = digOrigin.Y - distanceFromPlayer };
            _corridorTopLeft = _digCenter with { X = _digCenter.X - CorridorHalfWidth };
            _corridorBottomRight = digOrigin with { X = digOrigin.X + CorridorHalfWidth, Y = digOrigin.Y - 1 };
            _digDirection = PlayerAction.DigUp;
            ValidateDig(map);
            return true;
        }

        bool StartDigDown() {
            if (!_playerRoom.GetEdgePositions().Contains(digOrigin with { Y = digOrigin.Y + 1 })) {
                return false;
            }
            
            // adjust corridor position if player is in the corner of their room
            if ((int)digOrigin.X - (int)_playerRoom.GetTopLeft().X == 1) {
                digOrigin = digOrigin with { X = digOrigin.X + 1 };
            } else if ((int)_playerRoom.GetBottomRight().X - (int)digOrigin.X == 1) {
                digOrigin = digOrigin with { X = digOrigin.X - 1 };
            }
            
            _digCenter = digOrigin with { Y = digOrigin.Y + distanceFromPlayer };
            _corridorTopLeft = digOrigin with { X = digOrigin.X - CorridorHalfWidth, Y = digOrigin.Y + 1 };
            _corridorBottomRight = _digCenter with { X = _digCenter.X + CorridorHalfWidth };
            _digDirection = PlayerAction.DigDown;
            ValidateDig(map);
            return true;
        }
    }

    public static void DrawRoomBlueprint(SpriteBatch spriteBatch) {
        Texture2D cursorSprite = _isDigValid
            ? Images.UiSpriteSet[UISprites.CursorGreen]
            : Images.UiSpriteSet[UISprites.CursorRed];
        Vector2 topLeft = _digCenter with { X = _digCenter.X - _halfWidth, Y = _digCenter.Y - _halfHeight };
        int width = _halfWidth * 2 + 1;
        int height = _halfHeight * 2 + 1;
        for (int y = (int)topLeft.Y; y < topLeft.Y + height; y++) {
            for (int x = (int)topLeft.X; x < topLeft.X + width; x++) {
                Vector2 pixelPosition = new(x * Tile.Size, y * Tile.Size);
                spriteBatch.Draw(cursorSprite, pixelPosition, Color.White);
            }
        }

        for (int y = (int)_corridorTopLeft.Y; y <= _corridorBottomRight.Y; y++) {
            for (int x = (int)_corridorTopLeft.X; x <= _corridorBottomRight.X; x++) {
                Vector2 pixelPosition = new(x * Tile.Size, y * Tile.Size);
                spriteBatch.Draw(cursorSprite, pixelPosition, Color.White);
            }
        }
    }

    public static GameState AdjustBlueprint(PlayerAction playerAction, Map map) {
        const int minSize = 3;
        const int maxSize = 6;
        switch (playerAction) {
            case PlayerAction.DecreaseBlueprintWidth:
                if (_halfWidth > minSize) {
                    _halfWidth -= 1;
                }
                ValidateDig(map);
                return GameState.Digging;
            case PlayerAction.IncreaseBlueprintWidth:
                if (_halfWidth < maxSize) {
                    _halfWidth += 1;
                }
                ValidateDig(map);
                return GameState.Digging;
            case PlayerAction.IncreaseBlueprintHeight:
                if (_halfHeight < maxSize) {
                    _halfHeight += 1;
                }
                ValidateDig(map);
                return GameState.Digging;
            case PlayerAction.DecreaseBlueprintHeight:
                if (_halfHeight > minSize) {
                    _halfHeight -= 1;
                }
                ValidateDig(map);
                return GameState.Digging;
            case PlayerAction.MoveLeft:
                MoveCenterAndValidate(_digCenter with { X = _digCenter.X - 1 });
                return GameState.Digging;
            case PlayerAction.MoveRight:
                MoveCenterAndValidate(_digCenter with { X = _digCenter.X + 1 });
                return GameState.Digging;
            case PlayerAction.MoveUp:
                MoveCenterAndValidate(_digCenter with { Y = _digCenter.Y - 1 });
                return GameState.Digging;
            case PlayerAction.MoveDown:
                MoveCenterAndValidate(_digCenter with { Y = _digCenter.Y + 1 });
                return GameState.Digging;
            case PlayerAction.SubmitRoomBlueprint when _isDigValid:
                PerformDig(RoomType.EarthElementalFont, map);
                return GameState.Moving;
            default:
                return GameState.Digging;
        }

        void MoveCenterAndValidate(Vector2 newCenterPosition) {
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
            ValidateDig(map);
        }
    }

    private static void PerformDig(RoomType roomType, Map map, bool digCorridor = true) {
        Vector2 roomTopLeft = _digCenter with { X = _digCenter.X - _halfWidth, Y = _digCenter.Y - _halfHeight };
        int width = _halfWidth * 2;
        int height = _halfHeight * 2;
        Vector2 roomBottomRight = roomTopLeft with { X = roomTopLeft.X + width, Y = roomTopLeft.Y + height };
        Room newRoom = Room.CreateRoom(roomType, roomTopLeft, roomBottomRight);
        map.AddRoom(newRoom);
        
        if (digCorridor) {
            List<Vector2> newRoomTiles = newRoom.GetTilePositions();
            List<Vector2> oldRoomTiles = _playerRoom.GetTilePositions();
            HashSet<Vector2> corridorTiles = GetTileRegion(_corridorTopLeft, _corridorBottomRight);
            corridorTiles.ExceptWith(newRoomTiles);
            corridorTiles.ExceptWith(oldRoomTiles);
            
            bool isCorridorHorizontal = _digDirection is PlayerAction.DigLeft or PlayerAction.DigRight;
            Corridor corridor = new(corridorTiles.ToList(), isCorridorHorizontal);
            
            newRoom.AddDoorwayForCorridor(corridor.GetFloorTiles());
            _playerRoom.AddDoorwayForCorridor(corridor.GetFloorTiles());
            map.AddCorridor(corridor);
        }
    }

    public static void DigRoom(RoomType roomType, Map map, Vector2 center, int halfWidth, int halfHeight ) {
        _digCenter = center;
        _halfWidth = halfWidth;
        _halfHeight = halfHeight;
        PerformDig(roomType, map, false);
    }

    private static void ValidateDig(Map map) {
        Vector2 roomTopLeft = _digCenter with { X = _digCenter.X - _halfWidth, Y = _digCenter.Y - _halfHeight };
        Vector2 roomBottomRight = _digCenter with { X = _digCenter.X + _halfWidth, Y = _digCenter.Y + _halfHeight };
        if (_digDirection is PlayerAction.DigLeft or PlayerAction.DigRight) {
            if (roomTopLeft.Y > _corridorTopLeft.Y || roomBottomRight.Y < _corridorBottomRight.Y) {
                _isDigValid = false;
                return;
            } 
        } else if (_digDirection is PlayerAction.DigUp or PlayerAction.DigDown){
            if (roomTopLeft.X > _corridorTopLeft.X || roomBottomRight.X < _corridorBottomRight.X) {
                _isDigValid = false;
                return;
            }
        } 
        if (!CorridorIsLongEnough(roomTopLeft, roomBottomRight)) {
            _isDigValid = false;
            return;
        }

        if (roomTopLeft.X < 1 || roomTopLeft.Y < 1 || roomBottomRight.X >= _mapWidth-1 || roomBottomRight.Y >= _mapHeight-1) {
            _isDigValid = false;
            return;
        }
        
        HashSet<Vector2> blueprintTiles = GetTileRegion(roomTopLeft, roomBottomRight);
        blueprintTiles.UnionWith(GetTileRegion(_corridorTopLeft, _corridorBottomRight));
        blueprintTiles.ExceptWith(_playerRoom.GetTilePositions());
        if (blueprintTiles.Any(tile => !TerrainExtensions.DiggableTerrain.Contains(map.GetTileAt(tile).Terrain))) {
            _isDigValid = false;
            return;
        }
            
        _isDigValid = true;
    }

    private static bool CorridorIsLongEnough(Vector2 newRoomTopLeft, Vector2 newRoomBottomRight) {
        HashSet<Vector2> roomTiles = GetTileRegion(newRoomTopLeft, newRoomBottomRight);
        HashSet<Vector2> corridorTiles = GetTileRegion(_corridorTopLeft, _corridorBottomRight);

        corridorTiles.ExceptWith(roomTiles);
        return corridorTiles.Count >= 6;
    }

    public static HashSet<Vector2> GetTileRegion(Vector2 topLeft, Vector2 bottomRight) {
        HashSet<Vector2> roomTiles = new();
        for (int y = (int)topLeft.Y; y <= bottomRight.Y; y++) {
            for (int x = (int)topLeft.X; x <= bottomRight.X; x++) {
                roomTiles.Add(new Vector2(x, y));
            }
        }
        return roomTiles;
    }
}