using Application.Common.Exceptions;
using Application.Interfaces;
using AspNetCore.Yandex.ObjectStorage;
using AspNetCore.Yandex.ObjectStorage.Object.Responses;
using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.Files.Get
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
            // Get from db
            var dbFile = await _dbContext.Files.FirstOrDefaultAsync(file => file.Code == request.Code, cancellationToken);
            if (dbFile is null)
            {
                throw new NotFoundException(nameof(Domain.File), request.Code);
            }

            
            // Get from cloud
            var cloudFileName = $"{dbFile.Code}{dbFile.Name.Substring(dbFile.Name.LastIndexOf('.'))}";
            var result = await _storageService.ObjectService.GetAsync(cloudFileName);
            var content = await result.ReadAsByteArrayAsync();
            if (!result.IsSuccessStatusCode)
            {
                throw new S3RequestException(content.Errors);
            }

            var fileVm = _mapper.Map<FileVm>(dbFile);
            fileVm.Content = content.Value;
            return fileVm;
        }
    }
}
