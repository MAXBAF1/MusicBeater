namespace MusicBeater.Misc;

using Microsoft.Xna.Framework.Input;

public class MyKeyboard
{
    static KeyboardState currentKeyState;
    static KeyboardState previousKeyState;

    public static KeyboardState GetState()
    {
        previousKeyState = currentKeyState;
        currentKeyState = Keyboard.GetState();
        return currentKeyState;
    }
    
    public static bool IsKeyPressed(Keys key, bool oneShot)
    {
        if (oneShot) return currentKeyState.IsKeyDown(key) && !previousKeyState.IsKeyDown(key);
        return currentKeyState.IsKeyDown(key);
    }
}