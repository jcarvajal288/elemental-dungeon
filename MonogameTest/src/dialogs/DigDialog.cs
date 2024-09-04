using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;

namespace MonogameTest.dialogs;

public class DigDialog {
    
    public void Draw(SpriteBatch spriteBatch, BitmapFont font, Vector2 cameraTopLeft) {
        spriteBatch.DrawString(font, "What will you dig?", cameraTopLeft, Color.White);
        spriteBatch.DrawString(font, ">Room", cameraTopLeft with { Y = cameraTopLeft.Y + font.Size }, Color.White);
        spriteBatch.DrawString(font, " Corridor", cameraTopLeft with { Y = cameraTopLeft.Y + font.Size * 2 }, Color.White);
    }

    public static GameState HandleInput(PlayerAction playerAction) {
        if (playerAction == PlayerAction.SubmitDigDialog) {
            return GameState.Digging;
        }

        return GameState.InDigDialog;
    }
}