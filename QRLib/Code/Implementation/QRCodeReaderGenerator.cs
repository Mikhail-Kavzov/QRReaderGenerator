using Aspose.BarCode.BarCodeRecognition;
using Aspose.BarCode.Generation;
using QRLib.Code.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace QRLib
{
    public class QRCodeReaderGenerator : ICodeReader, ICodeGenerator
    {
        public string Generate(string input, string dataDir)
        {
            BarcodeGenerator gen = new BarcodeGenerator(EncodeTypes.QR, input);
            gen.Parameters.Barcode.XDimension.Pixels = 4;

            // Set Auto version
            gen.Parameters.Barcode.QR.QrVersion = QRVersion.Auto;

            //Set ForceMicroQR QR encode type
            gen.Parameters.Barcode.QR.QrEncodeType = QREncodeType.ForceMicroQR;
            var fileName = dataDir + GuidGenerator.GetGuid();
            gen.Save(fileName, BarCodeImageFormat.Png);
            return fileName;
        }

        public string Read(string path)
        {
            var builder = new StringBuilder();
            using (BarCodeReader reader = new(path, DecodeType.QR))
            {
                foreach (BarCodeResult result in reader.ReadBarCodes())
                    builder.Append(result.CodeText);
            }
            return builder.ToString();
        }
    }
}
