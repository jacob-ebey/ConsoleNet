using System;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace ConsoleNet
{
    /// <summary>
    /// The application.
    /// </summary>
    public class Application : RestrictiveFrameworkTree<FrameworkCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleNet.Application"/> class.
        /// </summary>
        /// <param name="applicationName">Application name.</param>
        public Application(string applicationName)
            : this(applicationName, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleNet.Application"/> class.
        /// </summary>
        /// <param name="applicationName">Application name.</param>
        /// <param name="children">Children.</param>
        public Application(string applicationName, IEnumerable<FrameworkCommand> children)
            : base(applicationName, children)
        {
            ParsedParams = new Dictionary<string, string>();
            string description = "Prints this help screen";
            AddChildProtected(new FrameworkCommand("help", a => Help()) { Description = description, Help =  description});
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ConsoleNet.Application"/> will pause before the application terminates.
        /// </summary>
        /// <value><c>true</c> if pause at end; otherwise, <c>false</c>.</value>
        public bool PauseAtEnd { get; set; }

        /// <summary>
        /// Gets the parsed parameters.
        /// </summary>
        /// <value>The parsed parameters.</value>
        /// <description>
        /// This is populated when the application is ran. This is gaurenteed to 
        /// not be null when the command is executed.
        /// </description>
        protected Dictionary<string, string> ParsedParams { get; set; }

        /// <summary>
        /// Add a command to this instance.
        /// </summary>
        /// <param name="command">Command.</param>
        public void AddCommand(FrameworkCommand command)
        {
            if (Children.Any(c => c.Label == command.Label))
                throw new ArgumentException(
                    string.Format(
                        "A command with the label {0} already exists. Duplicates are not allowed.", 
                        command.Label));
            
            AddChildProtected(command);
        }

        /// <summary>
        /// Try to get the param with the given name.
        /// </summary>
        /// <returns><c>true</c>, if get parameter was gotten, <c>false</c> otherwise.</returns>
        /// <param name="paramName">Parameter name.</param>
        /// <param name="value">Value.</param>
        /// <description>
        /// Flags can be used as well, example: if <code>myapplication.exe test param1</code> is
        /// the command then param1 is entered as the key and "true" as the value.
        /// If the paramName did not have a matching value to go with it's value will be "true".
        /// </description>
        public bool TryGetParam(string paramName, out string value)
        {
            value = null;
            if (ParsedParams.ContainsKey(paramName))
                value = ParsedParams[paramName];

            return value != null;
        }

        /// <summary>
        /// Parses the arguments.
        /// </summary>
        /// <returns>The command name to execute.</returns>
        /// <param name="args">Arguments.</param>
        protected virtual string ParseArgs(string[] args)
        {
            if (args.Length < 1)
                return "help";

            string commandName = args[0];

            // TODO: Generate a Dictionary<string, string> as a property of Application called Params
            Dictionary<string, string> parsedParams = new Dictionary<string, string>();

            string key = null;

            for (int i = 1; i < args.Length; i++)
            {
                bool isPotentialKey = args[i].Length > 2 && args[i].Substring(0, 2) == "--";

                if (key == null)
                {
                    if (isPotentialKey)
                        key = args[i].Substring(2);
                    else
                        parsedParams[args[i]] = "true";
                }
                else
                {
                    parsedParams[key] = args[i];
                    key = null;
                }
            }

            if (key != null)
                parsedParams[key] = "true";

            ParsedParams = parsedParams;

            return commandName;
        }

        /// <summary>
        /// This prints/displays the help for the application.
        /// </summary>
        /// <description>
        /// When Application is initialized this method is registered to the instance
        /// as a command named "help". When overriding this method do not call the base
        /// unless you also want the default help screen printed (beware, it's ugly!).
        /// </description>
        public virtual void Help()
        {
            if (ParsedParams.Any())
            {
                string commandName = ParsedParams.FirstOrDefault().Key;
                FrameworkCommand command = Children.FirstOrDefault(c => c.Label == commandName);

                if (command != null)
                    Console.WriteLine(command.Help ?? command.Description);
                else
                    Console.WriteLine(string.Format("`{0}` is not a valid command.", commandName)); 

                return;
            }

            Console.WriteLine(string.Format("To use the below commands enter `{0} command-name`.", Label));
            Console.WriteLine();

            foreach (var command in Children.OrderBy(c => c.Label))
            {
                Console.WriteLine(
                    string.Format("{0} : {1}",
                        command.Label, 
                        command.Description ?? string.Format("{0} has no help avaliable...", command.Label)));
            }
        }

        /// <summary>
        /// Run this instance.
        /// </summary>
        public void Run(string[] args)
        {
            string commandName = ParseArgs(args);

            FrameworkCommand command = Children.FirstOrDefault(c => c.Label == commandName);

            if (command == null)
            {
                Help();
                return;
            }

            command.Execute(this);

            if (PauseAtEnd)
            {
                Console.Write("Press any key to continue . . .");
                Console.ReadKey(true);
            }
                
        }

        /// <summary>
        /// Runs this instance asynchronously.
        /// </summary>
        /// <returns>The task to wait upon.</returns>
        public async Task RunAsync(string[] args)
        {
            string commandName = ParseArgs(args);

            FrameworkCommand command = Children.FirstOrDefault(c => c.Label == commandName);

            if (command == null)
            {
                Help();
                return;
            }

            await command.ExecuteAsync(this);

            if (PauseAtEnd)
            {
                Console.Write("Press any key to continue . . .");
                Console.ReadKey(true);
            }
        }
    }
}

