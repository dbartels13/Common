# Binary Lists {#BinaryListMd}

A binary list is a <a href="https://docs.microsoft.com/en-us/dotnet/api/system.collections.objectmodel.readonlycollection-1?view=net-5.0" target="blank">ReadOnlyCollection</a> that gives a huge performance boost for searching for items.
C# does have a list method called BinarySearch(), but this still requires the following to use:
1. The list must already be sorted
2. You specify the comparison operator

This [BinaryList](@ref Sphyrnidae.Common.BinaryList) provides this ability (and new abilities) in a much cleaner way.
First, you must have an IEnumerable.
Then you call the extension method ToBinaryList() (see [ListExtensions](@ref Sphyrnidae.Common.Extensions.ListExtensions)).
This converts the IEnumerable to a ReadOnlyCollection where you can perform Binary Searches (or any other methods on the ReadOnlyCollection).

## Binary List {#BinaryClassMd}
The conversion method requires you to specify the "keySelector".
This is a lamba expression which selects the item in the list (eg. x => x.myField).
The field in the list must be an IComparable.
The binary list will automatically sort on that field so that all method calls will work against the already ordered list.

Methods:
1. BinarySearch: You specify a value and this will return the index of the item in the list (or -1 if not found). If there are multiple records that matched, there is no guarantee as to which index it will be (eg. first, last, somewhere in the middle)
2. FindBinary: Similar to BinarySearch, this will return a matching record (or default if not found)
3. Has: Returns true if BinarySearch finds an item, otherwise returns false
4. FindBinaryList: This will return all records that have the supplied value (or an empty list if none were found)
5. Intersect: This compares the BinaryList to another IEnumerable and returns all records from the IEnumerable that have a matching value in the BinaryList
6. NonIntersect: The opposite of intersect (records in the IEnumerable that don't exist in the BinaryList)

## Case Insensitive Binary List {#CaseInsensitiveBinaryListMd}
By calling the ToCaseInsensitiveBinaryList() method on a list (see [ListExtensions](@ref Sphyrnidae.Common.Extensions.ListExtensions)), this creates a [CaseInsensitiveBinaryList](@ref Sphyrnidae.Common.CaseInsensitiveBinaryList).
This class is a specific instance of the BinaryList where the keySelector must point to a field that is of type string.
The resulting class will allow the same actions as BinaryList, but comparisons will be done case-insensitive.
Eg. "AbC" matches "abc".

If your IEnumerable contains strings (as opposed to a class with fields), then you should call the ToCaseInsensitiveBinaryList() method without a keySelector.

## Where Used {#BinaryListWhereUsedMd}
1. [Settings](@ref SettingsMd): A setting will be stored and looked up based on the "key" of the record (case-insensitive)
2. [SearchableRoles](@ref Sphyrnidae.Common.Authentication.SphyrnidaeIdentity.SearchableRoles): This is the property that should be used to find roles for a user as it is a BinaryList (as opposed to [Roles](@ref Sphyrnidae.Common.Authentication.SphyrnidaeIdentity.Roles) which is used for adding roles to a user)
3. [Logger Configuration](@ref Sphyrnidae.Common.Logging.LoggerConfiguration): All configurations will be a CaseInsensitiveBinaryList for optimal performance

## Examples {#BinaryListExampleMd}
<pre>
	var stringList = new List<string> { "A", "b", "C" };
	var binaryList = stringList.CaseInsensitiveBinaryList();
	binaryList.FindBinary("a"); // => "A"

	var widgetList = new List<Widget> {
		new Widget { Id = 1, Name = "foo" },
		new Widget { Id = 2, Name = "foo" },
		new Widget { Id = 3, Name = "foo2" }
	};
	var binaryWidgetList = widgetList.ToBinaryList(x => x.Name);
	var matchedItems = binaryWidgetList.FindBinaryList("foo"); // 2 Widgets are returned in a list
</pre> 
