using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Microsoft.Xna.Framework;
using MusicBeater.Code.View;
using MusicBeater.Code.View.Controls;

namespace MusicBeater.Code.Model;

public class Model
{
    public WindowState WindowState;
    
    public readonly Stopwatch SongWatch;
    public Color CurrColor = Color.White;
    private float _colorAmount;
    public BackgroundType BackgroundType = BackgroundType.Default;
    public Color BColorLerp { get; private set; }
    
    public readonly string[] IdealTimings = 
        File.ReadAllLines(@"D:\Microsoft Visual Studio\repos\MusicBeater\MusicBeater\Content\Audio\SevenNationArmyTimings.txt");
    public long CurTiming;
    public long CurTimingNumber;
    public bool IsPressed;
    public long PressDelta;
    public const int Tolerance = 100;
    public bool IsIdealTime;

    public readonly ScoresData ScoresData = new();

    public Model()
    {
        SongWatch = new Stopwatch();
    }
    
    public void UpdateGame(GameTime gameTime)
    {
        SetCurTiming();
        BColorLerp = Color.Lerp(Color.White, CurrColor, _colorAmount);
        IsIdealTime = Math.Abs(SongWatch.ElapsedMilliseconds - CurTiming) <= Tolerance;
        AnimateBackground();
    }

    public void UpdateMenu(GameTime gameTime, List<Component> menuComponents)
    {
        foreach (var component in menuComponents)
            component.Update(gameTime);
    }
    
    public void UpdateScores(GameTime gameTime, List<Component> scoresComponents)
    {
        foreach (var component in scoresComponents)
            component.Update(gameTime);
    }
    
    private void SetCurTiming()
    {
        if (SongWatch.ElapsedMilliseconds - CurTiming <= Tolerance) return;
        ScoresData.IdealNiceCLickCnt++;
        if (!IsPressed)
        {
            ScoresData.BadCLickCnt++;
            CurrColor = Color.Red;
            BackgroundType = BackgroundType.FadeIn;
        }

        IsPressed = false;
        CurTimingNumber++;
        if (CurTimingNumber < IdealTimings.Length)
            CurTiming = long.Parse(IdealTimings[CurTimingNumber]);
        else
        {
            SongWatch.Stop();
            WindowState = WindowState.Scores;
        }
    }

    private void AnimateBackground()
    {
        const float animationSpeed = 0.05f;
        switch (BackgroundType)
        {
            case BackgroundType.FadeIn:
                _colorAmount += animationSpeed;
                if (_colorAmount >= 1f) BackgroundType = BackgroundType.FadeOut;
                break;
            case BackgroundType.FadeOut:
                _colorAmount -= animationSpeed;
                if (_colorAmount <= 0f) BackgroundType = BackgroundType.Default;
                break;
            case BackgroundType.Default:
                CurrColor = Color.White;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}