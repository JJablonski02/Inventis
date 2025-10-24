using System.Windows.Input;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Inventis.UI.Styles;

namespace Inventis.UI.Controls;

/// <summary>
/// Button control
/// </summary>
public partial class ConfirmationButton : UserControl
{
    /// <summary>
    /// Constructor
    /// </summary>
    public ConfirmationButton()
    {
        InitializeComponent();

        this.CornerRadius = new CornerRadius(20);

		this.Background = Brush.Parse("#FF7700");
	}

	/// <summary>
	/// Initializes the component
	/// </summary>
	private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    /// <summary>
    /// Gets or sets the 'ICommand'
    /// </summary>
    public ICommand? Command
    {
        get => GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    /// <summary>
    /// Defines the '<see cref="Command"/>'
    /// </summary>
    public static readonly StyledProperty<ICommand?> CommandProperty =
        AvaloniaProperty.Register<ConfirmationButton, ICommand?>(nameof(Command));


    /// <summary>
    /// Gets or sets the parameter for the <see cref="Command"/>
    /// </summary>
    public object? CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    /// <summary>
    /// Defines the property '<see cref="CommandParameter"/>'
    /// </summary>
    public static readonly StyledProperty<object?> CommandParameterProperty =
        AvaloniaProperty.Register<ConfirmationButton, object?>(nameof(CommandParameter));

    /// <summary>
    /// Gets or sets the icon
    /// </summary>
    public string? Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    /// <summary>
    /// Defines '<see cref="Icon"/>'
    /// </summary>
    public static readonly StyledProperty<string?> IconProperty =
        AvaloniaProperty.Register<ConfirmationButton, string?>(nameof(Icon));

    /// <summary>
    /// Gets or sets the displayed text
    /// </summary>
    public string? Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    /// <summary>
    /// Defines the text '<see cref="Text"/>'
    /// </summary>
    public static readonly StyledProperty<string?> TextProperty =
        AvaloniaProperty.Register<ConfirmationButton, string?>(nameof(Text));

}
