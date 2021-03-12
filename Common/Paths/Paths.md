# Url and Path Builder {#PathsMd}

## Overview {#PathsOverviewMd}
There are 2 classes for you to use:
1. [UrlBuilder](@ref Sphyrnidae.Common.Paths.UrlBuilder)
2. [RelativePathBuilder](@ref Sphyrnidae.Common.Paths.RelativePathBuilder)

Each of these classes utilize the <a href="https://en.wikipedia.org/wiki/Builder_pattern" target="blank">builder pattern</a>.
Behind the scenes, the <a href="https://docs.microsoft.com/en-us/dotnet/api/system.uribuilder?view=net-5.0" target="blank">UriBuilder</a> is used to perform most of the work.
What these individual classes offer is some enhanced capabilities and safer usage (eg. not needing to check for slashes "/").

## Where Used {#PathsWhereUsedMd}
None

## Examples {#PathsExampleMd}
<pre>
    // To generate the following URL: https://www.foo.com:123/a/b/c?attr1=1&attr2=2#gohere
    var url = new UrlBuilder()
        .AsHttps()
        .WithHost("www.foo.com")
        .WithPort(123)
        .AddPathSegment("a")
        .AddPathSegment("b")
        .AddPathSegment("c")
        .AddQueryString("attr1", "1")
        .AddQueryString("attr2", "2")
        .WithFragment("gohere")
        .Build();

    // To modify http://www.me.com/a/b/c?foo=bar#here to instead be https://www.me.com/a/b/d?attr1=1
    url = new UrlBuilder("http://www.me.com/a/b/c?foo=bar#here")
        .AsHttps()
        .RemoveLastSegment()
        .AddPathSegment("d")
        .ClearQueryString()
        .AddQueryString("attr1", "1")
        .WithFragment("")
        .Build();

    // Will generate the path: /a/b/c?attr1=1&attr2=2#gohere
    var path = new RelativePathBuilder()
        .AddPathSegment("a")
        .AddPathSegment("b")
        .AddPathSegment("c")
        .AddQueryString("attr1", "1")
        .AddQueryString("attr2", "2")
        .WithFragment("gohere")
        .Build();
</pre>