﻿using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Utility.Serialization;

namespace Legendary.Services
{
    public class Chapters : Service
    {
        public Chapters(HttpContext context) : base(context)
        {
        }

        public struct chapter
        {
            public string title;
            public int num;
        }

        public string GetList(int bookId)
        {
            if (!CheckSecurity()) { return AccessDenied(); }
            var list = new List<chapter>();
            var query = new Query.Chapters();
            query.GetList(bookId).ForEach((Query.Models.Chapter chap) =>
            {
                list.Add(new chapter()
                {
                    num = chap.chapter,
                    title = chap.title
                });
            });
            return Serializer.WriteObjectToString(list);
        }

        public string GetMax(int bookId)
        {
            if (!CheckSecurity()) { return AccessDenied(); }
            var query = new Query.Chapters();
            return query.GetMax(bookId).ToString();

        }

        public string CreateChapter(int bookId, int chapter, string title, string summary)
        {
            if (!CheckSecurity()) { return AccessDenied(); }
            var query = new Query.Chapters();
            try
            {
                query.CreateChapter(bookId, chapter, title, summary);
            }
            catch (Exception)
            {
                return Error();
            }
            return Success();
        }
    }
}
