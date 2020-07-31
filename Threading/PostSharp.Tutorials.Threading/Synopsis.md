# Synopsis

## General Instructions

* Recording resolution: 1280�720 
* Visual Studio 2019
* White Theme
* Font size: x pt
* Include the mouse pointer in the capture

The following steps are temporary bug workaround:

* In VS, make sure other versions of PostSharp Tools for Visual Studio are uninstalled.
* Install the VSIX given in the OneDrive folder (this is another one than for the previous video).
* Download the `*.nupkg` files from OneDrive and copy the directory path to the clipboard.
* In VS, go to Tools / Options / NuGet Package Manager / Package Sources.
* Name it say `PostSharp Private` and paste the directory path.
* Disable PostSharp Assistant in PostSharp Options.


## Step 0. Preparation

* Open the pre-prepared project from https://github.com/postsharp/PostSharp.Tutorials/tree/master/Threading/PostSharp.Tutorials.Threading.
  Attention: this is the _before_ branch. The master branch is what you get when you've completed the steps below.

 ## Step 1. Introduction

 [2]

 * Start two instances of the sample application side by side, in such a way that they don't overlap.
 * Connect one as the server.
 * Connect the other as the client.
 * In the first instance, click on a creature and then, using arrows, move it.
 * In the second instance, click on _Add creature_ and then, using the arrows, move it.

 [3]

 * Go back to VS and expand all solution folders (except References and Properties).

 [4]

 * Open `MainWindow.xaml.cs`, place the caret in the class name, press `Alt+Enter` to open the refactoring menu,
   choose _Apply threading model_, choose _Thread affine_ and click _Next_.

[5]

* Click _Next_.

[6]

* Go to `IViewModel` and add the '[ThreadAffine]' custom attribute.

* Go to `BoardViewModel.Creatures` and add '[Child]`.

[7]

* Go to `Board`, open the refactoring menu with `Alt+Enter`, choose _Apply threading model_, choose _reader-writer synchronized_, click _Next_.
  For the `Creatures` field, choose _Collection of children_, click _Next_.

[8]

* Go to `Board.Dispose` and add `[Writer]
* Go to `Creature` and add the attribute `[ReaderWriterSynchronized]` to the class.
* Add the `[Writer]` attribute to `TryMove`,  `Rotate`, `MoveTo`, `TryMoveTo`.

[9]

* Go to `BoardServiceClient` and add the attribute `[Immutable]` to the class.

[10]

* Go to `BoardServiceSession` and add the attribute `[Immutable]` to the class.

[11]

* Go to `BoardService` and highlight `sessions` at post production.

[12]

* Add `[Immutable]` to BoardService.

* Add `[Reference]` to `sessions`.

* Change the type of `sessions`  to `ConcurrentDictionary<Guid, BoardServiceSession>` and made appropriate changes in source code:

```cs
        internal void AddSession(BoardServiceSession session)
        {
            this.sessions.TryAdd(session.Id, session);
        }

        internal void RemoveSession(Guid sessionId)
        {
             this.sessions.TryRemove(sessionId, out _);
        }
```

[13]

* Start the app in the debugger.

* Wait for the ThreadAccessException in OnTimer.

[14]

[15]

* Stop the debugger.

* Add `[Writer]` to the `OnTimer` method.

[16]

* Go to `MainWindow.xaml.cs` and add `[Dispatched(DispatchedExecutionMode.NonBlocking)]` to the `OnConnectionClosed` method.

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

* Build and show that the warnings have disappeared.

[20]

* Go to `ViewModelKeyedCollection.OnModelCollectionChanged`.

* Remove the following lines in the beginning of the method:

```cs
            void RaiseEvents()
            {
```

* Remove this at the end:

```cs
     }

            if (this.dispatcher.CheckAccess())
            {
                RaiseEvents();
            }
            else
            {
                this.dispatcher.BeginInvoke(new Action(RaiseEvents));
            }
```

* Add the attribute to the method: `[Dispatched(DispatchedExecutionMode.NonBlockingContextSwitch)]`.

[21]

* Go to `BoardService.OnCreaturePropertyChanged` and add `[Background].

* Go to `BoardService.OnCreatureCollectionChanged` and add `[Background].

* Go to `BoardServiceClient.OnCreatureCollectionChanged` and add `[Background].

* Go to `BoardServiceClient.OnCreatureCollectionChanged` and add `[Background].

[22]

* Start two instances of this app
* Connect one as a server and the second as client.
* Play a bit with both instances.
