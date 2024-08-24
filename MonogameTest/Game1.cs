using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonogameTest.map;

namespace MonogameTest;

public class Game1 : Game {
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

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
        // TODO: Add your initialization logic here

        base.Initialize();
    }

    protected override void LoadContent() {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        Images.LoadImages(GraphicsDevice);
        _map = new Map(100, 100);
        _player = new Player(new Vector2(20, 10));
        _map.DigRoom(18, 8, 5, 5);
    }
    
    protected override void Update(GameTime gameTime) {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        PlayerAction playerAction = PlayerInput.ReadKeyboard();
        if (_gameState == GameState.Moving) {
            _player.SendAction(playerAction, _map);
            _gameState = RoomDigger.CheckForNewDig(_gameState, playerAction, _player.Position);
        } else {
            RoomDigger.AdjustBlueprint(playerAction);
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime) {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        _spriteBatch.Begin();
        
        _map.Draw(_spriteBatch);
        _player.Draw(_spriteBatch);
        if (_gameState == GameState.Digging) {
            RoomDigger.DrawRoomBlueprint(_spriteBatch);
        }
        
        _spriteBatch.End();
        
        base.Draw(gameTime);
    }
}