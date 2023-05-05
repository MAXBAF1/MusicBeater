using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using MusicBeater.Code.Controller;
using MusicBeater.Code.Model;
using MusicBeater.Code.View.Controls;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace MusicBeater.Code.View;

public class Views
{
    private readonly GameController _game;
    private static SpriteBatch _spriteBatch;
    private readonly Size _windowSize;
    public Texture2D CurrentBackground;
    public List<Component> GameComponents;

    public Texture2D MenuBackground;
    public Texture2D GameBackground;

    public Song Song; private bool isColored = false;
    public BackgroundType BackgroundType = BackgroundType.Default;

    public Views(GameController game)
    {
        _game = game;
        var graphics = new GraphicsDeviceManager(game);
        graphics.GraphicsProfile = GraphicsProfile.HiDef;

        graphics.IsFullScreen = true;
        _windowSize = new Size(1920, 1080);
        graphics.PreferredBackBufferWidth = _windowSize.Width;
        graphics.PreferredBackBufferHeight = _windowSize.Height;
        graphics.ApplyChanges();
    }

    public void LoadContent()
    {
        _spriteBatch = new SpriteBatch(_game.GraphicsDevice);
        _game.Content.RootDirectory = "Content";
        Song = _game.Content.Load<Song>("Audio/SevenNationArmy");


        GameBackground = _game.Content.Load<Texture2D>("Backgrounds/normal");
        MenuBackground = _game.Content.Load<Texture2D>("Backgrounds/menu");
        CurrentBackground = MenuBackground;

        LoadButtons();
    }

    private void LoadButtons()
    {
        var position = new Vector2(_game.Window.ClientBounds.Width / 2 - 62, _game.Window.ClientBounds.Height / 2 - 60);
        //var position = new Vector2(10, 10);
        var startButton =
            new Button(_game.Content.Load<Texture2D>("Controls/Button"), _game.Content.Load<SpriteFont>("Fonts/Font"))
            {
                Position = position,
                Text = "Start",
            };

        startButton.Click += delegate { _game.Logic.StartButton_Click(); };

        var quitButton = new Button(_game.Content.Load<Texture2D>("Controls/Button"),
            _game.Content.Load<SpriteFont>("Fonts/Font"))
        {
            Position = position with { Y = position.Y + 50 },
            Text = "Quit",
        };

        quitButton.Click += delegate { _game.Exit(); };

        GameComponents = new List<Component>
        {
            startButton,
            quitButton
        };
    }

    public Color OldColor = Color.White;
    public Color CurrColor = Color.White;
    private float _colorAmount;
    private Color _bColor;

    public void DrawGame(GameTime gameTime)
    {
        _spriteBatch.Begin();
        _bColor = Color.Lerp(OldColor, CurrColor, _colorAmount);
        _spriteBatch.Draw(CurrentBackground, new Rectangle(0, 0, _windowSize.Width, _windowSize.Height), _bColor);
        _spriteBatch.End();
    }

    public void AnimateBackground()
    {
        switch (BackgroundType)
        {
            case BackgroundType.FadeIn:
            {
                _colorAmount += 0.1f;
                if (_colorAmount >= 1f) BackgroundType = BackgroundType.FadeOut;
                break;
            }
            case BackgroundType.FadeOut:
            {
                if (_colorAmount <= 0f) BackgroundType = BackgroundType.Default;
                break;
            }
            case BackgroundType.Default:
            {
                _bColor = Color.White;
                CurrColor = Color.White;
                break;
            }
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void DrawMenu(GameTime gameTime)
    {
        _spriteBatch.Begin();
        _spriteBatch.Draw(CurrentBackground, new Rectangle(0, 0, _windowSize.Width, _windowSize.Height), Color.White);

        foreach (var component in GameComponents)
            component.Draw(gameTime, _spriteBatch);

        _spriteBatch.End();
    }
}