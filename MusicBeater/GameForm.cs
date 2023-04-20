using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace MusicBeater;

public partial class GameForm : Form
{
    private readonly Label _label;
    private WMPLib.WindowsMediaPlayer _wmp;
    public GameForm()
    {
        WindowState = FormWindowState.Maximized;
        PlaySimpleSound();
        
        _label = new Label
        {
            Location = new Point(0, 0),
            Size = ClientSize with { Height = 30 },
            Text = "0",
            BackColor = Color.Aqua
        };
        Controls.Add(_label);
        

        KeyUp += SpacePressed;
        MouseUp += MouseClicked;
        Closing += delegate { Application.Exit(); };
    }
    
    private void PlaySimpleSound()
    {
        _wmp = new WMPLib.WindowsMediaPlayer
        {
            URL = @"D:\Microsoft Visual Studio\repos\GameProject\MusicBeater\Musics\DRIVE.mp3"
        };
        _wmp.controls.play();
    }

    private void SpacePressed(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Space)
            Increment();
        else if (e.KeyCode == Keys.Escape)
            _wmp.controls.stop();
    }

    private void Increment() => _label.Text = (int.Parse(_label.Text) + 1).ToString();
    private void MouseClicked(object sender, EventArgs e) => Increment();
}