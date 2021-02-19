using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace DiscordRichPresencePresets.Avalonia
{
	public class LoadDialog : Window
	{
		public LoadDialog()
		{
			AvaloniaXamlLoader.Load(this);
		}
		
		private void ButtonCancel_OnClick(object?             sender, RoutedEventArgs           e) => Close(false);
		private void ButtonOk_OnClick(object?                 sender, RoutedEventArgs           e) => Close(true);
	}
}