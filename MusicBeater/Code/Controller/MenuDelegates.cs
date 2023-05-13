using System;

namespace MusicBeater.Code.Controller;

public class MenuDelegates
{
    public readonly EventHandler StartButtonClick;
    public readonly EventHandler ExitButtonClick;

    public MenuDelegates(EventHandler startClick, EventHandler exitClick)
    {
        StartButtonClick = startClick;
        ExitButtonClick = exitClick;
    }
}