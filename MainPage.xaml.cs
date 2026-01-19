using Microsoft.Maui.Controls;

namespace OGSelector;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        BindingContext = new ViewModels.MainViewModel();
    }
}
