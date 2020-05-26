# Synopsis

## Step 1. Preparation

* Open the pre-prepared project from https://github.com/postsharp/PostSharp.Tutorials/tree/before/Logging/PostSharp.Tutorials.Logging. Attention: this is the _before_ branch. The master branch is what you get when you've completed the steps below.

* Add the NuGet package `PostSharp.Patterns.Diagnostics`.

*  Create a new C# file named `GlobalAspects.cs` (the name is arbitrary) and add this code:

```cs
using PostSharp.Patterns.Diagnostics;
[assembly: Log]
```

The next steps will configure PostSharp Logging to write to the console:

* Add the namespaces:

```cs
using PostSharp.Patterns.Diagnostics;
using PostSharp.Patterns.Diagnostics.Backends.Console;
```

* Go back to `Program.cs`.

 * Add this code on the top of the `Main` method:

```cs
// Configure PostSharp Logging to write to the console.
var backend = new ConsoleLoggingBackend();
backend.Options.Theme = ConsoleThemes.Dark;
backend.Options.IncludeExceptionDetails = false;
LoggingServices.DefaultBackend = backend;
```

* Run the application and look at the log on the console.

* Notice the following in the log:
   * How detailed it is. Every method call is present.
   * Parameters value are printed. This is useful to reproduce a problem,
   * However, the log is too detailed. That's  property getters/setters are instrumented.

## Step 2. Reducing noise

* Go back to `GlobalAspects.cs`.

* Edit `[assembly: Log]` to change it to `[assembly: Log(AttributePriority = 1)]` (insert the missing text).

* Add this: 

```cs
[assembly: Log(AttributePriority = 2, AttributeExclude = true, AttributeTargetMembers = "regex:get_*|set_*")]
```

* Recompile, run the program, and notice that the log has much less noise.

## Step 3. Adding execution time

Wouldn't it be nice to display the execution time?  

* Create a `postsharp.config` (in VS solution explorer, right-click on the project and doo Add > New Item > PostSharp > PostSharp Configuration File)  

* Paste the following code within the `Project` element:

```xml
<Logging xmlns="clr-namespace:PostSharp.Patterns.Diagnostics;assembly:PostSharp.Patterns.Diagnostics">
	<Profiles>
		<LoggingProfile Name="Default"  IncludeExecutionTime="True" ExecutionTimeThreshold="200">
			<DefaultOptions>
				<LoggingOptions IncludeParameterName="True"/>
			</DefaultOptions>
		</LoggingProfile>
	</Profiles>
</Logging>
```

* That includes three settings:
  * Adding the method execution time to the log, for all methods.
  * When the execution time is higher than 200 ms, write a warning.
  * Include the parameter name in te log.

* Execute the program and see the new features in the log:
 the execution time, the warning when some methods takes longer, and the parameter name.

## Step 4. Security

You can see in the log that some password has been printed. How bad. Let's fix that.

* Go to the source code of `CrmClient` and add [NotLogged] to the custom attribute. 

* Run the application again and show that the password is no longer in the log.





