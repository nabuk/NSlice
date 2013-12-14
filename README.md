##About NSlice

NSlice is a free .NET library, designed to give you more flexibility in manipulating collections, originally inspired by *Python slicing* feature.

*(Jump straight to the [Cheat Sheet](https://github.com/nabuk/NSlice/wiki/Cheat-Sheet) if you want to see code samples right away.)*

## Overview

NSlice was written to allow slicing 3 types of .NET collections in the most efficient manner:
  
- Indexed collections (`IList<>` implementations) - slicing is performed eagerly and instantly. It does not matter whether the collection has 10 elements or 10 million elements. This is because the result is not created by copying elements, but by creating a transparent view over source collection.
- Enumerables (`IEnumerable<>` implementations) - slicing is performed lazily. Each possible slice scenario was implemented separately to achieve the best speed performance and least memory footprint.  
It fits nicely into the LINQ model and could be even used to slice a stream, if the latter was wrapped into `IEnumerable<>` implementation.
- Strings - slicing is performed eagerly and a new string is returned as a result.

##What is *slicing* ?

*Slicing* is just a `for` loop shortcut. Look at the following example:

	var collection = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
	var result = collection.Slice(1, 9, 2); //result: { 1, 3, 5, 7 }

**Note:**
> Slice method has the following signature:  
> `Slice(int? from = null, int? to = null, int step = 1)`   
> All arguments are optional:
> 
> - `from` - start index
> - `to` - exclusive boundary
> - `step` - iteration step

But there is more. Slice allows each argument to be negative. It might seem weird to use negative indices but they are really useful. What are negative indices? They just index the collection backwards, for example: `-1` means last, `-2` means one before last and so on. See the following two examples to grasp the idea:

- Get last five elements: `Slice(-5)`

- Get collection without first and last element: `Slice(1, -1)`

##Documentation

- [Wiki](https://github.com/nabuk/NSlice/wiki)
