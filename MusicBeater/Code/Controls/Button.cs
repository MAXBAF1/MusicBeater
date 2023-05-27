using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MusicBeater.Code.View.Controls;

public class Button : Component
{
    public event EventHandler Click;
    private Color PenColor { get; set; }
    public Vector2 Position { get; init; }
    private Rectangle Rectangle => new((int)Position.X, (int)Position.Y, _texture.Width, _texture.Height);
    public string Text { get; init; }

    private readonly SpriteFont _font;
    private bool _isHovering;
    private readonly Texture2D _texture;
    private readonly Texture2D _hoverTexture;
    private Texture2D _drawingTexture;
    
    private MouseState _currentMouse;
    private MouseState _previousMouse;

    public Button(Texture2D texture, Texture2D hoverTexture, SpriteFont font)
    {
        _texture = texture;
        _hoverTexture = hoverTexture;
        _font = font;
        PenColor = Color.White;
    }
    
    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        _drawingTexture = _texture;
        var drawColor = Color.White;
        if (_isHovering)
        {
            _drawingTexture = _hoverTexture;
            drawColor = Color.Lime;
        }
        
        spriteBatch.Draw(_texture, Rectangle,  drawColor);
        
        if (string.IsNullOrEmpty(Text)) return;
        
        var x = Rectangle.X + Rectangle.Width / 2 - _font.MeasureString(Text).X / 2;
        var y = Rectangle.Y + Rectangle.Height / 2 - _font.MeasureString(Text).Y / 2;

        spriteBatch.DrawString(_font, Text, new Vector2(x, y), PenColor);
    }

    public override void Update(GameTime gameTime)
    {
        _previousMouse = _currentMouse;
        _currentMouse = Mouse.GetState();

        var mouseRectangle = new Rectangle(_currentMouse.X, _currentMouse.Y, 1, 1);
        _isHovering = false;

        if (!mouseRectangle.Intersects(Rectangle)) return;
        
        _isHovering = true;

        if (_previousMouse.LeftButton == ButtonState.Released && _currentMouse.LeftButton == ButtonState.Pressed)
            Click?.Invoke(this, EventArgs.Empty);
    }
}
