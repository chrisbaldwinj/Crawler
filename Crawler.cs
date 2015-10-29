using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace Crawler
{

    #region FileDefinition Section class
    /// <summary>
    /// This struct is used for files information.
    /// </summary>
    /// <remarks>Still trying to make the main class only allowed to set the values. So far no luck!</remarks>
    public struct FileDef
    {
        public string Name { get; set; }
        public string FullPath { get; set; }
        public string Parent { get; set; }
        public long Size { get; set; }
        public string Type { get; set; }
        public bool ReadOnly { get; set; }

    }
    #endregion




    /// <summary>
    /// Main Function
    /// </summary>
    class Crawl
    {
        // List of FileDefs
        public List<FileDef> Files = new List<FileDef>();
        // Tells us if we should log any Errors or Exceptions
        public static bool __DEBUG__ = true;
        // StreamWriter for the debugging
        private static StreamWriter Debug;
        // Continuation Token is basically our test to see if we should continue, this is how Crawl.Stop() works.
        private static bool continuation_token = true;
        // Our Debug File Name.
        const string __DEBUG_FILE__ = "debug.log";


        #region public methods
        /// <summary>
        ///     Main Crawl method, only accepts one argument which is the string root.
        /// </summary>
        /// <param name="root">Root should be a computer readable string for a directory. Copy and Paste from properties works best.</param>
        public Crawl(string root)
        {
            CrawlerFunction(root, true, "");
        }

        /// <summary>
        ///     Overload Crawl. Accepts two arguments Root and Depth
        /// </summary>
        /// <param name="root">Root should be a computer readable string for a directory. Copy and Paste from properties works best.</param>
        /// <param name="deep">If false Crawl will only view files from within the root directory.</param>
        public Crawl(string root, bool deep)
        {
            CrawlerFunction(root, deep, "");
        }

        /// <summary>
        ///     Overload Crawl. Accepts two arguments Root and Extensions
        /// </summary>
        /// <param name="root">Root should be a computer readable string for a directory. Copy and Paste from properties works best.</param>
        /// <param name="extension">Extension should be a string of extension names without the period, separated by a space.</param>
        public Crawl(string root, string extension)
        {
            CrawlerFunction(root, true, extension);
        }

        /// <summary>
        ///     Overload Crawl. Accepts Three Arguments Root, Deep, and Extension
        /// </summary>
        /// <param name="root">Root should be a computer readable string for a directory. Copy and Paste from properties works best.</param>
        /// <param name="deep">If false Crawl will only view files from within the root directory.</param>
        /// <param name="extension">Extension should be a string of extension names without the period, separated by a space.</param>
        public Crawl(string root, bool deep, string extension)
        {
            CrawlerFunction(root, deep, extension);
        }

        /// <summary>
        ///     Stop will prevent the Crawl going any further.
        /// </summary>
        public static void Stop()
        {
            if (continuation_token)
            {
                continuation_token = false;
                Dispose(Debug);
            }
        }
        #endregion 



        /// <summary>
        ///     Main Crawler function for behind the scenes.
        /// </summary>
        /// <param name="root">Root should be a computer readable string for a directory. Copy and Paste from properties works best.</param>
        /// <param name="deep">If false Crawl will only view files from within the root directory.</param>
        /// <param name="extension">Extension should be a string of extension names without the period, separated by a space.</param>
        private void CrawlerFunction(string root, bool deep, string extension) 
        {
            //if debug is true we'll initialize the stream writer.
            if (__DEBUG__)
            {
                Debug = new StreamWriter(__DEBUG_FILE__, true);
            }

            //check if the directory root exists
            if (!Directory.Exists(root))
            {
                //log to the Debug file our message.
                Log("error", root + " does not exist. Program shutting down.", Debug);
            }
            
            //Stack of directories.
            Stack<string> dirs = new Stack<string>();

            //add this to the first initial list
            dirs.Push(root);

            //loop the directory sorting out files and directories. Only continues if dir is greater than 0 and continuation token is set to true
            while (dirs.Count > 0 && continuation_token != false)
            {
                //grab the last dir that was entered (LIFO)
                string currentDir = dirs.Pop();
                //Sub directory array placeholder
                string[] subDirs = { };
                //Files array placeholder
                string[] files = { };


                //you'll find this test three places, here so we prevent any further looping
                if (!continuation_token)
                    return;

                try
                {
                    //if deep is true
                    if (deep)
                    {
                        //get all subdirs
                        subDirs = Directory.GetDirectories(currentDir);
                        //loop them and push them to the main dir section
                        foreach (string dirsub in subDirs)
                        {
                            dirs.Push(dirsub);
                        }
                    }
                } //Exception Testing
                catch (UnauthorizedAccessException e)
                {
                    Log("Exception", e.Message, Debug);
                    continue;
                }
                catch (DirectoryNotFoundException e)
                {
                    Log("Exception", e.Message, Debug);
                    continue;
                }



                //GET FILES!
                try
                {
                    //Get files from the current directory that Crawl is in.
                    files = Directory.GetFiles(currentDir);
                    //loop this.
                    foreach (string file in files)
                    {
                        //get the file attributes.
                        FileInfo fileAttrs = new FileInfo(file);
                        //create a new instance of FileDef
                        FileDef currentFile = new FileDef();
                        //set the full path
                        currentFile.FullPath = fileAttrs.FullName;
                        //set the name
                        currentFile.Name = fileAttrs.Name;
                        //set the size of the file
                        currentFile.Size = fileAttrs.Length;
                        //set if is readonly
                        currentFile.ReadOnly = fileAttrs.IsReadOnly;
                        //set the type without period
                        currentFile.Type = fileAttrs.Extension.Replace(".","");
                        //set the parent Directory
                        currentFile.Parent = fileAttrs.DirectoryName;
                        //if the extension is not null or empty or whitespace then we can replace the name by taking the extension off.
                        if (!String.IsNullOrEmpty(fileAttrs.Extension) || !String.IsNullOrWhiteSpace(fileAttrs.Extension))
                        {
                            currentFile.Name = currentFile.Name.Replace(fileAttrs.Extension, "");
                        }
                        //if the extension parameter means we want all the files no matter the extension
                        if (String.IsNullOrEmpty(extension) || String.IsNullOrWhiteSpace(extension))
                        {
                            Files.Add(currentFile);
                        }
                        else //else we want to look for specific extensions.
                        {
                            //create the bool for matching on the extension string. work boundary extension word boundary. Flag ignore case
                            bool result = Regex.IsMatch(currentFile.Type, "\\b"+extension+"\\b", RegexOptions.IgnoreCase);
                            if (result)
                            {   //if result is true add the currentFile
                                Files.Add(currentFile);
                            }
                        }
                        //test returns here, but returns out of the foreach I belive 
                        if (!continuation_token)
                            return;
                    }
                    //returns out of the function
                    if (!continuation_token)
                        return;
                }
                catch (UnauthorizedAccessException e)
                {
                    Log("Exception", e.Message, Debug);
                    continue;
                }
                catch (DirectoryNotFoundException e)
                {
                    Log("Exception", e.Message, Debug);
                    continue;
                }
                catch (IOException e)
                {
                    Log("Exception", e.Message, Debug);
                    continue;
                }
            }


            //Close and dispose function
            Dispose(Debug);
        }



        #region Logging Functions
        public static void ClearLog()
        {
            if (!__DEBUG__) return;
            using (StreamWriter writer = new StreamWriter(__DEBUG_FILE__, false))
            {
                writer.Write("");
                Dispose(writer);
            }
        }
        public static void Log(string type, string logMessage, TextWriter w)
        {
            if (!__DEBUG__) return;
            string message = "[" + type.ToUpper() + "] : " + DateTime.Now.ToLongTimeString() + " " + DateTime.Now.ToLongDateString() + " " + logMessage +"\r\n";
            w.Write(message);
        }
        public static void Dispose(TextWriter w)
        {
            if (!__DEBUG__) return;
            w.Flush();
            w.Close();
            w.Dispose();
        }
        #endregion


    }

}
