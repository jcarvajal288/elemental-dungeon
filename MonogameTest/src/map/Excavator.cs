using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonogameTest.player;

namespace MonogameTest.map;

public class Excavator(Map map, Player player, bool isDiggingRoom) {

    private Vector2 _digCenter;
    private PlayerAction _digDirection;
    private readonly Room _playerRoom = map.GetRoomForId(map.GetRoomIdForPosition(player.Position));
    private Vector2 _playerPosition = player.Position;
    private int _halfWidth = 3;
    private int _halfHeight = 3;
    private const int CorridorHalfWidth = 2;

    private Vector2 _corridorTopLeft;
    private Vector2 _corridorBottomRight;
    
    private bool _isDigValid;

    public Vector2 DigCenter => _digCenter;

    public void StartDigLeft() {
        Vector2 digOrigin = _playerPosition;
        const int distanceFromPlayer = 6;
        
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
        ValidateBlueprint();
    }

    public void StartDigRight() {
        Vector2 digOrigin = _playerPosition;
        const int distanceFromPlayer = 6;
        
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
        ValidateBlueprint();
    }

    public void StartDigUp() {
        Vector2 digOrigin = _playerPosition;
        const int distanceFromPlayer = 6;
        
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
        ValidateBlueprint();
    }

    public void StartDigDown() {
        Vector2 digOrigin = _playerPosition;
        const int distanceFromPlayer = 6;
        
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
        ValidateBlueprint();
    }

    public void DrawBlueprint(SpriteBatch spriteBatch) {
        Texture2D cursorSprite = _isDigValid
            ? Images.UiSpriteSet[UISprites.CursorGreen]
            : Images.UiSpriteSet[UISprites.CursorRed];
        Vector2 topLeft = _digCenter with { X = _digCenter.X - _halfWidth, Y = _digCenter.Y - _halfHeight };
        int width = _halfWidth * 2 + 1;
        int height = _halfHeight * 2 + 1;
        if (isDiggingRoom) {
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

    public GameState AdjustBlueprint(PlayerAction playerAction) {
        const int minSize = 3;
        const int maxSize = 6;
        switch (playerAction) {
            case PlayerAction.DecreaseBlueprintWidth:
                if (_halfWidth > minSize) {
                    _halfWidth -= 1;
                }
                ValidateBlueprint();
                return GameState.Digging;
            case PlayerAction.IncreaseBlueprintWidth:
                if (_halfWidth < maxSize) {
                    _halfWidth += 1;
                }
                ValidateBlueprint();
                return GameState.Digging;
            case PlayerAction.IncreaseBlueprintHeight:
                if (_halfHeight < maxSize) {
                    _halfHeight += 1;
                }
                ValidateBlueprint();
                return GameState.Digging;
            case PlayerAction.DecreaseBlueprintHeight:
                if (_halfHeight > minSize) {
                    _halfHeight -= 1;
                }
                ValidateBlueprint();
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
                if (isDiggingRoom) {
                    DigRoomWithCorridor(RoomType.EarthElementalFont);
                } else {
                    DigCorridor();
                }
                return GameState.Moving;
            default:
                return GameState.Digging;
        }

        void MoveCenterAndValidate(Vector2 newCenterPosition) {
            if (newCenterPosition.X < 0 || 
                newCenterPosition.Y < 0 || 
                newCenterPosition.X >= map.GetWidth() ||
                newCenterPosition.Y >= map.GetHeight()) {
                return;
            }
            
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
            ValidateBlueprint();
        }
    }

    private void DigRoomWithCorridor(RoomType roomType) {
        Vector2 roomTopLeft = _digCenter with { X = _digCenter.X - _halfWidth, Y = _digCenter.Y - _halfHeight };
        int width = _halfWidth * 2;
        int height = _halfHeight * 2;
        Vector2 roomBottomRight = roomTopLeft with { X = roomTopLeft.X + width, Y = roomTopLeft.Y + height };
        Room newRoom = Room.CreateRoom(roomType, roomTopLeft, roomBottomRight);
        map.AddRoom(newRoom);
        
        List<Vector2> newRoomTiles = newRoom.GetTilePositions();
        List<Vector2> oldRoomTiles = _playerRoom.GetTilePositions();
        HashSet<Vector2> corridorTiles = Map.GetTileRegion(_corridorTopLeft, _corridorBottomRight);
        corridorTiles.ExceptWith(newRoomTiles);
        corridorTiles.ExceptWith(oldRoomTiles);
        
        bool isCorridorHorizontal = _digDirection is PlayerAction.DigLeft or PlayerAction.DigRight;
        Corridor corridor = new(corridorTiles.ToList(), _playerPosition, isCorridorHorizontal, isDiggingRoom, map);
        
        newRoom.AddDoorwayForCorridor(corridor.GetFloorTiles());
        _playerRoom.AddDoorwayForCorridor(corridor.GetFloorTiles());
        map.AddCorridor(corridor);
    }

    private void DigCorridor() {
        List<Vector2> oldRoomTiles = _playerRoom.GetTilePositions();
        HashSet<Vector2> corridorTiles = Map.GetTileRegion(_corridorTopLeft, _corridorBottomRight);
        corridorTiles.ExceptWith(oldRoomTiles);
        
        bool isCorridorHorizontal = _digDirection is PlayerAction.DigLeft or PlayerAction.DigRight;
        Corridor corridor = new(corridorTiles.ToList(), _playerPosition, isCorridorHorizontal, isDiggingRoom, map);
        
        _playerRoom.AddDoorwayForCorridor(corridor.GetFloorTiles());
        map.AddCorridor(corridor);
    }

    private void ValidateBlueprint() {
        if (isDiggingRoom) {
            ValidateRoom();
        } else {
            ValidateCorridor();
        } 
    }

    private void ValidateRoom() {
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

        if (roomTopLeft.X < 1 || roomTopLeft.Y < 1 || roomBottomRight.X >= map.GetWidth()-1 || roomBottomRight.Y >= map.GetHeight()-1) {
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

    private void ValidateCorridor() {
        HashSet<Vector2> blueprintTiles = Map.GetTileRegion(_corridorTopLeft, _corridorBottomRight);
        blueprintTiles.ExceptWith(_playerRoom.GetTilePositions());
        if (blueprintTiles.Any(tile => TerrainExtensions.WalkableTerrain.Contains(map.GetTileAt(tile).Terrain))) {
            _isDigValid = false; // overlapping room floors
            return;
        }

        List<int> roomsInBlueprint = blueprintTiles.Select(map.GetRoomIdForPosition).Distinct().Where(id => id == -1).ToList();
        if (roomsInBlueprint.Count > 1) {
            roomsInBlueprint.ForEach(id => Console.Out.WriteLine($"{id}"));
            _isDigValid = false; // trying to connect to multiple rooms
            return;
        }

        if (blueprintTiles.Count <= 5) {
            _isDigValid = false; // corridor too short
            return;
        }

        if (blueprintTiles.All(tile => TerrainExtensions.DiggableTerrain.Contains(map.GetTileAt(tile).Terrain))) {
            _isDigValid = false; // must connect to a room
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