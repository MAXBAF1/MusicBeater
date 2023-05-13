using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MusicBeater.Code.Model;
using MusicBeater.Code.View;
using MusicBeater.Misc;
using SharpDX.MediaFoundation;

namespace MusicBeater.Code.Controller;

public class GameController : Game
{
    private WindowState _windowState;
    private readonly Logic _model;
    private readonly Views _views;

    public GameController()
    {
        var graphics = new GraphicsDeviceManager(this);
        var delegates = new MenuDelegates(StartButtonClick, (_,_) => Exit());
        _views = new Views(Content, graphics, Window, delegates);
        _model = new Logic();

        _windowState = WindowState.Menu;
        
        IsMouseVisible = true;
    }
    
    protected override void LoadContent()
    {
        _views.LoadContent();
        _model.CurrentBackground = _views.MenuBackground;
        
        base.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        if (_windowState == WindowState.Menu)
            _model.UpdateMenu(gameTime, _views.MenuComponents);
        if (_windowState == WindowState.Game)
            _model.UpdateGame(gameTime, PauseGame, Tap);
        
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        if (_windowState == WindowState.Menu)
            _views.DrawMenu(gameTime, _model.CurrentBackground);
        if (_windowState == WindowState.Game)
        {
            var bColor = Color.Lerp(Color.White, _model.CurrColor, _model.ColorAmount);
            _views.DrawGame(gameTime, _model.CurrentBackground, bColor, _model.CirclePos);
        }

        base.Draw(gameTime);
    }

    private const int BpmMSec = 60000 / 124;
    private const double Offset = 50;
    private const double Lag = 200;

    private void Tap()
    {
        AnimateBackground();
        
        if (MyKeyboard.IsKeyPressed(Keys.Space, true) && _model.SongWatch.IsRunning)
        {
            var pressTime = (_model.SongWatch.ElapsedMilliseconds - Lag) % BpmMSec;
            //Debug.WriteLine($"{pressTime} {BpmSec - Offset}");
            
            _model.CurrColor = pressTime is < Offset or > BpmMSec - Offset ? Color.Lime : Color.Red;
            _model.BackgroundType = BackgroundType.FadeIn;
        }
    }

    private void AnimateBackground()
    {
        switch (_model.BackgroundType)
        {
            case BackgroundType.FadeIn:
            {
                _model.ColorAmount += 0.05f;
                if (_model.ColorAmount >= 1f) _model.BackgroundType = BackgroundType.FadeOut;
                break;
            }
            case BackgroundType.FadeOut:
            {
                _model.ColorAmount -= 0.05f;
                if (_model.ColorAmount <= 0f) _model.BackgroundType = BackgroundType.Default;
                break;
            }
            case BackgroundType.Default:
            {
                _model.CurrColor = Color.White;
                break;
            }
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void StartButtonClick(object sender, EventArgs eventArgs)
    {
        MediaPlayer.Play(_views.Song);
        _model.SongWatch.Start();
        _model.CurrentBackground = _views.GameBackground;
        _windowState = WindowState.Game;
    }
    
    private void PauseGame()
    {
        _model.SongWatch.Stop();
        MediaPlayer.Stop();
        _windowState = WindowState.Menu;
        _model.CurrentBackground = _views.MenuBackground;
    }
}