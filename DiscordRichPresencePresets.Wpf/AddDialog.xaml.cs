using System.Windows;

namespace DiscordRichPresencePresets.Wpf
{
	/// <summary>
	///     Interaction logic for AddDialog.xaml
	/// </summary>
	public partial class AddDialog : Window
	{
		public AddDialog()
		{
			InitializeComponent();
		}

		private void ButtonOk_OnClick(object sender, RoutedEventArgs e) => DialogResult = true;

		private void ButtonCancel_OnClick(object sender, RoutedEventArgs e) => DialogResult = false;
	}
}