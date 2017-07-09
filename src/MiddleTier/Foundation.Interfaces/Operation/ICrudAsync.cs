//-----------------------------------------------------------------------
// <copyright file="ICrudAsync.cs" company="Genesys Source">
//      Copyright (c) Genesys Source. All rights reserved.
//      All rights are reserved. Reproduction or transmission in whole or in part, in
//      any form or by any means, electronic, mechanical or otherwise, is prohibited
//      without the prior written consent of the copyright owner.
// </copyright>
//-----------------------------------------------------------------------
using Genesys.Foundation.Entity;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Genesys.Foundation.Operation
{
    /// <summary>
    /// CRUD operations
    /// Create, Read, Update, Delete.
    ///  Purpose is to encapsulate IGetAsync and CRUD methods 
    ///   for asyncronous datastore access, primarily via Http
    /// </summary>
    /// <typeparam name="TEntity">Type of class supporting CRUD methods</typeparam>
    /// <typeparam name="TID">Type of identifier for this record. Typically int ID or Guid Key</typeparam>
    [CLSCompliant(true)]
    public interface ICrudAsync<TEntity, TID> : ISaveAsync<TEntity, TID> where TEntity : IEntity where TID : struct
    {
        /// <summary>
        /// Create operation on the object
        /// </summary>
        /// <returns></returns>
        Task<TEntity> CreateAsync();

        /// <summary>
        /// Retrieve TEntity objects operation
        /// </summary>
        /// <param name="key">Expression to query the datastore</param>
        /// <returns></returns>
        Task<TEntity> ReadAsync(TID key);

        /// <summary>
        /// Update the object
        /// </summary>
        Task<TEntity> UpdateAsync();

        /// <summary>
        /// Deletes operation on this entity
        /// </summary>
        Task<bool> DeleteAsync();      
    }
}
