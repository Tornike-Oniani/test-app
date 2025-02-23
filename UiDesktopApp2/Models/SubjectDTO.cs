using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UiDesktopApp2.Models
{
    public class SubjectDTO
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public List<ResultDTO>? Results { get; set; }
    }
}
