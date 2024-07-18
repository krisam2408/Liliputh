using System.Diagnostics.CodeAnalysis;

namespace LiliputhApp.Model.DataTransfer;

internal struct CommandTransfer
{
    internal CommandPossibilities Command { get; set; }
    internal FileExtensions Extension { get; private set; }
    internal bool IsOnly { get; set; }

    private CommandTransfer(CommandPossibilities command, FileExtensions extension, bool isOnly)
    {
        Command = command;
        Extension = extension;
        IsOnly = isOnly;
    }

    internal bool IsSimpleMerge => Command == CommandPossibilities.SimpleMerge;
    internal bool IsTemplateMerge => Command == CommandPossibilities.TemplateMerge;
    internal bool IsMinify => Command == CommandPossibilities.Minify;

    internal static CommandTransfer JustMinify() => new(CommandPossibilities.Minify, FileExtensions.NULL, true);
    internal static CommandTransfer Minify(FileExtensions extension) => new(CommandPossibilities.Minify, extension, false);
    internal static CommandTransfer JustSimpleMerge(FileExtensions extension) => new(CommandPossibilities.SimpleMerge, extension, true);
    internal static CommandTransfer SimpleMerge(FileExtensions extension) => new(CommandPossibilities.SimpleMerge, extension, false);
    internal static CommandTransfer JustTemplateMerge(FileExtensions extension) => new(CommandPossibilities.TemplateMerge, extension, false);
    internal static CommandTransfer TemplateMerge(FileExtensions extension) => new(CommandPossibilities.TemplateMerge, extension, false);

    public static bool operator ==(CommandTransfer a, CommandPossibilities b)
    {
        return a.Command == b;
    }

    public static bool operator !=(CommandTransfer a, CommandPossibilities b)
    {
        return a.Command != b;
    }

    public static bool operator ==(CommandTransfer a, CommandTransfer b)
    {
        return a.Equals(b);
    }

    public static bool operator !=(CommandTransfer a, CommandTransfer b)
    {
        return !a.Equals(b);
    }

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
