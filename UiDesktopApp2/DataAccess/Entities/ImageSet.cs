﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UiDesktopApp2.DataAccess.Entities
{
    public class ImageSet
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public bool IsUknown { get; set; }
        public int Number { get; set; }
        public int TestId { get; set; }
        public Test Test { get; set; }
        public List<ImageVariant> ImageVariants { get; set; }
    }
}
