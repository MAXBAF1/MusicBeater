using Microsoft.Xna.Framework;
using MusicBeater;
using MusicBeater.Code;

namespace MusicBeaterTests;

public class Tests
{

    [Test]
    public void RunGameIsWindowStateMenu()
    {
        var game = new MainGame();
        game.Run();
        game.Exit();
        
        Assert.That(game.Model.WindowState, Is.EqualTo(WindowState.Menu));
    }

    [Test]
    public void RunGameBackgroundCurrColorIsWhite()
    {
        var game = new MainGame();
        game.Run();
        game.Exit();
        
        Assert.That(game.Model.CurrColor, Is.EqualTo(Color.White));
    }

    [Test]
    public void StartGameIsWindowStateGame()
    {
        var game = new MainGame();
        game.Run();
        game.StartButtonClick();
        game.Exit();
        
        Assert.That(game.Model.WindowState, Is.EqualTo(WindowState.Game));
    }

    [Test]
    public void StartGameSongWatchIsRunning()
    {
        var game = new MainGame();
        game.Run();
        game.StartButtonClick();
        game.Exit();
        
        Assert.That(game.Model.SongWatch.IsRunning, Is.True);
    }

    [Test]
    public void StartGameBadClickCurrColorIsRed()
    {
        var game = new MainGame();
        game.Run();
        game.StartButtonClick();
        game.Click();
        game.Exit();
        
        Assert.That(game.Model.CurrColor, Is.EqualTo(Color.Red));
    }

    [Test]
    public void StartGameNiceClickCurrColorIsLime()
    {
        var game = new MainGame();
        game.Run();
        game.StartButtonClick();
        Thread.Sleep((int)game.Model.CurTiming);
        game.Click();
        game.Exit();
        
        Assert.That(game.Model.CurrColor, Is.EqualTo(Color.Lime));
    }
    
    //     [Test]
//     public void StartGameNiceClickAllGame()
//     {
//         var game = new MainGame();
//         game.Run();
//         game.StartButtonClick();
//         var preTiming = 0;
//         while (game.Model.SongWatch.IsRunning)
//         {
//             Thread.Sleep((int)game.Model.CurTiming - preTiming);
//             game.Click();
//             preTiming += (int)game.Model.CurTiming;
//         }
//         
//         Assert.That(game.Model.ScoresData.GetPercentage(), Is.EqualTo(100));
//     }
}
