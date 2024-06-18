namespace LiliputhApp.Views.Components;

public partial class LabelCheckBox : ContentView
{
	public LabelCheckBox()
	{
		InitializeComponent();
	}

	public static BindableProperty TextProperty = BindableProperty
		.Create(nameof(Text), typeof(string), typeof(LabelCheckBox), "Text");

	public string Text { get => (string)GetValue(TextProperty); set => SetValue(TextProperty, value); }

	public static BindableProperty ValueProperty = BindableProperty
		.Create(nameof(Value), typeof(bool), typeof(LabelCheckBox), false);

	public bool Value { get => (bool)GetValue(ValueProperty); set => SetValue(ValueProperty, value); }
}