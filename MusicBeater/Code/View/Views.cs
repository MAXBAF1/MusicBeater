using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
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
    private readonly ContentManager _content;
    private readonly GraphicsDeviceManager _graphics;
    private readonly GameWindow _window;
    private static SpriteBatch _spriteBatch;
    private readonly Size _windowSize;

    public List<Component> MenuComponents;
    public Texture2D MenuBackground;
    private readonly MenuDelegates _btnDelegates;
    
    public Texture2D GameBackground;
    public Song Song;
    private Texture2D _whiteArea;
    private Texture2D _limeArea;
    private Texture2D _circle;

    public Views(ContentManager content, GraphicsDeviceManager graphics, GameWindow window, MenuDelegates menuDelegates)
    {
        _content = content;
        _graphics = graphics;
        _windowSize = new Size(1920, 1080);
        _window = window;
        _btnDelegates = menuDelegates;

        _graphics.GraphicsProfile = GraphicsProfile.HiDef;
        //_graphics.IsFullScreen = true;
        _graphics.PreferredBackBufferWidth = _windowSize.Width;
        _graphics.PreferredBackBufferHeight = _windowSize.Height;
        _graphics.ApplyChanges();
    }

    public void LoadContent()
    {
        _spriteBatch = new SpriteBatch(_graphics.GraphicsDevice);
        _content.RootDirectory = "Content";
        Song = _content.Load<Song>("Audio/SevenNationArmy");

        GameBackground = _content.Load<Texture2D>("Backgrounds/normal");
        MenuBackground = _content.Load<Texture2D>("Backgrounds/menu");
        
        LoadButtons();
        
        _limeArea = new Texture2D(_graphics.GraphicsDevice, 1, 1);
        _limeArea.SetData(new[] { Color.White });
        
        _whiteArea = _content.Load<Texture2D>("Assets/whiteRectangle");

        _circle = _content.Load<Texture2D>("Assets/circle");
    }

    private void LoadButtons()
    {
        var position = new Vector2(_window.ClientBounds.Width / 2 - 62, _window.ClientBounds.Height / 2 - 60);
        //var position = new Vector2(10, 10);
        var startButton =
            new Button(_content.Load<Texture2D>("Controls/Button"), _content.Load<SpriteFont>("Fonts/Font"))
            {
                Position = position,
                Text = "Start",
            };

        startButton.Click += _btnDelegates.StartButtonClick;

        var quitButton = new Button(_content.Load<Texture2D>("Controls/Button"),
            _content.Load<SpriteFont>("Fonts/Font"))
        {
            Position = position with { Y = position.Y + 50 },
            Text = "Quit",
        };

        quitButton.Click += _btnDelegates.ExitButtonClick;

        MenuComponents = new List<Component>
        {
            startButton,
            quitButton
        };
    }


    public void DrawGame(GameTime gameTime, Texture2D backgroundImg, Color backgroundColor, Vector2 circlePos)
    {
        _spriteBatch.Begin();
        _spriteBatch.Draw(backgroundImg, new Rectangle(0, 0, _windowSize.Width, _windowSize.Height), backgroundColor);
        
        _spriteBatch.Draw(_whiteArea, new Rectangle(700, 900, 600, 50), Color.White);
        _spriteBatch.Draw(_limeArea, new Rectangle(800, 900, 100, 50), Color.Lime);
        _spriteBatch.Draw(_circle, circlePos, Color.White);
        _spriteBatch.End();
    }

    public void DrawMenu(GameTime gameTime, Texture2D backgroundImg)
    {
        _spriteBatch.Begin();
        _spriteBatch.Draw(backgroundImg, new Rectangle(0, 0, _windowSize.Width, _windowSize.Height), Color.White);

        foreach (var component in MenuComponents)
            component.Draw(gameTime, _spriteBatch);

        _spriteBatch.End();
    }
}