using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MusicBeater.Code.Controller;
using MusicBeater.Code.View;
using MusicBeater.Code.View.Controls;
using MusicBeater.Misc;

namespace MusicBeater.Code.Model;

public class Logic
{
    public Texture2D CurrentBackground;

    public readonly Stopwatch SongWatch;
    public Color CurrColor = Color.White;
    public float ColorAmount;
    public BackgroundType BackgroundType = BackgroundType.Default;
    public Vector2 CirclePos = new(1450, 905);


    public Logic()
    {
        SongWatch = new Stopwatch();
    }

    private const float Speed = 17f;

    public void UpdateGame(GameTime gameTime, Action pauseGame, Action tap)
    {
        var keyboardState = MyKeyboard.GetState();

        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            keyboardState.IsKeyDown(Keys.Escape))
            pauseGame();

        CirclePos.X -= Speed;
        if (Math.Abs(CirclePos.X - 850) < 1)
            Debug.WriteLine($"{SongWatch.ElapsedMilliseconds}");
        
        if (CirclePos.X < 700)
            CirclePos.X = 1300;
        
        tap();
    }

    public void UpdateMenu(GameTime gameTime, List<Component> menuComponents)
    {
        foreach (var component in menuComponents)
            component.Update(gameTime);
    }
}