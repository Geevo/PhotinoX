using Photino.NET;

var app = new PhotinoApplication
{
    ShutdownMode = PhotinoShutdownMode.OnMainWindowClose
};

var window = new PhotinoWindow()
    .SetTitle("Transparency Demo")
    .SetChromeless(true)
    .SetSize(720, 620)
    .Center()
    .SetTransparent(true)
    .Load("wwwroot/index.html");

window.RegisterWebMessageReceivedHandler((_, args) =>
{
    switch (args.Message)
    {
        case "transparent":
            window.Transparent = true;
            break;

        case "opaque":
            window.Transparent = false;
            break;

        case "toggle":
            window.Transparent = !window.Transparent;
            break;

        case "close":
            window.Close();
            break;
    }

    if (!window.IsClosed)
        window.SendWebMessage(window.Transparent ? "transparent" : "opaque");
});

return app.Run(window);