# Voiceover

## Pronunciation hints

* XAML, see https://en.wikipedia.org/wiki/Extensible_Application_Markup_Language

* `INotifyPropertyChanged`: I-Notify-Property-Changed

## Text

Building a XAML or WinForms app traditionally required you to write a lot of boilerplate code, especially for data binding. PostSharp MVVM almost completely eliminates this frustration and gives you back your time for something more meaningful.

Here is a simple contact editor. If you modify the street number for an address, you can see that the change is not propagated to the list. This is because the classes don't implement the `INotifyPropertyChanged` interface yet. 

Let's look at the code. There are only two model classes: customer and address. The address class has a _computed property_ named `FullAddress` which contains all other properties. The view-model class has an even more complex computed property: `LabelContent`. It depends on a property _of a property_ **of a property**. Because of these recursive dependencies on child objects, implementing `INotifyPropertyChanged` manually would be extremely cumbersome. Let's see how this can be done with PostSharp.

Go to the `ModelBase` class and open the refactoring menu. Select _Implement INotifyPropertyChanged_. The first time you run this wizard may take a little longer, as PostSharp will install a few packages into your project. The only other change in your code will be a new custom attribute on the top of the class.

Go to the `CustomerViewModel` class and do the same.

Done! 

Now run the application again. Changes in address details are now immediately visible in the list.

However, the label preview control does not work yet. Let's look at that.

To bind a control property to dynamic data, the XAML doc says you have to make it a _dependency property_. Let's do it.

Open the refactoring menu for the `Text` property, select _Add another aspect_ and then choose _Make a XAML dependency property_. This only adds a custom attribute to your code. You can do it manually the next time if you prefer.

Now go to the XAML code of the main window and add a data binding to the `Text` property.

Let's see if that worked.

Great!

The next step is to implement the code behind the _Set Principal Address_ button. XAML has a nice pattern for this: command binding.

Go back to the `MainWindow` class and code the behavior as a normal method.

Now open the refactoring menu and choose _Expose as a XAML command with CanExecute_. 

Provide an implementation for the `CanExecute` property. Note that the wizard has added the `NotifyPropertyChanged` aspect to this class,
and a new public property to expose the command.

Now go to the XAML code and bind the button to the new command property.

We're done!

The button now does what it's meant to do, and it is dynamically disabled or enabled as it should be.

Finally,  let's  add some validation to this form. We want the _Town_ field to be a required one.

Go to the `Town` property in the `AddressModel` class. Open the refactoring menu and choose _Require a non-empty value_. This is just one of the many contracts you can add to your fields, properties
or parameters.

Run the app a last time. You can see that _Town_ is now a required field.

Thank you for watching this demo. In just a few minutes, thanks to PostSharp, you have implemented `INotifyPropertyChanged` for simple _and_ complex properties, turned a regular C# property into
a XAML dependency property, added a command with dynamic state notification, and added validation to your model. But most importantly: you avoided a lot of boilerplate and kept your business code crystal-clear.
