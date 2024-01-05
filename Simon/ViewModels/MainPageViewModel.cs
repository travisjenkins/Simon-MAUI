using CommunityToolkit.Maui.Views;
using Simon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Simon.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        private Game? _game = null;
        private readonly MediaElement _mediaElementBlue;
        private readonly MediaElement _mediaElementGreen;
        private readonly MediaElement _mediaElementRed;
        private readonly MediaElement _mediaElementYellow;
        private readonly MediaElement _mediaElementWrong;

        private Color _gridBackgroundColor = Colors.Black;
        public Color GridBackgroundColor { get => _gridBackgroundColor; set => SetProperty(ref _gridBackgroundColor, value); }

        private Color _blueButtonBackgroundColor = Color.FromRgba(0, 0.29, 0.92, 1);
        public Color BlueButtonBackgroundColor { get => _blueButtonBackgroundColor; set => SetProperty(ref _blueButtonBackgroundColor, value); }

        private Color _greenButtonBackgroundColor = Color.FromRgba(0.00, 0.74, 0.09, 1);
        public Color GreenButtonBackgroundColor { get => _greenButtonBackgroundColor; set => SetProperty(ref _greenButtonBackgroundColor, value); }

        private Color _redButtonBackgroundColor = Color.FromRgba(0.91, 0.01, 0.01, 1);
        public Color RedButtonBackgroundColor { get => _redButtonBackgroundColor; set => SetProperty(ref _redButtonBackgroundColor, value); }

        private Color _yellowButtonBackgroundColor = Color.FromRgba(0.99, 1, 0.04, 1);
        public Color YellowButtonBackgroundColor { get => _yellowButtonBackgroundColor; set => SetProperty(ref _yellowButtonBackgroundColor, value); }

        private Color _startButtonBackgroundColor = Color.FromRgba(0.91, 0.01, 0.01, 1);
        public Color StartButtonBackgroundColor { get => _startButtonBackgroundColor; set => SetProperty(ref _startButtonBackgroundColor, value); }

        private double _blueButtonOpacity = 0.5;
        public double BlueButtonOpacity { get => _blueButtonOpacity; set => SetProperty(ref _blueButtonOpacity, value); }

        private double _greenButtonOpacity = 0.5;
        public double GreenButtonOpacity { get => _greenButtonOpacity; set => SetProperty(ref _greenButtonOpacity, value); }

        private double _redButtonOpacity = 0.5;
        public double RedButtonOpacity { get => _redButtonOpacity; set => SetProperty(ref _redButtonOpacity, value); }

        private double _yellowButtonOpacity = 0.5;
        public double YellowButtonOpacity { get => _yellowButtonOpacity; set => SetProperty(ref _yellowButtonOpacity, value); }

        private string _gameMessage = string.Empty;
        public string GameMessage { get => _gameMessage; set => SetProperty(ref _gameMessage, value); }

        private readonly List<int> _difficulties = [1, 2, 3, 4];
        public List<int> Difficulties { get => _difficulties; }

        private int _selectedDifficulty = 1;
        public int SelectedDifficulty { get => _selectedDifficulty; set => SetProperty(ref _selectedDifficulty, value); }

        private string _startButtonText = "START";
        public string StartButtonText { get => _startButtonText; set => SetProperty(ref _startButtonText, value); }

        Command? _startButtonCommand;
        public ICommand StartButtonCommand => _startButtonCommand ??= new Command(async () => await StartButtonClicked());

        Command? _simonButtonCommand;
        public ICommand SimonButtonCommand => _simonButtonCommand ??= new Command(async (color) => await SimonButtonClicked(color));

        public MainPageViewModel(MediaElement mediaElementBlue, MediaElement mediaElementGreen, MediaElement mediaElementRed, MediaElement mediaElementYellow, MediaElement mediaElementWrong)
        {
            Title = "simon";
            GameMessage = "Press Start to Begin!";
            _mediaElementBlue = mediaElementBlue;
            _mediaElementGreen = mediaElementGreen;
            _mediaElementRed = mediaElementRed;
            _mediaElementYellow = mediaElementYellow;
            _mediaElementWrong = mediaElementWrong;
        }

        private void StopMediaElement(string color)
        {
            switch (color)
            {
                case "Red":
                    if (_mediaElementRed.CurrentState != CommunityToolkit.Maui.Core.Primitives.MediaElementState.Stopped)
                        _mediaElementRed.Stop();
                    break;
                case "Blue":
                    if (_mediaElementBlue.CurrentState != CommunityToolkit.Maui.Core.Primitives.MediaElementState.Stopped)
                        _mediaElementBlue.Stop();
                    break;
                case "Yellow":
                    if (_mediaElementYellow.CurrentState != CommunityToolkit.Maui.Core.Primitives.MediaElementState.Stopped)
                        _mediaElementYellow.Stop();
                    break;
                case "Green":
                    if (_mediaElementGreen.CurrentState != CommunityToolkit.Maui.Core.Primitives.MediaElementState.Stopped)
                        _mediaElementGreen.Stop();
                    break;
                case "Wrong":
                    if (_mediaElementWrong.CurrentState != CommunityToolkit.Maui.Core.Primitives.MediaElementState.Stopped)
                        _mediaElementWrong.Stop();
                    break;
                default:
                    break;
            }
        }

        public async Task PlaySound(string color)
        {
            switch (color)
            {
                case "Red":
                    _mediaElementRed.Play();
                    break;
                case "Blue":
                    _mediaElementBlue.Play();
                    break;
                case "Yellow":
                    _mediaElementYellow.Play();
                    break;
                case "Green":
                    _mediaElementGreen.Play();
                    break;
                case "Wrong":
                    _mediaElementWrong.Play();
                    break;
                default:
                    break;
            }
            await Task.Delay(500);
            StopMediaElement(color);
        }

        private void DimButton(string color)
        {
            switch (color)
            {
                case "Red":
                    RedButtonOpacity = 0.5;
                    break;
                case "Blue":
                    BlueButtonOpacity = 0.5;
                    break;
                case "Yellow":
                    YellowButtonOpacity = 0.5;
                    break;
                case "Green":
                    GreenButtonOpacity = 0.5;
                    break;
                default:
                    break;
            }
        }

        private void LightButton(string color)
        {
            switch (color)
            {
                case "Red":
                    RedButtonOpacity = 1;
                    break;
                case "Blue":
                    BlueButtonOpacity = 1;
                    break;
                case "Yellow":
                    YellowButtonOpacity = 1;
                    break;
                case "Green":
                    GreenButtonOpacity = 1;
                    break;
                default:
                    break;
            }
        }

        private async Task FlashButtonWithSound(string color)
        {
            LightButton(color);
            await PlaySound(color);
            DimButton(color);
        }

        private async Task DisplayCurrentSequence()
        {
            if (_game != null)
            {
                int delay = _game.GetDelay();
                foreach (var color in _game.GetGamePattern())
                {
                    await FlashButtonWithSound(color);
                    await Task.Delay(delay);
                }
            }   
        }

        public async Task StartButtonClicked()
        {
            if (IsBusy) return;
            IsBusy = true;
            if (_game != null)
            {
                await ResetGame();
            }
            else
            {
                _game = new(SelectedDifficulty);
            }
            _game.NextSequence();
            GameMessage = $"Level {_game.Level}";
            StartButtonText = "RESET";
            StartButtonBackgroundColor = Colors.Orange;
            await Task.Delay(1000);
            await DisplayCurrentSequence();
            IsBusy = false;
        }

        private async Task ResetGame()
        {
            if (GridBackgroundColor != Colors.Black) GridBackgroundColor = Colors.Black;
            _game?.ResetGame(SelectedDifficulty);
            await Task.Delay(1000);
        }


        public async Task SimonButtonClicked(object color)
        {
            if (_game is null) return;
            if (_game.GameOver) return;
            if (_game.IsWinner()) return;
            if (_game.Level == 0) return;
            if (IsBusy) return;
            if (color is not string buttonColor) return;
            IsBusy = true;
            _game.UserPattern.Add(buttonColor);
            await FlashButtonWithSound(buttonColor);
            int currentValue = _game.UserPattern.Count - 1;
            bool correct = _game.CheckAnswer(currentValue);
            if (correct)
            {
                if (_game.PatternsAreEqualSize())
                {
                    if (!_game.IsWinner())
                    {
                        GameMessage = $"Level {_game.Level}";
                        _game.NextSequence();
                        await Task.Delay(1000);
                        await DisplayCurrentSequence();
                        _game.ResetUserPattern();
                    }
                    else await Winner();
                }
            }
            else await GameOver();
            IsBusy = false;
        }

        private async Task Winner()
        {
            GameMessage = "WINNER";
            if (_game != null) _game.GameOver = true;
            await DisplayWinEffect();
        }

        private async Task DisplayWinEffect()
        {
            List<Color> colors = [RedButtonBackgroundColor, GreenButtonBackgroundColor, BlueButtonBackgroundColor, YellowButtonBackgroundColor];

            for (int i = 0; i < 3; i++)
            {
                foreach (var color in colors)
                {
                    GridBackgroundColor = color;
                    await Task.Delay(500);
                }
            }
            GridBackgroundColor = Colors.Black;
        }

        private async Task GameOver()
        {
            await PlaySound("Wrong");
            GridBackgroundColor = Color.FromRgba(0.91, 0.01, 0.01, 0.4);
            GameMessage = "GAME OVER";
            if (_game != null) _game.GameOver = true;
        }
    }
}
