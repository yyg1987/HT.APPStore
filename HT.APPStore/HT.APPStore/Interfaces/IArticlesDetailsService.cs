using HT.APPStore.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HT.APPStore.Interfaces
{
    public interface IArticlesDetailsService
    {
        Task<ResponseMessage> GetArticlesAsync(int id);
        Task<ResponseMessage> GetCommentAsync(string blogApp, int id, int pageIndex = 1, int pageSize = 20);
        Task<ResponseMessage> PostCommentAsync(string blogApp, int id, string content);
    }
}
