using System.ComponentModel;
using System.Runtime.InteropServices;
using Photino.NET;

internal static unsafe partial class Program
{
    [STAThread]
    public static int Main(string[] args)
    {
        var app = new PhotinoApplication().SetShutdownMode(PhotinoShutdownMode.OnMainWindowClose)
            .SetNotificationsEnabled(!args.Contains("--no-notifications"));
        var trayWindow = CreateWindowExW(0, "STATIC", "Tray", 0, 0, 0, 0, 0, 0, 0, 0, 0);
        if (trayWindow == 0) throw new Win32Exception(Marshal.GetLastPInvokeError());
        var icon = new IconData { Size = (uint)sizeof(IconData), Window = trayWindow,
            Id = 1, Flags = 2, Icon = LoadIconW(0, 32512) };
        var added = false;
        int result;
        try
        {
            if (Shell_NotifyIconW(0, ref icon) == 0) throw new Win32Exception(Marshal.GetLastPInvokeError());
            added = true;
            icon.Version = 4;
            if (Shell_NotifyIconW(4, ref icon) == 0) throw new IOException("Could not set the tray icon version.");
            var window = new PhotinoWindow().SetTitle("Windows shutdown reproduction").SetSize(600, 400)
                .SetUserDataFolder(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "PhotinoXShutdownRepro", "WebView"))
                .LoadString("<p>Close this window to reproduce the shutdown crash.</p>");
            window.Initialize();
            app.MainWindow = window;
            window.Show();
            result = app.Run();
        }
        finally
        {
            if (added) Shell_NotifyIconW(2, ref icon);
            DestroyWindow(trayWindow);
        }
        Console.WriteLine("Returning normally");
        return result;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct IconData
    {
        public uint Size; public nint Window; public uint Id, Flags, Callback; public nint Icon;
        public fixed char Tip[128]; public uint State, StateMask; public fixed char Info[256];
        public uint Version; public fixed char Title[64]; public uint InfoFlags; public Guid Guid; public nint BalloonIcon;
    }

    [LibraryImport("user32", StringMarshalling = StringMarshalling.Utf16, SetLastError = true)]
    private static partial nint CreateWindowExW(uint extended, string className, string title, uint style, int x, int y, int width, int height, nint parent, nint menu, nint instance, nint parameter);
    [LibraryImport("user32")]
    private static partial int DestroyWindow(nint window);
    [LibraryImport("user32")]
    private static partial nint LoadIconW(nint instance, nint resource);
    [LibraryImport("shell32", SetLastError = true)]
    private static partial int Shell_NotifyIconW(uint message, ref IconData data);
}
