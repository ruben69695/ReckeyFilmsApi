using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ReckeyFilmsApi.Models;
using System.Net;
using System.Net.Http;
using System.IO;
using Newtonsoft.Json;

namespace ReckeyFilmsApi.Classes
{  
    /// <summary>
    /// This class send a get request to a web Server in DotNet Core in C#
    /// </summary>
    class GetWebRequest
    {
        private WebRequest Request;
        private Task<WebResponse> Response;
        private StreamReader Reader;
        private Stream Str;
        /// <summary>
        /// Text response from server
        /// </summary>
        /// <returns>String</returns>
        public string TextResponseFromServer { get; set; } = "";
        /// <summary>
        /// URL for the Web Request
        /// </summary>
        /// <returns>String</returns>
        public string URL { get; set; } = "";

        /// <summary>
        /// First constructor of the class
        /// </summary>
        public GetWebRequest()
        {}

        /// <summary>
        /// Second constructor of the class
        /// </summary>
        /// <param name="url">Web URL for the web request</param>
        public GetWebRequest(string url)
        {
            this.URL = url;
        }

        /// <summary>
        /// This method sends the Get Request, if the request is OK save all the text returned from server into TextResponseFromServer local class variable
        /// </summary>      
        public bool SendRequest()
        {
            bool requestSent = false;

            try
            {
                Request = WebRequest.Create(URL);
                Response = Request.GetResponseAsync();
                Response.Wait();

                Console.WriteLine(DateTime.Now.Date.ToString() + "  /  " + this.GetType().ToString() + "  /  " + "Web Request Status: " + Response.Status);                

                if(Response.IsCompleted)
                {
                    Str = Response.Result.GetResponseStream();
                    Reader = new StreamReader(Str);
                    TextResponseFromServer = Reader.ReadToEnd();
                    requestSent = true;
                } 
            }
            catch (System.Exception ex)
            {  
                Console.WriteLine(ex.Message);
            }

            return requestSent;
        }
    }
}