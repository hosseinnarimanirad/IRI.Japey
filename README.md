# 🌍 Maptor Spatial Library
**A .NET Library for Spatial Data Processing and Geospatial Analysis**  

[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://github.com/hosseinnarimanirad/IRI.Japey/blob/master/LICENSE)
[![Build](https://img.shields.io/github/actions/workflow/status/hosseinnarimanirad/Maptor/master-release.yml)](https://github.com/hosseinnarimanirad/Maptor/actions)
[![Contributions Welcome](https://img.shields.io/badge/contributions-welcome-brightgreen.svg)](CONTRIBUTING.md)

**Maptor** is an open-source **.NET GIS library** designed to make spatial operations, geospatial data processing, and map visualization easy and efficient.  
Built for **.NET 8+**, it provides a comprehensive set of tools for **geometry operations**, **coordinate transformations**, **raster & vector data handling**, and **OGC standards** support.
 
---

## ✨ Key Features  
✅ **Spatial Reference Systems (SRS)** – Coordinate transformations (WGS84, UTM, Mercator, WebMercator, custom SRIDs)   
✅ **Geometry Operations** – Points, Lines, Polygons, MultiPoints, MultiLines, MultiPolygons.   
✅ **Geodetic Calculations** – Distance, azimuth, and area computations.   
✅ **Raster & Vector I/O** – Shapefiles, GeoJSON, SQL Server, PostgreSQL, Personal GDB, GPX, WKB, WKT, and raster support.   
✅ **Graph algorithms** – BFS, DFS, Minimum spanning tree, Dijkstra, MinCut, FloydWarshall.   
✅ **OGC Standards** - WKB, WKT, WFS, WMS, GML 2, GML 3.  
✅ **Database integration** - SQL Server Spatial, PostGIS  
✅ WPF-based map viewer for desktop apps  

---

## 🏗 Repository Structure  

```
Maptor /
├── IRI.Sta/                     # Core spatial data structures & algorithms
├── IRI.Ket/                     # Utilities (data sources, file I/O)
├── IRI.Jab/                     # WPF components (MapViewer, dialogs, etc.)
├── IRI.Tst/                    # Unit tests
└── Examples/                    # Usage samples
```

---

## 📦 NuGet Packages

Maptor consists of multiple modular packages. Install only what you need:


### 📦 NuGet Packages

Maptor is modular. Install only what you need:  
👉 [Browse all packages on NuGet.org](https://www.nuget.org/profiles/narimanirad)

| Package | Description | Version |
|---------|-------------|---------|
| [IRI.Maptor.Core](https://www.nuget.org/packages/IRI.Maptor.Core) | Core GIS functionality: geometries, projections, spatial operations | ![NuGet](https://img.shields.io/nuget/v/IRI.Maptor.Core.svg?style=flat-square) |
| [IRI.Maptor.Ket.WebApiPersistence](https://www.nuget.org/packages/IRI.Maptor.Ket.WebApiPersistence) | Persistence layer for ASP.NET Core Web API | ![NuGet](https://img.shields.io/nuget/v/IRI.Maptor.Ket.WebApiPersistence.svg?style=flat-square) |
| [IRI.Maptor.Ket.SqlServerPersistence](https://www.nuget.org/packages/IRI.Maptor.Ket.SqlServerPersistence) | SQL Server integration for spatial persistence | ![NuGet](https://img.shields.io/nuget/v/IRI.Maptor.Ket.SqlServerPersistence.svg?style=flat-square) |
| [IRI.Maptor.Ket.SqlServerSpatialExtension](https://www.nuget.org/packages/IRI.Maptor.Ket.SqlServerSpatialExtension) | Adds SQL Server spatial data extensions | ![NuGet](https://img.shields.io/nuget/v/IRI.Maptor.Ket.SqlServerSpatialExtension.svg?style=flat-square) |
| [IRI.Maptor.UI](https://www.nuget.org/packages/IRI.Maptor.UI) | WPF map viewer for desktop apps | ![NuGet](https://img.shields.io/nuget/v/IRI.Maptor.UI.svg?style=flat-square) |

<details>
<summary>▶ Show more packages</summary>

| Package | Description | Version |
|---------|-------------|---------|
| [IRI.Maptor.Ket.Graph](https://www.nuget.org/packages/IRI.Maptor.Ket.Graph) | Graph algorithms (Dijkstra, Floyd-Warshall, etc.) | ![NuGet](https://img.shields.io/nuget/v/IRI.Maptor.Ket.Graph.svg?style=flat-square) |
| [IRI.Maptor.Ket.FileFormats](https://www.nuget.org/packages/IRI.Maptor.Ket.FileFormats) | Read/write GeoJSON, WKT, Shapefile | ![NuGet](https://img.shields.io/nuget/v/IRI.Maptor.Ket.FileFormats.svg?style=flat-square) |
| [IRI.Maptor.Ket.Raster](https://www.nuget.org/packages/IRI.Maptor.Ket.Raster) | Raster data handling | ![NuGet](https://img.shields.io/nuget/v/IRI.Maptor.Ket.Raster.svg?style=flat-square) |
| [IRI.Maptor.Ket.Gml](https://www.nuget.org/packages/IRI.Maptor.Ket.Gml) | GML and OGC standard support | ![NuGet](https://img.shields.io/nuget/v/IRI.Maptor.Ket.Gml.svg?style=flat-square) |
| [IRI.Maptor.Ket.WfsClient](https://www.nuget.org/packages/IRI.Maptor.Ket.WfsClient) | WFS client integration | ![NuGet](https://img.shields.io/nuget/v/IRI.Maptor.Ket.WfsClient.svg?style=flat-square) |
| [IRI.Maptor.Ket.WmsClient](https://www.nuget.org/packages/IRI.Maptor.Ket.WmsClient) | WMS client integration | ![NuGet](https://img.shields.io/nuget/v/IRI.Maptor.Ket.WmsClient.svg?style=flat-square) |
| [IRI.Maptor.Sta.Common](https://www.nuget.org/packages/IRI.Maptor.Sta.Common) | Common utilities for statistical analysis | ![NuGet](https://img.shields.io/nuget/v/IRI.Maptor.Sta.Common.svg?style=flat-square) |
| [IRI.Maptor.Sta.Spatial](https://www.nuget.org/packages/IRI.Maptor.Sta.Spatial) | Spatial statistical functions | ![NuGet](https://img.shields.io/nuget/v/IRI.Maptor.Sta.Spatial.svg?style=flat-square) |
| [IRI.Maptor.Sta.Graph](https://www.nuget.org/packages/IRI.Maptor.Sta.Graph) | Graph-based statistical analysis | ![NuGet](https://img.shields.io/nuget/v/IRI.Maptor.Sta.Graph.svg?style=flat-square) |

</details>

---
  
**Installation Example via NuGet CLI:**
```bash
dotnet add package IRI.Maptor.Sta.Common
dotnet add package IRI.Maptor.Ket.SqlServerPersistence
```


---

## 🚀 Quick Start  
### 1. Clone & Build  
```sh
git clone https://github.com/hosseinnarimanirad/Maptor.git  
dotnet build  
```

--- 

## 📚 Documentation

➡ [Full documentation and guides](https://github.com/hosseinnarimanirad/Maptor/wiki)

---

## 🤝 Contributing 
We welcome contributions! Please see [CONTRIBUTING.md](https://github.com/hosseinnarimanirad/Maptor/blob/master/CONTRIBUTING.md) and our [Code of Conduct](https://github.com/hosseinnarimanirad/Maptor/blob/master/CODE_OF_CONDUCT.md).

---

## 📜 License
Maptor is released under the [MIT License](https://github.com/hosseinnarimanirad/Maptor/blob/master/LICENSE.txt).
