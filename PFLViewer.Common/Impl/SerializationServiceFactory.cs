using PLFViewer.Common.Impl.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLFViewer.Common.Impl
{
    public class SerializationServiceFactory
    {
        public static ISerializationService Instance => new JsonSerializationService();
    }
}
