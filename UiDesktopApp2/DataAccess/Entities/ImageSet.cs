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
        public int TestId { get; set; }
        public Test Test { get; set; }
    }
}
