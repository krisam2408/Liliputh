namespace LiliputhApp.Model;

public enum AcceptedExtensions
{
    NOTACCEPTED,
    NULL,
    TXT,
    JSON,
    JS,
    CSS,
    HTML
}

public enum MergeOptions
{
    DontMerge,
    SimpleMerge,
    TemplateMerge
}

public enum CommandPossibilities
{
    Minify,
    SimpleMerge,
    TemplateMerge
}
