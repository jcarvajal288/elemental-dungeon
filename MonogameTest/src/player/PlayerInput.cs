using Microsoft.Xna.Framework.Input;

namespace MonogameTest.player;

public static class PlayerInput {

    private static KeyboardState _currentKeyState;
    private static KeyboardState _previousKeyState;

    private static void GetKeyState() {
        _previousKeyState = _currentKeyState;
        _currentKeyState = Keyboard.GetState();
    }

    public static bool IsPressed(Keys key) {
        return _currentKeyState.IsKeyDown(key);
    }

    private static bool JustPressed(Keys key) {
        return _currentKeyState.IsKeyDown(key) && !_previousKeyState.IsKeyDown(key);
    }

    public static PlayerAction ReadKeyboard(GameState gameState) {
        GetKeyState();
        if (JustPressed(Keys.Escape)) {
            return PlayerAction.Exit;
        }

        if (gameState == GameState.Moving) {
            return ReadMovementActions();
        }

        if (gameState == GameState.InDigDialog) {
            return ReadDigDialogActions();
        }

        return ReadDiggingActions();
    }

    private static PlayerAction ReadMovementActions() {
        if (JustPressed(Keys.Left)) {
            return IsPressed(Keys.D) ? PlayerAction.DigLeft : PlayerAction.MoveLeft;
        } 
        if (JustPressed(Keys.Right)) {
            return IsPressed(Keys.D) ? PlayerAction.DigRight : PlayerAction.MoveRight;
        } 
        if (JustPressed(Keys.Up)) {
            return IsPressed(Keys.D) ? PlayerAction.DigUp : PlayerAction.MoveUp;
        } 
        if (JustPressed(Keys.Down)) {
            return IsPressed(Keys.D) ? PlayerAction.DigDown : PlayerAction.MoveDown;
        }

        return PlayerAction.NoAction;
    }
    
    private static PlayerAction ReadDiggingActions() {
        if (JustPressed(Keys.Left)) {
            return IsPressed(Keys.LeftShift) ? PlayerAction.DecreaseBlueprintWidth : PlayerAction.MoveLeft;
        } 
        if (JustPressed(Keys.Right)) {
            return IsPressed(Keys.LeftShift) ? PlayerAction.IncreaseBlueprintWidth : PlayerAction.MoveRight;
        } 
        if (JustPressed(Keys.Up)) {
            return IsPressed(Keys.LeftShift) ? PlayerAction.IncreaseBlueprintHeight : PlayerAction.MoveUp;
        } 
        if (JustPressed(Keys.Down)) {
            return IsPressed(Keys.LeftShift) ? PlayerAction.DecreaseBlueprintHeight : PlayerAction.MoveDown;
        }
        if (JustPressed(Keys.Enter)) {
            return PlayerAction.SubmitRoomBlueprint;
        }
        
        return PlayerAction.NoAction;
    }

    private static PlayerAction ReadDigDialogActions() {
        if (JustPressed(Keys.Enter)) {
            return PlayerAction.SubmitDigDialog;
        }
        if (JustPressed(Keys.Up)) {
            return PlayerAction.MoveUp;
        } 
        if (JustPressed(Keys.Down)) {
            return PlayerAction.MoveDown;
        }
        return PlayerAction.NoAction;
    }

}