using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.ThemeManager;

namespace DiscordRichPresencePresets.Avalonia
{
	public class App : Application
	{
		public static ThemeSelector Selector;

		public override void Initialize()
		{
			InitThemeSelector();

			AvaloniaXamlLoader.Load(this);

			DeInitThemeSelector();
		}

		public override void OnFrameworkInitializationCompleted()
		{
			if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
				desktop.MainWindow = new MainWindow();

			base.OnFrameworkInitializationCompleted();
		}


		private static void InitThemeSelector()
		{
			Selector = ThemeSelector.Create("Themes");
			Selector.LoadSelectedTheme("AvaloniaApp.theme");
		}

		private static void DeInitThemeSelector() => Selector.SaveSelectedTheme("AvaloniaApp.theme");
	}
}