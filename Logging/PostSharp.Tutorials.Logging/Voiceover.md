# Voiceover script

## Instructions

There should be a pause after each paragraph, long enough for the recording to be
cut and synchronized with the video track. Sentences within a paragraph won't be cut.

## Script

This video will show you how to add highly detailed logging to an existing application
with just a few lines of initialization code.

I've opened a C# demo project in Visual Studio. This project simulates the processing of
different requests from a queue -- vacation requests, for instance. It's a console app
because it makes an easy demo, but you can use PostSharp with almost all kinds of .NET
apps.

Let's add logging to this app. Some people call this tracing, and here I will assume
these words as synonyms. I want a log message whenever a method starts, succeeds or fails. There will be just three steps.

The first step is to add a NuGet package to your projects.

Let's go to the package manager.

We need the package `PostSharp.Patterns.Diagnostics`.

Here it is.

We need to add it to all projects in our solution. Here we just have one project, it's easy.

Click on _Install_.

Done.

The second step is to add logging to all methods. We can do that with just a single custom attribute.
You probably know the file `AssemblyInfo.cs` that contains just custom attributes. We will have a similar file but will name it `GlobalAspects.cs`. The name is just really your choice. So let's create this file.

I'm just adding a `Log` custom attribute to the assembly. It means that _all_ methods in the
project will now be logged.

Great.

The final step is to configure PostSharp Logging. We have to specify where PostSharp has to write the log records. PostSharp supports the most popular logging framework including Log4Net, NLog, Serilog and of course the standard system APIs of .NET Framework and .NET Core., and many others.
Here we will just use the console.

Let's go to the `Program` file.

We will need these namespaces.

We want to initialize PostSharp Logging as soon as possible in the `Main` method. We need this code. It initializes the `LoggingServices.DefaultBackend` property to a `ConsoleBackend`.

Great. Let's now build the program and execute it.

Awesome. As you can see, we have a very detailed log. Every method call is now logged. The log includes parameter values and return values, which is very useful when you need to diagnose an issue.

The problem is now that the log is _too_ detailed. There is a lot of noise, for instance calls
to property getters and setters. Let's fine tune what needs to be logged.

We need to go back to `GlobalAspects.cs`. Edit the `Log` custom attribute to say that it should be evaluated first.

Then add a second `Log` custom attribute to exclude property getters and setters. We set the `AttributePriority` property to ensure it executes _after_ the first one.

Great. Let's now see the result.

Much better. The log no longer includes calls to property getters and setters.

Now, what if I want to know the execution time of each method? And what if I want to get a warning when a method takes more than 200 milliseconds to execute?

All we need to do is to create a configuration file named `postsharp.config`. Now let's add some XML to customize the default logging profile. We have the properties `IncludeExecutionTime` and 
`ExecutionTimeThreshold`, and here I'm also setting `IncludeParameterName` as one of the many
other customization options.

Let's run the program again.

Great. Now you can see the execution time for each method, and a yellow warning for all methods that lasted more than 200 ms.

There's a last problem with this log: it includes passwords. This is a dangerous security leaks. 

Let's fix that immediately and fix the offending method. Open `CrmClient` and go to the constructor. Add `NotLogged` to the `credentials` parameter.

Let's run the program a last time and look at the log.

Awesome. The password is no longer logged.

Thank you for watching this short demo. In just a few minutes, I have added extensive logging to an existing application and configured it to my needs. There are many more options and features to discover on our web site.





