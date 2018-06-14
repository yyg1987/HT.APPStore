using System;
using System.Collections.Generic;
using System.Text;

namespace HT.APPStore.Interfaces
{
    public interface IShares
    {
        void Shares(string url, string title);
        void SharesIcon(string url, string title, object icon);
    }
}
