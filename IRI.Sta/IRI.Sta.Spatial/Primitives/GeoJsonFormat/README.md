# GeoJson Support in Maptor

![geo](https://github.com/user-attachments/assets/21ea02ee-f3a9-4fc7-bfe7-1f9c15977fd6)

A robust .NET Standard implementation of GeoJson (RFC 7946) for spatial data interchange, supporting read/write operations, conversion to/from geometry types, and validation.

## Features

- **Full RFC 7946 Compliance**:
  - Parsing & serialization of all GeoJson types (Point, LineString, Polygon, etc.)
  - Support for Feature Collections and Geometry Collections
    
- **Advanced Conversion**:
  - Bidirectional conversion between GeoJson and native geometry types (WKT, WKB, ESRI Shapefile)
  
- **Validation**:
  - Coordinate system (CRS) awareness
  - Anti-meridian crossing detection
  

## Installation

Via NuGet Package Manager:
```bash
dotnet add package IRI.Sta.Spatial
```
