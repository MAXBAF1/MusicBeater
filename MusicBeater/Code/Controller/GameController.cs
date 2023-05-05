using System;
using Microsoft.Xna.Framework;
using MusicBeater.Code.Model;
using MusicBeater.Code.View;

namespace MusicBeater.Code.Controller;

public class GameController : Game
{
    public WindowState WindowState;
    public readonly Logic Logic;
    public readonly Views Views;

    public GameController()
    {
        Logic = new Logic(this);
        Views = new Views(this);

        WindowState = WindowState.Menu;
        
        IsMouseVisible = true;
    }

    protected override void LoadContent()
    {
        Views.LoadContent();
        
        base.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        Logic.Update(gameTime);
        
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        if (WindowState == WindowState.Menu)
            Views.DrawMenu(gameTime);
        if (WindowState == WindowState.Game)
            Views.DrawGame(gameTime);

        base.Draw(gameTime);
    }
}