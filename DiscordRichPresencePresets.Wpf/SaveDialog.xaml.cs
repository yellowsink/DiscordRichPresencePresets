namespace DiscordRichPresencePresets.Wpf
{
	/// <summary>
	///     Interaction logic for SaveDialog.xaml
	/// </summary>
	public partial class SaveDialog : Window
	{
		public SaveDialog()
		{
			InitializeComponent();
		}

		private void ButtonOk_OnClick(object sender, RoutedEventArgs e) => DialogResult = true;

		private void ButtonCancel_OnClick(object sender, RoutedEventArgs e) => DialogResult = false;
	}
}