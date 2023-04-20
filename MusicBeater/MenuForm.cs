using System.Windows.Forms;
using MusicBeater.Properties;

namespace MusicBeater;

public partial class MenuForm : Form
{
    private static Button _startBtn;
    private static Button _settingsBtn;
    private static Button _exitBtn;
    private static TableLayoutPanel _layoutPanel;

    public MenuForm()
    {
        EnterFullScreenMode();
        InitButtons();
        InitLayoutTable();

        Controls.Add(_layoutPanel);
    }

    private void InitButtons()
    {
        _startBtn = new Button
        {
            Text = Resources.MenuForm_Start,
            Dock = DockStyle.Fill
        };
        _settingsBtn = new Button
        {
            Text = Resources.MenuForm_Settings,
            Dock = DockStyle.Fill
        };
        _exitBtn = new Button
        {
            Text = Resources.MenuForm_Exit,
            Dock = DockStyle.Fill
        };

        _startBtn.Click += delegate { Open(new GameForm()); };
        _exitBtn.Click += delegate { Application.Exit(); };
    }

    private void Open(Form form)
    {
        form.Location = Location;
        form.StartPosition = FormStartPosition.Manual;
        form.KeyUp += (sender, args) =>  { if (args.KeyCode == Keys.Escape) Show(); };
        form.Show();
        Hide();
    }

    private static void InitLayoutTable()
    {
        _layoutPanel = new TableLayoutPanel();
        _layoutPanel.RowStyles.Clear();
        _layoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
        _layoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 80));
        _layoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 80));
        _layoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 80));
        _layoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
        _layoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
        _layoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        _layoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));

        _layoutPanel.Controls.Add(new Panel(), 0, 0);
        _layoutPanel.Controls.Add(_startBtn, 1, 1);
        _layoutPanel.Controls.Add(_settingsBtn, 1, 2);
        _layoutPanel.Controls.Add(_exitBtn, 1, 3);
        _layoutPanel.Controls.Add(new Panel(), 2, 4);

        _layoutPanel.Dock = DockStyle.Fill;
    }

    private void EnterFullScreenMode()
    {
        //WindowState = FormWindowState.Normal;
        //FormBorderStyle = FormBorderStyle.None;
        WindowState = FormWindowState.Maximized;
    }
}