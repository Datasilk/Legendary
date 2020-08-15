using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;

namespace Legendary.Controllers
{
    public class Upload : Controller
    {
        public override string Render(string body = "")
        {
            if(CheckSecurity() == false) { return Error404(); }
            if(Parameters.Files != null && Parameters.Files.Count > 0 && Parameters.ContainsKey("entryId"))
            {
                var entryId = int.Parse(Parameters["entryId"]);
                var folder = Server.MapPath("/Content/files/" + entryId + "/"); 
                var filenames = new List<Models.FileInfo>();
                
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                foreach (var file in Parameters.Files)
                {
                    var filename = file.Value.Filename.Replace(" ", "").Replace("-", "").Replace("_", "").ToLower();
                    var ext = filename.Split('.')[^1];
                    filename = Regex.Replace(filename.Replace("." + ext, ""), "[^0-9A-Za-z]+", "");
                    if(filename.Length > 16) { filename = filename.Substring(0, 16); }
                    var rnd = new Random();
                    var id = rnd.Next(1000, 9999);
                    var finalname =  filename + "_" + id + "." + ext;
                    var filetype = 1; //0 = unknown, 1 = image, 2 = document, 3 = zip file, 4 = video
                    switch (ext)
                    {
                        case "jpg": case "jpeg": case "png": case "gif":
                            break;
                        case "doc": case "docx": case "rtf": case "pdf": case "txt":
                        case "csv": case "xls": case "xlsx":
                            filetype = 2;
                            break;
                        case "zip": case "rar": case "7z":
                            filetype = 3;
                            break;
                        case "mp4": case "flv": case "ogg": case "avi": case "divx": case "xvid": case "mkv":
                            filetype = 4;
                            break;
                    }
                    
                    //save file to disk
                    using (var fw = new FileStream(folder + finalname, FileMode.OpenOrCreate))
                    {
                        file.Value.WriteTo(fw);
                    }

                    if(filetype == 1)
                    {
                        //create thumbnail image
                        if (!Directory.Exists(folder + "thumb\\"))
                        {
                            Directory.CreateDirectory(folder + "thumb\\");
                        }
                        Common.Utility.Image.Shrink(folder + finalname, folder + "thumb\\" + finalname, 1600);
                    }

                    //add file to result list
                    filenames.Add(new Models.FileInfo()
                    {
                        Path = "/file/" + entryId + "/",
                        Name = filename + "_" + id + "/" + ext,
                        Type = filetype
                    });
                }
                return JsonResponse(filenames);
            }
            
            return "[]";
        }
    }
}
