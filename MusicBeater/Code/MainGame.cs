using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using MusicBeater.Code.Controller;
using MusicBeater.Code.View;
using BackgroundType = MusicBeater.Code.View.BackgroundType;

namespace MusicBeater.Code;

public class MainGame : Game
{
    public Model.Model Model;
    private readonly Views _views;
    private readonly GameController _controller;

    public MainGame()
    {
        var graphics = new GraphicsDeviceManager(this);
        var menuDelegates = new BtnDelegates((_, _) => StartButtonClick(), (_, _) => Exit(), (_, _) => ExitToMenu());
        _views = new Views(Content, graphics, Window, menuDelegates);
        Model = new Model.Model();
        _controller = new GameController();

        Model.WindowState = WindowState.Menu;

        IsMouseVisible = true;
    }

    protected override void LoadContent()
    {
        _views.LoadContent();
        base.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        _controller.ListenUser(Click, PauseGame);

        if (Model.WindowState == WindowState.Menu)
            Model.UpdateMenu(gameTime, _views.MenuComponents);
        if (Model.WindowState == WindowState.Game)
            Model.UpdateGame(gameTime);
        if (Model.WindowState == WindowState.Scores)
            Model.UpdateScores(gameTime, _views.ScoresComponents);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        switch (Model.WindowState)
        {
            case WindowState.Menu:
                _views.DrawMenu(gameTime);
                break;
            case WindowState.Game:
                _views.DrawGame(Model.BColorLerp, Model.PressDelta, Model.IsIdealTime, Model.ScoresData);
                break;
            case WindowState.Scores:
                _views.DrawScores(gameTime, Model.ScoresData);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        base.Draw(gameTime);
    }


    public void StartButtonClick()
    {
        Model.CurTiming = long.Parse(Model.IdealTimings[Model.CurTimingNumber]);

        MediaPlayer.Play(_views.Song);
        Model.SongWatch.Start();
        Model.WindowState = WindowState.Game;
    }

    public void Click()
    {
        if (!Model.SongWatch.IsRunning) return;

        Model.PressDelta = Model.SongWatch.ElapsedMilliseconds - Model.CurTiming;
        if (Math.Abs(Model.PressDelta) <= Code.Model.Model.Tolerance)
        {
            Model.CurrColor = Color.Lime;
            Model.ScoresData.NiceCLickCnt++;
        }
        else
        {
            Model.ScoresData.BadCLickCnt++;
            Model.CurrColor = Color.Red;
        }

        Model.IsPressed = true;
        Model.BackgroundType = BackgroundType.FadeIn;
    }

    private void PauseGame()
    {
        Model = new Model.Model();

        MediaPlayer.Stop();
        Model.WindowState = WindowState.Menu;
    }

    private void ExitToMenu()
    {
        Model.WindowState = WindowState.Menu;
    }
}