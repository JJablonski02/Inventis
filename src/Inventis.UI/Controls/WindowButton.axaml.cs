using Avalonia.Controls;
using Avalonia.Markup.Xaml.Templates;
using Avalonia;
using Avalonia.Interactivity;
using System.Windows.Input;

namespace Inventis.UI.Controls;

public partial class WindowButton : UserControl
{
    private const string CloseVariant = "Close";

    private const string MinifyVariant = "Minify";

    private static readonly Dictionary<string, DataTemplate?> IconTemplates = new()
    {
        { CloseVariant, null },
        { MinifyVariant, null }
    };

    /// <summary>
    /// Constructor
    /// </summary>
    public WindowButton()
    {
        InitializeComponent();
        InitializeIconTemplates();
        SetIconType(Variant);

        var button = this.FindControl<Button>("ThisWindowButton")!;

        button.Click += OnClick;
    }

    public static readonly StyledProperty<string> VariantProperty =
    AvaloniaProperty.Register<WindowButton, string>(nameof(Variant), CloseVariant);

    /// <summary>
    /// Gets or sets variant of displaying icon
    /// </summary>
    public string Variant
    {
        get => GetValue(VariantProperty);
        set
        {
            SetValue(VariantProperty, value);
            SetIconType(value);
        }
    }

    /// <summary>
    /// Initializes IconTemplates
    /// </summary>
    private void InitializeIconTemplates()
    {
        IconTemplates[CloseVariant] = this.Resources[CloseVariant] as DataTemplate;
        IconTemplates[MinifyVariant] = this.Resources[MinifyVariant] as DataTemplate;
    }

    /// <summary>
    /// Sets the icon type based on the provided variant.
    /// </summary>
    /// <param name="iconType">The type of icon to set.</param>
    public void SetIconType(string iconType)
    {
        if (IconTemplates.TryGetValue(iconType, out var template))
        {
            IconContent.ContentTemplate = template;
        }
    }

    /// <summary>
    /// Defined a read-only instance property named CommandProperty
    /// </summary>
    public static readonly StyledProperty<ICommand> CommandProperty =
        AvaloniaProperty.Register<WindowButton, ICommand>(nameof(Command));

    /// <summary>
    /// Defined an instance property named Command of type ICommand.
    /// </summary>
    public ICommand Command
    {
        get => GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    /// <summary>
    /// Defined a read-only instance property named CommandParameterProperty
    /// </summary>
    public static readonly StyledProperty<object> CommandParameterProperty =
        AvaloniaProperty.Register<WindowButton, object>(nameof(CommandParameter));


    /// <summary>
    /// Defined an instance property named CommandParameter of type object.
    /// </summary>
    public object CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    public event EventHandler<RoutedEventArgs>? Click;

    private void OnClick(object? sender, RoutedEventArgs e)
        => Click?.Invoke(this, e);
}
