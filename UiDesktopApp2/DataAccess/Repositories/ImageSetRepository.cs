using AutoMapper;
using UiDesktopApp2.DataAccess.Entities;
using UiDesktopApp2.Models;

namespace UiDesktopApp2.DataAccess.Repositories
{
    public class ImageSetRepository(ApplicationDbContext context, IMapper mapper)
    {
        public async Task<int> CreateImageSet(ImageSetDTO imageSetDto)
        {
            var result = mapper.Map<ImageSet>(imageSetDto);
            context.ImageSets.Add(result);
            await context.SaveChangesAsync();
            return result.Id;
        }

        public async Task UpdateImageSet(ImageSetDTO imageSetDto)
        {
            var existingImageSet = await context.ImageSets.FindAsync(imageSetDto.Id);

            var imageSet = mapper.Map(imageSetDto, existingImageSet);
            await context.SaveChangesAsync();
        }
    }
}
