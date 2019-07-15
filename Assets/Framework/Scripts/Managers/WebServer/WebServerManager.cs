using System;
using System.IO;
using UnityEngine;
using UniWebServer;
using UniWebServer.Lib;

namespace Framework.Scripts.Managers.WebServer
{
    public class WebServerManager : BaseManager
    {
        public bool startOnAwake = true;
        public int port = 8079;
        public int workerThreads = 2;
        public bool processRequestsInMainThread = true;
        public bool logRequests = true;

        public string folderPath = "/hextris";

        UniWebServer.WebServer server;

        void Start()
        {
            if (processRequestsInMainThread)
                Application.runInBackground = true;
            server = new UniWebServer.WebServer(port, workerThreads, processRequestsInMainThread);
            server.logRequests = logRequests;
            server.HandleRequest += HandleRequest;
            if (startOnAwake)
            {
                server.Start();
            }
        }

        void OnApplicationQuit()
        {
            server.Dispose();
        }

        void Update()
        {
            if (server.processRequestsInMainThread)
            {
                server.ProcessRequests();
            }
        }


        void HandleRequest(Request request, Response response)
        {
            // get first part of the directory
            string folderRoot = Helper.GetFolderRoot(request.uri.LocalPath);
            if (folderRoot.Equals(folderPath))
            {
                try
                {
                    Handle(request, response);
                }
                catch (Exception e)
                {
                    response.statusCode = 500;
                    response.Write(e.Message);
                }
            }
            else
            {
                response.statusCode = 404;
                response.message = "Not Found.";
                response.Write(request.uri.LocalPath + " not found.");
            }
        }

        private void Handle(Request request, Response response)
        {
            // check if file exist at folder (need to assume a base local root)
            string folderRoot = Application.streamingAssetsPath;
            string fullPath = folderRoot + Uri.UnescapeDataString(request.uri.LocalPath);
            // get file extension to add to header
            string fileExt = Path.GetExtension(fullPath);
            // not found
            if (!File.Exists(fullPath))
            {
                response.statusCode = 404;
                response.message = "Not Found";
                return;
            }

            // serve the file
            response.statusCode = 200;
            response.message = "OK";
            response.headers.Add("Content-Type", MimeTypeMap.GetMimeType(fileExt));

            // read file and set bytes
            using (FileStream fs = File.OpenRead(fullPath))
            {
                int length = (int) fs.Length;
                byte[] buffer;

                // add content length
                response.headers.Add("Content-Length", length.ToString());

                // use binary for mostly all except text
                using (BinaryReader br = new BinaryReader(fs))
                {
                    buffer = br.ReadBytes(length);
                }

                response.SetBytes(buffer);
            }
        }
    }
}