using Application.Interfaces;
using AspNetCore.Yandex.ObjectStorage;
using AspNetCore.Yandex.ObjectStorage.Object.Responses;
using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Files.Queries.GetFile
{
    public class GetFileQueryHandler : IRequestHandler<GetFileQuery, FileVm>
    {
        private readonly ISecretsDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly YandexStorageService _storageService; // TODO: заменить на интерфейс

        public GetFileQueryHandler(ISecretsDbContext dbContext, IMapper mapper, YandexStorageService storageService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _storageService = storageService;
        }

        public async Task<FileVm> Handle(GetFileQuery request, CancellationToken cancellationToken) 
        {
            var dbFile = await _dbContext.Files.FirstOrDefaultAsync(file => file.Code == request.Code, cancellationToken);
            var fileName = $"{dbFile.Code.ToString().ToUpperInvariant()}{dbFile.Name.Substring(dbFile.Name.LastIndexOf('.'))}";
            S3ObjectGetResponse result = await _storageService.ObjectService.GetAsync(fileName);
            var fileContent = await result.ReadAsByteArrayAsync();

            FileVm fileVm = _mapper.Map<FileVm>(dbFile);
            fileVm.Content = fileContent.Value;

            return fileVm;
        }
    }
}
