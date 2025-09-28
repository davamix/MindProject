using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindProject.Desktop.Dtos;

class SaveNewNoteDto {
    public int ProjectId { get; set; }
    public string Content { get; set; }
}
