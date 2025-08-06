# Space-Filling Curves (SFC) in Maptor

 A .NET Standard library providing implementations of **Space-Filling Curves (SFCs)** for spatial data sorting, and indexing. Combines object-oriented and functional programming paradigms for flexibility.

## Features

- **Multiple SFC Types**: 
  - Hilbert curve
  - Z-order (Morton) curve 
  - Custom curve generation via higher-order functions
- **Preserved Spatial Locality**: 
  - Near points in multi-dimensional space remain as close as possible in 1D sequence
- **Interoperability**:
  - Works seamlessly with spatial data structures (KD-Trees, R-Trees)


## Installation

Via NuGet Package Manager:
```bash
dotnet add package IRI.Maptor.Sta.Spatial
```

## Applications

- **Spatial Indexing**: Accelerate KD-Tree/R-Tree constructions
- **Data Sorting**: Linearize 2D data while maintaining locality
- **Database Optimization**: Improve spatial query performance

## Samples

### SFC Generation: 
<video src="https://github.com/user-attachments/assets/a7a3ec69-7bc1-4c10-9bff-53ce2a1e00d6" height="150" width="150" controls></video>

### Point ordering using SFCs:
<video src="https://github.com/user-attachments/assets/6126d4f7-d680-4d99-8737-b27c9c160dd0" height="150" width="150" controls></video>

### Kd-Tree SFCs:
<video src="https://github.com/user-attachments/assets/ca1e3481-c79b-43cf-af74-aa6d1d7b6d98" height="150" width="150" controls></video>

### R-Tree SFCs:
<video src="https://github.com/user-attachments/assets/afde3d09-05de-4089-adcf-dfa012b81192" height="150" width="150" controls></video>

## Contributing

We welcome contributions! Please:
1. Fork the repository
2. Create a feature branch
3. Submit a pull request

---

**📦 NuGet**: [IRI.Maptor.Sta.Spatial](https://www.nuget.org/packages/IRI.Maptor.Sta.Spatial)
