## Release 0.1.0

### New Rules

Rule ID | Category | Severity | Notes
--------|----------|----------|--------------------
NI2S0001  | Usage   | Error  | [AlwaysInterleave] must only be used on the grain interface method and not the grain class method
NI2S0002  | Usage   | Error  | Reference parameter modifiers are not allowed
NI2S0003  | Usage   | Info   | Add serialization [Id] and [NonSerialized] attributes
NI2S0004  | Usage   | Info   | Add [GenerateSerializer] attribute to [Serializable] type.
NI2S0005  | Usage   | Error  | Abstract/serialized properties cannot be serialized
NI2S0006  | Usage   | Error  | 
NI2S0007  | Usage   | Error  | Grain interfaces cannot have properties
NI2S0008  | Usage   | Error  | Grain interface methods must return a compatible type
