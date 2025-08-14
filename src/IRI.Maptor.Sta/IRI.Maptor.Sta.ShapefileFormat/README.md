# 🗺️ Shapefile Format Library

[![NuGet Version](https://img.shields.io/nuget/v/IRI.Maptor.Sta.ShapefileFormat?color=blue&logo=nuget)](https://www.nuget.org/packages/IRI.Maptor.Sta.ShapefileFormat/)
[![License](https://img.shields.io/github/license/hosseinnarimanirad/Maptor)](LICENSE)

A comprehensive **.NET library** for reading, writing, and converting **ESRI Shapefile** formats with support for advanced geometries, projections, and international character encodings.

---

## ✨ Features

### Supported File Formats
- ✔ **SHP** – Geometry storage  
- ✔ **DBF** – Attribute data (with encoding support: UTF-8, Windows-1256, etc.)  
- ✔ **SHX** – Shape index file  
- ✔ **PRJ** – Projection information  

### Geometry Types
| Point Types | Polyline Types | Polygon Types | Multipoint Types |
|-------------|---------------|---------------|------------------|
| Point       | Polyline       | Polygon       | Multipoint       |
| PointM      | PolylineM      | PolygonM      | MultipointM      |
| PointZ      | PolylineZ      | PolygonZ      | MultipointZ      |

### Advanced Capabilities
- 🔄 Conversion between ESRI types and:
  - **WKT** (Well-Known Text)
  - **WKB** (Well-Known Binary)
- 🌍 Encoding support for **international characters** in DBF  
- 📦 **Memory-efficient streaming** API for very large shapefiles  
- 🔒 Strongly-typed attribute data  

---

## ⚙ Installation

```powershell
Install-Package IRI.Maptor.Sta.ShapefileFormat
```

---

## 💻 Usage Examples

### 1️⃣ Reading a Shapefile
```csharp
using IRI.Maptor.Sta.ShapefileFormat;

var shapes = await Shapefile.ReadShapesAsync("path/to/file.shp");

foreach (var shape in shapes)
{
    Console.WriteLine($"Geometry: {shape.AsSqlServerWkt()}");
}
```

---

### 2️⃣ Reading Shapefile with Attributes
```csharp
using IRI.Maptor.Sta.ShapefileFormat;

// Load geometry and attribute data
var shapefileData = await Shapefile.ReadAsync("data/roads.shp", "data/roads.dbf");

foreach (var record in shapefileData)
{
    var geometry = record.Shape.AsWkt();
    var name = record.Attributes["Name"]?.ToString();
    var type = record.Attributes["Type"]?.ToString();

    Console.WriteLine($"{name} ({type}) -> {geometry}");
}
```

---

### 3️⃣ Writing a New Shapefile
```csharp
using IRI.Maptor.Sta.ShapefileFormat;
using System.Collections.Generic;

var shapes = new List<EsriShape>
{
    new EsriShape(EsriShapeType.Point, new double[] { 51.3890, 35.6892 }) // Tehran
};

await Shapefile.WriteAsync("output/cities.shp", shapes);
```

---

### 4️⃣ Converting Between Formats
```csharp
var shapes = await Shapefile.ReadShapesAsync("data/buildings.shp");

foreach (var shape in shapes)
{
    var wkb = shape.AsWkb();  // Convert to Well-Known Binary
    var wkt = shape.AsWkt();  // Convert to Well-Known Text

    Console.WriteLine(wkt);
}
```

---

## 📌 Notes
- Make sure `.shp`, `.shx`, `.dbf`, and `.prj` files are in the same directory when reading shapefiles.  
- For large datasets, prefer **streaming methods** to avoid high memory usage.  
- Encoding can be specified when reading DBF files:  
```csharp
var shapes = await Shapefile.ReadShapesAsync("data/roads.shp", encoding: Encoding.UTF8);
```

---

## 📜 License
This project is licensed under the [MIT License](LICENSE).
