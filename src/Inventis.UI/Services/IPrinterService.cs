using BarcodeStandard;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using SkiaSharp;

namespace Inventis.UI.Services;

internal interface IPrinterService
{
	void PrintBarcodes(
		IReadOnlyCollection<string> eanCodes);
}

internal sealed class PrinterService : IPrinterService
{
	public void PrintBarcodes(
		IReadOnlyCollection<string> eanCodes)
	{
		List<byte[]> barcodeImages = new List<byte[]>();
		var barcodeGenerator = new Barcode { IncludeLabel = true };

		foreach (var code in eanCodes)
		{
			SKImage skImage = barcodeGenerator.Encode(BarcodeStandard.Type.Ean13, code, SKColors.Black, SKColors.White, 300, 120);
			using var data = skImage.Encode(SKEncodedImageFormat.Png, 100);
			barcodeImages.Add(data.ToArray());
		}

		Document.Create(container =>
		{
			container.Page(page =>
			{
				page.Size(32, 16, Unit.Millimetre);
				page.Margin(2, Unit.Millimetre);

				page
				.Content()
				.AlignCenter()
				.AlignMiddle()
				.Column(column =>
				{
					column.Spacing(4);

					foreach (var barcode in barcodeImages)
					{
						column.Item().Image(barcode);
					}
				});
			});
		}).GeneratePdfAndShow();
	}
}


