# HashingUtility

This is a simple utility that illustrates an approach to building reusable tools in c#/.net. This utility provides a managed
approach to generating hashes. The use case is a generic class that can detect when changes have occured. Detecting such
things is important when you consider that data is typically serialized and deserialized. What happens when your data is
at rest? It's one thing when your objects are hydrated and your business rules can be invoked. But what happens when
your objects are out of scope? We can't foreclose every possibility that data will be disrupted. But we can detect
if data changed.

To understand how this utility works, consult the associated unit test project.
