# Voiceover

[1]

Building a reliable multi-threaded app in .NET can be a very complex task. Working with locks, 
events and queues forces you to reason at a ridiculously low level of abstraction, and with so many
small details to think about, the code no longer fits in your brain. 
As a consequence, users report random exceptions that are impossible to reproduce.
 
With PostSharp Threading, you can address multi-threading at a higher level of abstraction, 
with deterministic machine-verified threading models. Thanks to PostSharp, your code will be cleaner,
you will avoid random errors, and you will get more time to focus on what matters.

[2]

Here is a sample multi-threaded app. This is a multi-player WPF game where the state can be changed
either because of the UI, the timer, or the network. If two of these sources change the state at the same time, 
random errors may happen.

Let's see how you can make this app thread-safe with PostSharp Threading.

[3]

The source code has four namespaces: the view, the view model, the model, and the communication stack. 
Instead of reasoning about individual fields, you can take decisions about whole namespaces.

[4]

For the view and the view model, the thread-affine model is a good choice. It means that only
the UI thread will have access to these objects. 

[5] 

In all classes assigned to a threading model, you must mark children fields with the `Child` attribute
so that PostSharp knows they are a part of the same entity. In this class, all fields and properties
are references.


[6]

Instead of applying the aspect to every single class, you can apply it on an interface and it
will automatically apply to all derived classes.

`Creatures` are children objects of `BoardViewModel`, so you can mark it with the `[Child]` attribute.


[7]

Unlike the view, the model classes are accessed from both the UI thread and several background
threads. Since the state of these objects is mutable, the reader-writer synchronized model is great
for this namespace. It allows several readers at the same time, but writers get exclusive access.

[8]

This model requires you to mark public methods as readers or writers.

[9]

Let's now look at the communication namespace.

The network client has no mutable state and can be marked as immutable. Of course this class
was already thread-safe before, but thanks to this aspect you are now guaranteed to get a runtime exception 
if someone inadvertently adds some mutable state to this class.

[10]

The same with the server-side session object.

[11]

For some classes, the choice is not so simple. The `BoardService` class contains a mutable list of 
client sessions, which seems to call for a lock-based model, but it also performs I/O so a lock-based approach 
would cause high thread contention. 

[12]

The best option here is to treat this class as two entities: `BoardService`
itself will be immutable, but the session list will be stored in a thread-safe dictionary. 

[13]

Run the app and see if it works.

[14]

Oops. The `ThreadAccessException` means that timer thread is attempting to access the `Board` object but does
not own the proper access right. This is because the `OnTimer` method is missing a `Writer` attribute. 

[15]

The exception is _intentional_ and will only happen in your _debug_ build. Without
this exception, you would have overlooked the defect and risked random failures in production. But thanks to this exception, 
the problem has been made visible and you're now forced to address it. That's what
we mean by deterministic machine-verified threading models. PostSharp only raises a compilation error 
for  _public_ members when it misses a `Reader` or `Writer` attribute, but this is a _private_ event handler method.
That's why the problem has only been identified at run time.

[16]

There is another occurrence of this error in the `OnConnectionClosed` handler of the thread-affine `MainWindow` class.
The handler is invoked from a background thread so it would also throw a `ThreadAccessException`.

To force the method to execute on the UI thread, apply the `Dispatched` aspect.

[17]

Run the app again.

Great! 

[18]

How can you know that you are done and that you didn't forget to assign any class to a threading model?

You can add the _Thread Safety Policy_ to the project. It will check your code during the build and warn you
about potentially unsafe classes and static fields.

[19]

Oh, we forgot the `RandomGenerator` class. Make it a _synchronized_ object. It never gets accessed concurrently.
Instead of an array, use an `ImmutableArray` wherever you can. It makes it easier to reason about your code.

Build again to see if the warnings disappeared.

Done!

[20]

You can still do a few optimizations on this codebase.

This code in `OnModelCollectionChanged` can be replaced by a simple `[Dispatched]` aspect.

[21]

The app runs more smoothly if the communication stack processes events in the background. 
You can do that with the `[Background]` aspect.

[22]

Let's run the app again and check that everything works.

Excellent.

Thank you for watching this demo.

Thanks to PostSharp Threading, you can make your multi-threaded application robust and reliable. By assigning
deterministic threading models to your code, you can identify defects that would have otherwise staid hidden and
caused random failures in production. With PostSharp Threading, you can take architectural decisions at the level 
of class hierarchies or namespaces instead of individual fields, reducing by an order of magnitude the number of decisions to be taken.
Trust the build process to enforce these decisions across your whole codebase and stop worrying about multi-threading.



 

 





 

  
