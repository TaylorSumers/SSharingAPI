using Application.Common.Mappings;
using AutoMapper;

namespace Application.Queries.Strings.Get
{
    public class StringVm : IMapWith<Domain.String>
    {
        public string Value { get; set; }
        public bool DeleteAfterDownload { get; set; }
        public Guid Code { get; set; }
        public string GetUrl { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.String, StringVm>()
                .ForMember(stringVm => stringVm.Value, opt => opt.MapFrom(dbFile => dbFile.Value))
                .ForMember(stringVm => stringVm.DeleteAfterDownload, opt => opt.MapFrom(dbFile => dbFile.DeleteAfterDownload))
                .ForMember(stringVm => stringVm.Code, opt => opt.MapFrom(dbFile => dbFile.Code))
                .ForMember(stringVm => stringVm.GetUrl, opt => opt.MapFrom(dbFile => $"https://localhost:7146/api/Strings/Get?code={dbFile.Code}")); // TODO: рефактор получения ссылки
        }
    }
}
