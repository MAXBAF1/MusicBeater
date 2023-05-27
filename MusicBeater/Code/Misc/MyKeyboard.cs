namespace MusicBeater.Misc;

using Microsoft.Xna.Framework.Input;

public class MyKeyboard
{
    private static KeyboardState _currentKeyState;
    private static KeyboardState _previousKeyState;

    public KeyboardState GetState()
    {
        _previousKeyState = _currentKeyState;
        _currentKeyState = Keyboard.GetState();
        return _currentKeyState;
    }
    
    public bool IsKeyPressed(Keys key, bool oneShot)
    {
        if (oneShot) return _currentKeyState.IsKeyDown(key) && !_previousKeyState.IsKeyDown(key);
        return _currentKeyState.IsKeyDown(key);
    }
}