using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("IRI.Ket.ShapefileFormat")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("IRI.Ket.ShapefileFormat")]
[assembly: AssemblyCopyright("Copyright © Microsoft 2010")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

//[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("IRI.Ket.DataManagement,PublicKey=0024000004800000940000000602000000240000525341310004000001000100c5f59997f117c0ff22ca22d843004ec0aee973834b65b9750141075b677f2218f82bcb0ee00a214a7dc049f7271cd1d85c8685ae5a9dfbbade64f990fa5a0eca107891deb84b5221d0b6ce0f1a90da0dae8876e956e02d23fca22cafaae2c6191dcd631aa33a4c7f03a9d95f0a328595c919472be0ed715b69cb8d738b4ffc9f")]
//[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("IRI.Bag.ShapefileFactory,PublicKey=0024000004800000940000000602000000240000525341310004000001000100c5f59997f117c0ff22ca22d843004ec0aee973834b65b9750141075b677f2218f82bcb0ee00a214a7dc049f7271cd1d85c8685ae5a9dfbbade64f990fa5a0eca107891deb84b5221d0b6ce0f1a90da0dae8876e956e02d23fca22cafaae2c6191dcd631aa33a4c7f03a9d95f0a328595c919472be0ed715b69cb8d738b4ffc9f")]
//[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("IRI.Bag.PrintCoordinates,PublicKey=0024000004800000940000000602000000240000525341310004000001000100c5f59997f117c0ff22ca22d843004ec0aee973834b65b9750141075b677f2218f82bcb0ee00a214a7dc049f7271cd1d85c8685ae5a9dfbbade64f990fa5a0eca107891deb84b5221d0b6ce0f1a90da0dae8876e956e02d23fca22cafaae2c6191dcd631aa33a4c7f03a9d95f0a328595c919472be0ed715b69cb8d738b4ffc9f")]
//[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("IRI.Bag.SqlCeDatabaseMaker,PublicKey=0024000004800000940000000602000000240000525341310004000001000100c5f59997f117c0ff22ca22d843004ec0aee973834b65b9750141075b677f2218f82bcb0ee00a214a7dc049f7271cd1d85c8685ae5a9dfbbade64f990fa5a0eca107891deb84b5221d0b6ce0f1a90da0dae8876e956e02d23fca22cafaae2c6191dcd631aa33a4c7f03a9d95f0a328595c919472be0ed715b69cb8d738b4ffc9f")]

//[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("IRI.Mor.DataPreparation,PublicKey=0024000004800000940000000602000000240000525341310004000001000100c5f59997f117c0ff22ca22d843004ec0aee973834b65b9750141075b677f2218f82bcb0ee00a214a7dc049f7271cd1d85c8685ae5a9dfbbade64f990fa5a0eca107891deb84b5221d0b6ce0f1a90da0dae8876e956e02d23fca22cafaae2c6191dcd631aa33a4c7f03a9d95f0a328595c919472be0ed715b69cb8d738b4ffc9f")]

//[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("IRI.Test.FileFormats,PublicKey=0024000004800000940000000602000000240000525341310004000001000100c5f59997f117c0ff22ca22d843004ec0aee973834b65b9750141075b677f2218f82bcb0ee00a214a7dc049f7271cd1d85c8685ae5a9dfbbade64f990fa5a0eca107891deb84b5221d0b6ce0f1a90da0dae8876e956e02d23fca22cafaae2c6191dcd631aa33a4c7f03a9d95f0a328595c919472be0ed715b69cb8d738b4ffc9f")]

//[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("IRI.MainProject,PublicKey=0024000004800000940000000602000000240000525341310004000001000100c5f59997f117c0ff22ca22d843004ec0aee973834b65b9750141075b677f2218f82bcb0ee00a214a7dc049f7271cd1d85c8685ae5a9dfbbade64f990fa5a0eca107891deb84b5221d0b6ce0f1a90da0dae8876e956e02d23fca22cafaae2c6191dcd631aa33a4c7f03a9d95f0a328595c919472be0ed715b69cb8d738b4ffc9f")]
//[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("IRI.App.ZaminNegar,PublicKey=0024000004800000940000000602000000240000525341310004000001000100c5f59997f117c0ff22ca22d843004ec0aee973834b65b9750141075b677f2218f82bcb0ee00a214a7dc049f7271cd1d85c8685ae5a9dfbbade64f990fa5a0eca107891deb84b5221d0b6ce0f1a90da0dae8876e956e02d23fca22cafaae2c6191dcd631aa33a4c7f03a9d95f0a328595c919472be0ed715b69cb8d738b4ffc9f")]                                                                                                 
//[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("IRI.NGO.KhoraksazHamrah.WpfUserInterface,PublicKey=0024000004800000940000000602000000240000525341310004000001000100c5f59997f117c0ff22ca22d843004ec0aee973834b65b9750141075b677f2218f82bcb0ee00a214a7dc049f7271cd1d85c8685ae5a9dfbbade64f990fa5a0eca107891deb84b5221d0b6ce0f1a90da0dae8876e956e02d23fca22cafaae2c6191dcd631aa33a4c7f03a9d95f0a328595c919472be0ed715b69cb8d738b4ffc9f")]
 

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("90001e17-30bb-412a-aa5a-0792c87fac9a")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.*")]
//[assembly: AssemblyFileVersion("1.0.0.0")]
