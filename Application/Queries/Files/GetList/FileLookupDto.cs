using Application.Common.Mappings;
using AutoMapper;

namespace Application.Queries.Files.GetList
{
    public class FileLookupDto : IMapWith<Domain.File>
    {
        public string Name { get; set; }
        public Guid Code { get; set; }
        public bool DeleteAfterDownload { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.File, FileLookupDto>()
                .ForMember(dto => dto.Name, opt => opt.MapFrom(dbFile => dbFile.Name))
                .ForMember(dto => dto.Code, opt => opt.MapFrom(dbFile => dbFile.Code))
                .ForMember(dto => dto.DeleteAfterDownload, opt => opt.MapFrom(dbFile => dbFile.DeleteAfterDownload));
        }
    }
}
