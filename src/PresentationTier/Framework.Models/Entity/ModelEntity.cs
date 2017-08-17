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
using Genesys.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Genesys.Framework.Entity
{
	/// <summary>
	/// ModelBase
	/// </summary>
	/// <remarks>ModelBase</remarks>
	[CLSCompliant(true)]
	public abstract class ModelEntity<TModel> : EntityInfo<TModel>, INotifyPropertyChanged where TModel : ModelEntity<TModel>, new()
	{
        private int idField = TypeExtension.DefaultInteger;
        private Guid keyField = TypeExtension.DefaultGuid;

        /// <summary>
        /// Primary key of this record. Internal use only, use Key when exposing data externally.
        /// </summary>
        public override int ID { get { return idField; } set { SetField(ref idField, value); } }

        /// <summary>
        /// Primary key of this record. To be used for external use.
        /// </summary>
        public override Guid Key { get { return keyField; } set { SetField(ref keyField, value); } }

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
    }
}
