using Newtonsoft.Json;
using PLFViewer.Common.Impl.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLFViewer.Common.Impl
{
    internal class JsonSerializationService : ISerializationService
    {
        public bool Serialize<T>(T data, string path)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));

            if (File.Exists(path) && File.GetAttributes(path).HasFlag(
                FileAttributes.ReadOnly & FileAttributes.System & FileAttributes.Hidden))
                throw new ArgumentException("File at specified path couldn't be rewrite.", nameof(path));


            return TrySerialize(data, path);
        }

        private bool TrySerialize(object data, string path)
        {
            try
            {
                var serialized = JsonConvert.SerializeObject(data);
                File.WriteAllText(path, serialized);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public T Deserialize<T>(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));

            if (!File.Exists(path))
                throw new ArgumentException("File not exists at specified path.", nameof(path));

            return TryDeserialize<T>(path);
        }

        private T TryDeserialize<T>(string path)
        {
            try
            {
                var deserialized = File.ReadAllText(path);
                return JsonConvert.DeserializeObject<T>(deserialized);
            }
            catch
            {
                return default(T);
            }
        }
    }
}
