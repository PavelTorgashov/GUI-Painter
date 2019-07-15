using System;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;

namespace GridTableBuilder.GridModel
{
    public static class SaverLoader
    {
        public static void SaveToFile(string fileName, Grid grid)
        {
            using (var fs = File.Create(fileName))
            using (var zip = new GZipStream(fs, CompressionMode.Compress))
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(zip, new VersionInfo());
                formatter.Serialize(zip, grid);
            }
        }

        public static Grid LoadFromFile(string fileName)
        {
            using (var fs = File.OpenRead(fileName))
            using (var zip = new GZipStream(fs, CompressionMode.Decompress))
            {
                var formatter = new BinaryFormatter();
                var versionInfo = (VersionInfo)formatter.Deserialize(zip);
                if (versionInfo.Version > VersionInfo.DEFAULT_VERSION)
                    throw new Exception(@"Downloadable file format not supported.");
                var grid = (Grid)formatter.Deserialize(zip);
                return grid;
            }
        }

        public static Image LoadTranslucentFromFile(string fileName)
        {
            var original = (Bitmap)Image.FromFile(fileName);
            return GraphicsHelper.Translucent(original);
        }

    }

}
