using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Markup.Xaml;

namespace Inventis.UI.Controls;

/// <summary>
/// Input control
/// </summary>
public partial class Input : UserControl
{
    /// <summary>
    /// Constructor
    /// </summary>
    public Input()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Name of the control 'InputField'
    /// </summary>
    private const string inputFieldName = "InputField";

    /// <summary>
    /// Name of the control 'InputCanvasFieldName'
    /// </summary>
    private const string inputCanvasFieldName = "InputCanvasFieldName";

    /// <summary>
    /// Reference to the Input control
    /// </summary>
    private TextBox? _textBox;

    /// <summary>
    /// Reference to the Canvas control
    /// </summary>
    private Canvas? _canvas;

    /// <summary>
    /// Component initialization
    /// </summary>
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);

        _textBox = this.FindControl<TextBox>(inputFieldName);

        if (_textBox is not null)
        {
            _textBox.TextChanged += OnTextChanged;
        }

        _canvas = this.FindControl<Canvas>(inputCanvasFieldName);

        if (_canvas is not null)
        {
            _canvas.IsVisible = false;
        }
    }

    /// <summary>
    /// Event triggered when the icon is clicked
    /// </summary>
    /// <param name="sender">The source of the event</param>
    /// <param name="e">Event arguments</param>
    private void OnCanvasPointerPressed(object sender, PointerPressedEventArgs e)
    {
        if (_textBox is not null)
        {
            if (_textBox.PasswordChar == '\0')
            {
                _textBox.PasswordChar = '*';
            }
            else
            {
                _textBox.PasswordChar = '\0';
            }
        }
    }

    /// <summary>
    /// Flag indicating whether the control is a password field
    /// </summary>
    public static readonly StyledProperty<bool> IsPasswordProperty =
        AvaloniaProperty.Register<Input, bool>(nameof(IsPassword), false);

    /// <summary>
    /// Indicates whether the entered text is treated as a password. <br/>
    /// When set to true, entered characters are hidden.
    /// </summary>
    public bool IsPassword
    {
        get => GetValue(IsPasswordProperty);
        set
        {
            SetValue(IsPasswordProperty, value);

            if (_textBox is not null)
            {
                _textBox.PasswordChar = '*';
            }

            if (_canvas is not null)
            {
                _canvas.IsVisible = IsPassword;
            }
        }
    }

    /// <summary>
    /// Text contained in the Input control. <br/>
    /// Allows the user to enter text values.
    /// </summary>
    public static readonly StyledProperty<string?> CustomTextProperty =
        AvaloniaProperty.Register<TextBox, string?>(nameof(CustomText), null, defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Current text value in the control
    /// </summary>
    public string? CustomText
    {
        get => GetValue(CustomTextProperty);
        set => SetValue(CustomTextProperty, value);
    }

    /// <summary>
    /// Event triggered when the text in the TextBox changes
    /// </summary>
    /// <param name="sender">The source of the event</param>
    /// <param name="e">Event arguments</param>
    private void OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        CustomText = _textBox?.Text;
    }

    /// <summary>
    /// Styled property for Watermark, which is used to indicate to the user what they should enter in the control
    /// </summary>
    public static readonly StyledProperty<string?> WatermarkProperty =
            AvaloniaProperty.Register<Input, string?>(nameof(Watermark));

    /// <summary>
    /// Text indicating what the user should enter in the Input control. <br/>
    /// Appears when the control is empty.
    /// </summary>
    public string? Watermark
    {
        get => GetValue(WatermarkProperty);
        set => SetValue(WatermarkProperty, value);
    }
}
