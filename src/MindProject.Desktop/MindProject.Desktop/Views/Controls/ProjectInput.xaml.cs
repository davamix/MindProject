using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using MindProject.Desktop.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MindProject.Desktop.Views.Controls;
public sealed partial class ProjectInput : UserControl {
    public static readonly DependencyProperty SaveProjectProperty =
        DependencyProperty.RegisterAttached(
            "SaveProject",
            typeof(RelayCommand<Project>),
            typeof(ProjectInput),
            new PropertyMetadata(null));

    public AsyncRelayCommand<Project> SaveProject {
        get => (AsyncRelayCommand<Project>)GetValue(SaveProjectProperty);
        set => SetValue(SaveProjectProperty, value);
    }

    public Project Project { get; set; } = new();

    public ProjectInput() {
        InitializeComponent();
    }

    private void btnSave_Click(object sender, RoutedEventArgs e) {
        this.Visibility = Visibility.Collapsed;

        SaveProject?.Execute(Project);
    }

    private void btnClose_Click(object sender, RoutedEventArgs e) {
        this.Visibility = Visibility.Collapsed;

        txtProjectName.Text = string.Empty;
        txtProjectDescription.Text = string.Empty;
        txtProjectRepo.Text = string.Empty;
    }

    private void Button_PointerEntered(object sender, PointerRoutedEventArgs e) {
        this.ProtectedCursor = InputSystemCursor.Create(InputSystemCursorShape.Hand);
    }

    private void Button_PointerExited(object sender, PointerRoutedEventArgs e) {
        this.ProtectedCursor = InputSystemCursor.Create(InputSystemCursorShape.Arrow);
    }
}
