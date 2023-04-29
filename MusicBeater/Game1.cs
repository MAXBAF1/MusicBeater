using System.Drawing;
using MusicBeater.Misc;

namespace MusicBeater;

using System.Diagnostics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

/// <summary>
/// This is the main type for your game.
/// </summary>
public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private BackgroundAssets _backgroundAssets;
    private Texture2D _currentBackground;
    private List<Component> _gameComponents;
    private Stopwatch _songWatch;
    private Song _song;
    private readonly Size _windowSize;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.IsFullScreen = false;
        _windowSize = new Size(1280, 720);
        _graphics.PreferredBackBufferWidth = _windowSize.Width;
        _graphics.PreferredBackBufferHeight = _windowSize.Height;
        _graphics.ApplyChanges();
        _graphics.PreferredBackBufferWidth = _windowSize.Width;
        _graphics.PreferredBackBufferHeight = _windowSize.Height;
        _graphics.ApplyChanges();

        Content.RootDirectory = "Content";
    }

    protected override void Initialize()
    {
        IsMouseVisible = true;
        _songWatch = new Stopwatch();


        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _song = Content.Load<Song>("Audio/SevenNationArmy");

        _backgroundAssets = new BackgroundAssets
        {
            Normal = Content.Load<Texture2D>("Backgrounds/normal"),
            Contrast = Content.Load<Texture2D>("Backgrounds/contrast"),
            Red = Content.Load<Texture2D>("Backgrounds/red")
        };
        _currentBackground = _backgroundAssets.Normal;

        LoadButtons();
    }

    private void LoadButtons()
    {
        //var position = new Vector2(Window.ClientBounds.Width / 2 - 62, Window.ClientBounds.Height / 2 - 60);
        var position = new Vector2(10, 10);
        var startButton =
            new Button(Content.Load<Texture2D>("Controls/Button"), Content.Load<SpriteFont>("Fonts/Font"))
            {
                Position = position,
                Text = "Start",
            };

        startButton.Click += StartButton_Click;

        var quitButton = new Button(Content.Load<Texture2D>("Controls/Button"), Content.Load<SpriteFont>("Fonts/Font"))
        {
            Position = position with { Y = position.Y + 50 },
            Text = "Quit",
        };

        quitButton.Click += QuitButton_Click;

        _gameComponents = new List<Component>
        {
            startButton,
            quitButton,
        };
    }

    private void QuitButton_Click(object sender, EventArgs e)
    {
        Exit();
    }

    private void StartButton_Click(object sender, EventArgs e)
    {
        MediaPlayer.Play(_song);
        _songWatch.Start();
    }

    protected override void UnloadContent()
    {
        // TODO: Unload any non ContentManager content here
    }

    protected override void Update(GameTime gameTime)
    {
        var keyboardState = MyKeyboard.GetState();

        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            keyboardState.IsKeyDown(Keys.Escape))
            Exit();

        const int bpmSec = 60000 / 124;
        const double offset = 50;
        const double lag = 200;

        if (MyKeyboard.IsKeyPressed(Keys.Space, true) && _songWatch.IsRunning)
        {
            var time = (_songWatch.ElapsedMilliseconds - lag) % bpmSec;
            Debug.WriteLine($"{time} {bpmSec - offset}");
            _currentBackground = time is < offset or > bpmSec - offset
                ? _backgroundAssets.Contrast
                : _backgroundAssets.Red;
        }

        foreach (var component in _gameComponents)
            component.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        _spriteBatch.Begin();
        _spriteBatch.Draw(_currentBackground, new Rectangle(0, 0, _windowSize.Width, _windowSize.Height), Color.White);

        foreach (var component in _gameComponents)
            component.Draw(gameTime, _spriteBatch);


        _spriteBatch.End();

        base.Draw(gameTime);
    }
}