//-----------------------------------------------------------------------
// <copyright file="CrudViewModel.cs" company="Genesys Source">
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
using Genesys.Foundation.Entity;
using Genesys.Foundation.Operation;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace Genesys.Foundation.Application
{
    /// <summary>
    /// ViewModel holds model and is responsible for server calls, navigation, etc.
    /// </summary>
    public abstract class CrudViewModel<TModel> : ViewModel<TModel>, ICrudAsync<TModel, int> where TModel : ModelEntity<TModel>, new()
    {
        /// <summary>
        /// Constructs a MVVM view-model that stores MyModel, MyApplication and MyWebService
        ///  for self-sufficient page navigation, view binding and web service calls.
        /// </summary>
        /// <param name="myWebServiceController">Controller name uri part to append to http://{MyWebService}/{ControllerName}</param>
        public CrudViewModel(string myWebServiceController)
            : base(myWebServiceController)
        {
        }

        /// <summary>
        /// Reads record based on Guid Key.
        /// int  TModel.ID is recommended for internal, on-line, high-performance use
        /// Guid TModel.Key is recommended for external, near-line, off-line use 
        /// </summary>
        /// <param name="id">Integer Key value that matches the record key</param>
        /// <returns>If found, TModel hydrated from database. If not found, new TModel()</returns>
        public async Task<TModel> ReadAsync(int id)
        {
            var returnData = new TModel();
            returnData = await base.Get(id);
            return returnData;
        }

        /// <summary>
        /// Create a record
        /// </summary>
        /// <returns></returns>
        public async Task<TModel> CreateAsync()
        {
            TModel returnData = new TModel();
            returnData = await base.Save(true);
            return returnData;
        }

        /// <summary>
        /// Edits a record
        /// </summary>
        /// <returns></returns>
        public async Task<TModel> UpdateAsync()
        {
            TModel returnData = new TModel();
            returnData = await base.Save(false);
            return returnData;
        }

        /// <summary>
        /// Pulls data by ID
        /// This is synonymous to ControllerName's HttpGet behavior
        /// </summary>
        /// <returns>True if successful delete</returns>
        public async Task<bool> DeleteAsync()
        {
            return await base.Delete();
        }
    }
}
