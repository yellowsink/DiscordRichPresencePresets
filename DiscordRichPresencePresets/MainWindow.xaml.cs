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
		private readonly string[] _images =
		{
			"among_drip", "donk", "gura", "nagisa", "thomas_bullshit", "yeetus", "yellowsink_avatar"
		};

		public List<Presence> Presences;

		public int Active;

		public PresenceApiWorker ApiWorker = new("810097912901402654");

		public MainWindow()
		{
			InitializeComponent();
			var loadedPresetCollection = LoadPresetCollection("default", out Active);
			Presences = loadedPresetCollection.Any()
				            ? loadedPresetCollection
				            : new()
				            {
					            new Presence
					            {
						            Data1 = "Welcome to Discord RP Presets",
						            Data2 = "To get started add a preset!"
					            }
				            };

			ApiWorker.SetRichPresence(Presences[Active]);

			UpdatePresenceDisplay();
		}

		private void AddPresence(object sender, RoutedEventArgs e)
		{
			var dialog = new AddDialog();
			foreach (var image in _images)
			{
				dialog.ComboBoxBigImg.Items.Add(image);
				dialog.ComboBoxSmallImg.Items.Add(image);
			}

			var result = dialog.ShowDialog();
			if (!result.HasValue || !result.Value) return;
			Presences.Add(new Presence
			{
				Data1          = dialog.TextBoxData1.Text,
				Data2          = dialog.TextBoxData2.Text,
				BigImage       = dialog.ComboBoxBigImg.Text,
				SmallImage     = dialog.ComboBoxSmallImg.Text,
				BigImageText   = dialog.TextBoxBigImg.Text,
				SmallImageText = dialog.TextBoxSmallImg.Text
			});
			UpdatePresenceDisplay();
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
				Presences.SavePresetCollection(dialog.ComboBoxSlots.Text, Active);
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
				Presences = LoadPresetCollection(dialog.ComboBoxSlots.Text, out Active);
				MakeActive(Active);
				UpdatePresenceDisplay();
			}
		}

		private void EditPresence(int i)
		{
			var dialog = new AddDialog
			{
				TextBlockTitle  = {Text  = "Edit Presence"},
				Root            = {Title = "Edit Presence"},
				TextBoxData1    = {Text  = Presences[i].Data1},
				TextBoxData2    = {Text  = Presences[i].Data2},
				TextBoxBigImg   = {Text  = Presences[i].BigImageText},
				TextBoxSmallImg = {Text  = Presences[i].SmallImageText}
			};
			foreach (var image in _images)
			{
				dialog.ComboBoxBigImg.Items.Add(image);
				dialog.ComboBoxSmallImg.Items.Add(image);
			}

			var result = dialog.ShowDialog();
			if (!result.HasValue || !result.Value) return;
			Presences[i] = new Presence
			{
				Data1          = dialog.TextBoxData1.Text,
				Data2          = dialog.TextBoxData2.Text,
				BigImage       = dialog.ComboBoxBigImg.Text,
				SmallImage     = dialog.ComboBoxSmallImg.Text,
				BigImageText   = dialog.TextBoxBigImg.Text,
				SmallImageText = dialog.TextBoxSmallImg.Text
			};

			if (Active == i) MakeActive(i);

			UpdatePresenceDisplay();
		}

		private void RemovePresence(int i)
		{
			Presences.RemoveAt(i);
			if (Presences.Count == 0) ApiWorker.RemoveRichPresence();
			else MakeActive(Active != 0 ? Active - 1 : Active);
			UpdatePresenceDisplay();
		}

		private void MakeActive(int i)
		{
			Active = i;

			ApiWorker.SetRichPresence(Presences[i]);

			UpdatePresenceDisplay();
		}

		private void UpdatePresenceDisplay()
		{
			PanelPresenceList.Children.Clear();
			for (var i = 0; i < Presences.Count; i++)
			{
				var presence = Presences[i];
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

				Grid.SetRow(smallImgText, 1);
				Grid.SetColumn(bigImgText,   1);
				Grid.SetColumn(smallImgText, 1);

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
					BorderBrush     = Active == i ? Brushes.ForestGreen : Brushes.LightGray,
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