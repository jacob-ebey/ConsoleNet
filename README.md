ConsoleNet [![Build status](https://ci.appveyor.com/api/projects/status/tj6sgf88apla2491/branch/master?svg=true)](https://ci.appveyor.com/project/jacob-ebey/consolenet/branch/master)
==========
A framework for easily creating console applications.

To see an example, visit the repository found at https://github.com/jacob-ebey/ConsoleNet-Example. But the below example pretty much summs it up.

Example
-------
```C#
using System;

using ConsoleNet;

namespace ConsoleNetTest
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            // Create a new application that will be refered to through out as ConsoleNetTest
            Application app = new Application("ConsoleNetTest");

            // Add a new command to the application.
            // The properties that are set below adds a description of the app, and a full help for the command.
            app.AddCommand(new FrameworkCommand("hello", SayHello)
                { 
                    Description = "Prints hello!",
                    Help = "Option: --name <Name>"
                });

            // Run the application, passing in the command line args.
            app.Run(args);
        }

        /// <summary>
        /// This is the method that is added as the 'hello' command.
        /// This command says hello to the applications name or the
        /// value of the name param if added.
        /// </summary>
        /// <param name="app">The command methods get passed an
        /// <see cref="ConsoleNet.Application"/> instance.</param>
        static void SayHello(Application app)
        {
            // Get the name parameter from the application instance.
            string name;
            app.TryGetParam("name", out name);

            // Print the name of the application if no name was entered.
            Console.WriteLine(string.Format("Hello, {0}!", name ?? app.Label));
        }
    }
}

```
