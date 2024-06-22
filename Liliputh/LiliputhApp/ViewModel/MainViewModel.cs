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

    public ICommand SelectFilesCommand { get; private set; }
    public ICommand ClearFilesCommand { get; private set; }
    public ICommand SetOutputCommand { get; private set; }
    public ICommand ApplyCommand { get; private set; }
    
    public MainViewModel()
    {
        m_title = "Liliputh";

        m_files = new();

        MergeOptionIndex = 0;

        SelectFilesCommand = new Command(async () => await SelectFiles());
        ClearFilesCommand = new Command(() => ClearFiles());
        SetOutputCommand = new Command(() => SetOutput());
        ApplyCommand = new Command(() => Apply());
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
    }

    public void ClearFiles()
    {
        Files.Clear();
    }

    public void SetOutput()
    {
        
    } 

    public void Apply()
    {

    }
}
