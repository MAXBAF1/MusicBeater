using Microsoft.Xna.Framework.Input;

namespace MusicBeater.Misc;

public class MyMouse
{
    private static MouseState _currentKeyState;
    private static MouseState _previousKeyState;

    public MouseState GetState()
    {
        _previousKeyState = _currentKeyState;
        _currentKeyState = Mouse.GetState();
        return _currentKeyState;
    }
    
    public bool IsKeyPressed(bool oneShot)
    {
        if (oneShot) return _currentKeyState.LeftButton == ButtonState.Pressed && _previousKeyState.LeftButton != ButtonState.Pressed;
        return _currentKeyState.LeftButton == ButtonState.Pressed;
    }
}