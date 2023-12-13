using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspNetCore.Yandex.ObjectStorage;
using AspNetCore.Yandex.ObjectStorage.Object;
using AspNetCore.Yandex.ObjectStorage.Object.Responses;
using FluentResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using SecretsSharingAPI.Database;
using SecretsSharingAPI.Models;
using File = SecretsSharingAPI.Database.File;

namespace SecretsSharingAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {

        private static DataContext db = new DataContext();

        private readonly YandexStorageService _objectStoreService;

        private List<FileType> _fileTypes;

        public FilesController(YandexStorageService yandexStorageService)
        {
            _objectStoreService = yandexStorageService;
            _fileTypes = db.FileType.ToList();
        }


        // GET: api/Files/GetFilesList/userId={userId}
        /// <summary>
        /// Get a list of all user files
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="404">User has no uploaded files</response>
        [HttpGet("GetFilesList/userId={userId}")]
        [Produces(typeof(ResponseFile))]
        public IActionResult GetFiles(int userId)
        {
            var files = db.Files.Where(f => f.Uploader.ID == userId).ToList();
            if (files.Count() == 0)
                return NotFound("User has no uploaded files");

            return Ok(files.ConvertAll(f => new ResponseFile(f)));
        }


        // DELETE: api/Files/DeleteFile/code={code}
        /// <summary>
        /// Delete file by code
        /// </summary>
        /// <param name="code">Unique file code</param>
        /// <returns></returns>
        /// <response code="200">Successfully deleted</response>
        /// <response code="404">File not found</response>
        /// <response code="400">Cloud storage error</response>
        [HttpDelete("DeleteFile/code={code}")]
        public async Task<IActionResult> DeleteFile(string code)
        {
            var file = db.Files.Where(f => f.Code == code).FirstOrDefault();
            if (file == null)
                return NotFound("File not found");

            // Delete from cloud storage
            if(file.FileType.ID == 1)
            {
                var cloudFileName = $"{file.Code}{file.Name.Substring(file.Name.Contains('.') ? file.Name.LastIndexOf('.') : 0)}";
                var response = await _objectStoreService.ObjectService.DeleteAsync(cloudFileName);
                if (!response.IsSuccessStatusCode)
                    return BadRequest(response);
            }
            
            // Remove from db
            db.Files.Remove(file);
            db.SaveChanges();

            return Ok("Deleted");
        }


        // GET: api/Files/GetString/code={code}
        /// <summary>
        /// Get string by code
        /// </summary>
        /// <remarks>
        /// Method returns only string content
        /// </remarks>
        /// <param name="code">Unique file code</param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="404">String not found</response>
        [HttpGet("GetString/code={code}")]
        [Produces(typeof(string))]
        public IActionResult GetString(string code)
        {
            // Search string
            var file = db.Files.Where(f => f.Code == code && f.FileType.ID == 2).FirstOrDefault();
            if (file == null)
                return NotFound("String not found");

            // Remove from database if DeleteAfterDownload flag set
            if (file.DeleteAfterDownload)
            {
                db.Files.Remove(file);
                db.SaveChanges();
            }

            return Ok(file.Name);
        }


        // GET: api/Files/GetFile/code={code}
        /// <summary>
        /// Get file by code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Cloud error</response>
        /// <response code="404">File not found</response>
        [HttpGet("GetFile/code={code}")]
        public async Task<IActionResult> GetFile(string code)
        {
            // Search file
            var file = db.Files.Where(f => f.Code == code && f.FileType.ID == 1).FirstOrDefault();
            if (file == null)
                return NotFound("File not found");

            // Get from cloud
            var cloudFileName = $"{file.Code}{file.Name.Substring(file.Name.Contains('.') ? file.Name.LastIndexOf('.') : 0)}";
            var result = await _objectStoreService.ObjectService.GetAsync(cloudFileName);
            byte[] byteArr;
            if (result.IsSuccessStatusCode)
                byteArr = (await result.ReadAsByteArrayAsync()).Value;
            else
                return BadRequest(result); // Cloud fetch error

            // Delete file from cloud and remove from database if DeleteAfterDownload flag set
            if (file.DeleteAfterDownload)
            {
                var response = await _objectStoreService.ObjectService.DeleteAsync(cloudFileName);
                if (!response.IsSuccessStatusCode)
                    return BadRequest(response); // Cloud delete error

                db.Files.Remove(file);
                db.SaveChanges();
            }

            return Ok(new { fileName = file.Name, bytes = byteArr });
        }


        // POST: api/Files/PostString
        /// <summary>
        /// Upload string
        /// </summary>
        /// <remarks>
        /// Method adds new string to database and returns it's unique code
        /// </remarks>
        /// <param name="uploadStr"></param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">String is not valid</response>
        [HttpPost("PostString")]
        [Produces(typeof(string))]
        public IActionResult PostString(RequiredString uploadStr)
        {
            // Find uploader
            var uploader = db.User.Find(uploadStr.Uploader);

            // Validate string
            if (uploader == null)
                ModelState.AddModelError("Upoader", "Uploader not exists in database");
            if (string.IsNullOrWhiteSpace(uploadStr.Content))
                ModelState.AddModelError("Content", "Content cannot be empty");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Add new string to database
            var fileCode = GenerateFileCode();
            var fileType = db.FileType.Find(2);
            var newFile = new File()
            {
                Name = uploadStr.Content,
                Code = fileCode,
                Uploader = uploader,
                FileType = fileType,
                DeleteAfterDownload = uploadStr.DeleteAfterDownload
            };

            db.Files.Add(newFile);
            db.SaveChanges();

            return Ok(newFile.Code);
        }


        // POST: api/Files/PostFile
        /// <summary>
        /// Upload file
        /// </summary>
        /// <remarks>
        /// Method adds info about new file (like name and uploader),
        /// send it to Yandex Object Storage and returns it's unique code
        /// </remarks>
        /// <param name="uploadFile"></param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="404">File is not valid or cloud error</response>
        [HttpPost("PostFile")]
        [Produces(typeof(string))]
        public async Task<IActionResult> PostFile([FromForm]RequiredFile uploadFile)
        {
            // Find uploader
            var uploader = db.User.Find(uploadFile.Uploader);
            
            // Validate file
            if (uploadFile == null || uploadFile.FileToUpload == null || uploadFile.FileToUpload.Length == 0)
                ModelState.AddModelError("File", "File not selected");
            if (uploader == null)
                ModelState.AddModelError("Upoader", "Uploader not exists in database");
            if(!ModelState.IsValid) 
                return BadRequest(ModelState);

            var fileName = uploadFile.FileToUpload.FileName;
            if(db.Files.Where(f => f.Uploader == uploader && f.Name == fileName).Count() > 0)
            {
                ModelState.AddModelError("File", "File with the same name already exists");
                return BadRequest(ModelState);
            }

            // Add file info to database
            var fileCode = GenerateFileCode();
            var fileType = db.FileType.Find(1);
            var newFile = new File()
            {
                Name = fileName,
                Code = fileCode,
                Uploader = uploader,
                DeleteAfterDownload = uploadFile.DeleteAfterDownload,
                FileType = fileType
            };
            db.Files.Add(newFile);
            db.SaveChanges();

            //Send to cloud
            S3ObjectPutResponse response;
            using (var stream = new MemoryStream())
            {
                uploadFile.FileToUpload.CopyTo(stream);
                response = await _objectStoreService.ObjectService.PutAsync(stream, $"{fileCode}{fileName.Substring(fileName.Contains('.') ? fileName.LastIndexOf('.') : 0)}");
            } // File name in cloud formed as "generated_file_code" + ".extension"

            if (response.IsSuccessStatusCode)
                return Ok(newFile.Code);
            else
                return BadRequest(response);
        }

        /// <summary>
        /// This method generate unique 9-character file code
        /// which will be used in download URL
        /// </summary>
        /// <returns>Generated code/returns>
        public static string GenerateFileCode()
        {
            var code = new StringBuilder(9);
            var uppercaseLetters = Enumerable.Range('A', 'Z' - 'A' + 1).Select(c => (char)c).ToArray();
            var lowercaseLetters = Enumerable.Range('a', 'z' - 'a' + 1).Select(c => (char)c).ToArray();
            var rnd = new Random();
            
            while (true)
            {
                for (int i = 0; i < 3; i++)
                {
                    code.Append(uppercaseLetters[rnd.Next(0, uppercaseLetters.Length - 1)]);
                    code.Append(lowercaseLetters[rnd.Next(0, lowercaseLetters.Length - 1)]);
                    code.Append($"{rnd.Next(0, 10)}");
                }

                if (db.Files.Where(f => f.Code == code.ToString()).Count() == 0)
                    break;
                else
                    code = new StringBuilder(9);
            }

            return code.ToString();
        }
    }
}
