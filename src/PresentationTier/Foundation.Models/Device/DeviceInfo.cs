//-----------------------------------------------------------------------
// <copyright file="DeviceInfo.cs" company="Genesys Source">
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
using System;
using Genesys.Extensions;
using Genesys.Foundation.Device;

namespace Genesys.Foundation.Device
{
    /// <summary>
    /// Singleton to deliver information on physical device
    /// </summary>
    public sealed class DeviceInfo : IDevice
    {
        private static volatile object lockObject = new object();
        private static DeviceInfo thisInstance;

        /// <summary>
        /// DeviceUUID - ASHWID or IMEI or OS Device ID
        /// </summary>
        /// <returns></returns>
        public string DeviceUUID { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// ApplicationUUID - Device + App ID
        /// </summary>
        /// <returns></returns>
        public string ApplicationUUID { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// ApplicationID in the context of the "store" serving this app per ecosystem
        /// </summary>
        public Guid StoreAppID { get { return new Guid(); } set { } }

        /// <summary>
        /// Model of device
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// Manufacturer of device
        /// </summary>
        public string Manufacturer { get; set; }

        /// <summary>
        /// Name of device
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// OS Name of device
        /// </summary>
        public string OSName { get; set; }
        
        /// <summary>
        /// Constructor
        /// </summary>
        private DeviceInfo() : base() { }
                
        /// <summary>
        /// Issues singleton class
        /// </summary>
        /// <returns></returns>
        public static DeviceInfo Create()
        {
            if (thisInstance == null)
            {
                lock(lockObject)
                {
                    if (thisInstance == null)
                    {
                        thisInstance = new DeviceInfo();
                    }
                }
            }

            return thisInstance;
        }
        
        /// <summary>
        /// Concatenates device info into friendly string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format("{0}-{1}-{2}-{3}", this.OSName, this.Name, this.Manufacturer, this.Model);
        }
        
        /// <summary>
        /// Pulls unique device ID. This only works when in the UI App assembly.
        /// </summary>
        /// <returns></returns>
        private static string GetDeviceUUID()
        {
            return TypeExtension.DefaultString;
        }
    }
}
