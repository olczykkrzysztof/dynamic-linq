# Info #

This project contains my extensions to Dynamic.cs library (commonly called DynamicLinq) included in [C# samples](http://msdn.microsoft.com/en-us/vstudio/bb894665.aspx).

# Added features #

  * `new Namespace.Type(expr as property_name)` grammar
  * `new @0(expr as property_name)` with Type given as placeholder grammar, i.e `Select("new @0(.....)", typeof(SomeType))`
  * `Select<OutputType>("....")` which returns `IQueryable<OutputType>` and makes sure the result of select is actually assignable to `OutputType`
  * `OutputType` of `Select<OutputType>`  visible in query under `@out` symbol - `Select<OutputType>("new @out (....)")`
  * `GroupBy<KeyType, ValType>("....", "....")` available similarly `Select<>`
  * other `GroupBy` variants
  * inferred `it` for indexers - i.e. `[0] > 10` equivalent to `it[0] > 10`
  * parentheses can be omitted when invoking argument-less lambda or aggregate
  * delegates can be passed for placeholder where lambda expression are expected.
  * `eval` operator which evaluates DynamicLinq language from string, e.g. `Where("some\_prop = eval @0", "10 + other\_prop");
  * Anonymous types in `new` operator can implement an interface - syntax: `new IInterface (expr as prop, ...)`.
