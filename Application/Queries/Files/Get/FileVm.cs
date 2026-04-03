using Application.Common.Mappings;
using AutoMapper;
using static System.Net.WebRequestMethods;
using DbFile = Domain.File;

namespace Application.Queries.Files.Get
{
    public class FileVm : IMapWith<DbFile>
    {
        public string Name { get; set; }
        public bool DeleteAfterDownload { get; set; }
        public Guid Code { get; set; }
        public string GetUrl { get; set; }
        public byte[]? Content { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<DbFile, FileVm>()
                .ForMember(fileVm => fileVm.Name, opt => opt.MapFrom(dbFile => dbFile.Name))
                .ForMember(fileVm => fileVm.DeleteAfterDownload, opt => opt.MapFrom(dbFile => dbFile.DeleteAfterDownload))
                .ForMember(fileVm => fileVm.Code, opt => opt.MapFrom(dbFile => dbFile.Code))
                .ForMember(fileVm => fileVm.GetUrl, opt => opt.MapFrom(dbFile => $"https://localhost:7146/api/Files/GetFile/{dbFile.Code}")); // TODO: убрать хардкод
        }
    }
}
