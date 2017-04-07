//-----------------------------------------------------------------------
// <copyright file="IWriteOperation.cs" company="Genesys Source">
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
using Genesys.Foundation.Activity;
using Genesys.Foundation.Data;
using Genesys.Foundation.Entity;
using System;

namespace Genesys.Foundation.Operation
{
    /// <summary>
    /// Interface for a fully-featured write operation
    /// Includes all Save() and Delete() overloads
    /// Use this interface to support all standard reading and writing methods
    /// </summary>
    [CLSCompliant(true)]
    public interface IWriteOperation<TEntity> : IReadOperation<TEntity>, ISaveableDatastore<TEntity> where TEntity : IEntity
    {
        /// <summary>
        /// Inserts or Updates this object in the database
        /// </summary>
        /// <returns>Object updated and all current values as of the save</returns>
        TEntity Save(TEntity entity);

        /// <summary>
        /// Inserts or Updates this object with Workflow-based tracking.
        /// </summary>
        /// <param name="entity">Entity to be committed to the datastore</param>        
        /// <param name="activity">Activity record owning this process</param>        
        TEntity Save(TEntity entity, IActivityContext activity);
    }
}
