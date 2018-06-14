﻿using HT.APPStore.Helpers;
using Humanizer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HT.APPStore.Models
{
    public class KbArticles
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Author { get; set; }
        public DateTime DateAdded { get; set; }
        public int ViewCount { get; set; }
        public int DiggCount { get; set; }
        public string Body { get; set; }
        [JsonIgnore]
        public string DateDisplay { get { return DateAdded.ToUniversalTime().Humanize(); } }
        [JsonIgnore]
        public string DiggValue
        {
            get
            {
                return DiggCount + " 推荐 · " + ViewCount + " 阅读";
            }
        }
        [JsonIgnore]
        public string BodyDisplay
        {
            get
            {
                return HtmlTemplate.ReplaceHtml(Body);
            }
        }
    }
}
