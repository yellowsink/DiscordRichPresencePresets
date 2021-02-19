using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace DiscordRichPresencePresets.Avalonia
{
	public class OptionsDialog : Window
	{
		public OptionsDialog()
		{
			AvaloniaXamlLoader.Load(this);
		}
		
		private void ButtonCancel_OnClick(object?             sender, RoutedEventArgs           e) => Close(false);
		private void ButtonOk_OnClick(object?                 sender, RoutedEventArgs           e) => Close(true);

		private void ComboBoxSaveLoc_SelectionChanged(object? sender, SelectionChangedEventArgs e)
			=> UpdateCustomSavePathBox();

		public void UpdateCustomSavePathBox()
		{
			if (this.FindControl<ComboBox>("ComboBoxSaveLoc").SelectedIndex == 3)
			{
				this.FindControl<Label>("LabelCustomSavePath").Opacity     = 1;
				this.FindControl<TextBox>("TextBoxCustomSavePath").Opacity   = 1;
				this.FindControl<TextBox>("TextBoxCustomSavePath").IsEnabled = true;
			}
			else
			{
				this.FindControl<Label>("LabelCustomSavePath").Opacity     = 0;
				this.FindControl<TextBox>("TextBoxCustomSavePath").Opacity   = 0;
				this.FindControl<TextBox>("TextBoxCustomSavePath").IsEnabled = false;
			}
		}
	}
}