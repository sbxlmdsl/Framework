//-----------------------------------------------------------------------
// <copyright file="ISaveAsync.cs" company="Genesys Source">
//      Copyright (c) Genesys Source. All rights reserved.
//      All rights are reserved. Reproduction or transmission in whole or in part, in
//      any form or by any means, electronic, mechanical or otherwise, is prohibited
//      without the prior written consent of the copyright owner.
// </copyright>
//-----------------------------------------------------------------------
using Genesys.Foundation.Entity;
using System;
using System.Threading.Tasks;

namespace Genesys.Foundation.Operation
{
    /// <summary>
    /// Write operation to a non-thread-safe datastore such as EF data context
    /// Includes all Save() and Delete() overloads, as well as Get..() methods
    /// </summary>
    /// <typeparam name="TEntity">Entity type to be read</typeparam>
    /// <typeparam name="TID">Type of identifier for this record. Typically int ID or Guid Key</typeparam>
    [CLSCompliant(true)]
    public interface ISaveAsync<TEntity, TID> : IGetAsync<TEntity, TID> where TEntity : IEntity where TID : struct
    {
        /// <summary>
        /// Saves the entity to the datastore
        /// </summary>
        /// <returns>Returns TEntity exactly as the data has been stored</returns>
        Task<TEntity> Save(bool forceInsert);

        /// <summary>
        /// Inserts or Updates this object in the database
        /// </summary>
        /// <returns>Object updated and all current values as of the save</returns>
        Task<TEntity> Save();

        /// <summary>
        /// Deletes the item from the datastore, using one or more identifiers in TEntity
        /// </summary>
        /// <returns></returns>
        Task<bool> Delete();
    }
}
