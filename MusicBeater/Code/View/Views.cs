using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using MonoGame.Extended;
using MusicBeater.Code.Controller;
using MusicBeater.Code.Model;
using MusicBeater.Code.View.Controls;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Size = System.Drawing.Size;

namespace MusicBeater.Code.View;

public class Views
{
    private readonly ContentManager _content;
    private readonly GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private readonly Size _windowSize;

    public List<Component> MenuComponents { get; private set; }
    public List<Component> ScoresComponents { get; private set; }

    public Song Song;

    private readonly BtnDelegates _btnDelegates;

    private readonly Rectangle _backgroundRect;
    private Texture2D _menuBackground;
    private Texture2D _gameBackground;
    private Texture2D _scoresBackground;
    private Texture2D _idealTapBack;
    private Texture2D _circle;

    public Views(ContentManager content, GraphicsDeviceManager graphics, GameWindow window, BtnDelegates btnDelegates)
    {
        _content = content;
        _graphics = graphics;
        _windowSize = new Size(1920, 1080);
        _btnDelegates = btnDelegates;
        _backgroundRect = new Rectangle(0, 0, _windowSize.Width, _windowSize.Height);

        _graphics.GraphicsProfile = GraphicsProfile.HiDef;
        _graphics.IsFullScreen = true;
        _graphics.PreferredBackBufferWidth = _windowSize.Width;
        _graphics.PreferredBackBufferHeight = _windowSize.Height;
        _graphics.ApplyChanges();
    }

    public void LoadContent()
    {
        _spriteBatch = new SpriteBatch(_graphics.GraphicsDevice);
        _content.RootDirectory = "Content";
        Song = _content.Load<Song>("Audio/SevenNationArmy");

        _gameBackground = _content.Load<Texture2D>("Backgrounds/normal");
        _menuBackground = _content.Load<Texture2D>("Backgrounds/menu");
        _scoresBackground = _content.Load<Texture2D>("Backgrounds/scores");

        LoadButtons();

        _idealTapBack = _content.Load<Texture2D>("Assets/scoresBackground");
        _circle = _content.Load<Texture2D>("Assets/circle");
    }

    private SpriteFont _interFont;
    private SpriteFont _bigInterFont;

    private void LoadButtons()
    {
        var position = new Vector2(1920 / 2 - 155, 1080 / 2 - 150);
        var texture = _content.Load<Texture2D>("Controls/Button");
        var hoverTexture = _content.Load<Texture2D>("Controls/HoverButton");
        _interFont = _content.Load<SpriteFont>("Fonts/Inter");
        _bigInterFont = _content.Load<SpriteFont>("Fonts/BigInter");

        var startButton = new Button(texture, hoverTexture, _interFont)
        {
            Position = position,
            Text = "Начать"
        };
        startButton.Click += _btnDelegates.StartButtonClick;

        var exitButton = new Button(texture, hoverTexture, _interFont)
        {
            Position = position with { Y = position.Y + 160 },
            Text = "Выход"
        };
        exitButton.Click += _btnDelegates.ExitButtonClick;

        MenuComponents = new List<Component> { startButton, exitButton };

        var exitToMenuButton = new Button(texture, hoverTexture, _interFont)
        {
            Position = position with { Y = _windowSize.Height - 360 },
            Text = "Выход"
        };
        exitToMenuButton.Click += _btnDelegates.ExitToMenuClick;

        ScoresComponents = new List<Component> { exitToMenuButton };
    }

    public void DrawMenu(GameTime gameTime)
    {
        _spriteBatch.Begin();
        _spriteBatch.Draw(_menuBackground, _backgroundRect, Color.White);

        foreach (var component in MenuComponents)
            component.Draw(gameTime, _spriteBatch);

        _spriteBatch.End();
    }

    public void DrawGame(Color backgroundColor, long pressDelta, bool isIdealTap, ScoresData scoresData)
    {
        var scoresRectangle = new Rectangle(810, 340, 300, 300);
        var deltaText = pressDelta.ToString();
        var deltaTextPos = GetCenterPositionText(scoresRectangle, deltaText);
        var niceBadText = $"{scoresData.NiceCLickCnt} / {scoresData.BadCLickCnt}";
        var niceBadTextPos = GetCenterPositionText(scoresRectangle with { Y = scoresRectangle.Y + 100 }, niceBadText);
        var procText = $"{Math.Round(scoresData.GetPercentage())}%";
        var procTextPos = GetCenterPositionText(scoresRectangle with { Y = scoresRectangle.Y - 100 }, procText);

        _spriteBatch.Begin();

        _spriteBatch.Draw(_gameBackground, new Rectangle(0, 0, _windowSize.Width, _windowSize.Height), backgroundColor);
        _spriteBatch.Draw(_circle, new Rectangle(1840, 40, 40, 40), isIdealTap ? Color.Lime : Color.White);
        _spriteBatch.Draw(_idealTapBack, scoresRectangle, Color.White);
        _spriteBatch.DrawString(_interFont, deltaText, deltaTextPos, Math.Abs(pressDelta) <= 100 ? Color.Lime : Color.Red);
        _spriteBatch.DrawString(_interFont, niceBadText, niceBadTextPos, Color.Black);
        _spriteBatch.DrawString(_interFont, procText, procTextPos, Color.Black);

        _spriteBatch.End();
    }

    public void DrawScores(GameTime gameTime, ScoresData scoresData)
    {
        var perText = $"{scoresData.GetPercentage()}%";
        var perTextPos = GetCenterPositionText(_backgroundRect with { Y = _backgroundRect.Y - 200}, perText);
        var niceText = $"Идеальные попадания: {scoresData.NiceCLickCnt}";
        var niceTextPos = GetCenterPositionText(_backgroundRect with { Y = _backgroundRect.Y + 50}, niceText);
        var badText = $"Плохие попадания: {scoresData.BadCLickCnt}";
        var badTextPos = GetCenterPositionText(_backgroundRect with { Y = _backgroundRect.Y + 100 }, badText);
        
        _spriteBatch.Begin();
        _spriteBatch.Draw(_scoresBackground, _backgroundRect, Color.White);
        
        _spriteBatch.DrawString(_bigInterFont, perText, perTextPos, Color.White);
        _spriteBatch.DrawString(_interFont, niceText, niceTextPos, Color.White);
        _spriteBatch.DrawString(_interFont, badText, badTextPos, Color.White);
        
        foreach (var component in ScoresComponents)
            component.Draw(gameTime, _spriteBatch);

        _spriteBatch.End();
    }

    private Vector2 GetCenterPositionText(Rectangle bounds, string text)
    {
        var deltaTextSize = _interFont.MeasureString(text);

        var position = new Vector2(
            bounds.X + (bounds.Width - deltaTextSize.X) / 2,
            bounds.Y + (bounds.Height - deltaTextSize.Y) / 2
        );

        return position;
    }
}