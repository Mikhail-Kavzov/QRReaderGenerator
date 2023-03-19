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

namespace QrReaderGenerator
{
    public partial class MainPage : ContentPage
    {
        private readonly QRCodeReaderGenerator _code;
        private readonly PickOptions _pickOptions;
        private readonly string DATA_DIR = 
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;
            _code = new QRCodeReaderGenerator();
            var customFileType =
            new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
            {{ DevicePlatform.Android, new[] { ".png" } }});
            var options = new PickOptions
            {
                PickerTitle = "Please select a file",
                FileTypes = customFileType,
                
            };
            _pickOptions = options;
            zxing.OnScanResult += (result) => Device.BeginInvokeOnMainThread(() => {
                GenerationResultlbl.Text = result.Text;
            });

        }

        private void GenerateBtn_Clicked(object sender, EventArgs e)
        {
            var input = QREnterText.Text;
            if (!string.IsNullOrEmpty(input))
            {
                var fileName = _code.Generate(input, DATA_DIR);
                ImageFile.Source = Path.Combine(DATA_DIR, fileName);
            }
        }

        private async Task PickAndShow(PickOptions options)
        {
            try
            {
                var result = await FilePicker.PickAsync(options);
                if (result != null)
                {
                    var fullPath = Path.Combine(DATA_DIR, result.FileName);
                    var text = _code.Read(fullPath);
                    ImageFile.Source = fullPath;
                    FileName.Text = $"File Name: {result.FileName}";
                    GenerationResultlbl.Text = text;
                }
            }
            catch (Exception)
            {
                await DisplayAlert("Error", "Error while reading a file", "OK");
            }
        }

        private async void PickerFileBtn_Clicked(object sender, EventArgs e)
        {
            await PickAndShow(_pickOptions);
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
    }
}
