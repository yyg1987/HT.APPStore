using HT.APPStore.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HT.APPStore.Interfaces
{
    public interface IStatusesCommentService
    {
        Task<ResponseMessage> GetCommentsAsync(int id);
        Task<ResponseMessage> PostCommentAsync(int id, string content);
        Task<ResponseMessage> DeleteCommentAsync(int statusId, int id);
    }
}
