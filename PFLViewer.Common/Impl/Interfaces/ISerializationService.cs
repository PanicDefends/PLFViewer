﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLFViewer.Common.Impl.Interfaces
{
    public interface ISerializationService
    {
        bool Serialize<T>(T data, string path);
        T Deserialize<T>(string path);
    }
}
