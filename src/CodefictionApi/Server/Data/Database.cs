﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Codefiction.CodefictionTech.CodefictionApi.Server.Data
{
    public class Database
    {
        public Podcast[] Podcasts { get; set; }
    }

    public class Podcast
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string SoundcloudUrl { get; set; }
        public string YoutubeUrl { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public string[] Attendees { get; set; }
        public string[] Tags { get; set; }
        public Relation[] Relations { get; set; }
        public DateTime PublishDate { get; set; }
    }

    public class Relation
    {
        public string Type { get; set; }
        public int Id { get; set; }
    }
}