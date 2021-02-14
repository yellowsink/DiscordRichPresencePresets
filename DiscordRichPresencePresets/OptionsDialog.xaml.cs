using System.Windows;

namespace DiscordRichPresencePresets
{
	/// <summary>
	///     Interaction logic for OptionsDialog.xaml
	/// </summary>
	public partial class OptionsDialog : Window
	{
		public OptionsDialog()
		{
			InitializeComponent();
		}

		private void ButtonOk_OnClick(object sender, RoutedEventArgs e) => DialogResult = true;

		private void ButtonCancel_OnClick(object sender, RoutedEventArgs e) => DialogResult = false;
	}
}