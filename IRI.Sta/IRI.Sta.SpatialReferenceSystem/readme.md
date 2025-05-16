# 🌐 IRI.Sta.SpatialReferenceSystem

**A .NET library for advanced spatial reference systems (SRS), geodetic transformations, and map projections**

Provided SRSs are based on horizontal spatial reference systems introduced in geodesy. Including main category of terresterial, celestial and orbital coordinate systems including

- Terrestrial Coordinate Systems
  - Conventional Terrestrial (CT or AT)
  - Instantaneous Terrestrial (IT)
  - Geodetic (G)
  - Local Geodetic (LG)
  - Local Astronomic
- Celestial Coordinate Systems
  - Apparent Places (AP)
  - Right Ascension (RA)
  - Horizontal Angle (HA)
- Orbital Coordinate Systems
  - Orbital (OR)
    
![Screenshot 2025-05-16 132143](https://github.com/user-attachments/assets/c6666d03-2f31-4247-9d6b-43d88838e04c)

*Fig. 1: Relationship between geocentric, topocentric, and 2D coordinate systems*

## 📌 Core Features

### 1. Support different Map projections and horizontal datums
Map projections such as TM, UTM, Mercator, WebMercator, Cylindrical Equal-area, Albers Equal Area Conic and Lambert Conformal Conic with 1 parallel and 2 parallel are supported. Also many ellipsoids have been defined as horizontal spatial datum.

### 2. Comprehensive Coordinate System Transformation Support
Transformation between different spatial reference systems are available including:
- CT <-> IT
- CT <-> G
- G1 <-> G2 (transform Geodetic system with different ellipsoids)
- CT <-> LA
- G <-> LG
- LA <-> LG
- LA <-> HA
- HA <-> AP
- IT <-> AP
- OR <-> AP

## 🚀 Getting Started

### Basic Usage

Convert WGS84 to AT:

``` C#
// Arrange
var wgs84Ellipsoid = Ellipsoids.WGS84;
double latitudeInDegrees = 35.123456;
double longitudeInDegrees = 51.123456;
    
var originalGeodeticPoint = new IRI.Sta.Common.Primitives.Point(longitudeInDegrees, latitudeInDegrees);

// Act - Test geodetic to Cartesian conversion
var cartesianFromTransform = Transformations.ToCartesian(originalGeodeticPoint, wgs84Ellipsoid);

var cartesianFromGeodeticPoint = new GeodeticPoint<Meter, Degree>(
    wgs84Ellipsoid, 
    new Meter(0),
    new Degree(longitudeInDegrees),
    new Degree(latitudeInDegrees))
    .ToCartesian<Meter>();

// Assert - Both Cartesian conversion methods should produce same result
Assert.Equal(cartesianFromGeodeticPoint.X.Value, cartesianFromTransform.X, 9);
Assert.Equal(cartesianFromGeodeticPoint.Y.Value, cartesianFromTransform.Y, 9);
Assert.Equal(cartesianFromGeodeticPoint.Z.Value, cartesianFromTransform.Z, 9);
```

## 🌐 Related Components

- ![IRI.Sta.Common](https://github.com/hosseinnarimanirad/IRI.Japey/tree/master/IRI.Sta/IRI.Sta.Common) - Core Japey (Maptor) standard class library.

## 🛠 Building from Source

```
git clone https://github.com/hosseinnarimanirad/IRI.Japey.git
cd IRI.Japey/IRI.Sta/IRI.Sta.SpatialReferenceSystem
dotnet build -c Release
```
