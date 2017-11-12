﻿//-----------------------------------------------------------------------
// <copyright file="ReadOnlyPage.cs" company="Genesys Source">
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
using Genesys.Framework.Application;
using Genesys.Extensions;
using Genesys.Extras.Collections;
using Genesys.Extras.Net;
using Genesys.Framework.Validation;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Navigation;

namespace Genesys.Framework.Pages
{
    /// <summary>
    /// Screen base for authenticated users
    /// </summary>    
    public abstract class ReadOnlyPage : Page
    {
        /// <summary>
        /// Currently running application
        /// </summary>
        public WpfApplication MyApplication { get { return (WpfApplication)System.Windows.Application.Current; } }

        /// <summary>
        /// Sender of main Http Verbs
        /// </summary>
        public HttpVerbSender HttpSender { get; set; } = new HttpVerbSender();

        /// <summary>
        /// Name of the controller used in web service calls
        /// </summary>
        public abstract string ControllerName { get; }

        /// <summary>
        /// Uri to currently active frame/page
        /// </summary>
        public Uri CurrentPage { get { return MyApplication.CurrentPage; } }

        /// <summary>
        /// Throws Exception if any UI elements overrun their text max length
        /// </summary>
        public bool ThrowExceptionOnTextOverrun { get; set; } = TypeExtension.DefaultBoolean;

        /// <summary>
        /// New model to load
        /// </summary>
        public event NewModelReceivedEventHandler NewModelReceived;

        /// <summary>
        /// OnNewModelReceived()
        /// </summary>
        protected virtual void OnNewModelReceived(object newModel)
        {
            if (this.NewModelReceived != null)
            {
                NewModelReceived(this, new NewModelReceivedEventArgs() { NewModelData = newModel });
            }
        }

        /// <summary>
        /// Workflow beginning. No custom to return.
        /// </summary>
        /// <param name="sender">Sender of event</param>
        /// <param name="e">Event arguments</param>
        public delegate void NewModelReceivedEventHandler(object sender, NewModelReceivedEventArgs e);

        /// <summary>
        /// Event Args for loading and binding new model data
        /// </summary>
        public class NewModelReceivedEventArgs : EventArgs
        {
            /// <summary>
            /// New model data
            /// </summary>
            public object NewModelData { get; set; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        protected ReadOnlyPage() : base()
        {
#if (DEBUG)
            ThrowExceptionOnTextOverrun = true;
#endif
            Loaded += Page_Loaded;
            SizeChanged += Page_SizeChanged;
            NewModelReceived += Page_ModelReceived;
        }

        /// <summary>
        /// Binds all model data to the screen controls and sets MyViewModel.MyModel property
        /// </summary>
        /// <param name="modelData">Model data to bind</param>
        protected abstract void BindModel(object modelData);

        /// <summary>
        /// Binds all model data to the screen controls and sets MyViewModel.MyModel property
        /// </summary>
        /// <param name="sender">Sender of event</param>
        /// <param name="e">Event arguments</param>
        protected abstract void Page_ModelReceived(object sender, NewModelReceivedEventArgs e);

        /// <summary>
        /// Page_Load event handler
        /// </summary>
        /// <param name="sender">Sender of event</param>
        /// <param name="e">Event arguments</param>
        protected virtual void Page_Loaded(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Page_SizeChanged event handler
        /// </summary>
        /// <param name="sender">Sender of event</param>
        /// <param name="e">Event arguments</param>
        protected virtual void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
        }

        /// <summary>
        /// Navigated to
        /// </summary>
        /// <param name="sender">Sender of event</param>
        /// <param name="e">Event arguments</param>
        public virtual void NavigationService_LoadCompleted(object sender, NavigationEventArgs e)
        {
            OnNewModelReceived(e.ExtraData);
            if (NavigationService != null) { NavigationService.LoadCompleted -= NavigationService_LoadCompleted; }
        }

        /// <summary>
        /// Binds a string to a Image
        /// </summary>
        /// <param name="item">Item to bind</param>
        /// <param name="bindingProperty">String name of property holding the data</param>
        public void SetBinding(ref Image item, string bindingProperty)
        {
            item.SetBinding(Image.SourceProperty, new Binding() { Path = new PropertyPath(bindingProperty), Mode = BindingMode.OneWay });
        }


        /// <summary>
        /// Binds a string to a TextBlock
        /// </summary>
        /// <param name="item">Item to bind</param>
        /// <param name="initialValue">Initial value or selection</param>
        /// <param name="bindingProperty">String name of property holding the data</param>
        public void SetBinding(ref TextBlock item, string initialValue, string bindingProperty)
        {
            item.SetBinding(TextBlock.TextProperty, new Binding() { Path = new PropertyPath(bindingProperty), Mode = BindingMode.OneWay });
        }

        /// <summary>
        /// Binds a string to a TextBox
        /// </summary>
        /// <param name="item">Item to bind</param>
        /// <param name="initialValue">Initial value or selection</param>
        /// <param name="bindingProperty">String name of property holding the data</param>
        public void SetBinding(ref TextBox item, string initialValue, string bindingProperty)
        {
            // Handle for no state
            initialValue = initialValue.Replace(TypeExtension.DefaultInteger.ToString(), "")
                .Replace(TypeExtension.DefaultGuid.ToString(), "").Replace(TypeExtension.DefaultDate.ToString(), "");
            item.SetBinding(TextBox.TextProperty, new Binding() { Path = new PropertyPath(bindingProperty), Mode = BindingMode.TwoWay });
        }

        /// <summary>
        /// Binds a string to a TextBox
        /// </summary>
        /// <param name="item">Item to bind</param>
        /// <param name="initialValue">Initial value or selection</param>
        /// <param name="bindingProperty">String name of property holding the data</param>
        public void SetBinding(ref TextBox item, DateTime initialValue, string bindingProperty)
        {
            item.SetBinding(TextBox.TextProperty, new Binding() { Path = new PropertyPath(bindingProperty), Mode = BindingMode.TwoWay });
        }

        /// <summary>
        /// Binds a string to a PasswordBox
        /// </summary>
        /// <param name="item">Item to bind</param>
        /// <param name="initialValue">Initial value or selection</param>
        /// <param name="bindingProperty">String name of property holding the data</param>
        public void SetBinding(ref PasswordBox item, string initialValue, string bindingProperty)
        {
            item.SetBinding(PasswordBox.PasswordCharProperty, new Binding() { Path = new PropertyPath(bindingProperty), Mode = BindingMode.TwoWay });
        }

        /// <summary>
        /// Binds a string to a DatePicker
        /// </summary>
        /// <param name="item">Item to bind</param>
        /// <param name="initialValue">Initial value or selection</param>
        /// <param name="bindingProperty">String name of property holding the data</param>
        public void SetBinding(ref DatePicker item, DateTime initialValue, string bindingProperty)
        {
            item.SetBinding(DatePicker.TextProperty, new Binding() { Path = new PropertyPath(bindingProperty), Mode = BindingMode.TwoWay });
        }

        /// <summary>
        /// Binds a standard key-value pair to a ComboBox
        /// </summary>
        /// <param name="item">Item to bind</param>
        /// <param name="collection">List of elements to bind</param>
        /// <param name="selectedKey">Default item to select, by key</param>
        /// <param name="bindingProperty">String name of property holding the data</param>
        public void SetBinding(ref ComboBox item, List<KeyValuePair<int, string>> collection, int selectedKey, string bindingProperty)
        {
            item.ItemsSource = collection;
            item.DisplayMemberPath = "Value";
            item.SelectedValuePath = "Key";
            item.SetBinding(ComboBox.SelectedValueProperty, new Binding() { Path = new PropertyPath(bindingProperty), Mode = BindingMode.TwoWay });
            // Handle for no state
            if (collection.Count == 1)
            {
                selectedKey = TypeExtension.DefaultInteger;
            }
            item.SelectedValue = selectedKey;
        }

        /// <summary>
        /// Binds a standard key-value pair to a ComboBox
        /// </summary>
        /// <param name="item">Item to bind</param>
        /// <param name="collection">List of elements to bind</param>
        /// <param name="selectedKey">Default item to select, by key</param>
        /// <param name="bindingProperty">String name of property holding the data</param>
        public void SetBinding(ref ComboBox item, List<KeyValuePair<Guid, string>> collection, int selectedKey, string bindingProperty)
        {
            item.ItemsSource = collection;
            item.DisplayMemberPath = "Value";
            item.SelectedValuePath = "Key";
            item.SetBinding(ComboBox.SelectedValueProperty, new Binding() { Path = new PropertyPath(bindingProperty), Mode = BindingMode.TwoWay });
            // Handle for no state
            if (collection.Count == 1)
            {
                selectedKey = TypeExtension.DefaultInteger;
            }
            item.SelectedValue = selectedKey;
        }

        /// <summary>
        /// Binds a standard key-value pair to a ComboBox
        /// </summary>
        /// <param name="item">Item to bind</param>
        /// <param name="collection">List of elements to bind</param>
        /// <param name="selectedKey">Default item to select, by key</param>
        /// <param name="bindingProperty">String name of property holding the data</param>
        public void SetBinding(ref ComboBox item, KeyValueListString collection, string selectedKey, string bindingProperty)
        {
            item.ItemsSource = collection;
            item.DisplayMemberPath = "Value";
            item.SelectedValuePath = "Key";
            item.SetBinding(ComboBox.SelectedValueProperty, new Binding() { Path = new PropertyPath(bindingProperty), Mode = BindingMode.TwoWay });
            // Handle for no state
            if (collection.Count == 1) { selectedKey = collection.FirstOrDefaultSafe().Key; }
            item.SelectedValue = selectedKey;
        }

        /// <summary>
        /// Validates Text Message
        /// </summary>
        /// <param name="uiControl">Control holding original text</param>
        /// <param name="textMessage">Text to validate length</param>
        public bool ValidateTextLength(Control uiControl, string textMessage)
        {
            Validator<Control> controlValidator = new Validator<Control>();
            TextBlock testControl = new TextBlock() { Text = textMessage };
            controlValidator.BusinessRules.Add(new ValidationRule<Control>("ActualWidth", item => item.ActualHeight <= testControl.ActualHeight));
            return controlValidator.IsValid(uiControl);
        }

        /// <summary>
        /// Replaces TypeExtension.Default{Type} with String.Empty with a readable value
        /// Values to replace are: 01/01/1900, -1, 0.00, 00000000-0000-0000-0000-000000000000
        /// I.e. Replace -1 with "". Replace 0.00 with "Free"
        /// </summary>
        /// <param name="item">Original value to replace</param>
        /// <param name="replaceWith">Value to replace default values with</param>
        /// <returns></returns>
        public string ToReadable(string item, string replaceWith = "")
        {
            var returnValue = item;
            returnValue = item.Replace(TypeExtension.DefaultInteger.ToString(), "")
                .Replace(TypeExtension.DefaultGuid.ToString(), replaceWith)
                .Replace(TypeExtension.DefaultDecimal.ToString(), replaceWith)
                .Replace(TypeExtension.DefaultDate.ToString(), replaceWith);
            return returnValue;
        }
    }
}