using Application.Common.Mappings;
using Application.Files.Commands.Upload;
using AutoMapper;

namespace SecretsSharingAPI.Models
{
    public class UploadFileDto : IMapWith<UploadStringCommand>
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string FileContent { get; set; }
        public bool DeleteAfterDownload { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UploadFileDto, UploadStringCommand>()
                .ForMember(command => command.UserId, opt => opt.MapFrom(dto => dto.UserId))
                .ForMember(command => command.Name, opt => opt.MapFrom(dto => dto.Name))
                .ForMember(command => command.FileContent, opt => opt.MapFrom(dto => Convert.FromBase64String(dto.FileContent)))
                .ForMember(command => command.DeleteAfterDownload, opt => opt.MapFrom(dto => dto.DeleteAfterDownload));
        }
    }
}
