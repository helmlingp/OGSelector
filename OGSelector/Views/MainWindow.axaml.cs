using Avalonia.Controls;
using Avalonia.Input;
using Microsoft.Extensions.Configuration;
using OGSelector.Services;
using OGSelector.ViewModels;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace OGSelector.Views;

public partial class MainWindow : Window
{
    static IConfiguration config = ConfigurationService.BuildConfiguration();
    static Settings settings = config.GetRequiredSection("Settings").Get<Settings>();

    private static bool _fullscreen = settings.Fullscreen;
    private static bool _canExit = settings.AllowExit;
    public static string RegKeyPath = settings.RegKeyPath.Replace("HKEY_LOCAL_MACHINE\\", "");

    // Windows API imports for low-level keyboard hook
    private const int WH_KEYBOARD_LL = 13;
    private const int WM_KEYDOWN = 0x0100;
    private const int WM_SYSKEYDOWN = 0x0104;
    private const int VK_LWIN = 0x5B;
    private const int VK_RWIN = 0x5C;
    
    private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);
    private static LowLevelKeyboardProc? _proc;
    private static IntPtr _hookID = IntPtr.Zero;

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool UnhookWindowsHookEx(IntPtr hhk);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr GetModuleHandle(string lpModuleName);

    public MainWindow()
    {
        // Set the DataContext to a new MainViewModel instance
        var viewModel = new MainViewModel();
        DataContext = viewModel;
        
        if (_canExit == false)
        {
            Closing += (sender, args) => args.Cancel = true;
        }

        if (_fullscreen == true)
        {
            this.WindowState = WindowState.FullScreen;
            
            // Install keyboard hook to block Windows+M only in fullscreen mode
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                _proc = HookCallback;
                _hookID = SetHook(_proc);
                Closing += (sender, args) => UnhookWindowsHookEx(_hookID);
            }
        }

        InitializeComponent();
        
        // Load data through the ViewModel
        _ = InitializeAsync(viewModel);
    }

    private static IntPtr SetHook(LowLevelKeyboardProc proc)
    {
        using (var curProcess = System.Diagnostics.Process.GetCurrentProcess())
        using (var curModule = curProcess.MainModule)
        {
            if (curModule?.ModuleName != null)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }
        return IntPtr.Zero;
    }

    private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
    {
        if (nCode >= 0 && (wParam == (IntPtr)WM_KEYDOWN || wParam == (IntPtr)WM_SYSKEYDOWN))
        {
            int vkCode = Marshal.ReadInt32(lParam);
            
            // Check if Windows key is being pressed
            if (vkCode == VK_LWIN || vkCode == VK_RWIN)
            {
                // Block the Windows key
                return (IntPtr)1;
            }
        }
        return CallNextHookEx(_hookID, nCode, wParam, lParam);
    }

    private async Task InitializeAsync(MainViewModel viewModel)
    {
        string? jsonUrl = null;

        // Check if jsonURL was provided as a command-line argument
        var args = Environment.GetCommandLineArgs();
        if (args.Length > 1 && !string.IsNullOrWhiteSpace(args[1]))
        {
            jsonUrl = args[1];
            System.Diagnostics.Debug.WriteLine($"Using jsonURL from command-line argument: {jsonUrl}");
        }
        else if (!string.IsNullOrWhiteSpace(settings.jsonURL))
        {
            // Fall back to jsonURL from settings
            jsonUrl = settings.jsonURL;
            System.Diagnostics.Debug.WriteLine($"Using jsonURL from settings: {jsonUrl}");
        }

        await viewModel.LoadDataAsync(jsonUrl);
    }
}

public sealed class Settings
{
    public required string jsonURL { get; set; }
    public required bool Fullscreen { get; set; }
    public required bool AllowExit { get; set; }
    public required string RegKeyPath { get; set; }
}