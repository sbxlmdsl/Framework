//-----------------------------------------------------------------------
// <copyright file="ModelEntity.cs" company="Genesys Source">
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
using System;
using System.ComponentModel;
using System.Reflection;
using Genesys.Extensions;
using Genesys.Extras.Serialization;
using System.Runtime.CompilerServices;
using System.Collections.Generic;

namespace Genesys.Foundation.Entity
{
	/// <summary>
	/// ModelBase
	/// </summary>
	/// <remarks>ModelBase</remarks>
	[CLSCompliant(true)]
	public abstract class ModelEntity<TModel> : IID, IKey, INotifyPropertyChanged where TModel : ModelEntity<TModel>, new()
	{
        private int idField = TypeExtension.DefaultInteger;
        private Guid keyField = TypeExtension.DefaultGuid;
        /// <summary>
        /// Primary key of this record. Internal use only, use Key when exposing data externally.
        /// </summary>
        public virtual int ID { get { return idField; } set { SetField(ref idField, value); } }

        /// <summary>
        /// Primary key of this record. To be used for external use.
        /// </summary>
        public virtual Guid Key { get { return keyField; } set { SetField(ref keyField, value); } }

        /// <summary>
        /// Constructor
        /// </summary>
        public ModelEntity() : base() { this.Initialize<ModelEntity<TModel>>(); }

        /// <summary>
        /// Property changed event handler for INotifyPropertyChanged
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Property changed event for INotifyPropertyChanged
        /// </summary>
        /// <param name="propertyName">String name of property</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Sets the property data as well as fired OnPropertyChanged() for INotifyPropertyChanged
        /// </summary>
        /// <typeparam name="TField"></typeparam>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        protected bool SetField<TField>(ref TField field, TField value,
        [CallerMemberName] string propertyName = null)
        {
            var returnValue = TypeExtension.DefaultBoolean;
            if (EqualityComparer<TField>.Default.Equals(field, value) == false)
            {
                field = value;
                OnPropertyChanged(propertyName);
                returnValue = true;
            }                
            return returnValue;
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
			foreach (var newObjectProperty in newObjectType.GetRuntimeProperties()) {
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
			return ID.ToString();
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
