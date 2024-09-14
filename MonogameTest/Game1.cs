using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.ViewportAdapters;
using MonogameTest.map;
using MonogameTest.player;

namespace MonogameTest;

public class Game1 : Game {
    private readonly GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private OrthographicCamera _camera;

    private GameState _gameState;
    private Map _map;
    private Player _player;
    private BitmapFont _font;
    private Excavator _excavator;

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
        BoxingViewportAdapter viewportAdapter = new(Window, GraphicsDevice, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
        _camera = new OrthographicCamera(viewportAdapter);
        base.Initialize();
    }

    protected override void LoadContent() {
        Images.LoadImages(GraphicsDevice);
        _map = new Map(99, 99);
        _player = new Player(new Vector2(50, 50));
        _font = BitmapFont.FromFile(GraphicsDevice, "Content/assets/fonts/00/00.fnt");

        Vector2 startingRoomCenter = new(50, 50);
        Vector2 roomTopLeft = startingRoomCenter with { X = startingRoomCenter.X - 3, Y = startingRoomCenter.Y - 3 };
        Vector2 roomBottomRight = startingRoomCenter with { X = startingRoomCenter.X + 3, Y = startingRoomCenter.Y + 3 };
        _map.AddRoom(Room.CreateRoom(RoomType.StartingRoom, roomTopLeft, roomBottomRight));
        
        CenterCameraOn(_player.Position);
    }
    
    protected override void Update(GameTime gameTime) {
        _gameState = InputHandler.HandleInput(_gameState, _player, _map, ref _excavator);

        switch (_gameState) {
            case GameState.Exit:
                Exit();
                break;
            case GameState.Moving:
                CenterCameraOn(_player.Position);
                break;
            case GameState.Digging:
                CenterCameraOn(_excavator.DigCenter);
                break;
        }

        base.Update(gameTime);
    }


    private void CenterCameraOn(Vector2 newPosition) {
        Vector2 newFocus = new((newPosition.X + 1) * Tile.Size, (newPosition.Y + 1) * Tile.Size);
        _camera.LookAt(newFocus);
    }

    protected override void Draw(GameTime gameTime) {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        _spriteBatch.Begin(transformMatrix: _camera.GetViewMatrix());
        
        _map.Draw(_spriteBatch);
        _player.Draw(_spriteBatch);
        
        if (_gameState == GameState.Digging) {
            _excavator.Draw(_spriteBatch, _camera.Position, _font);
        }

        _spriteBatch.End();
        
        base.Draw(gameTime);
    }
}