//-----------------------------------------------------------------------
// <copyright file="ISaveAsync.cs" company="Genesys Source">
//      Copyright (c) Genesys Source. All rights reserved.
//      All rights are reserved. Reproduction or transmission in whole or in part, in
//      any form or by any means, electronic, mechanical or otherwise, is prohibited
//      without the prior written consent of the copyright owner.
// </copyright>
//-----------------------------------------------------------------------
using Genesys.Foundation.Activity;
using Genesys.Foundation.Entity;
using System;

namespace Genesys.Foundation.Operation
{
    /// <summary>
    /// Write operation to a non-thread-safe datastore such as EF data context
    /// Includes all Save() and Delete() overloads, as well as Get..() methods
    /// </summary>
    [CLSCompliant(true)]
    public interface ISaveOperation<TEntity> : IGetOperation<TEntity> where TEntity : IEntity
    {
        /// <summary>
        /// Saves the entity to the datastore
        /// </summary>
        /// <returns>Returns TEntity exactly as the data has been stored</returns>
        TEntity Save(TEntity entity, bool forceInsert);

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

        /// <summary>
        /// Deletes the item from the datastore, using one or more identifiers in TEntity
        /// </summary>
        /// <returns></returns>
        bool Delete(TEntity entity);
    }
}
