using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace OGSelector.Views;

public partial class MainView : UserControl
{
    static IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

    static UI ui = config.GetRequiredSection("UI").Get<UI>();

    // private static string _exePath = Directory.GetCurrentDirectory();

    public MainView()
    {
        InitializeComponent();
        
        // Set UI text from appsettings.json
        var headline = this.FindControl<TextBlock>("Headline");
        var subtitle = this.FindControl<TextBlock>("Subtitle");
        var information = this.FindControl<TextBlock>("Information");
        var errorHeadline = this.FindControl<TextBlock>("ErrorHeadline");
        var errorSubtitle = this.FindControl<TextBlock>("ErrorSubtitle");
        var errorInfo = this.FindControl<WrapPanel>("ErrorInformation");
        
        if (headline != null) headline.Text = ui.Headline;
        if (subtitle != null) subtitle.Text = ui.Subtitle;
        if (information != null) information.Text = ui.Information;
        
        if (errorHeadline != null) errorHeadline.Text = ui.ErrorHeadline;
        if (errorSubtitle != null) errorSubtitle.Text = ui.ErrorSubtitle;
        if (errorInfo != null && errorInfo.Children.Count > 0 && errorInfo.Children[0] is TextBlock errorTextBlock)
        {
            errorTextBlock.Text = ui.ErrorInformation;
        }
        
        var companyLogo = this.FindControl<Image>("CompanyLogo");
        var _dir = Directory.GetCurrentDirectory();
        
        // Check for logo.png first, then logo.svg, then embedded Assets\logo.png
        string? logoPath = null;
        if (File.Exists(Path.Combine(_dir, "logo.png")))
        {
            logoPath = Path.Combine(_dir, "logo.png");
        }
        else if (File.Exists(Path.Combine(_dir, "logo.svg")))
        {
            logoPath = Path.Combine(_dir, "logo.svg");
        }
        // add elseif for embedded logo
        // else if (File.Exists(Path.Combine(_exePath, "Assets", "logo.png")))
        // {
        //     logoPath = Path.Combine(_exePath, "Assets", "logo.png");
        // }
        
        if (logoPath != null && companyLogo != null)
        {
            companyLogo.Source = new Bitmap(logoPath);
        }

    }
}


public sealed class UI
{
    public required string Headline { get; set; }
    public required string Subtitle { get; set; }
    public required string Information { get; set; }
    public required string ErrorHeadline { get; set; }
    public required string ErrorSubtitle { get; set; }
    public required string ErrorInformation { get; set; }

}
