//-----------------------------------------------------------------------
// <copyright file="SaveableControl.cs" company="Genesys Source">
//      Copyright (c) 2017 Genesys Source. All rights reserved.
//      All rights are reserved. Reproduction or transmission in whole or in part, in
//      any form or by any means, electronic, mechanical or otherwise, is prohibited
//      without the prior written consent of the copyright owner.
// </copyright>
//-----------------------------------------------------------------------
using Genesys.Framework.Worker;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics.CodeAnalysis;

namespace Genesys.Framework.UserControls
{
    /// <summary>
    /// Screen base for authenticated users
    /// </summary>
    public abstract class SaveableControl : ReadOnlyControl
    {
        /// <summary>
        /// OK cancel buttons for this processing screen
        /// </summary>
        protected abstract OkCancel OkCancel { get; set; } //ToDo: Fix

        /// <summary>
        /// Result of the screens process
        /// </summary>
        public WorkerResult Result { get; private set; } = new WorkerResult();

        /// <summary>
        /// Constructor
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SaveableControl() : base()
        {
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
    }
}