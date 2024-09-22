using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using MonogameTest.dialogs;
using MonogameTest.player;

namespace MonogameTest.map;

public class Excavator() {

    private enum State {
        SelectingRoomOrCorridor,
        AdjustingBlueprint,
        SelectingRoomType,
    }

    private State _state = State.SelectingRoomOrCorridor;
    private Map _map;

    private Vector2 _digCenter;
    private PlayerAction _digDirection;
    private readonly Room _playerRoom;
    private Vector2 _playerPosition;
    private int _halfWidth = 3;
    private int _halfHeight = 3;
    private const int CorridorHalfWidth = 2;

    private Vector2 _corridorTopLeft;
    private Vector2 _corridorBottomRight;
    
    private bool _isDigValid;
    private bool _isDiggingRoom;
    
    private readonly RoomOrCorridorDialog _roomOrCorridorDialog;
    private readonly RoomSelectionDialog _roomSelectionDialog;

    public Vector2 DigCenter => _digCenter;

    public Excavator(Map map, Player player, PlayerAction digDirection) : this() {
        _digDirection = digDirection;
        _playerRoom = map.GetRoomForId(map.GetRoomIdForPosition(player.Position));
        _playerPosition = player.Position;
        _roomOrCorridorDialog = new RoomOrCorridorDialog();
        _roomSelectionDialog = new RoomSelectionDialog();
        _map = map;
        switch (_digDirection) {
            case PlayerAction.DigLeft:
                StartDigLeft();
                break;
            case PlayerAction.DigRight:
                StartDigRight();
                break;
            case PlayerAction.DigUp:
                StartDigUp();
                break;
            case PlayerAction.DigDown:
                StartDigDown();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void StartDigLeft() {
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
    }

    private void StartDigRight() {
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
    }

    private void StartDigUp() {
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
    }

    private void StartDigDown() {
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
    }
    
    public GameState HandleInput(PlayerAction playerAction) {
        if (_state == State.SelectingRoomOrCorridor && _roomOrCorridorDialog.HasSelectedRoomOrCorridor(playerAction)) {
            _isDiggingRoom = _roomOrCorridorDialog.IsRoomSelected;
            _state = State.AdjustingBlueprint;
            ValidateBlueprint();
        } else if (_state == State.AdjustingBlueprint) {
            _state = AdjustBlueprint(playerAction);
        } else if (_state == State.SelectingRoomType && (!_isDiggingRoom || _roomSelectionDialog.HasSelectedRoomType(playerAction))) {
            DoDig();
            return GameState.Moving;
        }

        return GameState.Digging;
    }

    private void DoDig() {
        if (_isDiggingRoom) {
            DigRoomWithCorridor(_roomSelectionDialog.SelectedRoomType);
        } else {
            DigCorridor();
        }
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 cameraTopLeft, BitmapFont font) {
        switch (_state) {
            case State.SelectingRoomOrCorridor:
                _roomOrCorridorDialog.Draw(spriteBatch, font, cameraTopLeft);
                break;
            case State.AdjustingBlueprint:
                DrawBlueprint(spriteBatch);
                break;
            case State.SelectingRoomType:
                _roomSelectionDialog.Draw(spriteBatch, font, cameraTopLeft);
                break;
        }
    }

    private void DrawBlueprint(SpriteBatch spriteBatch) {
        Texture2D cursorSprite = _isDigValid
            ? Images.UiSpriteSet[UISprites.CursorGreen]
            : Images.UiSpriteSet[UISprites.CursorRed];
        Vector2 topLeft = _digCenter with { X = _digCenter.X - _halfWidth, Y = _digCenter.Y - _halfHeight };
        int width = _halfWidth * 2 + 1;
        int height = _halfHeight * 2 + 1;
        if (_isDiggingRoom) {
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

    private State AdjustBlueprint(PlayerAction playerAction) {
        const int minSize = 3;
        const int maxSize = 6;
        switch (playerAction) {
            case PlayerAction.DecreaseBlueprintWidth:
                if (_halfWidth > minSize) {
                    _halfWidth -= 1;
                }
                ValidateBlueprint();
                return State.AdjustingBlueprint;
            case PlayerAction.IncreaseBlueprintWidth:
                if (_halfWidth < maxSize) {
                    _halfWidth += 1;
                }
                ValidateBlueprint();
                return State.AdjustingBlueprint;
            case PlayerAction.IncreaseBlueprintHeight:
                if (_halfHeight < maxSize) {
                    _halfHeight += 1;
                }
                ValidateBlueprint();
                return State.AdjustingBlueprint;
            case PlayerAction.DecreaseBlueprintHeight:
                if (_halfHeight > minSize) {
                    _halfHeight -= 1;
                }
                ValidateBlueprint();
                return State.AdjustingBlueprint;
            case PlayerAction.MoveLeft:
                MoveCenterAndValidate(_digCenter with { X = _digCenter.X - 1 });
                return State.AdjustingBlueprint;
            case PlayerAction.MoveRight:
                MoveCenterAndValidate(_digCenter with { X = _digCenter.X + 1 });
                return State.AdjustingBlueprint;
            case PlayerAction.MoveUp:
                MoveCenterAndValidate(_digCenter with { Y = _digCenter.Y - 1 });
                return State.AdjustingBlueprint;
            case PlayerAction.MoveDown:
                MoveCenterAndValidate(_digCenter with { Y = _digCenter.Y + 1 });
                return State.AdjustingBlueprint;
            case PlayerAction.SubmitAction when _isDigValid:
                return State.SelectingRoomType;
            default:
                return State.AdjustingBlueprint;
        }

        void MoveCenterAndValidate(Vector2 newCenterPosition) {
            if (newCenterPosition.X < 0 || 
                newCenterPosition.Y < 0 || 
                newCenterPosition.X >= _map.GetWidth() ||
                newCenterPosition.Y >= _map.GetHeight()) {
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
        Room newRoom = Room.CreateRoom(roomType, roomTopLeft, roomBottomRight, _map.NextRoomId());
        _map.AddRoom(newRoom);
        
        List<Vector2> newRoomTiles = newRoom.GetTilePositions();
        List<Vector2> oldRoomTiles = _playerRoom.GetTilePositions();
        HashSet<Vector2> corridorTiles = Map.GetTileRegion(_corridorTopLeft, _corridorBottomRight);
        corridorTiles.ExceptWith(newRoomTiles);
        corridorTiles.ExceptWith(oldRoomTiles);
        
        bool isCorridorHorizontal = _digDirection is PlayerAction.DigLeft or PlayerAction.DigRight;
        Tuple<int, int> connectedRoomIds = new(_playerRoom.GetId(), newRoom.GetId());
        Corridor corridor = new(corridorTiles.ToList(), _playerPosition, isCorridorHorizontal, _isDiggingRoom, _map, connectedRoomIds);
        
        newRoom.AddDoorwayForCorridor(corridor.GetFloorTiles());
        _playerRoom.AddDoorwayForCorridor(corridor.GetFloorTiles());
        _map.AddCorridor(corridor);
    }

    private void DigCorridor() {
        List<Vector2> oldRoomTiles = _playerRoom.GetTilePositions();
        HashSet<Vector2> corridorTiles = Map.GetTileRegion(_corridorTopLeft, _corridorBottomRight);
        corridorTiles.ExceptWith(oldRoomTiles);

        int otherRoomId = corridorTiles.Select(_map.GetRoomIdForPosition).Distinct().First(id => id != -1);
        bool isCorridorHorizontal = _digDirection is PlayerAction.DigLeft or PlayerAction.DigRight;
        Tuple<int, int> connectedRoomIds = new(_playerRoom.GetId(), otherRoomId);
        Corridor corridor = new(corridorTiles.ToList(), _playerPosition, isCorridorHorizontal, _isDiggingRoom, _map, connectedRoomIds);
        
        _playerRoom.AddDoorwayForCorridor(corridor.GetFloorTiles());
        _map.AddCorridor(corridor);
    }

    private void ValidateBlueprint() {
        if (_isDiggingRoom) {
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

        if (roomTopLeft.X < 1 || roomTopLeft.Y < 1 || roomBottomRight.X >= _map.GetWidth()-1 || roomBottomRight.Y >= _map.GetHeight()-1) {
            _isDigValid = false;
            return;
        }
        
        HashSet<Vector2> blueprintTiles = Map.GetTileRegion(roomTopLeft, roomBottomRight);
        blueprintTiles.UnionWith(Map.GetTileRegion(_corridorTopLeft, _corridorBottomRight));
        blueprintTiles.ExceptWith(_playerRoom.GetTilePositions());
        if (blueprintTiles.Any(tile => !TerrainExtensions.DiggableTerrain.Contains(_map.GetTileAt(tile).Terrain))) {
            _isDigValid = false;
            return;
        }
        
        _isDigValid = true;
    }

    private void ValidateCorridor() {
        HashSet<Vector2> blueprintTiles = Map.GetTileRegion(_corridorTopLeft, _corridorBottomRight);
        blueprintTiles.ExceptWith(_playerRoom.GetTilePositions());
        if (blueprintTiles.Any(tile => TerrainExtensions.WalkableTerrain.Contains(_map.GetTileAt(tile).Terrain))) {
            _isDigValid = false; // overlapping room floors
            return;
        }

        List<int> roomsInBlueprint = blueprintTiles.Select(_map.GetRoomIdForPosition).Distinct().Where(id => id != -1).ToList();
        if (roomsInBlueprint.Count > 1) {
            roomsInBlueprint.ForEach(id => Console.Out.WriteLine($"{id}"));
            _isDigValid = false; // trying to connect to multiple rooms
            return;
        }

        if (blueprintTiles.Count <= 5) {
            _isDigValid = false; // corridor too short
            return;
        }

        if (blueprintTiles.All(tile => TerrainExtensions.DiggableTerrain.Contains(_map.GetTileAt(tile).Terrain))) {
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