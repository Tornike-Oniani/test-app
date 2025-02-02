using AutoMapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UiDesktopApp2.DataAccess.Entities;
using UiDesktopApp2.Models;

namespace UiDesktopApp2.Services
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Entity -> DTO
            CreateMap<Test, TestDTO>()
                .ForMember(dest => dest.ImageSets, opt => opt.MapFrom((src, dest, _, context) =>
                    new ObservableCollection<ImageSetDTO>(src.ImageSets != null
                        ? src.ImageSets.Select(x => context.Mapper.Map<ImageSetDTO>(x))
                        : Enumerable.Empty<ImageSetDTO>())));

            CreateMap<ImageSet, ImageSetDTO>()
                .ForMember(dest => dest.Images, opt => opt.MapFrom((src, dest, _, context) =>
                    new ObservableCollection<ImageVariantDTO>(src.ImageVariants != null
                        ? src.ImageVariants.Select(x => context.Mapper.Map<ImageVariantDTO>(x))
                        : Enumerable.Empty<ImageVariantDTO>())));

            CreateMap<ImageVariant, ImageVariantDTO>();

            // DTO -> Entity (for saving back to DB)
            CreateMap<TestDTO, Test>()
                .ForMember(dest => dest.ImageSets, opt => opt.MapFrom(src => src.ImageSets));

            CreateMap<ImageSetDTO, ImageSet>()
                .ForMember(dest => dest.ImageVariants, opt => opt.MapFrom(src => src.Images));

            CreateMap<ImageVariantDTO, ImageVariant>();
        }
    }
}
