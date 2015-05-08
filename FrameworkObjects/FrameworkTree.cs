using System;
using System.Collections.ObjectModel;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace ConsoleNet
{
    /// <summary>
    /// A framework tree.
    /// </summary>
    public class FrameworkTree : FrameworkObject
    {
        ObservableCollection<FrameworkObject> children;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleNet.FrameworkTree"/> class.
        /// </summary>
        /// <param name="label">Label.</param>
        public FrameworkTree(string label)
            : this(label, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleNet.FrameworkTree"/> class.
        /// </summary>
        /// <param name="label">Label.</param>
        /// <param name="children">Children.</param>
        public FrameworkTree(string label, IEnumerable<FrameworkObject> children)
            : base(label)
        {
            if (children != null)
                this.children = new ObservableCollection<FrameworkObject>(children);
            else
                this.children = new ObservableCollection<FrameworkObject>();
            this.children.CollectionChanged += OnChildrenChanged;
        }

        /// <summary>
        /// Gets the children.
        /// </summary>
        /// <value>The children.</value>
        public Collection<FrameworkObject> Children { get { return children; } }

        /// <summary>
        /// Adds the child.
        /// </summary>
        /// <param name="child">Child.</param>
        public void AddChild(FrameworkObject child)
        {
            children.Add(child);
        }

        /// <summary>
        /// Removes the child.
        /// </summary>
        /// <returns><c>true</c>, if child was removed, <c>false</c> otherwise.</returns>
        /// <param name="child">Child.</param>
        public bool RemoveChild(FrameworkObject child)
        {
            return children.Remove(child);
        }

        /// <summary>
        /// Raises the children changed event.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        protected virtual void OnChildrenChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var handler = ChildrenChanged;
            if (handler != null)
                handler(this, e);

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    OnChildAdded(new EventArgs());
                    break;
                case NotifyCollectionChangedAction.Remove:
                    OnChildRemoved(new EventArgs());
                    break;
            }
        }

        /// <summary>
        /// Raises the child added event.
        /// </summary>
        /// <param name="e">E.</param>
        protected virtual void OnChildAdded(EventArgs e)
        {
            var handler = ChildAdded;
            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// Raises the child removed event.
        /// </summary>
        /// <param name="e">E.</param>
        void OnChildRemoved(EventArgs e)
        {
            var handler = ChildRemoved;
            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// Occurs when a child is added.
        /// </summary>
        public event EventHandler ChildAdded;

        /// <summary>
        /// Occurs when a child is removed.
        /// </summary>
        public event EventHandler ChildRemoved;

        /// <summary>
        /// Occurs when children changed.
        /// </summary>
        public event EventHandler ChildrenChanged;
    }
}

