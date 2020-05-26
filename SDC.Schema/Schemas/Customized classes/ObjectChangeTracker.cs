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
   using Newtonsoft.Json;

    #region Tracking changes class
    public class ObjectChangeTracker : System.ComponentModel.INotifyPropertyChanged
{
    
       #region  Fields
        private bool isDeserializingField;
        private ObjectState originalobjectStateField = ObjectState.Added;
        private bool isInitilizedField = false;
        private readonly ObservableCollection<PropertyValueState> changeSetsField = new ObservableCollection<PropertyValueState>();
        private Delegate collectionChangedDelegateField = null;

        private bool objectTrackingEnabledField;
        private readonly object trackedObjectField;

        private PropertyValueStatesDictionary propertyValueStatesFields;
        //private ValueStatesDictionary valueStatesField;

        private ObjectsAddedToCollectionProperties objectsAddedToCollectionsField = new ObjectsAddedToCollectionProperties();
        private ObjectsRemovedFromCollectionProperties objectsRemovedFromCollectionsField = new ObjectsRemovedFromCollectionProperties();
        private ObjectsOriginalFromCollectionProperties objectsOriginalFromCollectionsField = new ObjectsOriginalFromCollectionProperties();
        #endregion

        public ObjectChangeTracker(object trackedObject)
        {
            trackedObjectField = trackedObject;
        }

        #region Events

        public event EventHandler<ObjectStateChangingEventArgs> ObjectStateChanging;

        #endregion

        protected virtual void OnObjectStateChanging(ObjectState newState)
        {
            if (ObjectStateChanging != null)
            {
                ObjectStateChanging(this, new ObjectStateChangingEventArgs() { NewState = newState });
            }
        }

        /// <summary>
        /// indicate current state
        /// </summary>
        private ObjectState stateField;

        /// <summary>
        /// Gets the state.
        /// </summary>
        /// <value>The state.</value>
        [JsonIgnore]
        public ObjectState State
        {
            get
            {
                return stateField; 
            }
        }

        /// <summary>
        /// Updates the state of the change.
        /// </summary>
        private void UpdateChangeState()
        {
            ObjectState resultState = ObjectState.Added;
            this.changeSetsField.Clear();
            if (this.originalobjectStateField == ObjectState.Added)
            {
                if (stateField != ObjectState.Added)
                {
                    stateField = ObjectState.Added;
                    OnPropertyChanged("State");
                    OnObjectStateChanging(stateField);
                }
                return;
            }
            foreach (var propertyValuestate in PropertyValueStates)
            {
                if (propertyValuestate.Value.State == ObjectState.Modified)
                {
                    changeSetsField.Add(propertyValuestate.Value);
                    resultState = ObjectState.Modified;
                }
            }
            if (ObjectsRemovedFromCollectionProperties.Count > 0)
            {
                foreach (var objectsRemovedFromCollectionProperty in ObjectsRemovedFromCollectionProperties)
                {
                    foreach (var objectsRemoved in objectsRemovedFromCollectionProperty.Value)
                    {
                        changeSetsField.Add(new PropertyValueState() { PropertyName = objectsRemovedFromCollectionProperty.Key, State = ObjectState.Deleted, CurrentValue = objectsRemoved.ToString() });
                    }
                }
                resultState = ObjectState.Modified;
            }

            if (ObjectsAddedToCollectionProperties.Count > 0)
            {
                foreach (var objectsAddedFromCollectionProperty in ObjectsAddedToCollectionProperties)
                {
                    foreach (var objectsAdded in objectsAddedFromCollectionProperty.Value)
                    {
                        changeSetsField.Add(new PropertyValueState() { PropertyName = objectsAddedFromCollectionProperty.Key, State = ObjectState.Added, CurrentValue = objectsAdded.ToString() });
                    }
                }
                resultState = ObjectState.Modified;
            }

            if (resultState == ObjectState.Modified)
            {
                if (stateField != ObjectState.Modified)
                {
                    stateField = ObjectState.Modified;                    
                    OnPropertyChanged("State");
                    OnObjectStateChanging(stateField);
                }
                return;
            }
            if (stateField != this.originalobjectStateField)
            {
                stateField = this.originalobjectStateField;                
                OnPropertyChanged("State");
                OnObjectStateChanging(stateField);
            }
            return;
        }

        [JsonIgnore]
        public ObservableCollection<PropertyValueState> ChangeSets 
        {
            get
            {
                return this.changeSetsField;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [change tracking enabled].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [change tracking enabled]; otherwise, <c>false</c>.
        /// </value>
        [JsonIgnore]
        public bool ObjectTrackingEnabled
        {
            get { return objectTrackingEnabledField; }
        }

        /// <summary>
        /// Starts record changes.
        /// </summary>
        public void StartTracking()
        {
            objectTrackingEnabledField = true;
            ResetTracking();
        }

        /// <summary>
        /// Starts record changes.
        /// </summary>
        public void StartTracking(bool keepLastRecords)
        {
            objectTrackingEnabledField = true;
            if (!keepLastRecords)
                StartTracking();
        }

        /// <summary>
        /// Starts record changes.
        /// </summary>
        public void StopTracking()
        {
            objectTrackingEnabledField = false;
        }

        // Resets the ObjectChangeTracker to the Unchanged state and
        // clears the original values as well as the record of changes
        // to collection properties
        public void ResetTracking()
        {
            if (this.objectTrackingEnabledField)
            {
                this.originalobjectStateField = ObjectState.Unchanged;
                ResetOriginalValue();
                ObjectsAddedToCollectionProperties.Clear();
                ObjectsRemovedFromCollectionProperties.Clear();
                ObjectsOriginalFromCollectionProperties.Clear();
                InitOriginalValue();
                UpdateChangeState();
            }
        }
        /// <summary>
        /// Inits the original value.
        /// </summary>
        private void InitOriginalValue()
        {
            var type = trackedObjectField.GetType();
            var propertyies = type.GetProperties();

            if (!isInitilizedField)
            {
                foreach (var propertyInfo in propertyies)
                {
                    if (!propertyInfo.Name.Equals("ChangeTracker") && !propertyInfo.Name.Equals("Item"))
                    {
                        object o = propertyInfo.GetValue(trackedObjectField, null);
                        if (propertyInfo.PropertyType.Name.Contains("TrackableCollection"))
                        {
                            var eventInfo = propertyInfo.PropertyType.GetEvent("TrackableCollectionChanged");
                            if (eventInfo != null)
                            {
                                Type tDelegate = eventInfo.EventHandlerType;
                                if (tDelegate != null)
                                {
                                    if (collectionChangedDelegateField == null)
                                        collectionChangedDelegateField = Delegate.CreateDelegate(tDelegate, this, "TrackableCollectionChanged");

                                    // Get the "add" accessor of the event and invoke it late bound, passing in the delegate instance. This is equivalent
                                    // to using the += operator in C#. The instance on which the "add" accessor is invoked.
                                    MethodInfo addHandler = eventInfo.GetAddMethod();
                                    Object[] addHandlerArgs = { collectionChangedDelegateField };
                                    addHandler.Invoke(o, addHandlerArgs);
                                }
                            }

                            var collection = o as IEnumerable;
                            if (collection != null)
                            {
                                foreach (var item in collection)
                                {
                                    RecordOriginalToCollectionProperties(propertyInfo.Name, item);
                                }
                            }
                        }
                        else
                        {
                            RecordCurrentValue(propertyInfo.Name, o);
                        }
                    }
                }
                isInitilizedField = true;
            }
        }

        /// <summary>
        /// Resets the original value.
        /// </summary>
        private void ResetOriginalValue()
        {
            PropertyValueStates.Clear();
            //ValueStates.Clear();

            if (isInitilizedField)
            {
                var type = trackedObjectField.GetType();
                var propertyies = type.GetProperties();
                foreach (var propertyInfo in propertyies)
                {
                    if (!propertyInfo.Name.Equals("ChangeTracker") && !propertyInfo.Name.Equals("Item"))
                    {
                        object o = propertyInfo.GetValue(trackedObjectField, null);
                        if (propertyInfo.PropertyType.Name.Contains("TrackableCollection"))
                        {
                            var eventInfo = propertyInfo.PropertyType.GetEvent("TrackableCollectionChanged");
                            if (eventInfo != null)
                            {
                                Type tDelegate = eventInfo.EventHandlerType;
                                if (tDelegate != null)
                                {
                                    if (collectionChangedDelegateField != null)
                                    {
                                        // Get the "remove" accessor of the event and invoke it latebound, passing in the delegate instance. This is equivalent
                                        // to using the += operator in C#. The instance on which the "add" accessor is invoked.
                                        MethodInfo removeHandler = eventInfo.GetRemoveMethod();
                                        Object[] addHandlerArgs = { collectionChangedDelegateField };
                                        removeHandler.Invoke(o, addHandlerArgs);
                                    }
                                }
                            }
                        }
                    }
                }
                isInitilizedField = false;
            }
        }

        /// <summary>
        /// Trackables the collection changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="NotifyTrackableCollectionChangedEventArgs"/> instance containing the event data.</param>
        /// <param name="propertyName">Name of the property.</param>
        private void TrackableCollectionChanged(object sender, NotifyCollectionChangedEventArgs e, string propertyName)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var newItem in e.NewItems)
                    {
                        //todo:implémenter la récursivité sur l'ajout des élements dans la collection
                        //var project = newItem as IObjectWithChangeTracker;
                        //if (project != null)
                        //{
                        //    if (this.ChangeTrackingEnabled)
                        //    {
                        //        project.ChangeTracker.Start();
                        //    }
                        //}
                        RecordAdditionToCollectionProperties(propertyName, newItem);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var odlItem in e.OldItems)
                    {
                        RecordRemovalFromCollectionProperties(propertyName, odlItem);
                    }
                    break;
                case NotifyCollectionChangedAction.Reset:
                    {
                        // Cas d'un Clear sur la collection.
                        // Vide le cache des modifications pour la collection.
                        if (ObjectsRemovedFromCollectionProperties.ContainsKey(propertyName))
                        {
                            ObjectsRemovedFromCollectionProperties.Remove(propertyName);
                        }

                        if (ObjectsAddedToCollectionProperties.ContainsKey(propertyName))
                        {
                            ObjectsAddedToCollectionProperties.Remove(propertyName);
                        }

                        // Tranfère les données de départ dans les élements supprimés.
                        if (ObjectsOriginalFromCollectionProperties.Count > 0)
                        {
                            foreach (var item in ObjectsOriginalFromCollectionProperties[propertyName])
                            {
                                RecordRemovalFromCollectionProperties(propertyName, item);
                            }
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    {
                        // Comment gérer le cas d'un changement d'instance dans la liste ?
                    }
                    break;
            }
            UpdateChangeState();
        }

        #region public properties
        // Returns the removed objects to collection valued properties that were changed.
        [JsonIgnore]
        public ObjectsRemovedFromCollectionProperties ObjectsRemovedFromCollectionProperties
        {
            get { return objectsRemovedFromCollectionsField ?? (objectsRemovedFromCollectionsField = new ObjectsRemovedFromCollectionProperties()); }
        }

        // Returns the original values for properties that were changed.
        [JsonIgnore]
        public PropertyValueStatesDictionary PropertyValueStates
        {
            get { return propertyValueStatesFields ?? (propertyValueStatesFields = new PropertyValueStatesDictionary()); }
        }

        // Returns the added objects to collection valued properties that were changed.
        [JsonIgnore]
        public ObjectsAddedToCollectionProperties ObjectsAddedToCollectionProperties
        {
            get { return objectsAddedToCollectionsField ?? (objectsAddedToCollectionsField = new ObjectsAddedToCollectionProperties()); }
        }

        // Returns the added objects to collection valued properties that were changed.
        [JsonIgnore]
        public ObjectsOriginalFromCollectionProperties ObjectsOriginalFromCollectionProperties
        {
            get { return objectsOriginalFromCollectionsField ?? (objectsOriginalFromCollectionsField = new ObjectsOriginalFromCollectionProperties()); }
        }

        #region MethodsForChangeTrackingOnClient

        [OnDeserializing]
        public void OnDeserializingMethod(StreamingContext context)
        {
            isDeserializingField = true;
        }

        [OnDeserialized]
        public void OnDeserializedMethod(StreamingContext context)
        {
            isDeserializingField = false;
        }
        #endregion

        /// <summary>
        /// Captures the original value for a property that is changing.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="value">The value.</param>
        public void RecordCurrentValue(string propertyName, object value)
        {
            if (objectTrackingEnabledField)
            {
                if (!PropertyValueStates.ContainsKey(propertyName))
                {
                    PropertyValueStates[propertyName] = new PropertyValueState() { PropertyName = propertyName, OriginalValue = value, CurrentValue = value, State = ObjectState.Unchanged };
                }
                // Compare original value new 
                else
                {
                    PropertyValueStates[propertyName].CurrentValue = value;
                    var originalValue = PropertyValueStates[propertyName].OriginalValue;
                    if (originalValue != null)
                    {
                        PropertyValueStates[propertyName].State = originalValue.Equals(value) ? ObjectState.Unchanged : ObjectState.Modified;
                    }
                    else
                    {
                        if (value == null)
                        {
                            PropertyValueStates[propertyName].State = ObjectState.Unchanged;
                        }
                        else
                        {
                            PropertyValueStates[propertyName].State = string.IsNullOrEmpty(value.ToString()) ? ObjectState.Unchanged : ObjectState.Modified;
                        }
                    }
                }
                UpdateChangeState();
            }
        }

        /// <summary>
        /// Records original items value of collection to collection valued properties on SelfTracking Entities.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="value">The value.</param>
        private void RecordOriginalToCollectionProperties(string propertyName, object value)
        {
            if (objectTrackingEnabledField)
            {
                if (!ObjectsOriginalFromCollectionProperties.ContainsKey(propertyName))
                {
                    ObjectsOriginalFromCollectionProperties[propertyName] = new ObjectList();
                    ObjectsOriginalFromCollectionProperties[propertyName].Add(value);
                }
                else
                {
                    ObjectsOriginalFromCollectionProperties[propertyName].Add(value);
                }
            }
        }


        /// <summary>
        /// Records an addition to collection valued properties on SelfTracking Entities.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="value">The value.</param>
        private void RecordAdditionToCollectionProperties(string propertyName, object value)
        {
            if (objectTrackingEnabledField)
            {
                // Add the entity back after deleting it, we should do nothing here then
                if (ObjectsRemovedFromCollectionProperties.ContainsKey(propertyName)
                    && ObjectsRemovedFromCollectionProperties[propertyName].Contains(value))
                {
                    ObjectsRemovedFromCollectionProperties[propertyName].Remove(value);
                    if (ObjectsRemovedFromCollectionProperties[propertyName].Count == 0)
                    {
                        ObjectsRemovedFromCollectionProperties.Remove(propertyName);
                    }
                    return;
                }

                if (!ObjectsAddedToCollectionProperties.ContainsKey(propertyName))
                {
                    ObjectsAddedToCollectionProperties[propertyName] = new ObjectList();
                    ObjectsAddedToCollectionProperties[propertyName].Add(value);
                }
                else
                {
                    ObjectsAddedToCollectionProperties[propertyName].Add(value);
                }
            }
        }

        /// <summary>
        /// Records a removal to collection valued properties on SelfTracking Entities.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="value">The object value.</param>
        internal void RecordRemovalFromCollectionProperties(string propertyName, object value)
        {
            if (objectTrackingEnabledField)
            {
                // Delete the entity back after adding it, we should do nothing here then
                if (ObjectsAddedToCollectionProperties.ContainsKey(propertyName)
                    && ObjectsAddedToCollectionProperties[propertyName].Contains(value))
                {
                    ObjectsAddedToCollectionProperties[propertyName].Remove(value);
                    if (ObjectsAddedToCollectionProperties[propertyName].Count == 0)
                    {
                        ObjectsAddedToCollectionProperties.Remove(propertyName);
                    }
                    return;
                }

                if (!ObjectsRemovedFromCollectionProperties.ContainsKey(propertyName))
                {
                    ObjectsRemovedFromCollectionProperties[propertyName] = new ObjectList();
                    ObjectsRemovedFromCollectionProperties[propertyName].Add(value);
                }
                else
                {
                    if (!ObjectsRemovedFromCollectionProperties[propertyName].Contains(value))
                    {
                        ObjectsRemovedFromCollectionProperties[propertyName].Add(value);
                    }
                }
            }
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Called when [property changed].
        /// </summary>
        /// <param name="info">The info.</param>
        public virtual void OnPropertyChanged(string info)
        {
            System.ComponentModel.PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new System.ComponentModel.PropertyChangedEventArgs(info));
            }
        }
}
#endregion
}
#pragma warning restore
