using HT.APPStore.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HT.APPStore.Interfaces
{
    public interface IArticlesService
    {
        Task<ResponseMessage> GetArticlesAsync(int position, int pageIndex = 1, int pageSize = 20);
    }
}
