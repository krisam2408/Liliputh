using LiliputhApp.ViewModel;

namespace LiliputhApp.Model;

public sealed class Terminal
{
    public int WindowWidth { get; set; }
    public int WindowHeight { get; set; }

    public MainViewModel? Main { get; set; }

    private static Terminal? m_instance;
    public static Terminal Instance
    {
        get
        {
            if(m_instance == null)
                m_instance = new Terminal();
            return m_instance;
        }
    }

    private Terminal()
    {
        WindowWidth = 256;
        WindowHeight = 512;
    }
}