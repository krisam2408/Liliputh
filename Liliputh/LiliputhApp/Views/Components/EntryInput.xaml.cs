using System.Windows.Input;

namespace LiliputhApp.Views.Components;

public partial class EntryInput : ContentView
{
	public EntryInput()
	{
		InitializeComponent();
	}

    public static BindableProperty PlaceholderProperty = BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(SingleValueButton), "");
    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }

    public static BindableProperty InputProperty = BindableProperty.Create(nameof(Input), typeof(string), typeof(SingleValueButton), "");
    public string Input
    {
        get => (string)GetValue(InputProperty);
        set => SetValue(InputProperty, value);
    }

    public static BindableProperty AcceptCommandProperty = BindableProperty.Create(nameof(AcceptCommand), typeof(ICommand), typeof(SingleValueButton), null);
    public ICommand AcceptCommand
    {
        get => (ICommand)GetValue(AcceptCommandProperty);
        set => SetValue(AcceptCommandProperty, value);
    }


}