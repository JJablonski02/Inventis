using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace Inventis.UI.Controls;

/// <summary>
/// FormInput control
/// </summary>
public partial class FormInput : UserControl
{
	/// <summary>
	/// Constructor
	/// </summary>
	public FormInput()
	{
		InitializeComponent();

		LabelProperty.Changed.AddClassHandler<FormInput>((x, e) => x.OnLabelChanged(e.NewValue as string));
	}

	/// <summary>
	/// Name of the control 'InputField'
	/// </summary>
	private const string inputFieldName = "InputField";

	/// <summary>
	/// Reference to the Input control
	/// </summary>
	private TextBox? _textBox;

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
			_textBox.LostFocus += InputField_LostFocus;
		}
	}

	private void InputField_LostFocus(object? sender, RoutedEventArgs e)
	{
		if (string.IsNullOrEmpty(CustomText))
		{
			return;
		}

		string cleaned = CustomText;

		if (OnlyDigits)
		{
		    cleaned = new string([.. CustomText.Where(c => char.IsDigit(c) || c == '.' || c == ',')]);

			if (string.IsNullOrEmpty(cleaned))
			{
				return;
			}

			cleaned = cleaned.Replace('.', ',');

			int firstCommaIndex = cleaned.IndexOf(',');
			if (firstCommaIndex >= 0)
			{
				string before = cleaned.Substring(0, firstCommaIndex);
				string after = new([.. cleaned[(firstCommaIndex + 1)..].Where(char.IsDigit)]);
				cleaned = before + "," + after;
			}

			if (DecimalPlaces.HasValue && DecimalPlaces.Value is not 0 &&
				decimal.TryParse(cleaned, NumberStyles.Any, CultureInfo.CurrentCulture, out var number))
			{
				number = Math.Round(number, 2, MidpointRounding.AwayFromZero);

				cleaned = number.ToString($"N{DecimalPlaces.Value}", CultureInfo.CurrentCulture);
			}
		}

		if (!string.IsNullOrEmpty(LostFocusSuffix) &&
			!cleaned.EndsWith(LostFocusSuffix))
		{
			cleaned += LostFocusSuffix;
		}

		CustomText = cleaned;
	}


	/// <summary>
	/// Optional number of decimal places to format the number with when losing focus.
	/// </summary>
	public static readonly StyledProperty<int?> DecimalPlacesProperty =
		AvaloniaProperty.Register<FormInput, int?>(nameof(DecimalPlaces), defaultValue: null);

	public int? DecimalPlaces
	{
		get => GetValue(DecimalPlacesProperty);
		set => SetValue(DecimalPlacesProperty, value);
	}

	/// <summary>
	/// Suffix that will be appended to CustomText when the TextBox loses focus.
	/// </summary>
	public static readonly StyledProperty<string?> LostFocusSuffixProperty =
		AvaloniaProperty.Register<FormInput, string?>(nameof(LostFocusSuffix), defaultValue: null);

	public string? LostFocusSuffix
	{
		get => GetValue(LostFocusSuffixProperty);
		set => SetValue(LostFocusSuffixProperty, value);
	}

	/// <summary>
	/// Flag to determine whether the suffix should be appended only if the text contains digits.
	/// </summary>
	public static readonly StyledProperty<bool> OnlyDigitsProperty =
		AvaloniaProperty.Register<FormInput, bool>(nameof(OnlyDigits), defaultValue: false);

	public bool OnlyDigits
	{
		get => GetValue(OnlyDigitsProperty);
		set => SetValue(OnlyDigitsProperty, value);
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

	/// <summary>
	/// Label styled property
	/// </summary>
	public static readonly StyledProperty<string?> LabelProperty =
			AvaloniaProperty.Register<TextBox, string?>(
				nameof(Label),
				null);

	/// <summary>
	/// Label value over control
	/// </summary>
	public string? Label
	{
		get => GetValue(LabelProperty);
		set => SetValue(LabelProperty, value);
	}


	/// <summary>
	/// Determines whether the label should be visible.
	/// </summary>
	public static readonly StyledProperty<bool> ShowLabelProperty =
		AvaloniaProperty.Register<FormInput, bool>(
			nameof(ShowLabel),
			false);

	public bool ShowLabel
	{
		get => GetValue(ShowLabelProperty);
		set => SetValue(ShowLabelProperty, value);
	}

	/// <summary>
	/// Method triggered whenever Label changes.
	/// </summary>
	private void OnLabelChanged(string? newLabel)
		=> ShowLabel = !string.IsNullOrWhiteSpace(newLabel);
}
