# Voiceover

## Pronunciation hints

* XAML, see https://en.wikipedia.org/wiki/Extensible_Application_Markup_Language

* `INotifyPropertyChanged`: I-Notify-Property-Changed

## Text

PostSharp MVVM eliminates  much of the boilerplate you would otherwise need to develop
a XAML or WinForms app.

Here is a simple contact editor form. If you modify the street number in an address, you can see
that this change is not propagated to the list. This is because the classes don't implement the
`INotifyPropertyChanged` interface yet. 

Let's look at the code. There are just two model classes: customer and address. The address class has a _computed property_ named `FullAddress`. It concatenates all other properties. The view model class has a more complex computed property: `LabelContent` depends on a property _of a property_ **of a property**. Implementing `INotifyPropertyChanged` manually would be extremely cumbersome. Let's see how to do that with PostSharp.

Go to the `ModelBase` class and open the refactoring menu. Select _Implement INotifyPropertyChanged_. The first time you run this wizard, PostSharp will install a few packages into your project. The only other
change in your code will be this custom attribute.

Go to the `CustomerViewModel` class and do the same.

Done! 

Now run the application again. Changes in address details are immediately visible in the list.

But the label preview control does not work yet. Let's look at that.

To bind a control property with data, you have to make it a dependency property. Open the refactoring
menu for the `Text` property, select _Add another aspect_ and then choose _Make a XAML dependency property_. This again just adds a custom attribute to your code. You can do it manually the next time if you want.

Now go to the XAML code of the main window and add a data binding to the `Text` property.

Let's see if that worked.

Great!

The next step is to implement the code behind the _Set Principal Address_ button. XAML has a nice pattern for this: command binding.

Go back to the `MainWindow` code and write the command as a normal method.

Now open the refactoring menu and choose _Expose as a XAML command with CanExecute_. 

Provide an implementation for the `CanExecute` property. Note that the wizard has added the `NotifyPropertyChanged` aspect to this class to,
and a new public property to expose the command.

Now go to the XAML code and bind the button to the new command property.

We're done!

The button now does what it's meant to do, and it is disabled or enabled as it should.

Let's finally add some validation to this form. We want the town field to be required.

Go to the `Town` property in the `AddressModel` class. Open the refactoring menu and choose 
_Require a non-empty value_. This is just one of the many contracts you can add to your fields, properties
and parameters.

Run the app a last time. You can see that Town is now a required field.

Thank you for watching this short demo. In just a few minutes with PostSharp, you have implemented `INotifyPropertyChanged` for simple _and_ complex properties, turned a regular C# property into
a XAML dependency property, added a command with dynamic state notification, and added validation to your
model. But most importantly: you saved a lot of boilerplate.











