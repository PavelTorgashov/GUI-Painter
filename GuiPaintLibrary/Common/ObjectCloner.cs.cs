using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace GuiPaintLibrary.Common
{
    /// <summary>
    /// Глубокое клонирование для [Serializable] объектов
    /// </summary>
    public static class ObjectCloner
    {
        public static T DeepClone<T>(this T obj) where T : class
        {
            if (obj == null)
                return null;

            var bf = new BinaryFormatter();
            using (var stream = new MemoryStream())
            {
                bf.Serialize(stream, obj);
                stream.Position = 0;
                return (T)bf.Deserialize(stream);
            }
        }
    }
}
