using HT.APPStore.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HT.APPStore.Interfaces
{
    public interface ISearchService
    {
        Task<ResponseMessage> GetSearchAsync(int position, string keyWords, int pageIndex = 1, int pageSize = 20);
    }
}
