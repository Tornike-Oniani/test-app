using Microsoft.Win32;
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
    public partial class ImageSetDTO : ObservableObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ObservableCollection<ImageVariantDTO> Images { get; set; } = new ObservableCollection<ImageVariantDTO>();

        [RelayCommand]
        private void OnAddImage()
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Title = "Select an image",
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif"
            };

            if (ofd.ShowDialog() == true)
            {
                ImageVariantDTO image = new ImageVariantDTO()
                {
                    Name = Path.GetFileName(ofd.FileName),
                    Source = new BitmapImage(new Uri(ofd.FileName))
                };
                this.Images.Add(image);
            }
        }
    }
}
