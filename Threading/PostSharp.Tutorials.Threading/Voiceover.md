# Voiceover

Building a reliable multi-threaded app in .NET can be a very complex task. Working with locks, 
events and queues force you to reason at a ridiculously low level of abstraction, with too many
small details to think about. Defects typically cause random errors that are impossible to reproduce.
 
With PostSharp Threading, you can address multi-threading at a higher level of abstraction, 
with deterministic machine-verified threading models. Thanks to PostSharp, your code will be cleaner,
you will avoid random errors, and you will get more time to focus on what matters.

Here is a sample multi-threaded app. This is a multi-player WPF game where modifications come
from three sources: the UI, the timer, and the network. If two of these sources trigger changes
 at the same time, random defects may happen.

Let's see how we can make this app thread-safe with PostSharp Threading.

This app has four parts: the view, the view model, the model, and the communication stack. Instead
of reasoning about individual fields, we will take decisions about whole families of classes.

For the view and the view model, we apply the thread-affine model, which means that only
the UI thread will have access to these objects. From the moment we take this decision, we can
completely stop thinking about multi-threading in these classes.

With all threading models, we must annotate children fields so that PostSharp knows they
should be protected as a part of the object.

Unlike the view, the model classes are accessed both from the UI thread and several background
threads. Since the state of these objects is mutable, we choose the reader-writer synchronized
model. 

Instead of applying the aspect to every single class, we can apply it on an interface and it
will automatically apply to all derived classes. 

The reader-writer synchronized model asks us to mark public methods as readers or writers.

Let's look at the communication stack.

The network client has no mutable state and can be marked as immutable. Of course this class
was already thread-safe before, but we are now guaranteed to get a runtime exception if someone
inadvertently adds some mutable state to this class.

The same with the server-side session object.

Sometimes it's not so easy. The server-side service has mutable state, the list of client sessions,
but a lock-based threading model would have poor performance because the class performs network I/O. 
So we mark this class as immutable with a reference to a thread-safe collection.

Let's run the app and see if it works.

Oops. This exception means that timer thread is attempting to access the `Board` object but does
not hold the proper access right. This is because we did not annotate the `OnTimer` method with the
`Writer` attribute. The exception is _intentional_ and will only happen in your debug build. Without
this exception, you would have missed a defect and risked random failures in production. That's what
we mean by deterministic machine-verified threading model.

PostSharp missed that error during compilation because this is a private method used as an event handler.
There is another occurrence of this error in this code: the `OnConnectionClosed` handler, invoked
from the background thread. Let's use the `Dispatched` aspect to force it to run on the UI thread.

Let's run the app again.

Great! 

How can we know that we are done and we didn't forget to assign any class to a threading model?

Let's add the _Thread Safety Policy_ to the project and build the project. This policy will warn us
about potentially unsafe classes and dangerous static fields.

Oh, we forgot the `RandomGenerator` class. Let's make it a _synchronized_ class so everything executes within a lock.

Done!

A last thing you can do with PostSharp Threading is easily dispatch the execution of a method to
a foreground or a background thread.

This code in `OnModelCollectionChanged` ensures that the collection is modified from the UI thread whereas 
the event is triggered from a background thread. This complex logic can be replaced by a simple `[Dispatched]` aspect.

The communication stack would run smoother if some methods were called in the background. We can do that with the `[Background]`
aspect.

Let's run the app again and check that everything works.

Excellent.

Thank you for watching this demo.

Thanks to PostSharp Threading, you can make your multi-threaded application robust and reliable. By assigning 
deterministic threading models to your code, you can identify defects that would have otherwise staid hidden and
caused random failures in production. You can take architectural decisions at the level of class hierarchies or namespaces
instead of individual fields, reducing by an order of magnitude the number of decisions to be taken. And you enforce these
 decisions across your whole codebase without relying on code reviews. 



 

 





 

  
