using Microsoft.Web.WebView2.WinForms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace OverlayApp.Classes
{
    public static class ScreenSizeAdjuster
    {
        public static bool IsFullHeight { get; private set; } = true;
        public static int MinimumHeight = 16;
        public static int XPadding = 0;
        public static int YPadding = 16;

        public static WebView2? WebView2;

        public static async Task AdjustScreenSize(bool AdjustHeight = true, bool AdjustWidth = true)
        {
            if (Form1.ActiveForm is null || Form1.ActiveForm.IsDisposed)
                return;

            if (WebView2 is null)
                return;

            string PageDimensionsJson = await WebView2.CoreWebView2.ExecuteScriptAsync("window.GetPageDimensions();");
            if (string.IsNullOrEmpty(PageDimensionsJson) || PageDimensionsJson == "null")
                return;

            PageDimensions? CurrentPageDimensions = JsonSerializer.Deserialize<PageDimensions>(PageDimensionsJson);
            if (CurrentPageDimensions is null)
                return;

            int ExtraWidth = Form1.ActiveForm.Width - Form1.ActiveForm.ClientSize.Width;
            int ExtraHeight = Form1.ActiveForm.Height - Form1.ActiveForm.ClientSize.Height;

            if (AdjustHeight)
                Form1.ActiveForm.Height = Math.Max(MinimumHeight, CurrentPageDimensions.height + ExtraHeight + YPadding);

            if (AdjustWidth)
                Form1.ActiveForm.Width = CurrentPageDimensions.width + ExtraWidth + XPadding;
        }

        public static async void ToggleFullHeight()
        {
            if (Form1.ActiveForm is null || Form1.ActiveForm.IsDisposed)
                return;

            IsFullHeight = !IsFullHeight;

            if (IsFullHeight)
                await AdjustScreenSize(true, false);
            else
                Form1.ActiveForm.Height = MinimumHeight;
        }
    }
}
