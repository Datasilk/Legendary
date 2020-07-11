using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.IO;
using Microsoft.Extensions.Configuration;

public static class Server
{

    //environment
    public enum Environment
    {
        development = 0,
        staging = 1,
        production = 2
    }
    public static Environment environment = Environment.development;
    public static string hostUri = "https://localhost:5000/";

    //server properties
    public static DateTime serverStart = DateTime.Now;
    public static double requestCount = 0;
    public static double pageRequestCount = 0;
    public static double apiRequestCount = 0;
    public static float requestTime = 0;
    public static string Version = "1.0";

    //config properties
    public static IConfiguration config;
    public static string[] servicePaths = new string[] { "api" };
    public static int bcrypt_workfactor = 10;
    public static string salt = "";
    public static bool hasAdmin = false; //no admin account exists
    public static bool resetPass = false; //force admin to reset password

    //other settings
    public static string ServerId = "";
    public static Dictionary<string, string> languages;
    public static bool IsDocker { get; set; }

    //Dictionary used for caching non-serialized objects, files from disk, or raw text
    public static Dictionary<string, object> Cache = new Dictionary<string, object>();

    //private properties
    private static string _rootPath { get; set; }

    public static string RootPath
    {

        get
        {
            if (string.IsNullOrEmpty(_rootPath))
            {
                _rootPath = Path.GetFullPath(".").Replace("\\", "/");
            }
            return _rootPath;
        }
    }



    public static string MapPath(string path = "")
    {
        path = path.Replace("\\", "/");
        if (path.Substring(0, 1) == "/") { path = path.Substring(1); }
        if (IsDocker)
        {
            return Path.Combine(RootPath, path);
        }
        else
        {
            return Path.Combine(RootPath.Replace("/", "\\"), path.Replace("/", "\\"));
        }


        //var str = strPath.Replace("\\", "/");
        //if (str.Substring(0, 1) == "/") { str = str.Substring(1); }
        //return Path.Combine(_rootPath.Concat(str.Split('/')).ToArray());
    }

    #region "Cache"
    /// <summary>
    /// Loads a file from cache. If the file hasn't been cached yet, then load file from a drive.
    /// </summary>
    /// <param name="filename">The relevant path to the file</param>
    /// <param name="noDevEnvCache">If true, it will not load a file from cache if the app is running in a development environment. Instead, it will always load the file from a drive.</param>
    /// <param name="noCache">If true, will not save to cache, but will instead load file from disk every time</param>
    /// <returns></returns>
    public static string LoadFileFromCache(string filename)
    {
        if (environment != Environment.development)
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
            if (environment != Environment.development)
            {
                Cache.Add(filename, file);
            }
            return file;
        }
        return "";
    }

    public static void SaveFileFromCache(string filename, string value)
    {
        File.WriteAllText(MapPath(filename), value);
        if (Cache.ContainsKey(filename))
        {
            Cache[filename] = value;
        }
        else
        {
            Cache.Add(filename, value);
        }
    }

    public static void SaveToCache(string key, object value)
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

    public static T LoadFromCache<T>(string key, Func<T> value, bool serialize = true)
    {
        if (Cache[key] == null)
        {
            var obj = value();
            SaveToCache(key, serialize ? (object)JsonSerializer.Serialize(obj) : obj);
            return obj;
        }
        else
        {
            return serialize ? JsonSerializer.Deserialize<T>((string)Cache[key]) : (T)Cache[key];
        }
    }
    #endregion
}
