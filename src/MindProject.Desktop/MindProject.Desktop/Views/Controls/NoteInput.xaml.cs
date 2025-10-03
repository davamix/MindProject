using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using MindProject.Desktop.Models;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MindProject.Desktop.Views.Controls;
public sealed partial class NoteInput : UserControl {
    public static readonly DependencyProperty SaveNoteProperty =
        DependencyProperty.RegisterAttached(
            "SaveNote",
            typeof(RelayCommand<Note>),
            typeof(NoteInput),
            new PropertyMetadata(null));

    public static readonly DependencyProperty ProjectProperty = 
        DependencyProperty.RegisterAttached(
            "Project",
            typeof(Project),
            typeof(NoteInput),
            new PropertyMetadata(null));

    public AsyncRelayCommand<Note> SaveNote {
        get => (AsyncRelayCommand<Note>)GetValue(SaveNoteProperty);
        set => SetValue(SaveNoteProperty, value);
    }

    public Project Project {
        get => (Project)GetValue(ProjectProperty);
        set => SetValue(ProjectProperty, value);
    }

    public Note Note { get; set; } = new();

    public NoteInput() {
        InitializeComponent();
    }

    private void btnSaveNote_Click(object sender, RoutedEventArgs e) {
        this.Visibility = Visibility.Collapsed;

        SaveNote?.Execute(Note);
    }

    private void btnClose_Click(object sender, RoutedEventArgs e) {
        this.Visibility = Visibility.Collapsed;

        txtNewNote.Text = string.Empty;
    }

    private void Button_PointerEntered(object sender, PointerRoutedEventArgs e) {
        this.ProtectedCursor = InputSystemCursor.Create(InputSystemCursorShape.Hand);
    }

    private void Button_PointerExited(object sender, PointerRoutedEventArgs e) {
        this.ProtectedCursor = InputSystemCursor.Create(InputSystemCursorShape.Arrow);
    }
}
