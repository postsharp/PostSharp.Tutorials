# Synopsis

## General Instructions

* Recording resolution: 1280�720 
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

* Open the pre-prepared project from https://github.com/postsharp/PostSharp.Tutorials/tree/master/Threading/PostSharp.Tutorials.Threading.
  Attention: this is the _before_ branch. The master branch is what you get when you've completed the steps below.

 ## Step 1. Introduction

 [2]

 * Start two instances of the sample application.
 * Connect one as the server.
 * Connect the other as the client.
 * In the first instance, click on a creature and then, using arrows, move it.
 * In the second instance, click on _Add creature_ and then, using the arrows, move it.

 [3]

 * Go back to VS and expand all solution folders.

 [4]

 * Open `MainWindow.xaml.cs`, place the caret in the class name, press `Alt+Enter` to open the refactoring menu,
   choose _Apply threading model_, choose _Thread affine_.

[5]

* Go to `IViewModel` and do the same.

[6]

* Go to `BoardViewModel.Creatures` and add this custom attribute: `[Child( ItemsRelationship = RelationshipKind.Child )]`.

[7]

* Go to `Board` and add the attribute `[ReaderWriterSynchronized]` to the class.
* Go to `Board.Creatures`  and add the attribute `[Child( ItemsRelationship = RelationshipKind.Child )]`.

[8]

* Go to `Board.Dispose` and add `[Writer]
* Go to `Creature` and add the attribute `[ReaderWriterSynchronized]` to the class.
* Go to `Creature.TryMove`  and add the attribute `[Writer]`. Do the same with the next methods: `Rotate`, `MoveTo`, `TryMoveTo`.

[9]

* Go to `BoardServiceClient` and add the attribute `[Immutable]` to the class.

[10]

* Go to `BoardServiceSession` and add the attribute `[Immutable]` to the class.

[11]

* Go to `BoardService` and highlight `sessions` at post production.

[12]

* Add `[Immutable]` to BoardService.

* Change the type of `sessions`  to `ConcurrentDictionary<Guid, BoardServiceSession>` and made appropriate changes in source code.

[13]

* Start the app in the debugger.

* Wait for the exception.

[14]

[15]

* Add `[Writer]` to the `OnTimer` method.

[16]

* Go to `MainWindow.xaml.cs` and add `[Writer]` to the `OnConnectionClosed` method.

[17]

* Start the app again in the debugger and play a bit with it.

* Close the app.

[18]

* In Solution Explorer, right click on the solution, choose _Add_ then _PostSharp policy_ then _thread-safety policy_.

* Rebuild.

[19]

* Open the Error List and add post-production rectangles to the warnings.

* Double click on the warning.

* Add `[Synchronized]` on the `RandomGenerator` class.

[20]

* Go to `ViewModelKeyedCollection.OnModelCollectionChanged`.

* Remove the code xxx

* Add the attribute to the method: `[Dispatched(DispatchedExecutionMode.NonBlockingContextSwitch)]`.

[21]

* Go to `BoardService.OnCreaturePropertyChanged` and add `[Background].

* Go to `BoardService.OnCreatureCollectionChanged` and add `[Background].

* Go to `BoardClient.OnCreatureCollectionChanged` and add `[Background].

* Go to `BoardClient.OnCreatureCollectionChanged` and add `[Background].

[22]

* Start two instances of this app
* Connect one as a server and the second as client.
* Play a bit with both instances.