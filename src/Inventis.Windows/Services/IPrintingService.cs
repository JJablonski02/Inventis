namespace Inventis.Windows.Services;

public interface IPrintingService
{
	void Print(Image img, CancellationToken cancellationToken);
}
