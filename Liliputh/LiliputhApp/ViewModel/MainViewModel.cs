using CommunityToolkit.Maui.Storage;
using LiliputhApp.Model;
using LiliputhApp.Model.DataTransfer;
using MVVMCore;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace LiliputhApp.ViewModel;

public sealed class MainViewModel : BaseViewModel
{
    #region General
    private string m_title;
    public string Title { get => m_title; set => SetValue(ref m_title, value); }

    private bool m_activityIndicatorVisibility;
    public bool ActivityIndicatorVisibility { get => m_activityIndicatorVisibility; set => SetValue(ref m_activityIndicatorVisibility, value); }

    private FileDataDictionary m_files;
    public FileDataDictionary Files { get => m_files; set => SetValue(ref m_files, value); }

    public ICommand ApplyCommand { get; private set; }

    private string? m_outputPath;
    private string? m_outputFilename;
    #endregion

    #region Messages
    private ObservableCollection<string> m_errorMessages;
    public ObservableCollection<string> ErrorMessages { get => m_errorMessages; set => SetValue(ref m_errorMessages, value); }

    private bool m_errorMessagesVisibility;
    public bool ErrorMessagesVisibility { get => m_errorMessagesVisibility; set => SetValue(ref m_errorMessagesVisibility, value); }

    private ObservableCollection<string> m_successMessages;
    public ObservableCollection<string> SuccessMessages { get => m_successMessages; set => SetValue(ref m_successMessages, value); }

    private bool m_successMessagesVisibility;
    public bool SuccessMessagesVisibility { get => m_successMessagesVisibility; set => SetValue(ref m_successMessagesVisibility, value); }
    #endregion

    #region Files Interface
    public ICommand SelectFilesCommand { get; private set; }
    public ICommand ClearFilesCommand { get; private set; }

    private bool m_clearFilesVisibility;
    public bool ClearFilesVisibility { get => m_clearFilesVisibility; set => SetValue(ref m_clearFilesVisibility, value); }
    #endregion

    #region Merge Options
    private readonly KeyValuePair<MergeOptions, string>[] m_mergeOptions =
    [
        new(MergeOptions.DontMerge, "Don't Merge"),
        new(MergeOptions.SimpleMerge, "Merge"),
        new(MergeOptions.TemplateMerge, "Via Template"),
    ];
    public KeyValuePair<MergeOptions, string>[] MergeOptionsList => m_mergeOptions;

    private MergeOptions m_selectedMergeOption;
    public int MergeOptionIndex
    {
        get { return (int)m_selectedMergeOption; }
        set
        {
            MergeOptions val = (MergeOptions)value;
            SetValue(ref m_selectedMergeOption, val);

            bool isTemplateMerge()
            {
                if(val == MergeOptions.TemplateMerge)
                    return true;
                return false;
            }

            bool isFileOutputRequired()
            {
                if (val == MergeOptions.DontMerge)
                    return false;
                return true;
            }

            TemplatePathVisible = isTemplateMerge();
            OutputFileFormVisibility = isFileOutputRequired();
        }
    }

    #endregion

    #region Template
    private bool m_templatePathVisible;
    public bool TemplatePathVisible { get => m_templatePathVisible; set => SetValue(ref m_templatePathVisible, value); }

    public ICommand ChooseTemplateCommand { get; private set; }
    public ICommand ClearTemplateCommand { get; private set; }

    private const string m_templateButtonDefaultText = "Choose Template";
    private string m_templateButtonText;
    public string TemplateButtonText { get => m_templateButtonText; set => SetValue(ref m_templateButtonText, value); }

    private bool m_templateClearVisibility;
    public bool TemplateClearVisibility { get => m_templateClearVisibility; set => SetValue(ref m_templateClearVisibility, value); }

    private FileData? m_template;
    private bool IsTemplateSet => m_template is not null;

    private ObservableCollection<TemplateRegion> m_templateRegions;
    public ObservableCollection<TemplateRegion> TemplateRegions { get => m_templateRegions; set => SetValue(ref m_templateRegions, value); }

    #endregion

    #region Minify
    private bool m_minify;
    public bool MinifyOption { get => m_minify; set => SetValue(ref m_minify, value); }

    public ICommand SetMinSuffixCommand { get; private set; }
    public ICommand ClearMinSuffixCommand { get; private set; }
    public ICommand AcceptMinSuffixCommand { get; private set; }

    private string? m_minSuffix;

    private const string m_minSuffixButtonDefaultText = "Set Min Suffix";
    private string m_minSuffixButtonText;
    public string MinSuffixButtonText { get => m_minSuffixButtonText; set => SetValue(ref m_minSuffixButtonText, value); }

    private bool m_minSuffixClearVisibility;
    public bool MinSuffixClearVisibility { get => m_minSuffixClearVisibility; set => SetValue(ref m_minSuffixClearVisibility, value); }

    private bool m_minSuffixEntryVisibility;
    public bool MinSuffixEntryVisibility { get => m_minSuffixEntryVisibility; set => SetValue(ref m_minSuffixEntryVisibility, value); }
    #endregion

    #region Output Directory
    public ICommand SetOutputPathCommand { get; private set; }
    public ICommand ClearOutputPathCommand { get; private set; }

    private const string m_outputPathButtonDefaultText = "Set Output Path";
    private string m_outputPathButtonText;
    public string OutputPathButtonText { get => m_outputPathButtonText; set => SetValue(ref m_outputPathButtonText, value); }

    private bool m_outputPathClearVisibility;
    public bool OutputPathClearVisibility { get => m_outputPathClearVisibility; set => SetValue(ref m_outputPathClearVisibility, value); }
    #endregion

    #region Filename
    public ICommand SetOutputFileCommand { get; private set; }
    public ICommand ClearOutputFileCommand { get; private set; }
    public ICommand AcceptFilenameCommand { get; private set; }

    private bool m_outputFileFormVisibility;
    public bool OutputFileFormVisibility { get => m_outputFileFormVisibility; set => SetValue(ref m_outputFileFormVisibility, value); }

    private const string m_outputFileButtonDefaultText = "Set Output File";
    private string m_outputFileButtonText;
    public string OutputFileButtonText { get => m_outputFileButtonText; set => SetValue(ref m_outputFileButtonText, value); }

    private bool m_outputFileClearVisibility;
    public bool OutputFileClearVisibility { get => m_outputFileClearVisibility; set => SetValue(ref m_outputFileClearVisibility, value); }

    private bool m_outputFileEntryVisibility;
    public bool OutputFileEntryVisibility { get => m_outputFileEntryVisibility; set => SetValue(ref m_outputFileEntryVisibility, value); }
    #endregion

    public MainViewModel()
    {
        m_title = "Liliputh";

        m_files = new();
        m_errorMessages = new();
        m_successMessages = new();
        m_templateRegions = new();

        #region Visibilities
        m_minify = false;
        m_activityIndicatorVisibility = false;
        m_errorMessagesVisibility = false;
        m_successMessagesVisibility = false;
        m_clearFilesVisibility = false;
        m_templatePathVisible = false;
        m_templateClearVisibility = false;
        m_minSuffixClearVisibility = false;
        m_minSuffixEntryVisibility = false;
        m_outputPathClearVisibility = false;
        m_outputFileFormVisibility = false;
        m_outputFileClearVisibility = false;
        m_outputFileEntryVisibility = false;

        m_templateButtonText = m_templateButtonDefaultText;
        m_minSuffixButtonText = m_minSuffixButtonDefaultText;
        m_outputPathButtonText = m_outputPathButtonDefaultText;
        m_outputFileButtonText = m_outputFileButtonDefaultText;
        #endregion

        #region Commands
        SelectFilesCommand = new Command(async () =>
        {
            ActivityIndicatorVisibility = true;
            await SelectFiles();
            ActivityIndicatorVisibility = false;
        });

        ClearFilesCommand = new Command(ClearFiles);

        ChooseTemplateCommand = new Command(async () =>
        {
            ActivityIndicatorVisibility = true;
            await SelectTemplate();
            ActivityIndicatorVisibility = false;
        });

        ClearTemplateCommand = new Command(ClearTemplate);

        SetMinSuffixCommand = new Command(SetMinSuffix);

        ClearMinSuffixCommand = new Command(ClearMinSuffix);

        AcceptMinSuffixCommand = new Command((p) => AcceptMinSuffix((string)p));

        SetOutputPathCommand = new Command(async () =>
        {
            ActivityIndicatorVisibility = false;
            await SetOutputPath();
            ActivityIndicatorVisibility = true;
        });

        ClearOutputPathCommand = new Command(ClearOutputPath);

        SetOutputFileCommand = new Command(SetOutputFile);

        ClearOutputFileCommand = new Command(ClearOutputFile);

        AcceptFilenameCommand = new Command((p) => AcceptFilename((string)p));

        ApplyCommand = new Command(async () =>
        {
            ActivityIndicatorVisibility = true;
            await Apply();
            ActivityIndicatorVisibility = false;
        });
        #endregion

        MergeOptionIndex = 0;
    }

    public void RemoveFile(Guid fileId)
    {
        if (Files.Remove(fileId))
        {
            if (Files.Count == 0)
                ClearFilesVisibility = false;
        }
    }

    #region Select Files
    private async Task SelectFiles()
    {
        IEnumerable<FileResult> files = await FilePicker
            .Default
            .PickMultipleAsync();

        foreach (FileResult file in files)
        {
            FileData item = new(file);

            bool isValid = item.IsValid;

            bool notDuplicate = !Files.ContainsKey(item);

            if(isValid && notDuplicate)
                Files.Add(new FileDataItem(item));
        }

        if(Files.Count > 0)
        {
            ClearFilesVisibility = true;
            RefreshFileDataItems();
        }
    }

    private void ClearFiles()
    {
        Files.Clear();
        ClearFilesVisibility = false;
    }

    #endregion

    #region Select Template
    private async Task SelectTemplate()
    {
        FileResult? templateResult = await FilePicker
            .Default
            .PickAsync();

        if (templateResult is null)
            return;

        FileData template = new(templateResult);

        if (!template.IsValid)
            return;

        m_template = template;
        TemplateButtonText = template.Filename;
        TemplateClearVisibility = true;

        List<TemplateRegion> regions = await template.CheckTemplateRegions();

        foreach (TemplateRegion region in regions)
        {
            TemplateRegions.Add(region);
        }

        RefreshFileDataItems();
    }

    private void RefreshFileDataItems()
    {
        foreach (FileDataItem item in Files)
        {
            item.Model.RegionVisibility = TemplateClearVisibility;

            item.Model.CheckRegions(TemplateRegions);
        }
    }

    private void ClearTemplate()
    {
        m_template = null;
        TemplateButtonText = m_templateButtonDefaultText;
        TemplateClearVisibility = false;
        TemplateRegions.Clear();

        RefreshFileDataItems();
    }

    #endregion

    #region Minify
    private void SetMinSuffix()
    {
        MinSuffixEntryVisibility = true;
    }

    private void AcceptMinSuffix(string suffix)
    {
        MinSuffixEntryVisibility = false;
        if (string.IsNullOrWhiteSpace(suffix))
            return;

        m_minSuffix = suffix;
        MinSuffixButtonText = suffix;
        MinSuffixClearVisibility = true;
    }

    private void ClearMinSuffix()
    {
        m_minSuffix = null;
        MinSuffixButtonText = m_minSuffixButtonDefaultText;
        MinSuffixClearVisibility = false;
    }

    #endregion

    #region Set Output
    private async Task SetOutputPath()
    {
        FolderPickerResult folder = await FolderPicker
            .Default
            .PickAsync();

        if (folder.Folder == null)
            return;

        m_outputPath = folder
            .Folder
            .Path;

        OutputPathButtonText = folder.Folder.Name;

        OutputPathClearVisibility = true;
    }

    private void ClearOutputPath()
    {
        m_outputPath = null;
        OutputPathButtonText = m_outputPathButtonDefaultText;
        OutputPathClearVisibility = false;
    }

    private void SetOutputFile()
    {
        OutputFileEntryVisibility = true;
    }

    private void AcceptFilename(string filename)
    {
        OutputFileEntryVisibility = false;
        if(string.IsNullOrWhiteSpace(filename)) 
            return;

        m_outputFilename = filename;
        OutputFileButtonText = filename;
        OutputFileClearVisibility = true;
    }

    private void ClearOutputFile()
    {
        m_outputFilename = null;
        OutputFileButtonText = m_outputFileButtonDefaultText;
        OutputFileClearVisibility = false;
    }

    #endregion

    private void AddErrorMessage(string message)
    {
        ErrorMessages.Add($"- {message}.");
        ErrorMessagesVisibility = true;
    }

    private void AddSuccessMessage(string message)
    {
        SuccessMessages.Add($"- {message}.");
        SuccessMessagesVisibility = true;
    }

    #region Apply
    private async Task Apply()
    {
        ErrorMessages.Clear();
        SuccessMessages.Clear();
        ErrorMessagesVisibility = false;
        SuccessMessagesVisibility = false;

        List<CommandTransfer> commands = CheckCommands();

        if (ErrorMessages.Count > 0)
        {
            return;
        }

        List<OutputFileTransfer> mergeBuilder = new();

        foreach(CommandTransfer command in commands)
        {
            if(command.IsSimpleMerge)
                mergeBuilder.Add(await SimpleMerge(command.Extension));

            if(command.IsTemplateMerge)
                mergeBuilder.Add(await TemplateMerge(command.Extension));

            if(command.IsMinify && command.IsOnly)
                mergeBuilder.AddRange(await MultipleMinify());

            if (command.IsMinify && !command.IsOnly)
                mergeBuilder[0] = SingleMinify(mergeBuilder[0]);
        }

        if(mergeBuilder.Count == 0)
        {
            AddErrorMessage("Error at commands");
            foreach (CommandTransfer command in commands)
            {
                AddErrorMessage($"{command.Command}");
            }
            ErrorMessagesVisibility = true;
            return;
        }

        foreach(OutputFileTransfer output in mergeBuilder)
        {
            int errorCount = 0;
            if(output.Error)
            {
                errorCount++;
            }

            if(errorCount == 1)
            {
                AddErrorMessage("Error file creation");
                return;
            }
            
            if(errorCount > 1)
            {
                AddErrorMessage($"{errorCount} errors at file creation");
                return;
            }
        }

        await CreateFiles(mergeBuilder);

        if(mergeBuilder.Count == 1)
        {
            AddSuccessMessage($"{mergeBuilder[0].Filename} file created");
            return;
        }

        if(mergeBuilder.Count > 1)
        {
            AddSuccessMessage($"{mergeBuilder.Count} files created");
            return;
        }
    }

    private List<CommandTransfer> CheckCommands()
    {
        List<CommandTransfer> result = new();

        if(Files.Count == 0)
            AddErrorMessage("No files selected");

        bool singleExtension = Files.HasSingleExtension(out FileExtensions extension);
        
        if(string.IsNullOrWhiteSpace(m_outputPath))
            AddErrorMessage("Output directory not set");

        if(MergeOptionIndex > 0 && string.IsNullOrWhiteSpace(m_outputFilename))
            AddErrorMessage("Output file not set");

        if(MergeOptionIndex > 0 && !singleExtension)
            AddErrorMessage("All files to be merged need to be from the same type");

        if(MergeOptionIndex == 1)
            result.Add(CommandTransfer.SimpleMerge(extension));

        if(MergeOptionIndex == 2)
        {
            if (m_template is null)
                AddErrorMessage("Template is not set");
            result.Add(CommandTransfer.TemplateMerge(extension));
        }

        if (MinifyOption)
            result.Add(CommandTransfer.Minify(extension));

        if(result.Count == 1)
        {
            CommandTransfer onlyCommand = result[0];
            if (onlyCommand == CommandPossibilities.Minify)
                result[0] = CommandTransfer.JustMinify();

            if (onlyCommand == CommandPossibilities.SimpleMerge)
                result[0] = CommandTransfer.JustSimpleMerge(extension);

            if (onlyCommand == CommandPossibilities.TemplateMerge)
                result[0] = CommandTransfer.JustTemplateMerge(extension);
        }

        if (result.Count > 0)
            return result;

        AddErrorMessage("Nothing to apply");
        return result;
    }

    private async Task<OutputFileTransfer> SimpleMerge(FileExtensions extension)
    {
        if (string.IsNullOrWhiteSpace(m_outputPath))
        {
            AddErrorMessage("Output directory not set");
            return OutputFileTransfer.FailedFile;
        }

        if (string.IsNullOrWhiteSpace(m_outputFilename))
        {
            AddErrorMessage("Output filename not set");
            return OutputFileTransfer.FailedFile;
        }

        List<string> lines = new();

        foreach (FileDataItem file in Files)
        {
            string[] fileLines = await File.ReadAllLinesAsync(file.File.Url);
            foreach (string line in fileLines)
            {
                lines.Add(line);
            }

            lines.Add("\n");
        }

        OutputFileTransfer result = new(lines, m_outputPath, m_outputFilename, extension);

        return result;
    }

    private async Task<OutputFileTransfer> TemplateMerge(FileExtensions extension)
    {
        if (string.IsNullOrWhiteSpace(m_outputPath))
        {
            AddErrorMessage("Output directory not set");
            return OutputFileTransfer.FailedFile;
        }

        if (string.IsNullOrWhiteSpace(m_outputFilename))
        {
            AddErrorMessage("Output filename not set");
            return OutputFileTransfer.FailedFile;
        }

        if (m_template is null)
        {
            AddErrorMessage("Template is not set");
            return OutputFileTransfer.FailedFile;
        }

        if (Files.Count != TemplateRegions.Count)
        {
            AddErrorMessage("Number of files doesn't match with number of regions");
            return OutputFileTransfer.FailedFile;
        }

        int regionErrors = 0;
        foreach (FileDataItem file in Files)
        {
            if(file.Model.SelectedRegion == 0)
            {
                AddErrorMessage($"File {file.File.Filename} doesn't have a region set");
                regionErrors++;
            }
        }

        if(regionErrors > 0)
            return OutputFileTransfer.FailedFile;

        List<string> lines = new();

        string[] templateLines = await File.ReadAllLinesAsync(m_template.Url);

        foreach (string line in templateLines)
        {
            if(TryGetRegion(line, out Dictionary<TemplateRegion, FileData> regions))
            {
                foreach (KeyValuePair<TemplateRegion, FileData> kvp in regions)
                {
                    string trimmedLine = line
                        .Replace(kvp.Key.FullString, "")
                        .Trim();

                    if(!string.IsNullOrWhiteSpace(trimmedLine))
                        lines.Add(trimmedLine);

                    string[] fileLines = await File.ReadAllLinesAsync(kvp.Value.Url);
                    foreach (string fileLine in fileLines)
                    {
                        lines.Add(fileLine);
                    }
                }

                continue;
            }

            lines.Add(line);
        }

        OutputFileTransfer result = new(lines, m_outputPath, m_outputFilename, extension);

        return result;
    }

    private bool TryGetRegion(string line, out Dictionary<TemplateRegion, FileData> regions)
    {
        regions = new();

        if (string.IsNullOrWhiteSpace(line))
            return false;

        if (m_template is null)
            return false;

        List<TemplateRegion> matches = m_template.MatchRegion(line);

        foreach(TemplateRegion match in matches)
        {
            foreach(FileDataItem file in Files)
            {
                if(match.Name == file.Model.Regions[file.Model.SelectedRegion].Name)
                {
                    regions.Add(match, file.File);
                    continue;
                }
            }
        }

        if (regions.Count > 0)
            return true;

        return false;
    }

    private OutputFileTransfer SingleMinify(OutputFileTransfer builder)
    {
        if (builder.Error)
        {
            AddErrorMessage("Error at single minify");
            return OutputFileTransfer.FailedFile;
        }

        if(string.IsNullOrWhiteSpace(m_minSuffix))
        {
            AddErrorMessage("Min suffix not set");
            return OutputFileTransfer.FailedFile;
        }

        StringBuilder minifyBuilder = new();

        int index = 0;
        foreach (string line in builder.Lines)
        {
            string minLine = line;

            if (string.IsNullOrWhiteSpace(minLine))
            {
                index++;
                continue;
            }

            if(builder.Extension == FileExtensions.JS)
                JSMinify(ref minLine, builder.Lines, index);

            RegularMinify(ref minLine);

            minifyBuilder.Append(minLine);
            index++;
        }

        string[] filenameParts = builder
            .Filename
            .Split('.');

        StringBuilder filenameBuilder = new();
        int partsLen = filenameParts.Length;

        for(int i = 0; i < partsLen - 1; i++)
        {
            filenameBuilder.Append(filenameParts[i]);
        }

        filenameBuilder.Append(m_minSuffix);
        filenameBuilder.Append($".{filenameParts[partsLen - 1]}");

        OutputFileTransfer result = new([minifyBuilder.ToString()], builder.Directory, filenameBuilder.ToString(), builder.Extension);

        return result;
    }

    private static void JSMinify(ref string line, string[] allLines, int index)
    {
        if(string.IsNullOrWhiteSpace(line))
            return;

        char[] scopeEndingChars =
        [
            '}',
            ']'
        ];

        line = line
            .Replace("} ", "};")
            .Replace("}\n", "};");

        int linesLen = allLines.Length;

        if (line.Last() == '}')
        {
            bool scopeEnding = false;
            for (int i = 1; i <= 3; i++)
            {
                if (linesLen >= index + i)
                {
                    string refLine = allLines[index + i].Trim();

                    if (string.IsNullOrWhiteSpace(refLine))
                        continue;

                    if (scopeEndingChars.Contains(refLine[0]))
                    {
                        scopeEnding = true;
                        break;
                    }
                }
            }

            if (!scopeEnding)
            {
                line = line
                    .Replace("}", "};");
            }
        }
    }

    private static void RegularMinify(ref string line)
    {
        line = line
            .Replace("\n", " ")
            .Replace("\t", " ")
            .Replace("  ", " ");

        int loopCount = 0;
        while (line.Contains("  ") || loopCount < 8)
        {
            line = line.Replace("  ", " ");
            loopCount++;
        }
    }

    private async Task<List<OutputFileTransfer>> MultipleMinify()
    {
        List<OutputFileTransfer> result = new();

        if (string.IsNullOrWhiteSpace(m_outputPath))
        {
            AddErrorMessage("Output directory not set");
            return result;
        }

        if (string.IsNullOrWhiteSpace(m_minSuffix))
        {
            AddErrorMessage("Min suffix not set");
            return result;
        }

        List<OutputFileTransfer> previewTransfers = new();
        foreach (FileDataItem file in Files)
        {
            string[] fileLines = await File.ReadAllLinesAsync(file.File.Url);
            OutputFileTransfer output = new(fileLines, m_outputPath, file.File.Filename, file.File.Extension);
            if(!output.Error)
                previewTransfers.Add(output);
        }

        foreach(OutputFileTransfer output in previewTransfers)
        {
            OutputFileTransfer minResult = SingleMinify(output);
            if (!minResult.Error)
                result.Add(minResult);
        }

        return result;
    }

    private static async Task CreateFiles(List<OutputFileTransfer> contents)
    {
        foreach (OutputFileTransfer file in contents)
        {
            if(!file.Error)
                await File.WriteAllLinesAsync(file.OutputPath, file.Lines);
        }
    }

    #endregion
}
