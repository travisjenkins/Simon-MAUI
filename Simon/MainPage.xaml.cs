using Simon.ViewModels;

namespace Simon
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainPageViewModel(mediaElementBlue, mediaElementGreen, mediaElementRed, mediaElementYellow, mediaElementWrong);
        }

        void ContentPage_Unloaded(object? sender, EventArgs e)
        {
            // Stop and cleanup MediaElement when we navigate away
            mediaElementBlue.Handler?.DisconnectHandler();
            mediaElementGreen.Handler?.DisconnectHandler();
            mediaElementRed.Handler?.DisconnectHandler();
            mediaElementWrong.Handler?.DisconnectHandler();
            mediaElementYellow.Handler?.DisconnectHandler();
        }
    }
}
