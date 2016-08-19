//-----------------------------------------------------------------------
// <copyright file="DeviceStorage.cs" company="Genesys Source">
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
using Genesys.Extras.Collections;
using Genesys.Extras.Configuration;
using Genesys.Extras.Text;

namespace Genesys.Foundation.Device
{
    /// <summary>
    /// Handles all local data storage
    /// </summary>
    public sealed class DeviceStorage
    {
        private static DeviceStorage myDeviceStorage = null;
        private static object lockObject = new object();
        private ApplicationDataContainer localSettings = new ApplicationDataContainer();

        /// <summary>
        /// MyWebService Url
        /// </summary>
        public string MyWebService { get; set; } = TypeExtension.DefaultString;

        /// <summary>
        /// ConfigurationManager xml information
        /// Emulate web.config, Local\AppSettings.config and Local\ConnectionStrings.config
        /// </summary>
        public ConfigurationManagerSafe ConfigurationManager { get; } = new ConfigurationManagerSafe();

        /// <summary>
        /// Private Constructor to force singleton
        /// </summary>
        private DeviceStorage() : base() { }

        /// <summary>
        /// Issues singleton class
        /// </summary>
        /// <returns></returns>
        public static DeviceStorage Create(string appSettingsFilePath = "Local\\AppSettings.config")
        {
            if (myDeviceStorage == null)
            {
                lock (lockObject)
                {
                    if (myDeviceStorage == null)
                    {
                        myDeviceStorage = new DeviceStorage();
                    }
                }
            }
            return myDeviceStorage;
        }

        /// <summary>
        /// Gets setting from local resources (hard-coded for String)
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>

        public string SettingGet(string key)
        {
            return this.SettingGet<StringMutable>(key);
        }

        /// <summary>
        /// Gets setting from local resources (hard-coded for String)
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Guid SettingGetGuid(string key)
        {
            return this.SettingGet<Guid>(key);
        }

        /// <summary>
        /// Gets setting from local resources
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>

        public TValue SettingGet<TValue>(string key) where TValue : new()
        {
            TValue returnValue = default(TValue);
            returnValue = this.localSettings.GetValue(key).DirectCastSafe<TValue>();
            return returnValue;
        }

        /// <summary>
        /// Writes to the local settings (hard-coded for String)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SettingSet(string key, string value)
        {
            this.SettingSet<String>(key, value);
        }

        /// <summary>
        /// Writes to the local settings (hard-coded for guid)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SettingSet(string key, Guid value)
        {
            this.SettingSet<Guid>(key, value);
        }

        /// <summary>
        /// Writes to the local settings
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>

        public void SettingSet<TValue>(string key, TValue value)
        {
            this.localSettings.Remove(key);
            this.localSettings.Add(key, value.ToStringSafe());
        }

        /// <summary>
        /// Key Value Pair dictionary of user session-specific data
        /// </summary>
        public class ApplicationDataContainer : KeyValueListString
        {
            
        }
    }
}
