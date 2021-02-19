namespace DiscordRichPresencePresets.Wpf
{
	/// <summary>
	///     Interaction logic for OptionsDialog.xaml
	/// </summary>
	public partial class OptionsDialog : Window
	{
		public OptionsDialog()
		{
			InitializeComponent();
			UpdateCustomSavePathBox();
		}

		private void ButtonOk_OnClick(object sender, RoutedEventArgs e) => DialogResult = true;

		private void ButtonCancel_OnClick(object sender, RoutedEventArgs e) => DialogResult = false;

		private void ComboBoxSaveLoc_DropDownClosed(object sender, EventArgs e) => UpdateCustomSavePathBox();

		private void UpdateCustomSavePathBox()
		{
			if (ComboBoxSaveLoc.SelectedIndex == 3)
			{
				LabelCustomSavePath.Opacity     = 1;
				TextBoxCustomSavePath.Opacity   = 1;
				TextBoxCustomSavePath.IsEnabled = true;
			}
			else
			{
				LabelCustomSavePath.Opacity     = 0;
				TextBoxCustomSavePath.Opacity   = 0;
				TextBoxCustomSavePath.IsEnabled = false;
			}
		}
	}
}