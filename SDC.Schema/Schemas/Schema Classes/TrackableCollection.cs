// ------------------------------------------------------------------------------
//  <auto-generated>
//    Generated by Xsd2Code++. Version 5.1.1.0. www.xsd2code.com
//  </auto-generated>
// ------------------------------------------------------------------------------
#pragma warning disable
namespace SDC.Schema
{
using System;
using System.Diagnostics;
using System.Xml.Serialization;
using System.Collections;
using System.Xml.Schema;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using System.Reflection;
using System.IO;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Xml;
using System.Collections.Generic;

#region TrackableCollection class
public class TrackableCollection<T> : ObservableCollection<T>

{
    
        /// <summary>
        /// Name of property
        /// </summary>
        private string propertyNameField;

        /// <summary>
        /// Occurs when [trackable collection changed].
        /// </summary>
        public virtual event NotifyTrackableCollectionChangedEventHandler TrackableCollectionChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="TrackableCollection&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        public TrackableCollection(string propertyName)
        {
            propertyNameField = propertyName;
            base.CollectionChanged += TrackableCollection_CollectionChanged;
        }

        /// <summary>
        /// Handles the CollectionChanged event of the TrackableCollection control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Collections.Specialized.NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
        void TrackableCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (this.TrackableCollectionChanged != null)
            {
                this.TrackableCollectionChanged(sender, e, this.propertyNameField);
            }
        }


}
#endregion
}
#pragma warning restore
