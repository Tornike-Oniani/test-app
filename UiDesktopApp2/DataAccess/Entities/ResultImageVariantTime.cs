using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UiDesktopApp2.DataAccess.Entities
{
    public class ResultImageVariantTime
    {
        [Key]
        public int Id { get; set; }
        public int ImageVariantId { get; set; }
        public int Seconds { get; set; }
        public int ResultId { get; set; }
        public Result Result { get; set; }
    }
}
