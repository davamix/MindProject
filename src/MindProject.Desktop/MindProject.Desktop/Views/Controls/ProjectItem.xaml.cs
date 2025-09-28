using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using MindProject.Desktop.Models;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MindProject.Desktop.Views.Controls;

public sealed partial class ProjectItem : UserControl {

    public static readonly DependencyProperty ProjectProperty =
        DependencyProperty.RegisterAttached(
            "Project",
            typeof(Project),
            typeof(ProjectItem),
            new PropertyMetadata(null));

    public static readonly DependencyProperty CommandProperty = 
        DependencyProperty.RegisterAttached(
            "Command", 
            typeof(RelayCommand<Project>), 
            typeof(ProjectItem), 
            new PropertyMetadata(null));

    public Project Project {
        get => (Project)GetValue(ProjectProperty);
        set => SetValue(ProjectProperty, value);
    }

    public AsyncRelayCommand<Project> Command {
        get => (AsyncRelayCommand<Project>)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public string UpdatedAtString => $"Last update: {Project?.UpdatedAt.ToString("g") ?? ""}";

    public ProjectItem() {
        InitializeComponent();
    }

    protected override void OnPointerReleased(PointerRoutedEventArgs e) {
        base.OnPointerReleased(e);

        Command?.Execute(Project);
    }

    protected override void OnPointerEntered(PointerRoutedEventArgs e) {
        base.OnPointerEntered(e);

        this.ProtectedCursor = InputSystemCursor.Create(InputSystemCursorShape.Hand);
    }

    protected override void OnPointerExited(PointerRoutedEventArgs e) {
        base.OnPointerExited(e);

        this.ProtectedCursor = InputSystemCursor.Create(InputSystemCursorShape.Arrow);
    }
}
