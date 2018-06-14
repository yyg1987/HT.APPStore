using HT.APPStore.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HT.APPStore.Interfaces
{
    public interface INewsDetailsService
    {
        Task<ResponseMessage> GetNewsAsync(int id);
        Task<ResponseMessage> GetCommentAsync(int id, int pageIndex, int pageSize);
        Task<ResponseMessage> PostCommentAsync(int id, string content, bool hasEdit = false);
        Task<ResponseMessage> DeleteCommentAsync(int id);
    }
}
