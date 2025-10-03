using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MindProject.Desktop.Dtos;
using MindProject.Desktop.Models;
using MindProject.Desktop.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindProject.Desktop.ViewModels;
public partial class ProjectsViewModel : ObservableObject {
    private readonly IMindProjectService _mindProjectService;

    [ObservableProperty]
    private Project _selectedProject;

    public ObservableCollection<Project> Projects { get; set; } = new();
    public ObservableCollection<Note> Notes { get; set; } = new();


    public ProjectsViewModel(IMindProjectService mindProjectService) {
        _mindProjectService = mindProjectService;

        LoadProjects();
    }

    private async void LoadProjects() {
        try {
            var projects = await _mindProjectService.GetProjects();

            foreach (var project in projects) {
                Projects.Add(project);
            }
        } catch (Exception ex) {
            Debug.WriteLine($"Error loading projects: {ex.Message}");
        }
    }

    [RelayCommand]
    private async Task LoadProjectDetails(Project project) {
        SelectedProject = project;

        var details = await _mindProjectService.GetProject(project.Id);

        Notes.Clear();

        foreach (var n in details.Notes) {
            Notes.Add(n);
        }
    }

    [RelayCommand]
    private async Task SaveProject(Project project) {
        try {
            var savedProject = await _mindProjectService.SaveProject(project);

            if (project.Id == Guid.Empty) {
                Projects.Add(savedProject);
            } else {
                UpdateProject(savedProject);
            }
        } catch (Exception e) {
            Debug.WriteLine($"Error saving project: {e.Message}");
        }
    }

    [RelayCommand]
    private async Task SaveNote(Note note) {
        if (SelectedProject == null) return;

        try {
            var savedNote = await _mindProjectService.SaveNote(note, SelectedProject.Id);

            //await LoadProjectDetails(SelectedProject);
            if (note.Id == Guid.Empty) {
                Notes.Add(savedNote);
            } else {
                UpdateNote(savedNote);
            }

        } catch (Exception ex) {
            Debug.WriteLine($"Error saving note: {ex.Message}");
        }
    }

    [RelayCommand]
    private async Task DeleteNote(Note note) {
        try {
            await _mindProjectService.DeleteNote(note.Id);

            Notes.Remove(note);
        } catch (Exception ex) {
            Debug.WriteLine($"Error deleting note: {ex.Message}");
        }

    }

    [RelayCommand]
    private async Task CreateNote() {
        if (_selectedProject == null) return;

    }

    private void UpdateNote(Note note) {
        var oldNote = Notes.First(x => x.Id == note.Id);
        var index = Notes.IndexOf(oldNote);
        Notes.Remove(oldNote);
        Notes.Insert(index, note);
    }

    private void UpdateProject(Project project) {
        var oldProject = Projects.First(x => x.Id == project.Id);
        var index = Projects.IndexOf(oldProject);
        Projects.Remove(oldProject);
        Projects.Insert(index, project);
    }
}
