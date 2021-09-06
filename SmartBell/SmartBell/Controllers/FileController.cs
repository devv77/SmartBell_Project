using Logic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SmartBell.Controllers
{
    [ApiController]
    [Route("File")]
    public class FileController : ControllerBase
    {
        public static readonly string[] fileExceptions = { "default.mp3" };
        ReadLogic readlogic;

        public FileController(ReadLogic readlogic)
        {
            this.readlogic = readlogic;
        }

        [HttpPost,DisableRequestSizeLimit]
        public IActionResult Upload(IFormFile FileToUpload)
        {
            try
            {
                var folderName = "Output";
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (FileToUpload != null && FileToUpload.Length > 0)
                {
                    string filename = FileToUpload.FileName.ToLower();
                    var fullpath = Path.Combine(pathToSave, filename);

                    using (var stream = new FileStream(fullpath, FileMode.Create))
                    {
                        FileToUpload.CopyTo(stream);
                    }
                    return Ok();
                }
                return BadRequest();
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Internal server error : {ex}");
            }
        }

        [HttpGet("{filename}")]
        public FileResult Download(string filename)
        {
            var folder = Path.Combine(Directory.GetCurrentDirectory(), "Output");
            var path = Path.Combine(folder, filename);
            byte[] allbytes = System.IO.File.ReadAllBytes(path);
            return File(allbytes, "application/octet-stream",filename);
        }

        [HttpPost("PostTTSString/{content} & {filename}")]
        public IActionResult PostTTSFileFromString(string content, string filename)
        {
            try
            {
                filename = filename.ToLower();
                if (filename.Split('.').Last() != "txt")
                {
                    filename = filename.Split('.').First() + ".txt";
                }
                var folderName = "Output";
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);


                var fullpath = Path.Combine(pathToSave, filename);
                if (!System.IO.File.Exists(fullpath))
                {
                    System.IO.File.WriteAllText(fullpath, content);
                    return Ok();
                }
                return BadRequest("File already exists.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpDelete("DeleteFile/{filename}")]
        public IActionResult DeleteFile(string filename)
        {
            try
            {
                int occurrence = readlogic.FileOccurrence(filename);
                if (occurrence != 0)
                {
                    throw new Exception($"This file's occurrence is {occurrence}.");
                }
                var folderName = "Output";
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);


                var fullpath = Path.Combine(pathToSave, filename);
                if (System.IO.File.Exists(fullpath))
                {
                    if (fileExceptions.Contains(filename))
                    {
                        return BadRequest("This file cannot be deleted.");
                    }
                    System.IO.File.Delete(fullpath);
                    return Ok();
                }
                return BadRequest("File does not exists.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
        [HttpGet("GetFileUsage/{filename}")]
        public IActionResult FileIsUsed(string filename)
        {
            try
            {
                return Ok(readlogic.FileIsUsed(filename));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
            
        }
        [HttpGet("GetAllFiles")]
        public IActionResult GetAllFiles()
        {
            try
            {
                var folderName = "Output";
                var outputFolder = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                string[] files = Directory.GetFiles(outputFolder);
                for (int i = 0; i < files.Length; i++)
                {
                    string[] split = files[i].Split(@"\");
                    files[i] = split.Last();
                }
                return Ok(files);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
        [HttpGet("GetAllTTSFiles")]
        public IActionResult GetAllTTSFiles()
        {
            try
            {
                var folderName = "Output";
                var outputFolder = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                string[] files = Directory.GetFiles(outputFolder).Where(x => (x.Split('.').Last() == "txt")).ToArray();
                for (int i = 0; i < files.Length; i++)
                {
                    string[] split = files[i].Split(@"\");
                    files[i] = split.Last();
                }
                return Ok(files);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
        [HttpGet("GetAllMp3Files")]
        public IActionResult GetAllMp3Files()
        {
            try
            {
                var folderName = "Output";
                var outputFolder = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                string[] files = Directory.GetFiles(outputFolder).Where(x => (x.Split('.').Last() == "mp3")).ToArray();
                for (int i = 0; i < files.Length; i++)
                {
                    string[] split = files[i].Split(@"\");
                    files[i] = split.Last();
                }
                return Ok(files);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
        [HttpGet("GetFileExceptions")]
        public IActionResult GetFileExceptions()
        {
            return Ok(fileExceptions);
        }
    }
}
