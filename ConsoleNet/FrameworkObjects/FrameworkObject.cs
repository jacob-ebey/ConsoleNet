using System;

namespace ConsoleNet
{
    /// <summary>
    /// Framework object to be used in the framework hierarchy.
    /// </summary>
    public class FrameworkObject
    {
        string label;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleNet.FrameworkObject"/> class.
        /// </summary>
        /// <param name="label">Label to be displayed if the object needs to be shown.</param>
        public FrameworkObject(string label)
        {
            Label = label;
        }

        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        /// <value>The label to be displayed if the object needs to be shown.</value>
        public string Label
        {
            get { return label; }
            set
            {
                if (label == value)
                    return;

                label = value;
                OnLabelChanged(new EventArgs());
            }
        }

        /// <summary>
        /// Raises the label changed event.
        /// </summary>
        /// <param name="e">E.</param>
        void OnLabelChanged(EventArgs e)
        {
            var handler = LabelChanged;
            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// Occurs when the label changed.
        /// </summary>
        public event EventHandler LabelChanged;
    }
}

