namespace LiliputhApp.Model.DataTransfer;

internal struct CommandTransfer
{
    internal CommandPossibilities Command { get; set; }
    internal bool IsOnly { get; set; }

    internal CommandTransfer(CommandPossibilities command, bool isOnly)
    {
        Command = command;
        IsOnly = isOnly;
    }

    internal static CommandTransfer JustMinify => new(CommandPossibilities.Minify, true);
    internal static CommandTransfer Minify => new(CommandPossibilities.Minify, false);
    internal static CommandTransfer JustSimpleMerge => new(CommandPossibilities.SimpleMerge, true);
    internal static CommandTransfer SimpleMerge => new(CommandPossibilities.SimpleMerge, false);
    internal static CommandTransfer JustTemplateMerge => new(CommandPossibilities.TemplateMerge, false);
    internal static CommandTransfer TemplateMerge => new(CommandPossibilities.TemplateMerge, false);
}
