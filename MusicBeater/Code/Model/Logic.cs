using System;
using System.Diagnostics;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MusicBeater.Code.Controller;
using MusicBeater.Code.View;
using MusicBeater.Misc;

namespace MusicBeater.Code.Model;

public class Logic
{
    private readonly GameController _game;
    private readonly Stopwatch _songWatch;

    public Logic(GameController game)
    {
        _game = game;
        _songWatch = new Stopwatch();
    }

    public void Update(GameTime gameTime)
    {
        var keyboardState = MyKeyboard.GetState();

        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.Escape))
        {
            _songWatch.Stop();
            MediaPlayer.Stop();
            _game.WindowState = WindowState.Menu;
            _game.Views.CurrentBackground = _game.Views.MenuBackground;
        }

        RunGame(gameTime);
        
        foreach (var component in _game.Views.GameComponents)
            component.Update(gameTime);
    }
    
    public void StartButton_Click()
    {
        MediaPlayer.Play(_game.Views.Song);
        _songWatch.Start();
        _game.Views.CurrentBackground = _game.Views.GameBackground;
        _game.WindowState = WindowState.Game;
    }
    
    private void RunGame(GameTime gameTime)
    {
        const int bpmSec = 60000 / 124;
        const double offset = 50;
        const double lag = 200;

        _game.Views.AnimateBackground();

        if (MyKeyboard.IsKeyPressed(Keys.Space, true) && _songWatch.IsRunning)
        {
            var time = (_songWatch.ElapsedMilliseconds - lag) % bpmSec;
            Debug.WriteLine($"{time} {bpmSec - offset}");
            
            _game.Views.OldColor = _game.Views.CurrColor;
            _game.Views.CurrColor = time is < offset or > bpmSec - offset ? Color.Lime : Color.Red;
            _game.Views.BackgroundType = BackgroundType.FadeIn;
        }
    }
}