//-----------------------------------------------------------------------
// <copyright file="TopMenu.cs" company="Genesys Source">
//      Copyright (c) 2017 Genesys Source. All rights reserved.
//      All rights are reserved. Reproduction or transmission in whole or in part, in
//      any form or by any means, electronic, mechanical or otherwise, is prohibited
//      without the prior written consent of the copyright owner.
// </copyright>
//-----------------------------------------------------------------------
using Genesys.Framework.Entity;
using Genesys.Framework.Pages;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Genesys.Framework.UserControls
{
    /// <summary>
    /// Top bar with title and back button
    /// </summary>
    
    public sealed partial class TopMenu : ReadOnlyControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public TopMenu()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Binds controls to the data 
        /// </summary>
        /// <param name="modelData"></param>
        protected override void BindModelData(object modelData)
        {
        }

        /// <summary>
        /// Partial and controls have been loaded
        /// </summary>
        /// <param name="sender">Sender of this event call</param>
        /// <param name="e">Event arguments</param>
        protected override void Partial_Loaded(object sender, EventArgs e)
        {
            base.Partial_Loaded(sender, e);
        }

        /// <summary>
        /// Navigates to CustomerSummary screen, passing a test Customer to bind and display
        /// </summary>
        /// <param name="sender">Sender of event</param>
        /// <param name="e">Event arguments</param>
        private void Search_Click(object sender, RoutedEventArgs e)
        {            

        }

        /// <summary>
        /// Navigates to CustomerCreate screen, passing a test Customer to bind and display
        /// </summary>
        /// <param name="sender">Sender of event</param>
        /// <param name="e">Event arguments</param>
        private void Create_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Navigates to CustomerEdit screen, passing a test Customer to bind and display
        /// </summary>
        /// <param name="sender">Sender of event</param>
        /// <param name="e">Event arguments</param>
        private void Edit_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Navigates to CustomerDelete screen, passing a test Customer to bind and display
        /// </summary>
        /// <param name="sender">Sender of event</param>
        /// <param name="e">Event arguments</param>
        private void Delete_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Validate this control
        /// </summary>
        /// <returns></returns>
        public override bool Validate()
        {
            return true;
        }
    }
}