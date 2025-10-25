
using System.Drawing.Printing;

namespace Inventis.Windows.Services;

internal sealed class PrintingService : IPrintingService
{
	public void Print(Image img, CancellationToken cancellationToken)
	{
		using var pd = new PrintDocument();
		pd.PrintPage += (s, e) =>
		{
			e.Graphics?.DrawImage(img, 0, 0, img.Width, img.Height);
		};

		using var dlg = new PrintDialog();
		dlg.Document = pd;

		if (dlg.ShowDialog() == DialogResult.OK)
		{
			pd.PrinterSettings = dlg.PrinterSettings;
			pd.Print();
		}
	}
}
