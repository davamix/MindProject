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
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MindProject.Desktop.Views.Controls;
public sealed partial class NoteItem : UserControl {
    public static readonly DependencyProperty NoteProperty =
        DependencyProperty.RegisterAttached(
            "Note",
            typeof(Note),
            typeof(NoteItem),
            new PropertyMetadata(null));

    public static readonly DependencyProperty DeleteNoteProperty =
        DependencyProperty.RegisterAttached(
            "DeleteNote",
            typeof(RelayCommand<Note>),
            typeof(NoteItem),
            new PropertyMetadata(null));

    public static readonly DependencyProperty SaveNoteProperty =
        DependencyProperty.RegisterAttached(
            "SaveNote",
            typeof(RelayCommand<Note>),
            typeof(NoteItem),
            new PropertyMetadata(null));

    public Note Note {
        get => (Note)GetValue(NoteProperty);
        set => SetValue(NoteProperty, value);
    }

    public AsyncRelayCommand<Note> DeleteNote {
        get => (AsyncRelayCommand<Note>)GetValue(DeleteNoteProperty);
        set => SetValue(DeleteNoteProperty, value);
    }

    public AsyncRelayCommand<Note> SaveNote {
        get => (AsyncRelayCommand<Note>)GetValue(SaveNoteProperty);
        set => SetValue(SaveNoteProperty, value);
    }

    public NoteItem() {
        InitializeComponent();
    }

    private void Button_PointerEntered(object sender, PointerRoutedEventArgs e) {
        this.ProtectedCursor = InputSystemCursor.Create(InputSystemCursorShape.Hand);
    }

    private void Button_PointerExited(object sender, PointerRoutedEventArgs e) {
        this.ProtectedCursor = InputSystemCursor.Create(InputSystemCursorShape.Arrow);
    }

    private void btnEdit_Click(object sender, RoutedEventArgs e) {
        btnEdit.Visibility = Visibility.Collapsed;
        btnSave.Visibility = Visibility.Visible;

        txtNoteContent.Visibility = Visibility.Visible;
        mdTextBlock.Visibility = Visibility.Collapsed;
    }

    private void btnSave_Click(object sender, RoutedEventArgs e) {
        btnSave.Visibility = Visibility.Collapsed;
        btnEdit.Visibility = Visibility.Visible;

        txtNoteContent.Visibility = Visibility.Collapsed;
        mdTextBlock.Visibility = Visibility.Visible;
    }
}
