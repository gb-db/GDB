using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Repository.Utility
{
    public class Helper
    {
        private IHostingEnvironment _environment;

        public Helper(IHostingEnvironment env)
        {
            _environment = env;
        }

        public void WriteToLog(string errorMessage)
        {
            //_log.Error(errorMessage);
            try
            {
                string dir = _environment.WebRootPath + @"\Logs\";
                //string dir = HttpContext.Current.Server.MapPath("~/logs/").Replace(@"/", @"\");
                bool bool_ = true;
                if (!Directory.Exists(dir))
                {
                    bool_ = CreateDirectory(dir);
                }

                if (bool_)
                {
                    using (StreamWriter w = File.AppendText(dir + @"/log.txt"))
                    {
                        Log(errorMessage, w);
                    }
                }

                //using (StreamWriter w = File.AppendText(HttpContext.Current.Server.MapPath("~/logs/log.txt")))
                //{
                //    Log(errorMessage, w);
                //}
            }
            catch (Exception ex)
            {
                //_log.Error($"{ex.Message} {ex.Source}");
            }

        }


        public bool CreateDirectory(string path)
        {
            DirectoryInfo di = new DirectoryInfo(path);
            bool bool_ = false;
            try
            {
                if (di.Exists)
                {
                    bool_ = true;
                    di.Delete(true);
                }

                di.Create();
                bool_ = true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace();
                string methodName = st.GetFrame(0).GetMethod().Name;

                string Msg = methodName + " ### " + ex.Message + " ### ";
                if (ex.InnerException != null)
                    Msg += ex.InnerException.Message + " ### Helper.CreateDirectory";

                WriteToLog(Msg);
            }
            return bool_;
        }

        private void Log(string logMessage, TextWriter w)
        {
            w.Write("\r\nLog Entry : ");
            w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                DateTime.Now.ToLongDateString());
            w.WriteLine("  :");
            w.WriteLine("  :{0}", logMessage);
            w.WriteLine("-------------------------------");
        }
    }
}
