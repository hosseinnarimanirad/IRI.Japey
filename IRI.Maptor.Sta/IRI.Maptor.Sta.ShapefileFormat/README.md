# Shapefile Format Library

[![NuGet Version](https://img.shields.io/nuget/v/IRI.Maptor.Sta.ShapefileFormat?color=blue&logo=nuget)](https://www.nuget.org/packages/IRI.Maptor.Sta.ShapefileFormat/)
[![License](https://img.shields.io/github/license/hosseinnarimanirad/Maptor)](LICENSE)

A comprehensive .NET library for reading, writing, and converting ESRI Shapefile formats with support for advanced geometries and character encodings.

## Features

### Supported File Formats
- **SHP** - Geometry storage
- **DBF** - Attribute data with special encoding support
- **SHX** - Shape index
- **PRJ** - Projection information

### Geometry Types
| Point Types | Polyline Types | Polygon Types | Multipoint Types |
|------------|---------------|--------------|-----------------|
| Point      | Polyline      | Polygon      | Multipoint      |
| PointM     | PolylineM     | PolygonM     | MultipointM     |
| PointZ     | PolylineZ     | PolygonZ     | MultipointZ     |

### Advanced Capabilities
- Conversion between ESRI types and standard formats:
  - **WKT** (Well-Known Text)
  - **WKB** (Well-Known Binary)
- DBF file encoding support for international character sets
- Memory-efficient streaming API for large files
- Strongly-typed attribute data handling

## Installation

```bash
Install-Package IRI.Maptor.Sta.ShapefileFormat
```

## Usage
### Reading a Shapefile

```
using IRI.Maptor.Sta.ShapefileFormat;

var esriShapes = await Shapefile.ReadShapesAsync("path/to/file.shp");

foreach (var shape in esriShapes)
{
    Console.WriteLine($"Geometry: {shape.AsSqlServerWkt()}");            
}
```

## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss proposed changes.
