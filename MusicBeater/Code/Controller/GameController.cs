using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MusicBeater.Misc;

namespace MusicBeater.Code.Controller;

public class GameController
{
    private readonly MyKeyboard _keyboard = new();
    private readonly MyMouse _mouse = new();
    public void ListenUser(Action tap, Action pauseGame)
    {
        _mouse.GetState();
        if (_keyboard.IsKeyPressed(Keys.Space, true) || _mouse.IsKeyPressed(true)) 
            tap();

        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            _keyboard.GetState().IsKeyDown(Keys.Escape))
            pauseGame();
    }
}