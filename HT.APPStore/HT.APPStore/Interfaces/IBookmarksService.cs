using HT.APPStore.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HT.APPStore.Interfaces
{
    public interface IBookmarksService
    {
        Task<ResponseMessage> GetBookmarksAsync(int pageIndex = 1, int pageSize = 20);
        Task<ResponseMessage> EditBookmarkAsync(Bookmarks bookmarks);
        Task<ResponseMessage> DeleteBookmarkAsync(int id);
    }
}
