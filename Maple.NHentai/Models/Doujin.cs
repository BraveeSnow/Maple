using System;
using System.Collections.Generic;

namespace Maple.NHentai.Models
{
    public class Doujin
    {
        internal int Id { get; set; }
        internal string Url { get; set; }
        internal string Title { get; set; }
        internal string NativeTitle { get; set; }
        internal string CoverArtUrl { get; set; }
        internal int Pages { get; set; }
        internal string Language { get; set; }
        internal readonly List<string> Tags = new();
        internal readonly List<string> Characters = new();
        internal readonly List<string> Parodies = new();
        internal DateTime PublishDate { get; set; }

        /* Accessors */
        public int GetId() => Id;
        public string GetUrl() => Url;
        public string GetTitle() => Title;
        public string GetNativeTitle() => NativeTitle;
        public string GetCoverArtUrl() => CoverArtUrl;
        public int GetPageCount() => Pages;
        public string GetLanguage() => Language;
        public IEnumerable<string> GetTagList() => Tags;
        public IEnumerable<string> GetCharacters() => Characters;
        public IEnumerable<string> GetParodies() => Parodies;
        public DateTime GetDatePublished() => PublishDate;
    }
}