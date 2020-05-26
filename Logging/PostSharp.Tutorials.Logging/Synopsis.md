# Synopsis

## Step 1. Preparation

* Open the pre-prepared project `https://github.com/postsharp/PostSharp.Tutorials/tree/before/Logging/PostSharp.Tutorials.Logging`. Attention: this is the _before_ branch.

* Add the NuGet package `PostSharp.Patterns.Diagnostics`.

*  Create a new C# file named GlobalAspects.cs (the name is arbitrary) and add this code:

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

 * Add this code on the top of the `Main` method:

```cs
// Configure PostSharp Logging to write to the console.
var backend = new ConsoleLoggingBackend();
backend.Options.Theme = ConsoleThemes.Dark;
backend.Options.IncludeExceptionDetails = false;
LoggingServices.DefaultBackend = backend;
```

* Run the application and show the log on the console.

* Insist on a few points: 
   * how detailed it is. Every method call is present.
   * parameters value are printed. This is useful to reproduce a problem,
   * say the log is too detailed because property getters/setters are there. Show this in the log.

## Step 2. Reducing noise

* Go back to GlobalAspects.cs 

* Edit `[assembly: Log]` to change it to `[assembly: Log(AttributePriority = 1)]` (insert the missing text)

* Add this: 

```cs
[assembly: Log(AttributePriority = 2, AttributeExclude = true, AttributeTargetMembers = "regex:get_*|set_*")]
```

* Recompile, run the program, and show the log without that. Great.

## Step 3. Adding execution time

Wouldn't it be nice to display the execution time?  

* Create postsharp.config (in VS, do New Item > PostSharp > PostSharp Configuration File)  paste the following code within the `Project` element:

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

Execute the program and show the execution time, the warning when some methods takes longer, and the parameter name.

## Step 4. Security

Show in the log that some password has been printed. How bad. Go to the source code of `CrmClient` and add [NotLogged] to the custom attribute. 
Run the application again and show that the password is no longer in the log.





