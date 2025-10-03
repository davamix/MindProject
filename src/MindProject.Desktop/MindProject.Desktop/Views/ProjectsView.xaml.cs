using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using MindProject.Desktop.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MindProject.Desktop.Views;
public sealed partial class ProjectsView : UserControl {
    public ProjectsView() {
        InitializeComponent();

        this.DataContext = Ioc.Default.GetService<ProjectsViewModel>();
    }

    private void btn_PointerEntered(object sender, PointerRoutedEventArgs e) {
        this.ProtectedCursor = InputSystemCursor.Create(InputSystemCursorShape.Hand);
    }

    private void btn_PointerExited(object sender, PointerRoutedEventArgs e) {
        this.ProtectedCursor = InputSystemCursor.Create(InputSystemCursorShape.Arrow);
    }

    private void btnAddNote_Click(object sender, RoutedEventArgs e) {
        noteInputControl.Visibility = Visibility.Visible;
    }

    private void btnAddProject_Click(object sender, RoutedEventArgs e) {
        projectInputControl.Visibility = Visibility.Visible;
    }
}
