//-----------------------------------------------------------------------
// <copyright file="IReadOperation.cs" company="Genesys Source">
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
using Genesys.Foundation.Data;
using Genesys.Foundation.Entity;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Genesys.Foundation.Operation
{
    /// <summary>
    /// Data access entity that can only read
    /// </summary>
    [CLSCompliant(true)]
    public interface IReadOperation<TEntity> : IReadOnlyDatastore<TEntity> where TEntity : IEntity
    {
        /// <summary>
        /// Get entities list by where clause
        /// </summary>
        /// <param name="whereClause">Where clause expression</param>
        /// <returns>Roughly: Entity.Where(whereClause)</returns>
        IQueryable<TEntity> GetByWhere(Expression<Func<TEntity, Boolean>> whereClause);

        /// <summary>
        /// Get entities list with paging system
        /// </summary>
        /// <param name="whereClause">Where clause expression</param>
        /// /// <param name="orderByClause">Order by clause expression</param>
        /// /// <param name="pageSize">Max number of results to be returned and in each page</param>
        /// /// <param name="pageNumber">Which page to retrieve</param>
        /// <returns>Roughly: Entity.Where(whereClause).OrderBy(orderByClause).Skip(pageSize*pageNumger).Take(pageSize)</returns>
        IQueryable<TEntity> GetByPage(Expression<Func<TEntity, Boolean>> whereClause, Expression<Func<TEntity, Boolean>> orderByClause, int pageSize, int pageNumber);
    }
}
