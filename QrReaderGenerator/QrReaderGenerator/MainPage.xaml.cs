using System;
using QRLib;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using System.IO;
using System.Runtime.CompilerServices;
using static System.Net.Mime.MediaTypeNames;
using System.Net.NetworkInformation;
using ZXing;
using ZXing.QrCode;

namespace QrReaderGenerator
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;
            zxing.OnScanResult +=  (result) => Device.BeginInvokeOnMainThread(async () => {
                zxing.IsAnalyzing = false;
                textlbl.Text = result.Text;
                DecorateLabel();
                zxing.IsAnalyzing = true;
            });
        }

        private void DecorateLabel()
        {
            if (textlbl.Text.StartsWith("http", StringComparison.InvariantCultureIgnoreCase))
            {
                linkBtn.IsVisible = true;
                textlbl.TextDecorations = TextDecorations.Underline;
            }
            else
            {
                linkBtn.IsVisible = false;
                textlbl.TextDecorations = TextDecorations.None;
            }
        }

        private void GenerateBtn_Clicked(object sender, EventArgs e)
        {
            var input = QREnterText.Text;
            if (!string.IsNullOrEmpty(input))
            {
                barImg.BarcodeValue = input;
            }          
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            zxing.IsScanning = true;
        }
        protected override void OnDisappearing()
        {
            zxing.IsScanning = false;
            base.OnDisappearing();
        }

        public async Task OpenBrowser()
        {
            try
            {              
                await Browser.OpenAsync(textlbl.Text, BrowserLaunchMode.SystemPreferred);
            }
            catch (Exception)
            {               
            }
        }

        private async void linkBtn_Clicked(object sender, EventArgs e)
        {
            await OpenBrowser();
        }
    }
}
