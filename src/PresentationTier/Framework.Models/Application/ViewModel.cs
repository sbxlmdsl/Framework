//-----------------------------------------------------------------------
// <copyright file="ViewModel.cs" company="Genesys Source">
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
using Genesys.Extensions;
using Genesys.Extras.Net;
using Genesys.Framework.Entity;
using Genesys.Framework.Operation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Genesys.Framework.Application
{
    /// <summary>
    /// ViewModel holds model and is responsible for server calls, navigation, etc.
    /// </summary>
    public abstract class ViewModel<TModel> : ISaveAsync<TModel, int>, IViewModel<TModel> where TModel : IEntity, new()
    {
        /// <summary>
        /// Name of the Web API controller, that will become the path in the Uri
        ///  - ViewModelUrl = MyWebService + / + ControllerName
        ///  - MyWebService from AppSettings.config MyWebService element
        /// </summary>
        public string MyWebServiceController { get; protected set; } = TypeExtension.DefaultString;

        /// <summary>
        /// Application context of current process
        /// </summary>
        public abstract IApplication MyApplication { get; protected set; }

        /// <summary>
        /// MyWebService
        /// </summary>
        public Uri MyViewModelWebService { get { return new Uri(MyApplication?.MyWebService.ToStringSafe().AddLast("/") + MyWebServiceController, UriKind.RelativeOrAbsolute); } }

        /// <summary>
        /// Model data
        /// </summary>
        public TModel MyModel { get; set; } = new TModel();
        
        /// <summary>
        /// Sender of main Http Verbs
        /// </summary>
        public HttpVerbSender Sender { get; protected set; } = new HttpVerbSender();

        /// <summary>
        /// Property changed event handler for INotifyPropertyChanged
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Property changed event for INotifyPropertyChanged
        /// </summary>
        /// <param name="propertyName">String name of property</param>
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Send is about to begin
        /// </summary>
        public event SendBeginEventHandler SendBegin;

        /// <summary>
        /// Send is complete
        /// </summary>
        public event SendBeginEventHandler SendEnd;

        /// <summary>
        /// Workflow beginning. No custom to return.
        /// </summary>
        /// <param name="sender">Sender of event</param>
        /// <param name="e">Event arguments</param>
        public delegate void SendBeginEventHandler(object sender, EventArgs e);

        /// <summary>
        /// OnSendBegin()
        /// </summary>
        public void OnSendBegin() { if (this.SendBegin != null) { SendBegin(this, EventArgs.Empty); } }

        /// <summary>
        /// OnSendEnd()
        /// </summary>
        public void OnSendEnd() { if (this.SendEnd != null) { SendEnd(this, EventArgs.Empty); } }

        /// <summary>
        /// Constructs a MVVM view-model that stores MyModel, MyApplication and MyWebService
        ///  for self-sufficient page navigation, view binding and web service calls.
        /// </summary>
        /// <param name="myWebServiceController">Name of the Web API controller that supports this view model</param>        
        public ViewModel(string myWebServiceController)
            : base()
        {
            MyWebServiceController = myWebServiceController;
        }

        /// <summary>
        /// Can this screen go back
        /// </summary>
        public bool CanGoBack { get { return MyApplication.CanGoBack; } }

        /// <summary>
        /// Navigates back to previous screen
        /// </summary>
        public void GoBack() { MyApplication.GoBack(); }

        /// <summary>
        /// Pulls data via HttpGet passing ID
        /// </summary>
        /// <param name="id">Integer value of the record ID</param>
        /// <returns>Single record that matches id or constructed object</returns>
        public async Task<TModel> Get(int id)
        {
            var fullUrl = new Uri(MyViewModelWebService.ToStringSafe().AddLast("/") + id.ToString().AddLast("/"), UriKind.RelativeOrAbsolute);
            MyModel = await Sender.SendGetAsync<TModel>(fullUrl);
            return MyModel;
        }

        /// <summary>
        /// Pulls data via HttpGet passing ID
        /// </summary>
        /// <param name="key">Guid value of the record key</param>
        /// <returns>Single record that matches key</returns>
        public async Task<TModel> Get(Guid key)
        {
            var fullUrl = new Uri(MyViewModelWebService.ToStringSafe().AddLast("/") + key.ToString().AddLast("/"), UriKind.RelativeOrAbsolute);
            MyModel = await Sender.SendGetAsync<TModel>(fullUrl);
            return MyModel;
        }

        /// <summary>
        /// Pulls all data via HttpGet
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<TModel>> GetAll()
        {
            var returnData = new List<TModel>();
            var fullUrl = new Uri(MyViewModelWebService.ToStringSafe().AddLast("/GetAll/"), UriKind.RelativeOrAbsolute);
            returnData = await Sender.SendGetAsync<List<TModel>>(fullUrl);
            return returnData;
        }

        /// <summary>
        /// Pulls all data via HttpGet with ID != TypeExtension.DefaultInteger, and Key != TypeExtension.DefaultGuid
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<TModel>> GetAllExcludeDefault()
        {
            var returnData = new List<TModel>();
            var fullUrl = new Uri(MyViewModelWebService.ToStringSafe().AddLast("/GetAllExcludeDefault/"), UriKind.RelativeOrAbsolute);
            returnData = await Sender.SendGetAsync<List<TModel>>(fullUrl);
            return returnData;
        }

        /// <summary>
        /// Saves this object to the database via Http Put
        /// </summary>
        /// <param name="forceInsert">True to force use of Insert stored procedure. False to allow automated decision of insert vs. update.</param>
        /// <returns>Record that was inserted or updated. Failure will result in a new TEntity() object with no data</returns>
        public async Task<TModel> Save(bool forceInsert)
        {
            MyModel = await Sender.SendPutAsync<TModel>(MyViewModelWebService, MyModel);
            return MyModel;
        }

        /// <summary>
        /// Saves this object to the database via Http Post
        /// </summary>
        /// <returns>Record that was inserted or updated. Failure will result in a new TEntity() object with no data</returns>
        public async Task<TModel> Save()
        {
            MyModel = await Sender.SendPostAsync<TModel>(MyViewModelWebService, MyModel);
            return MyModel;
        }

        /// <summary>
        /// Deletes this object from the database via Http Delete
        /// </summary>s
        /// <returns>True for success, false for failure</returns>
        public async Task<bool> Delete()
        {
            var success = TypeExtension.DefaultBoolean;
            var fullUrll = new Uri(MyViewModelWebService.ToStringSafe().AddLast("/").AddLast(MyModel.ID.ToString()).AddLast("/"), UriKind.RelativeOrAbsolute);
            success = await Sender.SendDeleteAsync(fullUrll);
            if (success) MyModel = new TModel();
            return success;
        }
    }
}
    