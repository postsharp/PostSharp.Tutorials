# Voiceover

[1]

Building a reliable multi-threaded app in .NET can be a very complex task. Working with locks, 
events and queues force you to reason at a ridiculously low level of abstraction, with too many
small details to think about. Defects typically cause random errors that are impossible to reproduce.
 
With PostSharp Threading, you can address multi-threading at a higher level of abstraction, 
with deterministic machine-verified threading models. Thanks to PostSharp, your code will be cleaner,
you will avoid random errors, and you will get more time to focus on what matters.

[2]

Here is a sample multi-threaded app. This is a multi-player WPF game where state changes come
from three sources: the UI, the timer, and the network. If two of these sources trigger changes
 at the same time, random defects may happen.

Let's see how we can make this app thread-safe with PostSharp Threading.

[3]

The source code has four namespaces: the view, the view model, the model, and the communication stack. 
Instead of reasoning about individual fields, we will take decisions about whole namespaces.

[4]

For the view and the view model, you can apply the thread-affine model, which means that only
the UI thread will have access to these objects. From the moment you take this decision, you can
completely stop thinking about multi-threading in these classes.

[5] 

In all classes assigned to a threading model, you must mark children fields with the `Child` attribute
so that PostSharp knows they are a part of the same entity. In this class, all fields and properties
are references.


[6]

Instead of applying the aspect to every single class, you can apply it on an interface and it
will automatically apply to all derived classes.

`Creatures` are children object of `BoardViewModel`, so you can mark it as the `[Child]` attribute.


[7]

Unlike the view, the model classes are accessed both from the UI thread and several background
threads. Since the state of these objects is mutable, we choose the reader-writer synchronized
model for this namespace. 

[8]

This model asks us to mark public methods as readers or writers.

[9]

Let's now look at the communication namespace.

The network client has no mutable state and can be marked as immutable. Of course this class
was already thread-safe before, but thanks to this aspect we are guaranteed to get a runtime exception 
if someone inadvertently adds some mutable state to this class.

[10]

The same with the server-side session object.

[11]

For some classes, the choice is not so simple. The `BoardService` class contains a mutable list of 
client sessions which seems to call for a lock-based model, but it also performs I/O so a lock-based approach 
would cause high thread contention. 

[12]

So we decide to treat this class as two objects: `BoardService`
itself will be immutable, and we choose a thread-safe dictionary to maintain the session list. 

[13]

Let's run the app and see if it works.

[14]

Oops. The `ThreadAccessException` means that timer thread is attempting to access the `Board` object but does
not hold the proper access right. This is because we did not annotate the `OnTimer` method with the
`Writer` attribute. 

[15]

The exception is _intentional_ and will only happen in your debug build. Without
this exception, you would have missed a defect and risked random failures in production. That's what
we mean by deterministic machine-verified threading model. PostSharp missed that error during compilation 
because this is a private method used as an event handler.

[16]

There is another occurrence of this error in this code: the `OnConnectionClosed` handler, invoked
from the background thread. Let's use the `Dispatched` aspect to force it to run on the UI thread.

[17]

Let's run the app again.

Great! 

[18]

How can we know that we are done and we didn't forget to assign any class to a threading model?

Let's add the _Thread Safety Policy_ to the project and build the project. This policy will warn us
about potentially unsafe classes and dangerous static fields.

[19]

Oh, we forgot the `RandomGenerator` class. Let's make it a _synchronized_ class so everything executes within a lock.

Done!

[20]

A last thing you can do with PostSharp Threading is easily dispatch the execution of a method to
a foreground or a background thread.

This code in `OnModelCollectionChanged` ensures that the collection is modified from the UI thread whereas 
the event is triggered from a background thread. This complex logic can be replaced by a simple `[Dispatched]` aspect.

[21]

The communication stack would run smoother if some methods were called in the background. We can do that with the `[Background]`
aspect.

[22]

Let's run the app again and check that everything works.

Excellent.

Thank you for watching this demo.

Thanks to PostSharp Threading, you can make your multi-threaded application robust and reliable. By assigning 
deterministic threading models to your code, you can identify defects that would have otherwise staid hidden and
caused random failures in production. You can take architectural decisions at the level of class hierarchies or namespaces
instead of individual fields, reducing by an order of magnitude the number of decisions to be taken. And you enforce these
 decisions across your whole codebase without relying on code reviews. 



 

 





 

  
