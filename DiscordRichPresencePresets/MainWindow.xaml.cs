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
		public List<Presence> Presences;

		public int Active;

		public MainWindow()
		{
			InitializeComponent();
			var loadedPresetCollection = LoadPresetCollection("default");
			Presences = loadedPresetCollection.Any()
				            ? loadedPresetCollection
				            : new()
				            {
					            new Presence
					            {
						            Title = "Hello, World!",
						            Data1 = "Welcome to Discord RP Presets",
						            Data2 = "To get started add a preset!"
					            }
				            };
			UpdatePresenceDisplay();
		}

		private void AddPresence(object sender, RoutedEventArgs e)
		{
			var dialog = new AddDialog();
			var result = dialog.ShowDialog();
			if (!result.HasValue || !result.Value) return;
			Presences.Add(new Presence
			{
				Title = dialog.TextBoxTitle.Text,
				Data1 = dialog.TextBoxData1.Text,
				Data2 = dialog.TextBoxData2.Text
			});
			UpdatePresenceDisplay();
		}

		private void SavePresences(object sender, RoutedEventArgs e) => Presences.SavePresetCollection("default");

		private void LoadPresences(object sender, RoutedEventArgs e)
		{
			Presences = LoadPresetCollection("default");
			UpdatePresenceDisplay();
		}

		private void EditPresence(int i)
		{
			var dialog = new AddDialog();
			dialog.TextBlockTitle.Text = "Edit Presence"; // Wow this is hacky
			dialog.Root.Title          = "Edit Presence";
			dialog.TextBoxTitle.Text   = Presences[i].Title;
			dialog.TextBoxData1.Text   = Presences[i].Data1;
			dialog.TextBoxData2.Text   = Presences[i].Data2;
			var result = dialog.ShowDialog();
			if (!result.HasValue || !result.Value) return;
			Presences[i] = new Presence
			{
				Title = dialog.TextBoxTitle.Text,
				Data1 = dialog.TextBoxData1.Text,
				Data2 = dialog.TextBoxData2.Text
			};
			UpdatePresenceDisplay();
		}

		private void RemovePresence(int i)
		{
			Presences.RemoveAt(i);
			if (Active != 0) Active--;
			UpdatePresenceDisplay();
		}

		private void MakeActive(int i)
		{
			Active = i;
			UpdatePresenceDisplay();
		}

		private void UpdatePresenceDisplay()
		{
			PanelPresenceList.Children.Clear();
			for (var i = 0; i < Presences.Count; i++)
			{
				var presence = Presences[i];
				var titleText = new TextBlock
				{
					Text              = presence.Title,
					FontSize          = 18,
					Margin            = new Thickness(5, 0, 0, 0),
					VerticalAlignment = VerticalAlignment.Center
				};
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

				Grid.SetRow(data1Text, 1);
				Grid.SetRow(data2Text, 2);

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
				deleteButton.Click += (sender, args) => RemovePresence(i1);
				activeButton.Click += (sender, args) => MakeActive(i1);
				editButton.Click   += (sender, args) => EditPresence(i1);

				Grid.SetColumn(activeButton, 1);
				Grid.SetColumn(editButton,   1);
				Grid.SetColumn(deleteButton, 1);
				Grid.SetRow(editButton,   1);
				Grid.SetRow(deleteButton, 2);

				var uiElement = new Border
				{
					BorderBrush     = Active == i ? Brushes.ForestGreen : Brushes.LightGray,
					BorderThickness = new Thickness(2),
					Margin          = new Thickness(5),
					Height          = 100,
					Width           = 400,
					Child = new Grid
					{
						ColumnDefinitions =
						{
							new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
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
							titleText,
							data1Text,
							data2Text,
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