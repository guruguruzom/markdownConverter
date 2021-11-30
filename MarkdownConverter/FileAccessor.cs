using LitJson;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MarkdownConverter
{
    class FileAccessor
    {
        static StreamWriter output = null;
        static Queue<Action> msgQueue = new Queue<Action>();

        private readonly string masterIp = @"MasterIP.json";
        private readonly string path = @"output.txt";
        private static FileAccessor _instance;

        public static FileAccessor Instance()
        {
            if (_instance == null)
            {
                _instance = new FileAccessor();
                
            }
            return _instance;
        }

        public string ReadJsonConfig()
        {
            string str = File.ReadAllText(masterIp);

            return str;
        }

        public void SetJsonConfig(JsonData jsonData)
        {
            using (StreamWriter output = new StreamWriter(masterIp, false))
            {
                output.WriteLine(jsonData.ToJson());
            }
        }

        public void WriteJson(string oneline)
        {
            using (output = new StreamWriter(path, true))
            {
                output.WriteLine(oneline);
            }
        }

        static Thread textLogThread;

        private Action action;
        private void TextThread()
        {
            try
            {
                using (output = new StreamWriter(path, true))
                {
                    while (msgQueue.Count != 0)
                    {
                        action = msgQueue.Dequeue();
                        action?.Invoke();
                    }
                }
            }
            finally
            {
                //  output.Dispose();
            }
            //textLogThread.Abort();
            textLogThread = null;
        }

    }
}
