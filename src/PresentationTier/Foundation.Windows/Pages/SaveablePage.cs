//-----------------------------------------------------------------------
// <copyright file="SaveablePage.cs" company="Genesys Source">
//      Copyright (c) 2017 Genesys Source. All rights reserved.
//      All rights are reserved. Reproduction or transmission in whole or in part, in
//      any form or by any means, electronic, mechanical or otherwise, is prohibited
//      without the prior written consent of the copyright owner.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Threading.Tasks;
using System.Windows;
using Genesys.Foundation.UserControls;
using Genesys.Foundation.Worker;
using System.Windows.Input;

namespace Genesys.Foundation.Pages
{
    /// <summary>
    /// Screen base for authenticated users
    /// </summary>
    public abstract class SaveablePage : ReadOnlyPage
    {
        /// <summary>
        /// OK cancel buttons for this processing screen
        /// </summary>
        protected abstract OkCancel OkCancel { get; set; }

        /// <summary>
        /// Result of the screens process
        /// </summary>
        public WorkerResult Result { get; private set; } = new WorkerResult();
        
        /// <summary>
        /// Constructor
        /// </summary>
        public SaveablePage() : base()
        {
        }

        /// <summary>
        /// Page Loaded, can interact with controls as everything is loaded
        /// </summary>
        /// <param name="sender">Sender of event</param>
        /// <param name="e">Event arguments</param>
        protected override void Page_Loaded(object sender, EventArgs e)
        {
            base.Page_Loaded(sender, e);
            OkCancel.OnOKClicked += OK_Click;
            OkCancel.OnCancelClicked += Cancel_Click;
        }

        /// <summary>
        /// OK button
        /// </summary>
        /// <param name="sender">Sender of event</param>
        /// <param name="e">Event arguments</param>
        protected async void OK_Click(object sender, RoutedEventArgs e)
        {
            OkCancel.StartProcessing();
            Result = await this.Process(sender, e);
            OkCancel.StopProcessing(this.Result);
        }

        /// <summary>
        /// Cancel Button
        /// </summary>
        /// <param name="sender">Sender of event</param>
        /// <param name="e">Event arguments</param>
        protected void Cancel_Click(object sender, RoutedEventArgs e)
        {
            OkCancel.CancelProcessing();
            Cancel(sender, e);
        }
        
        /// <summary>
        /// Processes a forms business function
        /// </summary>
        public abstract Task<WorkerResult> Process(object sender, RoutedEventArgs e);

        /// <summary>
        /// Processes a forms business function
        /// </summary>
        public abstract void Cancel(object sender, RoutedEventArgs e);

        /// <summary>
        /// Map the enter key
        /// </summary>
        /// <param name="sender">Sender of event</param>
        /// <param name="e">Event arguments</param>
        protected virtual void MapEnterKey(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && e.IsRepeat == false)
            {
                OK_Click(sender, e);
            }
        }
    }
}