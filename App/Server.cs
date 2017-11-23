﻿using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace Legendary
{
    public class Server
    {
        ////////////////////////////////////////////////
        //Server     (for application-wide memory store)
        ////////////////////////////////////////////////

        public enum enumEnvironment
        {
            development = 0,
            staging = 1,
            production = 2
        }

        public string Version = "";

        public Utility.Util Util = new Utility.Util();
        public enumEnvironment environment = enumEnvironment.development;
        public DateTime serverStart = DateTime.Now;
        public int requestCount = 0;
        public float requestTime = 0;
        public string sqlActive = "";
        public string sqlConnection = "";
        public Random Random = new Random();
        public bool resetPass = false; //empty or comprimised database?
        public bool hasAdmin = false; //admin account exists in database?
        public int bcrypt_workfactor = 10;
        private string _path = "";
        public IConfiguration config;

        //Dictionary used for caching non-serialized objects, files from disk, or raw text
        //be careful not to leak memory into the cache while causing an implosion!
        public Dictionary<string, object> Cache = new Dictionary<string, object>();

        //Dictionary used for HTML scaffolding of various files on the server. 
        //Value for key/value pair is an array of HTML (scaffold["key"][x].htm), 
        //         separated by scaffold variable name (scaffold["key"][x].name),
        //         where data is injected in between each array item.
        public Dictionary<string, structScaffold> Scaffold = new Dictionary<string, structScaffold>();


        /// <summary>
        /// Executed once the server is up & running
        /// </summary>
        public void Up()
        {
            var query = new Query.Users(sqlConnection);
            resetPass = !query.HasPasswords();
            hasAdmin = query.HasAdmin();
        }

        #region "System.UI.Web.Page.Server methods"
        public string path(string strPath = "")
        {
            if (_path == "") { _path = Path.GetFullPath(".") + "\\"; }
            var str = strPath.Replace("/", "\\");
            if (str.Substring(0, 1) == "\\") { str = str.Substring(1); }
            return _path + str;
        }

        public string MapPath(string strPath = "") { return path(strPath); }

        public string UrlDecode(string strPath)
        {
            return Uri.UnescapeDataString(strPath);
        }

        public string UrlEncode(string strPath)
        {
            return Uri.EscapeDataString(strPath);
        }

        #endregion

        #region "Cache"
        /// <summary>
        /// Loads a file from cache. If the file hasn't been cached yet, then load file from a drive.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="noDevEnvCache">If true, it will not load a file from cache if the app is running in a development environment. Instead, it will always load the file from a drive.</param>
        /// <returns></returns>
        public string LoadFileFromCache(string filename, bool noDevEnvCache = false, bool noCache = false)
        {
            if ((environment != enumEnvironment.development || noDevEnvCache == false) && noCache == false)
            {
                //next, check cache
                if (Cache.ContainsKey(filename))
                {
                    return (string)Cache[filename];
                }
            }
            if (File.Exists(MapPath(filename)))
            {
                //finally, check file system
                var file = File.ReadAllText(MapPath(filename));
                if (environment != enumEnvironment.development && noCache == false)
                {
                    Cache.Add(filename, file);
                }
                return file;
            }
            return "";
        }

        public void SaveToCache(string key, object value)
        {
            if (Cache.ContainsKey(key))
            {
                Cache[key] = value;
            }
            else
            {
                Cache.Add(key, value);
            }
        }

        public T GetFromCache<T>(string key, Func<T> value, bool serialize = true)
        {
            if(Cache[key] == null)
            {
                var obj = value();
                SaveToCache(key, serialize ? (object)Util.Serializer.WriteObjectToString(obj) : obj);
                return obj;
            }
            else
            {
                return serialize ? (T)Util.Serializer.ReadObject((string)Cache[key], typeof(T)) : (T)Cache[key];
            }
        }
        #endregion
    }
}
