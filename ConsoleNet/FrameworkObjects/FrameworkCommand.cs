using System;

using ConsoleNet;
using System.Threading.Tasks;

namespace ConsoleNet
{
    /// <summary>
    /// Framework command.
    /// </summary>
    public class FrameworkCommand : FrameworkObject
	{
        bool executing;
        Action<Application> action;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleNet.FrameworkCommand"/> class.
        /// </summary>
        /// <param name="name">The command name.</param>
        /// <param name="action">The action to perform if the command is requested to run by the user.</param>
        public FrameworkCommand(string name, Action<Application> action)
            : base(name)
        {
            this.action = action;
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the help message.
        /// </summary>
        /// <value>The help.</value>
        public string Help { get; set; }

        /// <summary>
        /// Execute the command in the context of the supplied application.
        /// </summary>
        /// <param name="application">Application.</param>
        internal void Execute(Application application)
        {
            if (executing)
                throw new Exception("Can not execute while already running.");

            executing = true;

            action(application);
        }

        /// <summary>
        /// Execute the command asynchronously in the context of the supplied application.
        /// </summary>
        /// <returns>The task to wait upon.</returns>
        /// <param name="application">Application.</param>
        internal Task ExecuteAsync(Application application)
        {
            return Task.Factory.StartNew(() => Execute(application));
        }
	}

}
