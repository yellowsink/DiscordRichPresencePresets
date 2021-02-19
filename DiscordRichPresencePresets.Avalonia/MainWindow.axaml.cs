using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using DiscordRichPresencePresets.Shared;
using static DiscordRichPresencePresets.Shared.PresetDataManager;

namespace DiscordRichPresencePresets.Avalonia
{
	public class MainWindow : Window
	{
		private readonly List<Presence> _presences = new()
		{
			new Presence()
			{
				Data1 = "Welcome to Discord RP Presets",
				Data2 = "To get started add a preset!"
			}
		};

		private readonly Options _options;

		private int _active;

		private readonly PresenceApiWorker _apiWorker;

		private string _currentCollection;

		public MainWindow()
		{
			InitializeComponent();
#if DEBUG
			this.AttachDevTools();
#endif

			_options = LoadOptions();

			_currentCollection = _options.DefaultCollection;

			var loadedPresetCollection = LoadPresetCollection(_options.DefaultCollection, out _active,
															  _options.SaveLocation,      _options.CustomSavePath);
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

#pragma warning disable 4014
		private void AddPresence(object? sender, RoutedEventArgs e) => AddPresence();
#pragma warning restore 4014

		private async Task AddPresence()
		{
			var dialog = new AddDialog();

			if (!await dialog.ShowDialog<bool>(this)) return;

			_presences.Add(new()
			{
				Data1          = dialog.FindControl<TextBox>("TextBoxData1").Text,
				Data2          = dialog.FindControl<TextBox>("TextBoxData2").Text,
				BigImage       = dialog.FindControl<TextBox>("TextBoxBigImg").Text,
				SmallImage     = dialog.FindControl<TextBox>("TextBoxSmallImg").Text,
				BigImageText   = dialog.FindControl<TextBox>("TextBoxBigImgTxt").Text,
				SmallImageText = dialog.FindControl<TextBox>("TextBoxSmallImgTxt").Text,
				Buttons = new List<PresenceButton>
				{
					new()
					{
						Text = dialog.FindControl<TextBox>("TextBoxBtnText1").Text,
						Url  = dialog.FindControl<TextBox>("TextBoxBtnUrl1").Text
					},
					new()
					{
						Text = dialog.FindControl<TextBox>("TextBoxBtnText2").Text,
						Url  = dialog.FindControl<TextBox>("TextBoxBtnUrl2").Text
					}
				}
			});

			UpdatePresenceDisplay();

			if (_options.AutoSave)
				_presences.SavePresetCollection(_currentCollection, _active, _options.MinifiedJson,
				                                _options.SaveLocation, _options.CustomSavePath);
		}

		private void SavePresences(object? sender, RoutedEventArgs e) { }

		private void LoadPresences(object? sender, RoutedEventArgs e) { }

		private void OptionsPopup(object? sender, RoutedEventArgs e) { }
		
		private async Task EditPresence(int i)
		{
			var p = _presences[i];
			var dialog = new AddDialog {Title = "Edit Presence"};
			dialog.FindControl<TextBlock>("TextBlockTitle").Text = "Edit Presence";
			dialog.FindControl<TextBox>("TextBoxData1").Text = p.Data1;
			dialog.FindControl<TextBox>("TextBoxData2").Text = p.Data2;
			dialog.FindControl<TextBox>("TextBoxBigImgTxt").Text = p.BigImageText;
			dialog.FindControl<TextBox>("TextBoxSmallImgTxt").Text = p.SmallImageText;
			dialog.FindControl<TextBox>("TextBoxBigImg").Text = p.BigImage;
			dialog.FindControl<TextBox>("TextBoxSmallImg").Text = p.SmallImage;
			dialog.FindControl<TextBox>("TextBoxBtnText1").Text = p.Buttons.ElementAtOrDefault(0)?.Text ?? string.Empty;
			dialog.FindControl<TextBox>("TextBoxBtnUrl1").Text = p.Buttons.ElementAtOrDefault(0)?.Url ?? string.Empty;
			dialog.FindControl<TextBox>("TextBoxBtnText2").Text = p.Buttons.ElementAtOrDefault(1)?.Text ?? string.Empty;
			dialog.FindControl<TextBox>("TextBoxBtnUrl2").Text = p.Buttons.ElementAtOrDefault(1)?.Url ?? string.Empty;

			if (!await dialog.ShowDialog<bool>(this)) return;
			
			_presences[i] = new Presence
			{
				Data1          = dialog.FindControl<TextBox>("TextBoxData1").Text,
				Data2          = dialog.FindControl<TextBox>("TextBoxData2").Text,
				BigImage       = dialog.FindControl<TextBox>("TextBoxBigImg").Text,
				SmallImage     = dialog.FindControl<TextBox>("TextBoxSmallImg").Text,
				BigImageText   = dialog.FindControl<TextBox>("TextBoxBigImgTxt").Text,
				SmallImageText = dialog.FindControl<TextBox>("TextBoxSmallImgTxt").Text,
				Buttons = new List<PresenceButton>
				{
					new() {Text = dialog.FindControl<TextBox>("TextBoxBtnText1").Text, Url = dialog.FindControl<TextBox>("TextBoxBtnUrl1").Text},
					new() {Text = dialog.FindControl<TextBox>("TextBoxBtnText2").Text, Url = dialog.FindControl<TextBox>("TextBoxBtnUrl2").Text}
				}
			};

			if (_active == i) MakeActive(i);

			UpdatePresenceDisplay();

			if (_options.AutoSave)
				_presences.SavePresetCollection(_currentCollection, _active, _options.MinifiedJson,
				                                _options.SaveLocation, _options.CustomSavePath);
		}

		private void RemovePresence(int i)
		{
			_presences.RemoveAt(i);
			if (_presences.Count == 0) _apiWorker.RemoveRichPresence();
			else MakeActive(_active != 0 ? _active - 1 : _active);
			UpdatePresenceDisplay();
			if (_options.AutoSave)
				_presences.SavePresetCollection(_currentCollection, _active, _options.MinifiedJson,
												_options.SaveLocation, _options.CustomSavePath);
		}

		private void MakeActive(int i)
		{
			_active = i;

			_apiWorker.SetRichPresence(_presences[i]);

			UpdatePresenceDisplay();
			if (_options.AutoSave)
				_presences.SavePresetCollection(_currentCollection, _active, _options.MinifiedJson,
												_options.SaveLocation, _options.CustomSavePath);
		}

		private void UpdatePresenceDisplay()
		{
			var panelPresenceList = this.FindControl<StackPanel>("PanelPresenceList");
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
					Classes = new Classes("actionbutton", "compactbutton")
				};
				var editButton = new Button
				{
					Content = "Edit",
					Classes = new Classes("actionbutton", "compactbutton")
				};
				var deleteButton = new Button
				{
					Content = "Delete",
					Classes = new Classes("actionbutton", "compactbutton")
				};

				var i1 = i;
				deleteButton.Click += (_,       _) => RemovePresence(i1);
				activeButton.Click += (_,       _) => MakeActive(i1);
				editButton.Click   += async (_, _) => await EditPresence(i1);

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