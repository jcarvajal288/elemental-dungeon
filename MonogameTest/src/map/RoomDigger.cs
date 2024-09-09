using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonogameTest.map;

public class RoomDigger {

    private Vector2 _digCenter;
    private PlayerAction _digDirection;
    private Room _playerRoom;
    private Vector2 _playerPosition;
    private int _halfWidth;
    private int _halfHeight;
    private const int CorridorHalfWidth = 2;

    private Vector2 _corridorTopLeft;
    private Vector2 _corridorBottomRight;
    
    private bool _isDigValid;
    public bool IsDiggingRoom { get; set; }
    private int _mapWidth;
    private int _mapHeight;
    
    public Vector2 DigCenter => _digCenter;
    
    public bool IsNewDigValid(PlayerAction playerAction, Vector2 playerPosition, Map map, int playerRoomId) {
        const int distanceFromPlayer = 6;
        _halfWidth = 3;
        _halfHeight = 3;
        _mapWidth = map.GetWidth();
        _mapHeight = map.GetHeight();
        _playerRoom = map.GetRoomForId(playerRoomId);
        _playerPosition = playerPosition;
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

            // adjust corridor and player position if player is in the corner of their room
            if ((int)digOrigin.Y - (int)_playerRoom.GetTopLeft().Y == 1) {
                digOrigin = digOrigin with { Y = digOrigin.Y + 1 };
                _playerPosition = _playerPosition with { Y = _playerPosition.Y + 1 };
            } else if ((int)_playerRoom.GetBottomRight().Y - (int)digOrigin.Y == 1) {
                digOrigin = digOrigin with { Y = digOrigin.Y - 1 };
                _playerPosition = _playerPosition with { Y = _playerPosition.Y - 1 };
            }
            
            _digCenter = digOrigin with { X = digOrigin.X - distanceFromPlayer };
            _corridorTopLeft = _digCenter with { Y = _digCenter.Y - CorridorHalfWidth };
            _corridorBottomRight = digOrigin with { X = digOrigin.X - 1, Y = digOrigin.Y + CorridorHalfWidth };
            _digDirection = PlayerAction.DigLeft;
            ValidateBlueprint(map);
            return true;
        }

        bool StartDigRight() {
            if (!_playerRoom.GetEdgePositions().Contains(digOrigin with { X = digOrigin.X + 1 })) {
                return false;
            }
            
            // adjust corridor and player position if player is in the corner of their room
            if ((int)digOrigin.Y - (int)_playerRoom.GetTopLeft().Y == 1) {
                digOrigin = digOrigin with { Y = digOrigin.Y + 1 };
                _playerPosition = _playerPosition with { Y = _playerPosition.Y + 1 };
            } else if ((int)_playerRoom.GetBottomRight().Y - (int)digOrigin.Y == 1) {
                digOrigin = digOrigin with { Y = digOrigin.Y - 1 };
                _playerPosition = _playerPosition with { Y = _playerPosition.Y - 1 };
            }
            
            _digCenter = digOrigin with { X = digOrigin.X + distanceFromPlayer };
            _corridorTopLeft = digOrigin with { X = digOrigin.X + 1, Y = digOrigin.Y - CorridorHalfWidth };
            _corridorBottomRight = _digCenter with { Y = _digCenter.Y + CorridorHalfWidth };
            _digDirection = PlayerAction.DigRight;
            ValidateBlueprint(map);
            return true;
        }

        bool StartDigUp() {
            if (!_playerRoom.GetEdgePositions().Contains(digOrigin with { Y = digOrigin.Y - 1 })) {
                return false;
            }
            
            // adjust corridor and player position if player is in the corner of their room
            if ((int)digOrigin.X - (int)_playerRoom.GetTopLeft().X == 1) {
                digOrigin = digOrigin with { X = digOrigin.X + 1 };
                _playerPosition = _playerPosition with { X = _playerPosition.X + 1 };
            } else if ((int)_playerRoom.GetBottomRight().X - (int)digOrigin.X == 1) {
                digOrigin = digOrigin with { X = digOrigin.X - 1 };
                _playerPosition = _playerPosition with { X = _playerPosition.X - 1 };
            }
            
            _digCenter = digOrigin with { Y = digOrigin.Y - distanceFromPlayer };
            _corridorTopLeft = _digCenter with { X = _digCenter.X - CorridorHalfWidth };
            _corridorBottomRight = digOrigin with { X = digOrigin.X + CorridorHalfWidth, Y = digOrigin.Y - 1 };
            _digDirection = PlayerAction.DigUp;
            ValidateBlueprint(map);
            return true;
        }

        bool StartDigDown() {
            if (!_playerRoom.GetEdgePositions().Contains(digOrigin with { Y = digOrigin.Y + 1 })) {
                return false;
            }
            
            // adjust corridor and player position if player is in the corner of their room
            if ((int)digOrigin.X - (int)_playerRoom.GetTopLeft().X == 1) {
                digOrigin = digOrigin with { X = digOrigin.X + 1 };
                _playerPosition = _playerPosition with { X = _playerPosition.X + 1 };
            } else if ((int)_playerRoom.GetBottomRight().X - (int)digOrigin.X == 1) {
                digOrigin = digOrigin with { X = digOrigin.X - 1 };
                _playerPosition = _playerPosition with { X = _playerPosition.X - 1 };
            }
            
            _digCenter = digOrigin with { Y = digOrigin.Y + distanceFromPlayer };
            _corridorTopLeft = digOrigin with { X = digOrigin.X - CorridorHalfWidth, Y = digOrigin.Y + 1 };
            _corridorBottomRight = _digCenter with { X = _digCenter.X + CorridorHalfWidth };
            _digDirection = PlayerAction.DigDown;
            ValidateBlueprint(map);
            return true;
        }
    }

    public void DrawBlueprint(SpriteBatch spriteBatch) {
        Texture2D cursorSprite = _isDigValid
            ? Images.UiSpriteSet[UISprites.CursorGreen]
            : Images.UiSpriteSet[UISprites.CursorRed];
        Vector2 topLeft = _digCenter with { X = _digCenter.X - _halfWidth, Y = _digCenter.Y - _halfHeight };
        int width = _halfWidth * 2 + 1;
        int height = _halfHeight * 2 + 1;
        if (IsDiggingRoom) {
            for (int y = (int)topLeft.Y; y < topLeft.Y + height; y++) {
                for (int x = (int)topLeft.X; x < topLeft.X + width; x++) {
                    Vector2 pixelPosition = new(x * Tile.Size, y * Tile.Size);
                    spriteBatch.Draw(cursorSprite, pixelPosition, Color.White);
                }
            }
        }

        for (int y = (int)_corridorTopLeft.Y; y <= _corridorBottomRight.Y; y++) {
            for (int x = (int)_corridorTopLeft.X; x <= _corridorBottomRight.X; x++) {
                Vector2 pixelPosition = new(x * Tile.Size, y * Tile.Size);
                spriteBatch.Draw(cursorSprite, pixelPosition, Color.White);
            }
        }
    }

    public GameState AdjustBlueprint(PlayerAction playerAction, Map map) {
        const int minSize = 3;
        const int maxSize = 6;
        switch (playerAction) {
            case PlayerAction.DecreaseBlueprintWidth:
                if (_halfWidth > minSize) {
                    _halfWidth -= 1;
                }
                ValidateBlueprint(map);
                return GameState.Digging;
            case PlayerAction.IncreaseBlueprintWidth:
                if (_halfWidth < maxSize) {
                    _halfWidth += 1;
                }
                ValidateBlueprint(map);
                return GameState.Digging;
            case PlayerAction.IncreaseBlueprintHeight:
                if (_halfHeight < maxSize) {
                    _halfHeight += 1;
                }
                ValidateBlueprint(map);
                return GameState.Digging;
            case PlayerAction.DecreaseBlueprintHeight:
                if (_halfHeight > minSize) {
                    _halfHeight -= 1;
                }
                ValidateBlueprint(map);
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
                if (IsDiggingRoom) {
                    DigRoomWithCorridor(RoomType.EarthElementalFont, map);
                } else {
                    DigCorridor(map);
                }
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
            ValidateBlueprint(map);
        }
    }

    private void DigRoomWithCorridor(RoomType roomType, Map map, bool digCorridor = true) {
        Vector2 roomTopLeft = _digCenter with { X = _digCenter.X - _halfWidth, Y = _digCenter.Y - _halfHeight };
        int width = _halfWidth * 2;
        int height = _halfHeight * 2;
        Vector2 roomBottomRight = roomTopLeft with { X = roomTopLeft.X + width, Y = roomTopLeft.Y + height };
        Room newRoom = Room.CreateRoom(roomType, roomTopLeft, roomBottomRight);
        map.AddRoom(newRoom);
        
        if (digCorridor) {
            List<Vector2> newRoomTiles = newRoom.GetTilePositions();
            List<Vector2> oldRoomTiles = _playerRoom.GetTilePositions();
            HashSet<Vector2> corridorTiles = Map.GetTileRegion(_corridorTopLeft, _corridorBottomRight);
            corridorTiles.ExceptWith(newRoomTiles);
            corridorTiles.ExceptWith(oldRoomTiles);
            
            bool isCorridorHorizontal = _digDirection is PlayerAction.DigLeft or PlayerAction.DigRight;
            Corridor corridor = new(corridorTiles.ToList(), _playerPosition, isCorridorHorizontal, IsDiggingRoom, map);
            
            newRoom.AddDoorwayForCorridor(corridor.GetFloorTiles());
            _playerRoom.AddDoorwayForCorridor(corridor.GetFloorTiles());
            map.AddCorridor(corridor);
        }
    }

    public void DigRoom(RoomType roomType, Map map, Vector2 center, int halfWidth, int halfHeight ) {
        _digCenter = center;
        _halfWidth = halfWidth;
        _halfHeight = halfHeight;
        DigRoomWithCorridor(roomType, map, false);
    }

    private void DigCorridor(Map map) {
        List<Vector2> oldRoomTiles = _playerRoom.GetTilePositions();
        HashSet<Vector2> corridorTiles = Map.GetTileRegion(_corridorTopLeft, _corridorBottomRight);
        corridorTiles.ExceptWith(oldRoomTiles);
        
        bool isCorridorHorizontal = _digDirection is PlayerAction.DigLeft or PlayerAction.DigRight;
        Corridor corridor = new(corridorTiles.ToList(), _playerPosition, isCorridorHorizontal, IsDiggingRoom, map);
        
        _playerRoom.AddDoorwayForCorridor(corridor.GetFloorTiles());
        map.AddCorridor(corridor);
    }

    private void ValidateBlueprint(Map map) {
        if (IsDiggingRoom) {
            ValidateRoom(map);
        } else {
            ValidateCorridor(map);
        } 
    }

    private void ValidateRoom(Map map) {
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
        
        HashSet<Vector2> blueprintTiles = Map.GetTileRegion(roomTopLeft, roomBottomRight);
        blueprintTiles.UnionWith(Map.GetTileRegion(_corridorTopLeft, _corridorBottomRight));
        blueprintTiles.ExceptWith(_playerRoom.GetTilePositions());
        if (blueprintTiles.Any(tile => !TerrainExtensions.DiggableTerrain.Contains(map.GetTileAt(tile).Terrain))) {
            _isDigValid = false;
            return;
        }
        
        _isDigValid = true;
    }

    private void ValidateCorridor(Map map) {
        HashSet<Vector2> blueprintTiles = Map.GetTileRegion(_corridorTopLeft, _corridorBottomRight);
        blueprintTiles.ExceptWith(_playerRoom.GetTilePositions());
        if (blueprintTiles.Any(tile => TerrainExtensions.WalkableTerrain.Contains(map.GetTileAt(tile).Terrain))) {
            _isDigValid = false;
            return;
        }

        List<int> roomsInBlueprint = blueprintTiles.Select(map.GetRoomIdForPosition).Distinct().SkipWhile(id => id == -1).ToList();
        if (roomsInBlueprint.Count > 1) {
            _isDigValid = false;
            return;
        }
        
        _isDigValid = true;
    }

    private bool CorridorIsLongEnough(Vector2 newRoomTopLeft, Vector2 newRoomBottomRight) {
        HashSet<Vector2> roomTiles = Map.GetTileRegion(newRoomTopLeft, newRoomBottomRight);
        HashSet<Vector2> corridorTiles = Map.GetTileRegion(_corridorTopLeft, _corridorBottomRight);

        corridorTiles.ExceptWith(roomTiles);
        return corridorTiles.Count >= 6;
    }
}