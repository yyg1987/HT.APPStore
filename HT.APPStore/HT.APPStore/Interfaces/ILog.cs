using System;
using System.Collections.Generic;
using System.Text;

namespace HT.APPStore.Interfaces
{
    public interface ILog
    {
        void SendLog(string message);
        void SaveLog(string tag, Exception ex);

    }
}
