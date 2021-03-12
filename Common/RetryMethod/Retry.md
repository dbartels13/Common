# Retry {#RetryMd}
The static [Retry](@ref Sphyrnidae.Common.RetryMethod.Retry) class has a Do() method (with several overloads) that allow you to retry execution of a method.
A failed attempt is anything that throws an exception.
If the method does not throw an exception, it will just run normally without being retried.
This method is essentially a wrapper around your method which allows it to [Retry](@ref Sphyrnidae.Common.RetryMethod.Retry) according to [RetryOptions](@ref Sphyrnidae.Common.RetryMethod.Models.RetryOptions).

## Where Used {#RetryWhereUsedMd}
None

## Examples {#RetryExampleMd}
<pre>
    var rand = new Random();
    var result = await Retry.Do(() =>
    {
        var num = rand.Next(100);
        if (num % 2 == 0)
            throw new Exception("Even numbers not allowed");
        return num;
    }); // Default retry options
</pre> 
