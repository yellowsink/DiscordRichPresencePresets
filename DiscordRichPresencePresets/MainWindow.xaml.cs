using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static DiscordRichPresencePresets.PresetDataManager;

namespace DiscordRichPresencePresets
{
	/// <summary>
	///     Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private List<Presence> _presences;

		private readonly Options _options;

		private int _active;

		private readonly PresenceApiWorker _apiWorker;

		private string _currentCollection;

		public MainWindow()
		{
			InitializeComponent();

			_options = LoadOptions();

			_currentCollection = _options.DefaultCollection;

			var loadedPresetCollection = LoadPresetCollection(_options.DefaultCollection, out _active);
			_presences = loadedPresetCollection.Any()
				             ? loadedPresetCollection
				             : new()
				             {
					             new Presence
					             {
						             Data1 = "Welcome to Discord RP Presets",
						             Data2 = "To get started add a preset!"
					             }
				             };
			_apiWorker = new(_options.ClientId);

			_apiWorker.SetRichPresence(_presences[_active]);

			UpdatePresenceDisplay();
		}

		private void AddPresence(object sender, RoutedEventArgs e)
		{
			var dialog = new AddDialog();

			var result = dialog.ShowDialog();
			if (!result.HasValue || !result.Value) return;
			_presences.Add(new Presence
			{
				Data1          = dialog.TextBoxData1.Text,
				Data2          = dialog.TextBoxData2.Text,
				BigImage       = dialog.TextBoxBigImg.Text,
				SmallImage     = dialog.TextBoxSmallImg.Text,
				BigImageText   = dialog.TextBoxBigImgTxt.Text,
				SmallImageText = dialog.TextBoxSmallImgTxt.Text,
				Buttons = new List<PresenceButton>
				{
					new() {Text = dialog.TextBoxBtnText1.Text, Url = dialog.TextBoxBtnUrl1.Text},
					new() {Text = dialog.TextBoxBtnText2.Text, Url = dialog.TextBoxBtnUrl2.Text}
				}
			});
			UpdatePresenceDisplay();

			if (_options.AutoSave) _presences.SavePresetCollection(_currentCollection, _active, _options.MinifiedJson);
		}

		private void SavePresences(object sender, RoutedEventArgs e)
		{
			var presenceSlots = GetPresetCollections();

			var dialog = new SaveDialog();
			foreach (var slot in presenceSlots) dialog.ComboBoxSlots.Items.Add(slot);

			var result = dialog.ShowDialog();
			if (!result.HasValue || !result.Value) return;

			if (string.IsNullOrWhiteSpace(dialog.ComboBoxSlots.Text))
			{
				MessageBox.Show("Please name the collection");
				SavePresences(sender, e); // ooh recursion
			}
			else
			{
				PresetDataManager.SavePresetCollection(_presences, dialog.ComboBoxSlots.Text, _active,
				                                       _options.MinifiedJson);
				_currentCollection = dialog.ComboBoxSlots.Text;
			}
		}

		private void LoadPresences(object sender, RoutedEventArgs e)
		{
			var presenceSlots = GetPresetCollections();

			var dialog = new SaveDialog
			{
				ComboBoxSlots  = {IsEditable = false, SelectedIndex = 0},
				Title          = "Load Presences",
				TextBlockTitle = {Text = "Load Presences"}
			};
			foreach (var slot in presenceSlots) dialog.ComboBoxSlots.Items.Add(slot);

			var result = dialog.ShowDialog();
			if (!result.HasValue || !result.Value) return;

			if (string.IsNullOrWhiteSpace(dialog.ComboBoxSlots.Text))
			{
				MessageBox.Show("Please choose a collection");
				LoadPresences(sender, e); // ooh recursion
			}
			else
			{
				_presences         = LoadPresetCollection(dialog.ComboBoxSlots.Text, out _active);
				_currentCollection = dialog.ComboBoxSlots.Text;
				MakeActive(_active);
				UpdatePresenceDisplay();
			}
		}

		private void OptionsPopup(object sender, RoutedEventArgs e)
		{
			var dialog = new OptionsDialog
			{
				CheckBoxAutoSave         = {IsChecked = _options.AutoSave},
				TextBoxClientId          = {Text      = _options.ClientId},
				TextBoxDefaultCollection = {Text      = _options.DefaultCollection},
				CheckBoxMinify           = {IsChecked = _options.MinifiedJson}
			};

			var result = dialog.ShowDialog();
			if (!result.HasValue || !result.Value) return;

			// ReSharper disable once PossibleInvalidOperationException
			_options.AutoSave          = dialog.CheckBoxAutoSave.IsChecked.Value;
			_options.ClientId          = dialog.TextBoxClientId.Text;
			_options.DefaultCollection = dialog.TextBoxDefaultCollection.Text;
			// ReSharper disable once PossibleInvalidOperationException
			_options.MinifiedJson = dialog.CheckBoxMinify.IsChecked.Value;

			SaveOptions(_options, _options.MinifiedJson);
			_apiWorker.Reset(_options.ClientId);
			MakeActive(_active);
		}

		private void EditPresence(int i)
		{
			var p = _presences[i];
			var dialog = new AddDialog
			{
				TextBlockTitle     = {Text  = "Edit Presence"},
				Root               = {Title = "Edit Presence"},
				TextBoxData1       = {Text  = p.Data1},
				TextBoxData2       = {Text  = p.Data2},
				TextBoxBigImgTxt   = {Text  = p.BigImageText},
				TextBoxSmallImgTxt = {Text  = p.SmallImageText},
				TextBoxBigImg      = {Text  = p.BigImage},
				TextBoxSmallImg    = {Text  = p.SmallImage},
				TextBoxBtnText1    = {Text  = p.Buttons.ElementAtOrDefault(0)?.Text ?? string.Empty},
				TextBoxBtnUrl1     = {Text  = p.Buttons.ElementAtOrDefault(0)?.Url ?? string.Empty},
				TextBoxBtnText2    = {Text  = p.Buttons.ElementAtOrDefault(1)?.Text ?? string.Empty},
				TextBoxBtnUrl2     = {Text  = p.Buttons.ElementAtOrDefault(1)?.Url ?? string.Empty}
			};

			var result = dialog.ShowDialog();
			if (!result.HasValue || !result.Value) return;
			_presences[i] = new Presence
			{
				Data1          = dialog.TextBoxData1.Text,
				Data2          = dialog.TextBoxData2.Text,
				BigImage       = dialog.TextBoxBigImg.Text,
				SmallImage     = dialog.TextBoxSmallImg.Text,
				BigImageText   = dialog.TextBoxBigImgTxt.Text,
				SmallImageText = dialog.TextBoxSmallImgTxt.Text,
				Buttons = new List<PresenceButton>
				{
					new() {Text = dialog.TextBoxBtnText1.Text, Url = dialog.TextBoxBtnUrl1.Text},
					new() {Text = dialog.TextBoxBtnText2.Text, Url = dialog.TextBoxBtnUrl2.Text}
				}
			};

			if (_active == i) MakeActive(i);

			UpdatePresenceDisplay();

			if (_options.AutoSave)
				_presences.SavePresetCollection(_currentCollection, _active, _options.MinifiedJson);
		}

		private void RemovePresence(int i)
		{
			_presences.RemoveAt(i);
			if (_presences.Count == 0) _apiWorker.RemoveRichPresence();
			else MakeActive(_active != 0 ? _active - 1 : _active);
			UpdatePresenceDisplay();
			if (_options.AutoSave)
				PresetDataManager.SavePresetCollection(_presences, _currentCollection, _active, _options.MinifiedJson);
		}

		private void MakeActive(int i)
		{
			_active = i;

			_apiWorker.SetRichPresence(_presences[i]);

			UpdatePresenceDisplay();
			if (_options.AutoSave)
				PresetDataManager.SavePresetCollection(_presences, _currentCollection, _active, _options.MinifiedJson);
		}

		private void UpdatePresenceDisplay()
		{
			PanelPresenceList.Children.Clear();
			for (var i = 0; i < _presences.Count; i++)
			{
				var presence = _presences[i];
				var data1Text = new TextBlock
				{
					Text              = presence.Data1,
					FontSize          = 18,
					Margin            = new Thickness(5, 0, 0, 0),
					VerticalAlignment = VerticalAlignment.Center
				};
				var data2Text = new TextBlock
				{
					Text              = presence.Data2,
					FontSize          = 18,
					Margin            = new Thickness(5, 0, 0, 0),
					VerticalAlignment = VerticalAlignment.Center
				};
				var imagesText = new TextBlock
				{
					Text              = (presence.BigImage ?? "none") + ", " + (presence.SmallImage ?? "none"),
					FontSize          = 18,
					Margin            = new Thickness(5, 0, 0, 0),
					VerticalAlignment = VerticalAlignment.Center
				};

				Grid.SetRow(data2Text,  1);
				Grid.SetRow(imagesText, 2);

				var bigImgText = new TextBlock
				{
					Text              = presence.BigImageText,
					FontSize          = 18,
					Margin            = new Thickness(5, 0, 0, 0),
					VerticalAlignment = VerticalAlignment.Center
				};
				var smallImgText = new TextBlock
				{
					Text              = presence.SmallImageText,
					FontSize          = 18,
					Margin            = new Thickness(5, 0, 0, 0),
					VerticalAlignment = VerticalAlignment.Center
				};
				var buttonsText = new TextBlock
				{
					Text = presence.Buttons.Count(b => !string.IsNullOrWhiteSpace(b.Text) &&
					                                   !string.IsNullOrWhiteSpace(b.Url)) switch
					{
						0 => "No buttons",
						1 => "1 button",
						2 => "2 buttons",
						_ => string.Empty
					},
					FontSize          = 18,
					Margin            = new Thickness(5, 0, 0, 0),
					VerticalAlignment = VerticalAlignment.Center
				};

				Grid.SetRow(smallImgText, 1);
				Grid.SetRow(buttonsText,  2);
				Grid.SetColumn(bigImgText,   1);
				Grid.SetColumn(smallImgText, 1);
				Grid.SetColumn(buttonsText,  1);

				var activeButton = new Button
				{
					Content = "Make Active",
					Margin  = new Thickness(5)
				};
				var editButton = new Button
				{
					Content = "Edit",
					Margin  = new Thickness(5)
				};
				var deleteButton = new Button
				{
					Content = "Delete",
					Margin  = new Thickness(5)
				};

				var i1 = i;
				deleteButton.Click += (_, _) => RemovePresence(i1);
				activeButton.Click += (_, _) => MakeActive(i1);
				editButton.Click   += (_, _) => EditPresence(i1);

				Grid.SetColumn(activeButton, 2);
				Grid.SetColumn(editButton,   2);
				Grid.SetColumn(deleteButton, 2);
				Grid.SetRow(editButton,   1);
				Grid.SetRow(deleteButton, 2);

				var uiElement = new Border
				{
					BorderBrush     = _active == i ? Brushes.ForestGreen : Brushes.LightGray,
					BorderThickness = new Thickness(2),
					Margin          = new Thickness(5),
					Height          = 100,
					Child = new Grid
					{
						ColumnDefinitions =
						{
							new ColumnDefinition {Width = new GridLength(4, GridUnitType.Star)},
							new ColumnDefinition {Width = new GridLength(3, GridUnitType.Star)},
							new ColumnDefinition {Width = GridLength.Auto}
						},
						RowDefinitions =
						{
							new RowDefinition {Height = new GridLength(1, GridUnitType.Star)},
							new RowDefinition {Height = new GridLength(1, GridUnitType.Star)},
							new RowDefinition {Height = new GridLength(1, GridUnitType.Star)}
						},
						Children =
						{
							data1Text,
							data2Text,
							imagesText,

							bigImgText,
							smallImgText,
							buttonsText,

							activeButton,
							editButton,
							deleteButton
						}
					}
				};
				PanelPresenceList.Children.Add(uiElement);
			}
		}
	}
}