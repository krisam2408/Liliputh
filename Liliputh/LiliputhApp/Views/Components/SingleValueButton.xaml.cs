using System.Windows.Input;

namespace LiliputhApp.Views.Components;

public partial class SingleValueButton : ContentView
{
	public SingleValueButton()
	{
		InitializeComponent();
	}

	public static BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(SingleValueButton), "Command");
	public string Text 
	{ 
		get => (string)GetValue(TextProperty); 
		set => SetValue(TextProperty, value); 
	}

	public static BindableProperty SubmitCommandProperty = BindableProperty.Create(nameof(SubmitCommand), typeof(ICommand), typeof(SingleValueButton), null);
	public ICommand SubmitCommand 
	{ 
		get => (ICommand)GetValue(SubmitCommandProperty); 
		set => SetValue(SubmitCommandProperty, value); 
	}

    public static BindableProperty CancelCommandProperty = BindableProperty.Create(nameof(CancelCommand), typeof(ICommand), typeof(SingleValueButton), null);
    public ICommand CancelCommand 
	{ 
		get => (ICommand)GetValue(CancelCommandProperty); 
		set => SetValue(CancelCommandProperty, value); 
	}

    public static BindableProperty CancelTooltipProperty = BindableProperty.Create(nameof(CancelTooltip), typeof(string), typeof(SingleValueButton), "Cancel");
    public string CancelTooltip 
	{ 
		get => (string)GetValue(CancelTooltipProperty); 
		set => SetValue(CancelTooltipProperty, value); 
	}

	public static BindableProperty CancelVisibilityProperty = BindableProperty.Create(nameof(CancelVisibility), typeof(bool), typeof(SingleValueButton), true, propertyChanged: OnCancelVisibilityPropertyChanged);
    public bool CancelVisibility
	{
		get => (bool)GetValue(CancelVisibilityProperty);
		set => SetValue(CancelVisibilityProperty, value);
	}

	private static void OnCancelVisibilityPropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
        if (newValue == oldValue)
            return;

        bool value = (bool)newValue;
        SingleValueButton component = (SingleValueButton)bindable;

        if (value)
        {
            component.SubmitColumnSpan = 1;
            return;
        }

        component.SubmitColumnSpan = 2;
    }

	public static BindableProperty SubmitColumnSpanProperty = BindableProperty.CreateAttached(nameof(SubmitColumnSpan), typeof(int), typeof(SingleValueButton), 1);
	public int SubmitColumnSpan 
	{ 
		get => (int)GetValue(SubmitColumnSpanProperty);
		set => SetValue(SubmitColumnSpanProperty, value); 
	}
}