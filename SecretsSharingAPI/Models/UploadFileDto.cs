using Application.Common.Mappings;
using Application.Files.Commands.Upload;
using AutoMapper;

namespace SecretsSharingAPI.Models
{
    public class UploadFileDto : IMapWith<UploadFileCommand>
    {
        public string Name { get; set; }
        public byte[] FileContent { get; set; }
        public bool DeleteAfterDownload { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UploadFileDto, UploadFileCommand>()
                .ForMember(createCommand => createCommand.Name, opt => opt.MapFrom(createDto => createDto.Name))
                .ForMember(createCommand => createCommand.FileContent, opt => opt.MapFrom(createDto => createDto.FileContent));
        }
    }
}
