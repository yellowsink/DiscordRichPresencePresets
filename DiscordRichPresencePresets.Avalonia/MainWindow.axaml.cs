using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using DiscordRichPresencePresets.Shared;

namespace DiscordRichPresencePresets.Avalonia
{
	public class MainWindow : Window
	{
		private List<Presence> _presences = new()
		{
			new()
			{
				Data1 = "Welcome to Discord RP Presets",
				Data2 = "To get started add a preset!"
			}
		};

		private int _active = 0;

		public MainWindow()
		{
			InitializeComponent();
#if DEBUG
			this.AttachDevTools();
#endif
			
			UpdatePresenceDisplay();
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
		
			private void UpdatePresenceDisplay()
		{
			var panelPresenceList = this.FindControl<Panel>("PanelPresenceList");
			panelPresenceList.Children.Clear();
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
				// deleteButton.Click += (_, _) => RemovePresence(i1);
				// activeButton.Click += (_, _) => MakeActive(i1);
				// editButton.Click   += (_, _) => EditPresence(i1);

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
				panelPresenceList.Children.Add(uiElement);
			}
		}

		private void InitializeComponent() { AvaloniaXamlLoader.Load(this); }
	}
}