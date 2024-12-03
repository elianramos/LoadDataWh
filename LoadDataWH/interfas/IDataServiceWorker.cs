using LoadDataWh.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadDataWh.interfas

    {
    public interface IDataServicesWorker
    {
        Task<OperactionResult> LoadDwh();
    }
}

