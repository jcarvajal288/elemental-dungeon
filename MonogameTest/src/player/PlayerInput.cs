using System;
using Microsoft.Xna.Framework.Input;

namespace MonogameTest;

public static class PlayerInput {

    private static KeyboardState _currentKeyState;
    private static KeyboardState _previousKeyState;

    private static KeyboardState GetState() {
        _previousKeyState = _currentKeyState;
        _currentKeyState = Keyboard.GetState();
        return _currentKeyState;
    }

    private static bool IsPressed(Keys key) {
        return _currentKeyState.IsKeyDown(key);
    }

    private static bool JustPressed(Keys key) {
        return _currentKeyState.IsKeyDown(key) && !_previousKeyState.IsKeyDown(key);
    }

    public static PlayerAction ReadKeyboard() {
        GetState();
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
}