using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

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
                    filename = filename.Replace("." + ext, "");
                    if(filename.Length > 16) { filename = filename.Substring(0, 16); }
                    var rnd = new Random();
                    var id = rnd.Next(1000, 9999);
                    var finalname =  filename + "_" + id + "." + ext;
                    var filetype = 1; //0 = unknown, 1 = image, 2 = document, 3 = zip file, 4 = video
                    
                    //save file to disk
                    using (var fw = new FileStream(folder + finalname, FileMode.OpenOrCreate))
                    {
                        file.Value.WriteTo(fw);
                    }

                    //add file to result list
                    filenames.Add(new Models.FileInfo()
                    {
                        Path = "/file/" + entryId + "/",
                        Name = finalname,
                        Type = filetype
                    });
                }
                return JsonResponse(filenames);
            }
            
            return "[]";
        }
    }
}
