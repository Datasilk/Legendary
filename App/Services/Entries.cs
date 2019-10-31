﻿using System;
using Microsoft.AspNetCore.Http;

namespace Legendary.Services
{
    public class Entries : Service
    {
        public Entries(HttpContext context, Parameters parameters) : base(context, parameters)
        {
        }

        public string GetList(int bookId, int entryId, int start = 1, int length = 500, int sort = 0)
        {
            if (!CheckSecurity()) { return AccessDenied(); }
            return Common.Platform.Entries.GetList(User.userId, bookId, entryId, start, length, (Common.Platform.Entries.SortType)sort);
            
        }

        public string CreateEntry(int bookId, string title, string summary, int chapter = 0, int sort = 0)
        {
            if (!CheckSecurity()) { return AccessDenied(); }
            try
            {
                var entryId = Common.Platform.Entries.CreateEntry(User.userId, bookId, title, summary, chapter);
                return 
                    entryId + "|" + 
                    Common.Platform.Entries.GetList(User.userId, bookId, entryId, 1, 500, (Common.Platform.Entries.SortType)sort);
            }
            catch (ServiceErrorException ex)
            {
                return Error(ex.Message);
            }
        }

        public string SaveEntry(int entryId, string content)
        {
            if (!CheckSecurity()) { return AccessDenied(); }
            try
            {
                Common.Platform.Entries.SaveEntry(User.userId, entryId, content);
            }
            catch (ServiceErrorException)
            {
                return Error("An error occurred while saving your entry");
            }
            return Success();
        }

        public string LoadEntry(int entryId, int bookId)
        {
            if (!CheckSecurity()) { return AccessDenied(); }
            try
            {
                return Common.Platform.Entries.LoadEntry(entryId, bookId);
            }
            catch (ServiceErrorException ex)
            {
                return Error(ex.Message);
            }
        }

        public string LoadEntryInfo(int entryId, int bookId)
        {
            if (!CheckSecurity()) { return AccessDenied(); }
            try
            {
                return Common.Platform.Entries.LoadEntryInfo(User.userId, entryId, bookId);
            }
            catch (ServiceErrorException ex)
            {
                return Error(ex.Message);
            }
        }

        public string UpdateEntryInfo(int entryId, int bookId, DateTime datecreated, string title, string summary, int chapter)
        {
            if (!CheckSecurity()) { return AccessDenied(); }
            try
            {
                Common.Platform.Entries.UpdateEntryInfo(entryId, bookId, datecreated, title, summary, chapter);
                return Success();
            }
            catch (ServiceErrorException ex)
            {
                return Error(ex.Message);
            }
        }

        public string TrashEntry(int entryId)
        {
            if (!CheckSecurity()) { return AccessDenied(); }
            try
            {
                return Common.Platform.Entries.TrashEntry(User.userId, entryId).ToString();
            }
            catch (ServiceErrorException ex)
            {
                return Error(ex.Message);
            }
        }
    }
}
