using Application.Common.Mappings;
using AutoMapper;

namespace Application.Queries.Strings.GetList
{
    public class StringLookupDto : IMapWith<Domain.String>
    {
        public string Value { get; set; }
        public Guid Code { get; set; }
        public bool DeleteAfterDownload { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.String, StringLookupDto>()
                .ForMember(dto => dto.Value, opt => opt.MapFrom(dbFile => dbFile.Value))
                .ForMember(dto => dto.Code, opt => opt.MapFrom(dbFile => dbFile.Code))
                .ForMember(dto => dto.DeleteAfterDownload, opt => opt.MapFrom(dbFile => dbFile.DeleteAfterDownload));
        }
    }
}
