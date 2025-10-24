using Avalonia.Controls;
using Avalonia.Media.Imaging;
using BarcodeStandard;
using SkiaSharp;

namespace Inventis.UI.Controls;

internal sealed partial class BarcodeView : UserControl
{
	public void ShowBarcode(string ean13)
	{
		var b = new Barcode
		{
			IncludeLabel = true
		};

		SKImage skImage = b.Encode(BarcodeStandard.Type.Ean13, ean13, SKColors.Black, SKColors.White, 300, 120);

		using var data = skImage.Encode(SKEncodedImageFormat.Png, 100);
		using var ms = new MemoryStream(data.ToArray());

		var bitmap = new Bitmap(ms);

		Content = new Image
		{
			Source = bitmap,
			Width = 300,
			Height = 120,
		};
	}
}
