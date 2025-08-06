# 🌍 GeoJson Support in Maptor

![GeoJson](https://img.shields.io/badge/GeoJson-RFC_7946_compliant-blue) 
![.NET](https://img.shields.io/badge/.NET-Standard_2.0-green)

A .NET Standard implementation of GeoJson (RFC 7946) for spatial data interchange, supporting read/write operations, conversion to/from geometry types, and validation.

![geo](https://github.com/user-attachments/assets/21ea02ee-f3a9-4fc7-bfe7-1f9c15977fd6)

## 📌 Features

- **Full GeoJSON Support**  
  ✅ RFC 7946 compliant  
  ✅ Geometry types: `Point`, `MultiPoint`, `LineString`, `MultiLineString`, `Polygon`, `MultiPolygon`  
  ✅ Feature collections with properties
  
- **Conversion Tools**  
  🔄 GeoJSON ↔ Geometry  
  🔄 GeoJSON ↔ ESRI Shapefile
  

## 📦 Installation
```bash
dotnet add package IRI.Maptor.Sta.Spatial
```

## 🚀 Quick Start
Basic Parsing from file

```C#
using IRI.Maptor.Sta.Spatial.GeoJsonFormat;

// Read file and Parse from string
var features = GeoJson.ReadFeatures(geoJsonfile);

foreach (var feature in features)
{
    Console.WriteLine($"Feature: {feature.Type}");
}
```

Deserialize GeoJson geometry

```C#
var multiPointString = "{\"type\": \"MultiPoint\", \"coordinates\": [[10.1, 40.1], [40.1, 30.1], [20.1, 20.1], [30.1, 10.1]]}";
IGeoJsonGeometry geoJsonMultiPoint = GeoJson.Deserialize(multiPointString);

var lineStringString = "{\"type\": \"LineString\", \"coordinates\": [[30.1, 10.1], [10.1, 30.1], [40.1, 40.1]]}";
IGeoJsonGeometry lineString = GeoJson.Deserialize(lineStringString);
```

Convert Geometry to GeoJson
```C#
var point = Geometry<Point>.Create(30, 10);
var geoJsonPoint = point.AsGeoJson();
```


## 📚 Documentation
- [GeoJSON RFC 7946](https://tools.ietf.org/html/rfc7946)   

## 🤝 Contributing
Pull requests welcome! Please:
1. Fork the repository  
2. Create your feature branch  
3. Submit a PR with tests  
