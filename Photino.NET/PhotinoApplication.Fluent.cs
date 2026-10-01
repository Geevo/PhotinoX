using static Photino.NET.NativeMethods;

namespace Photino.NET;

partial class PhotinoApplication
{
    /// <summary>
    /// Sets the application display name used by application-level native services.
    /// </summary>
    /// <param name="applicationName">The application display name.</param>
    /// <returns>The current <see cref="PhotinoApplication"/> instance.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when called after the application has started.
    /// </exception>
    public PhotinoApplication SetName(string? applicationName)
    {
        Name = applicationName;
        return this;
    }

    /// <summary>
    /// Sets the application icon path used by application-level native services.
    /// </summary>
    /// <param name="iconPath">The application icon path.</param>
    /// <returns>The current <see cref="PhotinoApplication"/> instance.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when called after the application has started.
    /// </exception>
    public PhotinoApplication SetIconPath(string? iconPath)
    {
        IconPath = iconPath;
        return this;
    }

    /// <summary>
    /// Sets the native notification registration identifier.
    /// </summary>
    /// <param name="registrationId">The native notification registration identifier.</param>
    /// <returns>The current <see cref="PhotinoApplication"/> instance.</returns>
    /// <remarks>
    /// On Windows, this value is used as the application notification registration identifier.
    /// On Linux, this value is passed as the notification desktop-entry hint.
    /// </remarks>
    /// <exception cref="InvalidOperationException">
    /// Thrown when called after the application has started.
    /// </exception>
    public PhotinoApplication SetNotificationRegistrationId(string? registrationId)
    {
        NotificationRegistrationId = registrationId;
        return this;
    }

    /// <summary>
    /// Enables or disables application notifications.
    /// </summary>
    /// <param name="enabled"><see langword="true"/> to enable notifications; otherwise, <see langword="false"/>.</param>
    /// <returns>The current <see cref="PhotinoApplication"/> instance.</returns>
    public PhotinoApplication SetNotificationsEnabled(bool enabled)
    {
        NotificationsEnabled = enabled;
        return this;
    }

    /// <summary>
    /// Sets the application shutdown mode.
    /// </summary>
    /// <param name="shutdownMode">The shutdown mode to use.</param>
    /// <returns>The current <see cref="PhotinoApplication"/> instance.</returns>
    public PhotinoApplication SetShutdownMode(PhotinoShutdownMode shutdownMode)
    {
        ShutdownMode = shutdownMode;
        return this;
    }

    /// <summary>
    /// Sets the WebView2 runtime path used by Windows WebView2 initialization.
    /// </summary>
    /// <remarks>
    /// Windows only. This method must be called before any WebView2-backed window is created.
    /// On other platforms, this method has no effect.
    /// </remarks>
    /// <param name="path">The WebView2 runtime path, or <see langword="null"/> to clear it.</param>
    /// <returns>The current <see cref="PhotinoApplication"/> instance.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the WebView2 runtime path could not be set.
    /// </exception>
    /// <seealso href="https://learn.microsoft.com/en-us/microsoft-edge/webview2/concepts/distribution">
    /// Distribute your app and the WebView2 Runtime
    /// </seealso>
    public PhotinoApplication SetWebView2RuntimePath(string? path)
    {
        if (!Platform.IsWindows)
            return this;

        if (!Photino_SetWebView2RuntimePath(path))
            throw new InvalidOperationException("Failed to set the WebView2 runtime path.");

        return this;
    }
}