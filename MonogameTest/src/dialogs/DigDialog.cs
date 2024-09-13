using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;

namespace MonogameTest.dialogs;

public class DigDialog(PlayerAction digDirection) {

    public readonly PlayerAction DigDirection = digDirection;
    public bool IsRoomSelected = true;
    
    public void Draw(SpriteBatch spriteBatch, BitmapFont font, Vector2 cameraTopLeft) {
        spriteBatch.DrawString(font, "What will you dig?", cameraTopLeft, Color.White);
        if (IsRoomSelected) { 
            spriteBatch.DrawString(font, ">Room", cameraTopLeft with { Y = cameraTopLeft.Y + font.Size }, Color.Yellow);
            spriteBatch.DrawString(font, "  Corridor", cameraTopLeft with { Y = cameraTopLeft.Y + font.Size * 2 }, Color.White);
        } else {
            spriteBatch.DrawString(font, "  Room", cameraTopLeft with { Y = cameraTopLeft.Y + font.Size }, Color.White);
            spriteBatch.DrawString(font, ">Corridor", cameraTopLeft with { Y = cameraTopLeft.Y + font.Size * 2 }, Color.Yellow);
        }
    }

    public GameState HandleInput(PlayerAction playerAction) {
        switch (playerAction) {
            case PlayerAction.SubmitDigDialog:
                return GameState.Digging;
            case PlayerAction.MoveUp:
            case PlayerAction.MoveDown:
                IsRoomSelected = !IsRoomSelected;
                return GameState.InDigDialog;
            default:
                return GameState.InDigDialog;
        }
    }

}