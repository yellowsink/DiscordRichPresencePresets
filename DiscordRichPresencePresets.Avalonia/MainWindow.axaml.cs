using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace DiscordRichPresencePresets.Avalonia
{
	public class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
#if DEBUG
			this.AttachDevTools();
#endif
		}
		
		private void AddPresence(object? sender, RoutedEventArgs e)
		{
			
		}

		private void SavePresences(object? sender, RoutedEventArgs e)
		{
			
		}

		private void LoadPresences(object? sender, RoutedEventArgs e)
		{
			
		}

		private void OptionsPopup(object? sender, RoutedEventArgs e)
		{
			
		}

		private void InitializeComponent() { AvaloniaXamlLoader.Load(this); }
	}
}