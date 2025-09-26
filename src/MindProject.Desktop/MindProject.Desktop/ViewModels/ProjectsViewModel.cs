using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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

    public ObservableCollection<Project> Projects { get; set; } = new();
    public ObservableCollection<Note> Notes { get; set; } = new();


    public ProjectsViewModel(IMindProjectService mindProjectService) {
        _mindProjectService = mindProjectService;

        LoadProjects();
    }

    private async void LoadProjects() {
        var projects = await _mindProjectService.GetProjects();
        
        foreach (var project in projects) {
            Projects.Add(project);
        }
    }

    [RelayCommand]
    private async Task LoadProjectDetails(Project project) {
        var details = await _mindProjectService.GetProject(project.Id);

        foreach(var n in details.Notes) {
            Notes.Add(n);
        }
    }
}
