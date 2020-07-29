# Synopsis

## General Instructions

* Recording resolution: 1280×720 
* Visual Studio 2019
* White Theme
* Font size: x pt
* Include the mouse pointer in the capture

The following steps are temporary bug workaround:

* In VS, make sure other versions of PostSharp Tools for Visual Studio are uninstalled.
* Install the VSIX given in the OneDrive folder.
* Download the `*.nupkg` files from OneDrive and copy the directory path to the clipboard.
* In VS, go to Tools / Options / NuGet Package Manager / Package Sources.
* Name it say `PostSharp Private` and paste the directory path.


## Step 0. Preparation

* Open the pre-prepared project from https://github.com/postsharp/PostSharp.Tutorials/tree/before/Logging/PostSharp.Tutorials.MVVM.
  Attention: this is the _before_ branch. The master branch is what you get when you've completed the steps below.

## Step 1. Adding INotifyPropertyChanged

* Run the application. 
  * Click on the first item in the address list.
  * Change the street number. 
  * Using a post-production rectangle,  show that the street number has not changed in the address list. 
    (The point is to say that `INotifyPropertyChanged` has not been implemented)
  * Close the application.

* Open the file `Model/AddressModel.cs` and show the `FullAddress` property using a post-production rectangle.
  (The point is to say that we support computed properties.)


* Open the file `ViewModel/CustomerViewModel.cs` and show the `LabelContent` property and use post-production 
  rectangles to highlight the expression `Customer.PrincipalAddress?.FullAddress?`. 
  (The point is to say that we support dependencies to children object.)

* Open the file `Model/ModelBase.cs`. Place the caret inside the `ModelBase` class name. Click on the brush icon, choose _Implement INotifyPropertyChanged_. Go through the wizard.

  At post-production, just take a fraction of second of the package install time.

* Open the file `ViewModel/CustomerViewModel.cs` and do the same.

* Run the application again. 
  * Click on the first item in the address list.
  * Change the street number.
  * Using a post-production rectangle, show that the street number has changed in the address list. 
  * Using a post-production rectangle, show the _Printed Label Preview_. 
    (The point is to say that it has not been implemented and that's what we will do)


## Step 2. Adding a dependency property.

* Open the file `LabelPreviewControl.xaml.cs`. Place the caret in within the name of the `Text` property.
  Click on the brush icon, choose _Add another aspect_, then in the _XAML_ group, select 
  _Make a XAML dependency property_.

* Open `MainWindow.xaml`. In the designer, click on the label control. In the XAML view,
  replace the value of the `Text` attribute by the following: {Binding LabelContent}.

* Run the application. 
  * Click on the first item in the address list.
  * Change the street number.
  * Using a post-production rectangle, show the updated street number in the _Printed Label Preview_. 
    

## HACK!

At this point you need to implement a hack, **off record**, because you will need to apply a package
version that is not yet been published. This step wll not be necessary when the bug fixes will have been made public.

* Using a text editor, edit the `csproj` file and edit the two `PackageReference` of PostSharp
  so that the `Version` element is the version of the private build, e.g. `6.6.10-b7f224`.

* Reload the project in VS and rebuild.

## Step 3. Adding a command.

* Open the file `MainWindow.xaml.cs`.  After the constructor, paste the following code:

```cs
private void SetPrincipalAddress() 
   => _customerViewModel.Customer.PrincipalAddress = _customerViewModel.SelectedAddress;
```

* Place the caret inside the name of the `SetPrincipalAddress` method. Click on the brush icon.
Choose _Expose as a XAML command (with CanExecute)_. Wait a second or two on the preview, then apply.

* Go to `CanSetPrincipalAddress` the replace `true` by this:

```cs
_customerViewModel != null && _customerViewModel.SelectedAddress != null &&
!_customerViewModel.IsSelectedAddressPrincipal
```

* Open `MainWindow.xaml`. In the designer, click on the _Set Principal_ button. Go to the XAML view
and add the following attribute to the `<Button>` element:

```xml
Command="{Binding ElementName=Window, Path=SetPrincipalAddressCommand}"
```

* Run the application.
  * Select the first address.
  * At post-production, set a focus rectangle on the _Set Principal_ button. The point is to show that the button is disabled. Don't remove the post-production rectangle now.
  * Select the second address. The button will be enabled. Click on the button.
  * Remove the post-production rectangle from the button, put it on the label preview control.
  * Close the app.

## Step 4. Adding contracts

* Open `Model/AddressModel.cs`. Place the caret inside the name of the `Town` property.
  Click on the brush icon, choose _Require a non-empty and non-whitespace value_. Go through the wizard.

* Run the application.
  * Select the first address.
  * Delete the value in the _Town_ text box.
  * At post-production, set a focus rectangle (with a large enough margin from the text box)
    to show that validation works.
