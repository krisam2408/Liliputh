using CommunityToolkit.Maui.Storage;
using LiliputhApp.Model;
using LiliputhApp.Model.DataTransfer;
using MVVMCore;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace LiliputhApp.ViewModel;

public sealed class MainViewModel : BaseViewModel
{
    private string m_title;
    public string Title { get => m_title; set => SetValue(ref m_title, value); }

    private ObservableCollection<FileData> m_files;
    public ObservableCollection<FileData> Files { get => m_files; set => SetValue(ref m_files, value); }

    private bool m_clearFilesVisibility;
    public bool ClearFilesVisibility { get => m_clearFilesVisibility; set => SetValue(ref m_clearFilesVisibility, value); }

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
            if(val == MergeOptions.TemplateMerge)
            {
                IsTemplatePathVisible = true;
                return;
            }
            IsTemplatePathVisible = false;
        }
    }

    private bool m_templatePathVisible;
    public bool IsTemplatePathVisible { get => m_templatePathVisible; set => SetValue(ref m_templatePathVisible, value); }

    private bool m_minify;
    public bool MinifyOption { get => m_minify; set => SetValue(ref m_minify, value); }

    private string? m_outputPath;
    private string? m_outputFilename;

    private const string m_outputPathButtonDefaultText = "Set Output Path";
    private string m_outputPathButtonText;
    public string OutputPathButtonText { get => m_outputPathButtonText; set => SetValue(ref m_outputPathButtonText, value); }

    private bool m_outputPathClearVisibility;
    public bool OutputPathClearVisibility { get => m_outputPathClearVisibility; set => SetValue(ref m_outputPathClearVisibility, value); }

    private const string m_outputFileButtonDefaultText = "Set Output File";
    private string m_outputFileButtonText;
    public string OutputFileButtonText { get => m_outputFileButtonText; set => SetValue(ref m_outputFileButtonText, value); }

    private bool m_outputFileClearVisibility;
    public bool OutputFileClearVisibility { get => m_outputFileClearVisibility; set => SetValue(ref m_outputFileClearVisibility, value); }

    private bool m_outputFileEntryVisibility;
    public bool OutputFileEntryVisibility { get => m_outputFileEntryVisibility; set => SetValue(ref m_outputFileEntryVisibility, value); }

    public ICommand SelectFilesCommand { get; private set; }
    public ICommand ClearFilesCommand { get; private set; }
    public ICommand SetOutputPathCommand { get; private set; }
    public ICommand ClearOutputPathCommand { get; private set; }
    public ICommand SetOutputFileCommand { get; private set; }
    public ICommand ClearOutputFileCommand { get; private set; }
    public ICommand AcceptFilenameCommand { get; private set; }
    public ICommand ApplyCommand { get; private set; }
    
    public MainViewModel()
    {
        m_title = "Liliputh";

        m_files = new();

        m_clearFilesVisibility = false;

        MergeOptionIndex = 0;
        m_outputPathButtonText = m_outputPathButtonDefaultText;
        m_outputFileButtonText = m_outputFileButtonDefaultText;

        m_outputPathClearVisibility = false;
        m_outputFileClearVisibility = false;
        m_outputFileEntryVisibility = false;

        SelectFilesCommand = new Command(async () => await SelectFiles());
        ClearFilesCommand = new Command(ClearFiles);
        SetOutputPathCommand = new Command(async () => await SetOutputPath());
        ClearOutputPathCommand = new Command(ClearOutputPath);
        SetOutputFileCommand = new Command(SetOutputFile);
        ClearOutputFileCommand = new Command(ClearOutputFile);
        AcceptFilenameCommand = new Command((p) => AcceptFilename((string)p));
        ApplyCommand = new Command(Apply);
    }

    public async Task SelectFiles()
    {
        IEnumerable<FileResult> files = await FilePicker
            .Default
            .PickMultipleAsync();

        foreach (FileResult file in files)
        {
            FileData item = new(file);

            bool isValid = item.IsValid;

            bool notDuplicate = !Files.Contains(item);

            if(isValid && notDuplicate)
                Files.Add(item);
        }

        if(Files.Count > 0)
            ClearFilesVisibility = true;
    }

    public void ClearFiles()
    {
        Files.Clear();
        ClearFilesVisibility = false;
    }

    public async Task SetOutputPath()
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

    public void ClearOutputPath()
    {
        m_outputPath = null;
        OutputPathButtonText = m_outputPathButtonDefaultText;
        OutputPathClearVisibility = false;
    }

    public void SetOutputFile()
    {
        OutputFileEntryVisibility = true;
    }

    public void AcceptFilename(string filename)
    {
        OutputFileEntryVisibility = false;
        if(string.IsNullOrWhiteSpace(filename)) 
            return;

        m_outputFilename = filename;
        OutputFileButtonText = filename;
        OutputFileClearVisibility = true;
    }

    public void ClearOutputFile()
    {
        m_outputFilename = null;
        OutputFileButtonText = m_outputFileButtonDefaultText;
        OutputFileClearVisibility = false;
    }

    public void Apply()
    {

    }
}
