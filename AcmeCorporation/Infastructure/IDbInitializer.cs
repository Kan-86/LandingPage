using AcmeCorporation.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcmeCorporation.Infastructure
{
    public interface IDbInitializer
    {
        void Initialize(DrawContext context);
    }
}
