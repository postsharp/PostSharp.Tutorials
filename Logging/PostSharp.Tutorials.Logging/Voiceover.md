# Voiceover script

## Instructions

There should be a pause after each paragraph, long enough for the recording to be cut and synchronized with the video track. Sentences within a paragraph won't be cut.

## Script

With PostSharp Logging, you can add high-resolution logging to your existing codebase within minutes and with almost no impact on your existing code base.

Here is a sample C# project that simulates the processing of requests from a queue. It's a console app, but you can use PostSharp with almost any kind of .NET app.

Let's add logging to this project. Some people call this tracing, and here I take these words as synonyms. Whenever any method starts, succeeds or fails, we want to write a detailed message. You can do that in just three steps.

First, add a NuGet package to your projects.

Open the package manager.

Find the package `PostSharp.Patterns.Diagnostics`.

Select all projects you want to log. Here, we just have one project, it's easy.

Click on _Install_.

Done!

The second step is to add logging to all methods in these projects.

Create a configuration file named `postsharp.config`. If you store this file in the project directory, it only applies to this specific project. But if you store it in a parent directory, for instance to the root of your repository, it applies to all projects in all subdirectories.

This XML code adds logging to all methods of my projects.

The third and final step is to say where PostSharp has to write the log records. PostSharp supports the most popular logging frameworks including Log4Net, NLog, Serilog, the standard system .NET APIs, and many others. In this video, we just use the console.

Go to the `Program` file.

Add a few namespaces.

Go to the `Main` method and initialize the  `LoggingServices.DefaultBackend` property to a new instance of `ConsoleBackend`.

We're done!

Now run the app and sit back. The first time you build your project may be a little longer than usual.

Awesome! As you can see, we have a very detailed log. Every single method call is there. 

The log even includes parameters and return values. This is very useful when you need to diagnose an issue - especially one that happens only in production.

The problem is now that the log is _too_ detailed. There is a lot of noise, for instance property getters and setters. Let's fine tune what needs to be logged.

Go back to `postsharp.config`.

Add a second `Log` element, but this time by setting  `AttributeExclude` and `AttributeTargetMembers`  to make sure we're excluding property getters and setters. Here we're relying on naming conventions, but you can also include or exclude methods based on visibility, and many other criteria.

Run the app again.

Much better! The log no longer includes property getters and setters.

Now, what if you want to know the execution time of each method? And what if you want to get a warning when a method takes more than 200 milliseconds to execute? 

Go back to `postsharp.config` and add some XML to customize the default logging profile. The properties you need are `IncludeExecutionTime` and `ExecutionTimeThreshold`, and since you're here you can look at other customization options, such as  `IncludeParameterName`.

Run the program again.

Great. Now you can see the execution time for each method, and a yellow warning for all methods that lasted more than 200 ms.

There's a last problem with this log: it includes passwords. This could cause a dangerous security leak, so let's fix that immediately. 

Open `CrmClient` and go to the constructor. Add the custom attribute `NotLogged` to the `credentials` parameter.

Run the program a last time.

Awesome! The password is no longer logged.

Thank you for watching this short demo. In just a few minutes, you have added extensive logging to an existing application and customized it to you needs. Of course, this was just the beginning. There are many more options and features for you to discover.
