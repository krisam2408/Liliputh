namespace LiliputhApp.Model;

public sealed class Locator
{
    public Terminal Terminal { get; private set; }

    public Locator()
    {
        Terminal = Terminal.Instance;
    }
}
