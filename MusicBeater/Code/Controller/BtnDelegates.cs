using System;

namespace MusicBeater.Code.Controller;

public class BtnDelegates
{
    public readonly EventHandler StartButtonClick;
    public readonly EventHandler ExitButtonClick;
    public readonly EventHandler ExitToMenuClick;

    public BtnDelegates(EventHandler startClick, EventHandler exitClick, EventHandler exitToMenu)
    {
        StartButtonClick = startClick;
        ExitButtonClick = exitClick;
        ExitToMenuClick = exitToMenu;
    }
}