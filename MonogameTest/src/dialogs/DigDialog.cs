using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using MonogameTest.map;

namespace MonogameTest.dialogs;

public class DigDialog {

    private bool _isRoomSelected = true;
    
    public void Draw(SpriteBatch spriteBatch, BitmapFont font, Vector2 cameraTopLeft) {
        spriteBatch.DrawString(font, "What will you dig?", cameraTopLeft, Color.White);
        if (_isRoomSelected) { 
            spriteBatch.DrawString(font, ">Room", cameraTopLeft with { Y = cameraTopLeft.Y + font.Size }, Color.Yellow);
            spriteBatch.DrawString(font, "  Corridor", cameraTopLeft with { Y = cameraTopLeft.Y + font.Size * 2 }, Color.White);
        } else {
            spriteBatch.DrawString(font, "  Room", cameraTopLeft with { Y = cameraTopLeft.Y + font.Size }, Color.White);
            spriteBatch.DrawString(font, ">Corridor", cameraTopLeft with { Y = cameraTopLeft.Y + font.Size * 2 }, Color.Yellow);
        }
    }

    public GameState HandleInput(PlayerAction playerAction, RoomDigger roomDigger) {
        switch (playerAction) {
            case PlayerAction.SubmitDigDialog:
                roomDigger.IsDiggingRoom = _isRoomSelected;
                return GameState.Digging;
            case PlayerAction.MoveUp:
            case PlayerAction.MoveDown:
                _isRoomSelected = !_isRoomSelected;
                return GameState.InDigDialog;
            default:
                return GameState.InDigDialog;
        }
    }
}