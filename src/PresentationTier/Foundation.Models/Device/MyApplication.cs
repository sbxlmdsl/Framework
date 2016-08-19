//-----------------------------------------------------------------------
// <copyright file="MyApplication.cs" company="Genesys Source">
//      Copyright (c) Genesys Source. All rights reserved.
//      Licensed to the Apache Software Foundation (ASF) under one or more 
//      contributor license agreements.  See the NOTICE file distributed with 
//      this work for additional information regarding copyright ownership.
//      The ASF licenses this file to You under the Apache License, Version 2.0 
//      (the 'License'); you may not use this file except in compliance with 
//      the License.  You may obtain a copy of the License at 
//       
//        http://www.apache.org/licenses/LICENSE-2.0 
//       
//       Unless required by applicable law or agreed to in writing, software  
//       distributed under the License is distributed on an 'AS IS' BASIS, 
//       WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.  
//       See the License for the specific language governing permissions and  
//       limitations under the License. 
// </copyright>
//-----------------------------------------------------------------------
using System.Threading.Tasks;
using Genesys.Extensions;
using Genesys.Extras.Net;
using Genesys.Foundation.Session;
using System;

namespace Genesys.Foundation.Device
{
    /// <summary>
    /// Adds critical functionality to allow a Mvc app to work with the GenesysFramework
    /// </summary>    
    public class MyApplication
    {
        private SessionContext myContext;
        private static volatile object lockObject = new object();
        private static MyApplication thisInstance;

        /// <summary>
        /// Issues singleton class
        /// </summary>
        /// <returns></returns>
        public static MyApplication Current()
        {
            if (thisInstance == null)
            {
                lock (lockObject)
                {
                    if (thisInstance == null)
                    {
                        thisInstance = new MyApplication();
                    }
                }
            }

            return thisInstance;
        }

        /// <summary>
        /// ID of application
        /// </summary>
        public string ApplicationUUID { get; }

        /// <summary>
        /// Device info. Enough to determine device + application combination.
        /// </summary>
        public DeviceInfo MyDevice
        {
            get
            {
                return DeviceInfo.Create();
            }
        }

        /// <summary>
        /// Local storage
        /// </summary>
        public DeviceStorage MyData
        {
            get
            {
                return DeviceStorage.Create();
            }
        }

        /// <summary>
        /// Context
        /// </summary>
        public SessionContext MyContext
        {
            get
            {
                if (myContext == null)
                {
                    myContext = new SessionContext { ApplicationUUID = this.ApplicationUUID, DeviceUUID = this.MyDevice.DeviceUUID };
                }
                return myContext;
            }
        }

        /// <summary>
        /// MyWebService
        /// </summary>
        public virtual UrlInfo MyWebService
        {
            get
            {
                return new UrlInfo(this.MyData.ConfigurationManager.AppSettingValue("MyWebService"));
            }
        }

        /// <summary>
        /// Display copyright information
        /// </summary>
        public string MyCopyright
        {
            get
            {
                return String.Format("{0} {1}. {2}", "Copyright (c)", DateTime.UtcNow.Year, "All rights reserved.");
            }
        }

        /// <summary>
        /// Entry point Screen (Typically login screen)
        /// </summary>
        public UrlInfo EntryPoint { get; }

        /// <summary>
        /// Home dashboard screen
        /// </summary>
        public UrlInfo HomeScreen { get; }

        /// <summary>
        /// Error screen
        /// </summary>
        public virtual UrlInfo ErrorScreen { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        public MyApplication() : base()
        {
        }

        /// <summary>
        /// Wakes up any sleeping threads, and MyWebService chain
        /// </summary>
        /// <returns></returns>
        public virtual async Task WakeServicesAsync()
        {
            if (this.MyWebService.ToString() == TypeExtension.DefaultString)
            {
                HttpRequestGetString Request = new HttpRequestGetString(this.MyWebService.ToString());
                Request.ThrowExceptionWithEmptyReponse = false;
                await Request.SendAsync();
            }
        }


        /// <summary>
        /// Can this screen go back
        /// </summary>
        protected bool CanGoBack { get { return true; } }

        /// <summary>
        /// Navigates back to previous screen
        /// </summary>
        protected void GoBack() { }

        /// <summary>
        /// Navigates to any screen
        /// </summary>
        /// <param name="destination"></param>
        /// <param name="dataToPass"></param>
        public void Navigate(UrlInfo destination, object dataToPass = null) { }
    }
}
