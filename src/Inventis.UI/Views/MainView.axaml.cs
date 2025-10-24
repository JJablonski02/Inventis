using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.VisualTree;
using Inventis.UI.ViewModels;

namespace Inventis.UI.Views;

internal sealed partial class MainView : UserControl
{
	public MainView()
	{
		InitializeComponent();
	}

	// Disables the unreachable code warning, variable is declared but never used (CS0162, CS0168)
#pragma warning disable CS0162, CS0168

	private DateTime _lastTimeClickTime; // Records the last time the mouse button was clicked.

	private bool _mouseDownForWindowMoving = false; // Indicates if the mouse button is currently pressed for window movement.

	private PointerPoint _originalPoint; // Stores the original position of the mouse pointer when dragging begins.

	/// <summary>
	/// Handles the mouse movement event over the grid control.
	/// Moves the window when the mouse button is held down.
	/// </summary>
	/// <param name="sender">The object that triggered the event.</param>
	/// <param name="e">The pointer event arguments.</param>
	private void Grid_OnPointerMoved(object? sender, PointerEventArgs e)
	{
#if !DEBUG
    return;
#endif
		var window = GetCurrentWindow(sender);

		if (window is not null)
		{
			if (!_mouseDownForWindowMoving) return;

			PointerPoint currentPoint = e.GetCurrentPoint(this);

			window.Position = new PixelPoint(window.Position.X + (int)(currentPoint.Position.X - _originalPoint.Position.X),
			window.Position.Y + (int)(currentPoint.Position.Y - _originalPoint.Position.Y));
		}
	}


	/// <summary>
	/// Handles the mouse button pressed event over the grid control.
	/// Toggles the window state if double-clicked and enables window movement.
	/// </summary>
	/// <param name="sender">The object that triggered the event.</param>
	/// <param name="e">The pointer pressed event arguments.</param>
	private void Grid_OnPointerPressed(object? sender, PointerPressedEventArgs e)
	{
#if !DEBUG
    return;
#endif
		var window = GetCurrentWindow(sender);

		if (window is not null)
		{
			var currentTime = DateTime.Now;

			var timeDifference = currentTime - _lastTimeClickTime;

			if (timeDifference.TotalMilliseconds < 300)
			{
				ToggleWindowState(window);
			}

			_lastTimeClickTime = currentTime;

			if (window.WindowState == WindowState.Maximized || window.WindowState == WindowState.FullScreen) return;

			_mouseDownForWindowMoving = true;
			_originalPoint = e.GetCurrentPoint(this);
		}
	}

	/// <summary>
	/// Handles the mouse button released event over the grid control.
	/// Stops the window movement when the mouse button is released.
	/// </summary>
	/// <param name="sender">The object that triggered the event.</param>
	/// <param name="e">The pointer released event arguments.</param>
	private void Grid_OnPointerReleased(object? sender, PointerReleasedEventArgs e)
	{
#if !DEBUG
    return;
#endif
		_mouseDownForWindowMoving = false;
	}

	/// <summary>
	/// Retrieves the current window from the sender object.
	/// </summary>
	/// <param name="sender">The object that triggered the event.</param>
	/// <returns>The current window or null if not found.</returns>
	private static Window? GetCurrentWindow(object? sender)
	{
		return (sender as Control)?.GetVisualRoot() as Window;
	}

	/// <summary>
	/// Toggles the window state between maximized and normal.
	/// </summary>
	/// <param name="window">The window to toggle.</param>
	private static void ToggleWindowState(Window window)
	{
		if (window.WindowState == WindowState.Maximized)
		{
			window.WindowState = WindowState.Normal;
		}
		else
		{
			window.WindowState = WindowState.Maximized;
		}
	}

	// Restores the unreachable code warning and variable is declared but never used (CS0162, CS0168)
#pragma warning restore CS0162, CS0168
}
