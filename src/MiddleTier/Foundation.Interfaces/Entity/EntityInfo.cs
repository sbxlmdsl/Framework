//-----------------------------------------------------------------------
// <copyright file="EntityInfo.cs" company="Genesys Source">
//      Copyright (c) Genesys Source. All rights reserved.
//      All rights are reserved. Reproduction or transmission in whole or in part, in
//      any form or by any means, electronic, mechanical or otherwise, is prohibited
//      without the prior written consent of the copyright owner.
// </copyright>
//-----------------------------------------------------------------------
using Genesys.Extensions;
using Genesys.Extras.Serialization;
using System;
using System.Reflection;
using Genesys.Foundation.Validation;

namespace Genesys.Foundation.Entity
{
    /// <summary>
    /// ReadEntityBase
    /// </summary>
    /// <remarks>ReadEntityBase</remarks>
    [CLSCompliant(true)]
    public abstract class EntityInfo<TEntity> : Validator<TEntity>, IEntity where TEntity : class, IEntity, new()
    {
        /// <summary>
        /// ID of record
        /// </summary>
        public virtual int ID { get; set; } = TypeExtension.DefaultInteger;

        /// <summary>
        /// Guid of record
        /// </summary>
        public virtual Guid Key { get; set; } = TypeExtension.DefaultGuid;

        /// <summary>
        /// Workflow activity that created this record
        /// </summary>
        public virtual int ActivityContextID { get; set; } = TypeExtension.DefaultInteger;

        /// <summary>
        /// Date record was created
        /// </summary>
        public virtual DateTime CreatedDate { get; set; } = TypeExtension.DefaultDate;

        /// <summary>
        /// Date record was modified
        /// </summary>
        public virtual DateTime ModifiedDate { get; set; } = TypeExtension.DefaultDate;

        /// <summary>
        /// Status of this record
        /// </summary>
        public virtual EntityStates Status { get; set; } = EntityStates.Default;

        /// <summary>
        /// Is this a new object not yet committed to the database (ID == -1)
        /// </summary>
        public virtual bool IsNew
        {
            get
            {
                return (this.ID == TypeExtension.DefaultInteger && this.Key == TypeExtension.DefaultGuid ? true : false);
            }
        }

        /// <summary>
        /// Serialize and Deserialize this class
        /// </summary>
        private JsonSerializer<EntityInfo<TEntity>> serializer = new JsonSerializer<EntityInfo<TEntity>>();

        /// <summary>
        /// Forces initialization of EF-generated properties (PropertyValue = TypeExtension.Default{Type})
        /// </summary>
        public EntityInfo() : base() { this.Initialize<EntityInfo<TEntity>>(); }

        /// <summary>
        /// Fills this object with another object's data (of the same type)
        /// </summary>
        /// <param name="newItem"></param>
        /// <remarks></remarks>
        public bool Equals(TEntity newItem)
        {
            bool returnValue = TypeExtension.DefaultBoolean;
            Type newObjectType = newItem.GetType();

            // Start True
            returnValue = true;
            // Loop through all new item's properties
            foreach (var newObjectProperty in newObjectType.GetRuntimeProperties())
            {
                // Copy the data using reflection
                PropertyInfo currentProperty = typeof(TEntity).GetRuntimeProperty(newObjectProperty.Name);
                if (currentProperty.CanWrite == true)
                {
                    // Check for equivalence
                    if (object.Equals(currentProperty.GetValue(this, null), newObjectProperty.GetValue(newItem, null)) == false)
                    {
                        returnValue = false;
                        break;
                    }
                }
            }

            // Return data
            return returnValue;
        }

        /// <summary>
        /// Null-safe cast to the type TEntity
        /// </summary>
        /// <returns>This object casted to type TEntity</returns>
        public TEntity ToEntity()
        {
            return this.DirectCastSafe<TEntity>();
        }

        /// <summary>
        /// Start with ID as string representation
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public override string ToString()
        {
            return Key.ToString();
        }
        
        /// <summary>
        /// Serializes this object into a Json string
        /// </summary>
        /// <returns></returns>
        public string Serialize()
        {
            return serializer.Serialize(this);
        }

        /// <summary>
        /// De-serializes a string into this object
        /// </summary>
        /// <returns></returns>
        public IEntity Deserialize(string data)
        {
            return serializer.Deserialize(data);
        }
    }
}
