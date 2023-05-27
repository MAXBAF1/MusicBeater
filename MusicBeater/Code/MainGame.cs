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
    public readonly Views Views;
    public readonly GameController Controller;
    
    public MainGame()
    {
        var graphics = new GraphicsDeviceManager(this);
        var menuDelegates = new BtnDelegates((_, _) => StartButtonClick(), (_, _) => Exit(), (_, _) => ExitToMenu());
        Views = new Views(Content, graphics, Window, menuDelegates);
        Model = new Model.Model();
        Controller = new GameController();

        Model.WindowState = WindowState.Menu;

        IsMouseVisible = true;
    }

    protected override void LoadContent()
    {
        Views.LoadContent();
        base.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        Controller.ListenUser(Click, PauseGame);

        if (Model.WindowState == WindowState.Menu)
            Model.UpdateMenu(gameTime, Views.MenuComponents);
        if (Model.WindowState == WindowState.Game)
            Model.UpdateGame(gameTime);
        if (Model.WindowState == WindowState.Scores)
            Model.UpdateScores(gameTime, Views.ScoresComponents);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        switch (Model.WindowState)
        {
            case WindowState.Menu:
                Views.DrawMenu(gameTime);
                break;
            case WindowState.Game:
                Views.DrawGame(Model.BColorLerp, Model.PressDelta, Model.IsIdealTime, Model.ScoresData);
                break;
            case WindowState.Scores:
                Views.DrawScores(gameTime, Model.ScoresData);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        base.Draw(gameTime);
    }

    private string _path = @"D:\Microsoft Visual Studio\repos\MusicBeater\output.txt";

    public void StartButtonClick()
    {
        Model.CurTiming = long.Parse(Model.IdealTimings[Model.CurTimingNumber]);

        MediaPlayer.Play(Views.Song);
        Model.SongWatch.Start();
        Model.WindowState = WindowState.Game;
    }

    public void Click()
    {
        if (!Model.SongWatch.IsRunning) return;

        File.AppendAllText(_path, $"{Model.SongWatch.ElapsedMilliseconds}\n");
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