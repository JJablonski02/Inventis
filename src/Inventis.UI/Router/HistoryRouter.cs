namespace Inventis.UI.Router;

/// <summary>
/// Router that allows navigation forward and backward
/// </summary>
/// <typeparam name="TViewModelBase"></typeparam>
internal sealed class HistoryRouter<TViewModelBase>
	: IHistoryRouter<TViewModelBase> where TViewModelBase : class
{
	/// <summary>
	/// Current 'View-Model'
	/// </summary>
	private TViewModelBase _currentViewModel = null!;

	/// <summary>
	/// Delegated function representing a method that takes a 'Type' argument and returns 'TViewModelBase'
	/// </summary>
	private readonly Func<Type, TViewModelBase> _createViewModel;

	/// <summary>
	/// Event triggered when the current ViewModel changes.
	/// Subscribers can respond to this change by receiving the new instance of the ViewModel.
	/// </summary>
	public event Action<TViewModelBase>? CurrentViewModelChanged;

	/// <summary>
	/// Index
	/// </summary>
	private int _historyIndex = -1;

	/// <summary>
	/// List of view models
	/// </summary>
	private List<TViewModelBase> _history = [];

	/// <summary>
	/// Maximum history size
	/// </summary>
	private const uint HistoryMaxSize = 100;

	/// <summary>
	/// Flag indicating if there is a next item to navigate to
	/// </summary>
	public bool HasNext => _history.Count > 0 && _historyIndex < _history.Count - 1;

	/// <summary>
	/// Flag indicating if there is a previous item to navigate to
	/// </summary>
	public bool HasPrev => _historyIndex > 0;

	/// <summary>
	/// Constructor
	/// </summary>
	/// <param name="createViewModel">Creates 'View-Model'</param>
	public HistoryRouter(Func<Type, TViewModelBase> createViewModel)
	{
		_createViewModel = createViewModel;
	}

	/// <summary>
	/// Gets or sets the current ViewModel.
	/// If the new ViewModel is different from the current one, it triggers the ViewModel change event.
	/// </summary>
	private TViewModelBase CurrentViewModel
	{
		get => _currentViewModel;
		set
		{
			if (value == _currentViewModel) return;
			_currentViewModel = value;
			OnCurrentViewModelChanged(value);
		}
	}

	/// <summary>
	/// Invoked when the current ViewModel changes.
	/// Notifies subscribers of the new ViewModel.
	/// </summary>
	/// <param name="viewModel">The new ViewModel that has been set.</param>
	private void OnCurrentViewModelChanged(TViewModelBase viewModel)
	{
		CurrentViewModelChanged?.Invoke(viewModel);
	}

	/// <summary>
	/// Adds 'View-Model' to the history
	/// </summary>
	/// <param name="item"></param>
	public void Push(TViewModelBase item)
	{
		if (HasNext)
		{
			_history = _history.Take(_historyIndex + 1).ToList();
		}
		_history.Add(item);
		_historyIndex = _history.Count - 1;
		if (_history.Count > HistoryMaxSize)
		{
			_history.RemoveAt(0);
		}
	}

	/// <summary>
	/// Navigation based on the provided offset parameter
	/// </summary>
	/// <param name="offset">Offset</param>
	/// <returns>The 'View-Model' class</returns>
	public TViewModelBase? Go(int offset = 0)
	{
		if (offset == 0)
		{
			return null;
		}

		var newIndex = _historyIndex + offset;
		if (newIndex < 0 || newIndex > _history.Count - 1)
		{
			return null;
		}
		_historyIndex = newIndex;
		var viewModel = _history[_historyIndex];
		CurrentViewModel = viewModel;
		return viewModel;
	}

	/// <summary>
	/// Navigation one step back
	/// </summary>
	/// <returns>Returns 'Interface'</returns>
	public TViewModelBase? Back() => HasPrev ? Go(-1) : null;

	/// <summary>
	/// Navigation one step forward
	/// </summary>
	/// <returns></returns>
	public TViewModelBase? Forward() => HasNext ? Go(1) : null;

	/// <summary>
	/// Navigation to a generically passed 'View-Model'
	/// </summary>
	/// <typeparam name="TViewModel">Class deriving from 'ViewModelBase'</typeparam>
	/// <returns>Returns the destination (class deriving from 'ViewModelBase')</returns>
	public TViewModel GoTo<TViewModel>() where TViewModel : TViewModelBase
	{
		var destination = InstantiateViewModel<TViewModel>();
		CurrentViewModel = destination;
		Push(destination);
		return destination;
	}

	/// <summary>
	/// Instantiates a ViewModel based on its type.
	/// </summary>
	/// <typeparam name="T">The type of ViewModel to instantiate.</typeparam>
	/// <returns>A newly created instance of the ViewModel.</returns>
	private T InstantiateViewModel<T>() where T : TViewModelBase
	{
		return (T)Convert.ChangeType(_createViewModel(typeof(T)), typeof(T));
	}
}

