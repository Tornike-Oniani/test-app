﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace UiDesktopApp2.Models
{
    public class ImageSetDTO : ObservableObject
    {
        public int Id { get; set; }
        public int TestId { get; set; }
        public string? Name { get; set; }
        public bool IsKnown { get; set; }
        public ObservableCollection<ImageVariantDTO> Images { get; set; } = new ObservableCollection<ImageVariantDTO>();
    }
}
