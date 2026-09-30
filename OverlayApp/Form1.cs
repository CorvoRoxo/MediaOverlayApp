using Microsoft.AspNetCore.Components.WebView.WindowsForms;
using Microsoft.Extensions.DependencyInjection;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using OverlayApp.Classes;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Text.Json;
using Windows.Media.Core;
using Windows.System.Update;
using WindowsMediaController;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OverlayApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            ScreenSizeAdjuster.WebView2 = blazorWebView1.WebView;
            
            var services = new ServiceCollection();
            services.AddWindowsFormsBlazorWebView();
            blazorWebView1.HostPage = "wwwroot\\index.html";
            blazorWebView1.Services = services.BuildServiceProvider();
            blazorWebView1.RootComponents.Add<Miau>("#app");
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (Screen.PrimaryScreen is null)
                return;

            Rectangle ScreenLoaded = Screen.PrimaryScreen.Bounds;
            this.Width = ScreenLoaded.Width;
        }
    }

    public class PageDimensions
    {
        public int width { get; set; }
        public int height { get; set; }
    }
}
