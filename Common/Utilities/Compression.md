# Compression {#CompressionMd}

## Overview {#CompressionOverviewMd}
Compression is a technique to remove extra space in something to have that something be represented by the smallest possible object.
Zipping a file is a common compression technique.
The [Compression](@ref Sphyrnidae.Common.Utilities.Compression) class has static methods that work against strings and byte arrays.
Eg. Takes a string, compresses it to a byte[], then can decompress this back to a string.

## Where Used {#CompressionWhereUsedMd}
None

## Examples {#CompressionExampleMd}
<pre>
	var myString = "Some string that needs compressed, was possibly a serialized object";
	var bytes = myString.Compress();
	var copyOfMyString = bytes.DecompressToString(); // myString == copyOfMyString
</pre>