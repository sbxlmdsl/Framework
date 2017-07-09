//-----------------------------------------------------------------------
// <copyright file="IGetAsync.cs" company="Genesys Source">
//      Copyright (c) Genesys Source. All rights reserved.
//      All rights are reserved. Reproduction or transmission in whole or in part, in
//      any form or by any means, electronic, mechanical or otherwise, is prohibited
//      without the prior written consent of the copyright owner.
// </copyright>
//-----------------------------------------------------------------------
using Genesys.Foundation.Entity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Genesys.Foundation.Operation
{
    /// <summary>
    /// Read operations against an Async datastore, such as Http resource server
    /// Requires "pure API" narrowing 1-1 record identifier to either int ID or Guid Key:
    ///  For Internal, high-performance, multi-join lookups: int ID
    ///  For External, low-volume tables, obfuscated, guaranteed-unique: Guid Key
    /// </summary>
    /// <typeparam name="TEntity">Entity type to be read</typeparam>
    /// <typeparam name="TID">Type of identifier for this record. Typically int ID or Guid Key</typeparam>
    [CLSCompliant(true)]
    public interface IGetAsync<TEntity, TID> where TEntity : IEntity where TID : struct
    {
        /// <summary>
        /// Gets one or more items from the datastore
        ///  Usually implemented with a built-in Take/Top cap to avoid accidental huge selects
        /// </summary>
        /// <returns></returns>
        Task<TEntity> Get(TID id);

        /// <summary>
        /// All data in this datastore subset
        /// Usage: Expected to be a small lookup list, or to be protected against excessive records
        /// </summary>
        Task<IEnumerable<TEntity>> GetAll();

        /// <summary>
        /// All data in this datastore subset, except records with default ID/Key
        ///  Criteria: Where ID != TypeExtension.DefaultInteger And Also Key != TypeExtension.DefaultGuid
        ///  Goal: To exclude "Not Selected" records from lookup tables
        /// </summary>
        Task<IEnumerable<TEntity>> GetAllExcludeDefault();
    }
}
