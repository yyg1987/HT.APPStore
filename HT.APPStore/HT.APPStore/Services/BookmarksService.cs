﻿using HT.APPStore.Helpers;
using HT.APPStore.Helpers.Https;
using HT.APPStore.Interfaces;
using HT.APPStore.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HT.APPStore.Services
{
    public class BookmarksService : IBookmarksService
    {
        public BookmarksService()
        {
        }
        public async Task<ResponseMessage> GetBookmarksAsync(int pageIndex = 1, int pageSize = 20)
        {
            var url = string.Format(Apis.Bookmarks, pageIndex, pageSize);
            return await UserHttpClient.Current.GetAsyn(url);
        }
        public async Task<ResponseMessage> EditBookmarkAsync(Bookmarks bookmark)
        {
            var url = "";
            var parameters = new Dictionary<string, string>();
            parameters.Add("LinkUrl", bookmark.LinkUrl);
            parameters.Add("Title", bookmark.Title);
            parameters.Add("Summary", bookmark.Summary);
            parameters.Add("Tags", bookmark.TagsDisplay);
            parameters.Add("FromCNBlogs", bookmark.FromCNBlogs.ToString());

            if (bookmark.WzLinkId > 0)
            {
                url = string.Format(Apis.BookmarkEdit, bookmark.WzLinkId);
                parameters.Add("WzLinkId", bookmark.WzLinkId.ToString());
                parameters.Add("DateAdded", bookmark.DateAdded.ToString());
                return await UserHttpClient.Current.PatchAsync(url, new FormUrlEncodedContent(parameters));
            }
            else
            {
                url = string.Format(Apis.BookmarkAdd);
                return await UserHttpClient.Current.PostAsync(url, new FormUrlEncodedContent(parameters));
            }
        }
        public async Task<ResponseMessage> DeleteBookmarkAsync(int id)
        {
            var url = string.Format(Apis.BookmarkEdit, id);
            return await UserHttpClient.Current.DeleteAsync(url);
        }
    }
}
