using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MusicBeater.Code.View.Controls;

public class Button : Component
{
    public event EventHandler Click;
    public bool Clicked { get; private set; }
    public Color PenColor { get; set; }
    public Vector2 Position { get; set; }
    public Rectangle Rectangle => new((int)Position.X, (int)Position.Y, texture.Width, texture.Height);
    public string Text { get; set; }

    private SpriteFont font;
    private bool isHovering;
    private Texture2D texture;
    private MouseState currentMouse;
    private MouseState previousMouse;

    public Button(Texture2D texture2D, SpriteFont font)
    {
        texture = texture2D;
        this.font = font;
        PenColor = Color.Black;
    }
    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        var color = Color.White;
        if (isHovering)
            color = Color.Gray;
        spriteBatch.Draw(texture, Rectangle, color);
        if (!string.IsNullOrEmpty(Text))
        {
            var x = Rectangle.X + Rectangle.Width / 2 - font.MeasureString(Text).X / 2;
            var y = Rectangle.Y + Rectangle.Height / 2 - font.MeasureString(Text).Y / 2;

            spriteBatch.DrawString(font, Text, new Vector2(x, y), PenColor);
        }
    }

    public override void Update(GameTime gameTime)
    {
        previousMouse = currentMouse;
        currentMouse = Mouse.GetState();

        var mouseRectangle = new Rectangle(currentMouse.X, currentMouse.Y, 1, 1);
        isHovering = false;

        if (mouseRectangle.Intersects(Rectangle))
        {
            isHovering = true;

            if (previousMouse.LeftButton == ButtonState.Released && currentMouse.LeftButton == ButtonState.Pressed)
            {
                Click?.Invoke(this, new EventArgs());
            }
        }
    }
}
