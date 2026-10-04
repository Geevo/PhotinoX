using System.Text.Json;

using Photino.NET;

var application = new PhotinoApplication
{
    ShutdownMode = PhotinoShutdownMode.OnMainWindowClose,
    NotificationsEnabled = false,
};

var window = new PhotinoWindow()
    .SetTitle("PhotinoX Dialogs Demo")
    .SetSize(1100, 760)
    .Center()
    .Load("wwwroot/index.html");

window.RegisterWebMessageReceivedHandler((_, args) =>
{
    try
    {
        switch (args.Message)
        {
            case "open-file":
            {
                var paths = window.ShowOpenFile(
                    title: "Choose a file",
                    multiSelect: false,
                    filters:
                    [
                        ("Images", ["png", "jpg", "jpeg", "gif", "webp"]),
                        ("Text files", ["txt", "md", "json", "xml"]),
                        ("All files", ["*"])
                    ]);

                SendResult("Open file", paths);
                break;
            }

            case "open-files":
            {
                var paths = window.ShowOpenFile(
                    title: "Choose files",
                    multiSelect: true,
                    filters:
                    [
                        ("Images", ["png", "jpg", "jpeg", "gif", "webp"]),
                        ("Text files", ["txt", "md", "json", "xml"]),
                        ("All files", ["*"])
                    ]);

                SendResult("Open multiple files", paths);
                break;
            }

            case "open-folder":
            {
                var paths = window.ShowOpenFolder(
                    title: "Choose a folder",
                    multiSelect: false);

                SendResult("Open folder", paths);
                break;
            }

            case "open-folders":
            {
                var paths = window.ShowOpenFolder(
                    title: "Choose folders",
                    multiSelect: true);

                SendResult("Open multiple folders", paths);
                break;
            }

            case "save-file":
            {
                var path = window.ShowSaveFile(
                    title: "Save a text file",
                    filters:
                    [
                        ("Text files", ["txt"]),
                        ("JSON files", ["json"]),
                        ("All files", ["*"])
                    ],
                    defaultFileName: "document.txt");

                SendResult("Save file", path is null ? [] : [path]);
                break;
            }

            case "show-message":
            {
                var result = window.ShowMessage(
                    title: "PhotinoX Dialogs Demo",
                    text: "This message is displayed by the native operating-system dialog.",
                    buttons: PhotinoDialogButtons.YesNoCancel,
                    icon: PhotinoDialogIcon.Question);

                SendMessage(new
                {
                    type = "message-result",
                    result = result.ToString()
                });

                break;
            }
        }
    }
    catch (Exception exception)
    {
        SendMessage(new
        {
            type = "error",
            message = exception.Message
        });
    }
});

return application.Run(window);

void SendResult(string title, IReadOnlyList<string> paths)
{
    SendMessage(new
    {
        type = "dialog-result",
        title,
        paths
    });
}

void SendMessage(object message)
{
    if (!window.IsClosed)
        window.SendWebMessage(JsonSerializer.Serialize(message));
}