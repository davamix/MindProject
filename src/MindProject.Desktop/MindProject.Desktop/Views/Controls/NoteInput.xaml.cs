using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MindProject.Desktop.Views.Controls;
public sealed partial class NoteInput : UserControl {
    public static readonly DependencyProperty SaveNoteProperty =
        DependencyProperty.RegisterAttached(
            "SaveNote",
            typeof(RelayCommand<string>),
            typeof(NoteInput),
            new PropertyMetadata(null));

    public AsyncRelayCommand<string> SaveNote {
        get => (AsyncRelayCommand<string>)GetValue(SaveNoteProperty);
        set => SetValue(SaveNoteProperty, value);
    }
    
    public string NoteContent { get; set; } = string.Empty;

    public NoteInput() {
        InitializeComponent();
    }

    private void btnSaveNote_Click(object sender, RoutedEventArgs e) {
        this.Visibility = Visibility.Collapsed;

        SaveNote?.Execute(NoteContent);
    }
}
