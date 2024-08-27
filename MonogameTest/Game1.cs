using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;
using MonogameTest.map;
using MonogameTest.player;

namespace MonogameTest;

public class Game1 : Game {
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private OrthographicCamera _camera;

    private GameState _gameState;
    private Map _map;
    private Player _player;

    public Game1() {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.PreferredBackBufferWidth = 1280;
        _graphics.PreferredBackBufferHeight = 720;
        _gameState = GameState.Moving;
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize() {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        BoxingViewportAdapter viewportAdapter = new BoxingViewportAdapter(Window, GraphicsDevice, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
        _camera = new OrthographicCamera(viewportAdapter);
        base.Initialize();
    }

    protected override void LoadContent() {
        Images.LoadImages(GraphicsDevice);
        _map = new Map(99, 99);
        _player = new Player(new Vector2(50, 50));
        RoomDigger.DigRoom(RoomType.StartingRoom, _map, new Vector2(50, 50), 3, 3);
        CenterCameraOn(_player.Position);
    }
    
    protected override void Update(GameTime gameTime) {
        PlayerAction playerAction = PlayerInput.ReadKeyboard(_gameState);
        if (playerAction == PlayerAction.Exit) {
            if (_gameState == GameState.Moving) {
                Exit();
            } else {
                _gameState = GameState.Moving;
            }
        } 
        
        if (_gameState == GameState.Moving) {
            _player.SendAction(playerAction, _map);
            CenterCameraOn(_player.Position);
            int currentRoomId = _map.GetRoomIdForPosition(_player.Position);
            if (currentRoomId >= 0) {
                _gameState = RoomDigger.CheckForNewDig(_gameState, playerAction, _player.Position, _map, currentRoomId);
            }
        } else {
            _gameState = RoomDigger.AdjustBlueprint(playerAction, _map);
        }

        base.Update(gameTime);
    }

    private void CenterCameraOn(Vector2 newPosition) {
        Vector2 newFocus = new(newPosition.X * Tile.Size, newPosition.Y * Tile.Size);
        _camera.LookAt(newFocus);
    }

    protected override void Draw(GameTime gameTime) {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        _spriteBatch.Begin(transformMatrix: _camera.GetViewMatrix());
        
        _map.Draw(_spriteBatch);
        _player.Draw(_spriteBatch);
        if (_gameState == GameState.Digging) {
            RoomDigger.DrawRoomBlueprint(_spriteBatch);
        }
        
        _spriteBatch.End();
        
        base.Draw(gameTime);
    }
}