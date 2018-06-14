using HT.APPStore.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HT.APPStore.Interfaces
{
    public interface IKbArticlesService
    {
        Task<ResponseMessage> GetKbArticlesAsync(int pageIndex = 1);
    }
}
