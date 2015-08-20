//-----------------------------------------------------------------------
// <copyright file="Algorithm.cs" company="Jim Ma">
//     Copyright (c) Jim Ma. All rights reserved.
// </copyright>
// <author>Jim Ma</author>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace NeatCat
{
    public static class Web
    {
        public static string RequestUrl(string url)
        {
            string returnText = string.Empty;
            using (WebClient wc = new WebClient())
            {
                wc.Credentials = CredentialCache.DefaultCredentials;
                wc.Encoding = Encoding.UTF8;
                returnText = wc.DownloadString(url);
            }

            return returnText;
        }

        public static string PostRawUrl(string url)
        {
            string result = string.Empty;

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(url);
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }

            return result;
        }

        public static Stream PostJsonUrlReturnStream(string url, string postJsonContent)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(postJsonContent);
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            Stream result = httpResponse.GetResponseStream();

            return result;
        }

        public static Stream RequestUrlReturnStream(string url)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.Method = "GET";

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            Stream result = httpResponse.GetResponseStream();

            return result;
        }

        public static string PostJsonUrl(string url, string postJsonContent)
        {
            string result = string.Empty;

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(postJsonContent);
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }

            return result;
        }

        public static string PostUrl(string url, NameValueCollection parameters)
        {
            string returnText = string.Empty;
            using (WebClient wc = new WebClient())
            {
                wc.Credentials = CredentialCache.DefaultCredentials;
                wc.Encoding = Encoding.UTF8;
                byte[] responseBytes = wc.UploadValues(url, "POST", parameters);
                returnText = Encoding.UTF8.GetString(responseBytes);
            }

            return returnText;
        }

        /// <summary>
        /// Gets the schema and authority segment of request uri.
        /// E.g. "https://www.google.com/?q=1" will return "https://www.google.com".
        /// </summary>
        /// <returns>A string contains the schema and authority of the uri. </returns>
        public static string GetRequestUriSchemaAndAuthority(Uri requestUri)
        {
            if (requestUri == null)
            {
                throw new ArgumentNullException("The 'requestUri' cannot be null.");
            }

            var requestUriLeftPart = requestUri.GetLeftPart(UriPartial.Authority);

            return requestUriLeftPart;
        }
    }
}
