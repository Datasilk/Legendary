using System.IO;
using ImageSharp;

namespace Utility
{
    public struct structImage
    {
        public string path;
        public string filename;
        public int width;
        public int height;
        public Image<Rgba32> bitmap;
    }
        

    public class Images
    {
        Server Server { get; } = Server.Instance;
        public Images() {}

        
        public structImage Load(string path, string filename)
        {
            structImage newImg = new structImage();
            using (var fs = File.OpenRead(Server.MapPath(path + filename)))
            {
                var image = Image.Load(fs);
                newImg.bitmap = image;
                newImg.filename = filename;
                newImg.path = path;
                newImg.width = image.Width;
                newImg.height = image.Height;
            }
            return newImg;
        }
        
        public void Shrink(string filename, string outfile, int width)
        {
            using (var fs = File.OpenRead(Server.MapPath(filename)))
            {
                var image = Image.Load(fs);

                if (image.Width > width)
                {
                    image = image.Resize(new ImageSharp.Processing.ResizeOptions()
                    {
                        Size = new SixLabors.Primitives.Size(width, 0)
                    });
                }
                image.Save(outfile);
                fs.Dispose();
            }
        }
    }
}
