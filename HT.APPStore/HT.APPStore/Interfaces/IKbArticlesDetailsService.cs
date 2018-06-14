using HT.APPStore.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HT.APPStore.Interfaces
{
    public interface IKbArticlesDetailsService
    {
        Task<ResponseMessage> GetKbArticlesAsync(int id);
    }
}
