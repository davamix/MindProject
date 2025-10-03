﻿using System;

namespace MindProject.Desktop.Dtos;

public class UpdateProjectDto {
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string RepoAddress { get; set; }
}
