using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UiDesktopApp2.DataAccess.Entities
{
    public class ImageVariant
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Source { get; set; }
        public int ImageSetId { get; set; }
        public ImageSet ImageSet { get; set; }
    }
}
