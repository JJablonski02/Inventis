namespace Inventis.UI.Router;

internal interface IHistoryRouter<TViewModelBase> where TViewModelBase : class
{
	public event Action<TViewModelBase>? CurrentViewModelChanged;
	bool HasNext { get; }
	bool HasPrev { get; }
	void Push(TViewModelBase item);
	TViewModelBase? Go(int offset = 0);
	TViewModelBase? Back();
	TViewModel GoTo<TViewModel>() where TViewModel : TViewModelBase;
}

