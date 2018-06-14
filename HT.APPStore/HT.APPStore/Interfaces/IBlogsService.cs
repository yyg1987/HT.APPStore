using HT.APPStore.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HT.APPStore.Interfaces
{
    public interface IBlogsService
    {
        Task<ResponseMessage> GetArticlesAsync(string blogApp, int pageIndex = 1, int pageSize = 20);
    }
}
