# CleanHandling
A clean way to deal with business logic and flows! 

Write only the code that is important for your business rules, because this library can handle the errors flows, reducing boilerplate code.

First get the result from a method, for example:
```c#
var result = await Result.Try(_session.SaveChangesAsync());
```
Then you can use the methods from the ResultExtension like:
```c#
var stringValueResult = result.Then(val => val.ToString());
Console.WriteLine(stringValueResult); // implicit convert result -> string
```
```c#
result.When(val => val is not null,
     @then: val => Result.From("Not Null"),
     @else: val => Result.From("Null"))
```


## Real examples in the [Finance Controlinator](https://github.com/gumberss/FinanceControlinator)
![image](https://user-images.githubusercontent.com/38296002/189543138-926eb261-ce7c-462a-9ec0-c613eefdb91d.png)
![image](https://user-images.githubusercontent.com/38296002/189543029-1c6e7756-1a0d-42a1-b9e4-0ffb1886d2e7.png)
