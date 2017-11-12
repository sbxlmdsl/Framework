//-----------------------------------------------------------------------
// <copyright file="ModelEntity.cs" company="Genesys Source">
//      Copyright (c) Genesys Source. All rights reserved.
//      All rights are reserved. Reproduction or transmission in whole or in part, in
//      any form or by any means, electronic, mechanical or otherwise, is prohibited
//      without the prior written consent of the copyright owner.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.ComponentModel;
using System.Reflection;
using Genesys.Extensions;
using Genesys.Extras.Serialization;

namespace Genesys.Foundation.Entity
{
	/// <summary>
	/// ModelBase
	/// </summary>
	/// <remarks>ModelBase</remarks>
	[CLSCompliant(true)]
	public abstract class ModelEntity<TModel> : IID, IKey, INotifyPropertyChanged where TModel : ModelEntity<TModel>, new()
	{
        /// <summary>
        /// Primary key of this record. Internal use only, use Key when exposing data externally.
        /// </summary>
        public virtual int ID { get; set; } = TypeExtension.DefaultInteger;

        /// <summary>
        /// Primary key of this record. To be used for external use.
        /// </summary>
        public virtual Guid Key { get; set; } = TypeExtension.DefaultGuid;

        /// <summary>
        /// Constructor
        /// </summary>
        public ModelEntity() : base() { this.Initialize<ModelEntity<TModel>>(); }
		
        /// <summary>
        /// Fired when property is changed
        /// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Save and raise changed event
		/// </summary>
		/// <param name="propertyName"></param>
		/// <remarks></remarks>
		protected void NotifyPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null) {
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		
		/// <summary>
		/// Fills this object with another object's data (of the same type)
		/// </summary>
		/// <param name="newItem"></param>
		/// <remarks></remarks>
		public bool Equals(TModel newItem)
		{
			bool returnValue = TypeExtension.DefaultBoolean;
			Type newObjectType = newItem.GetType();

			// Start True
			returnValue = true;
			// Loop through all new item's properties
			foreach (PropertyInfo newObjectProperty in newObjectType.GetRuntimeProperties()) {
				// Copy the data using reflection
				PropertyInfo currentProperty = typeof(TModel).GetRuntimeProperty(newObjectProperty.Name);
				if (currentProperty.CanWrite == true) {
					// Check for equivalence
					if (object.Equals(currentProperty.GetValue(this, null), newObjectProperty.GetValue(newItem, null)) == false) {
						returnValue = false;
						break;
					}
				}
			}

			// Return data
			return returnValue;
		}
		
		/// <summary>
		/// Start with ID as string representation
		/// </summary>
		/// <returns></returns>
		/// <remarks></remarks>
		public override string ToString()
		{
			return this.ID.ToString();
		}
        
        /// <summary>
        /// Serializes this object into a Json string
        /// </summary>
        /// <returns></returns>
        public string Serialize()
        {
            JsonSerializer<TModel> serializer = new JsonSerializer<TModel>();
            return serializer.Serialize(this);
        }

        /// <summary>
        /// De-serializes a string into this object
        /// </summary>
        /// <returns></returns>
        public static TModel Deserialize(string jsonString)
        {
            JsonSerializer<TModel> serializer = new JsonSerializer<TModel>();
            return serializer.Deserialize(jsonString);
        }
    }
}
