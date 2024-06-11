using MVVMCore;

namespace LiliputhApp.ViewModel;

public sealed class MainViewModel : BaseViewModel
{
    private string m_title;
    public string Title { get { return m_title; } set { SetValue(ref m_title, value); } }

    public MainViewModel()
    {
        m_title = "Liliputh";
    }
}
