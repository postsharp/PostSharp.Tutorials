# Synopsis

## General Instructions

* Recording resolution: 1280×720 
* Visual Studio 2019
* White Theme
* Font size: x pt
* Include the mouse pointer in the capture

## Step 1. Preparation

* Open the pre-prepared project from https://github.com/postsharp/PostSharp.Tutorials/tree/before/Logging/PostSharp.Tutorials.Logging. Attention: this is the _before_ branch. The master branch is what you get when you've completed the steps below.

* Add the NuGet package `PostSharp.Patterns.Diagnostics`.

* Create a `postsharp.config` (in VS solution explorer, right-click on the project and doo Add > New Item > PostSharp > PostSharp Configuration File) . 

* Paste the following code within the `Project` element:

```xml
<Multicast xmlns:d="clr-namespace:PostSharp.Patterns.Diagnostics;assembly:PostSharp.Patterns.Diagnostics">
	<!-- Adds logging to every method -->
	<d:Log/>
</Multicast>
```

The next steps will configure PostSharp Logging to write to the console:

* Go back to `Program.cs`.

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

* Run the application and look at the log on the console.

* Scroll up in the log and mark pauses so that the following can be added in post-production:
   * Highlighting some parameter values
   * Highlighting return values.
   * Highlighting a few calls to getters/setters.

## Step 2. Reducing noise

* Go back to `postsharp.config`.

* Append the following XML code into the `Multicast` element:

```xml
<!-- Remove logging from property getters and setters -->
<d:Log AttributeExclude="true" AttributeTargetMembers="regex:get_*|set_*"/>
```

* Recompile, run the program, show the log. Mark a pause to show it no longer includes getter/setter.

## Step 3. Adding execution time


* Go back to `postsharp.config`.

* Paste the following code after the `Multicast` element:

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

* Execute the program and show the new features in the log. In pot-production, highlight:
     * the execution time, 
	 * the warning when some methods takes longer, and
	 * the parameter names.

## Step 4. Security

* Scroll to the top of the log. In post-production, highlight the `supersecret` password.

* Go to the constructor of `CrmClient` and add [NotLogged] to the `credentials` parameter.

* Run the application again and scroll up to the top of the log. In post-production, highlight the `***` instead of the password.





