using Microsoft.Extensions.Logging;

#if WINDOWS
using Microsoft.Maui.LifecycleEvents;
using Microsoft.UI;
using Microsoft.UI.Windowing;

using Windows.Graphics;
#endif

namespace ScrollViewBugDemo;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif
#if WINDOWS
        builder.ConfigureLifecycleEvents(events =>
        {
            events.AddWindows(wndLifeCycleBuilder =>
            {
                wndLifeCycleBuilder.OnWindowCreated(window =>
                {
                    IntPtr nativeWindowHandle = WinRT.Interop.WindowNative.GetWindowHandle(window);
                    WindowId win32WindowsId = Win32Interop.GetWindowIdFromWindow(nativeWindowHandle);
                    AppWindow winuiAppWindow = AppWindow.GetFromWindowId(win32WindowsId);

                    const int width = 768;      // 4:3 aspect with 1024:768
                    const int height = 1024;
                    const int yOffset = 20;     // for StatusBar not to occlude the page

                    var size = winuiAppWindow.Size;

                    winuiAppWindow.MoveAndResize(new RectInt32((1920 - width) / 2, (1080 - height) / 2 - yOffset, width, height));
                });
            });
        });
#endif

        return builder.Build();
	}
}
