# Build and Deployment

## Build pipeline

### Local development

```bash
# Restore dependencies
dotnet restore

# Build solution
dotnet build NuciAPI.sln

# Run tests
dotnet test NuciAPI.sln

# Pack NuGet package (local)
dotnet pack NuciAPI/NuciAPI.csproj -c Release -o ./nupkg
```

### CI/CD (GitHub Actions)

#### `.github/workflows/dotnet.yml` — Continuous Integration

**Triggers:** Push to master, PR to master

**Jobs:**
1. **Build** — `dotnet build --no-restore`
2. **Test** — `dotnet test --no-build --verbosity normal`

**Environment:** Ubuntu latest, .NET 10.0.x

#### `.github/workflows/github-release.yml` — Continuous Deployment

**Triggers:** GitHub Release published

**Jobs:**
1. **Pack** — `dotnet pack -c Release --output ./nupkg -p:Version=<tag>`
2. **Checksum** — SHA256 of `.nupkg`
3. **Upload** — `gh release upload` package to GitHub Release
4. **Update notes** — Append SHA256 to release body

**Permissions:** `contents: write`

**Environment:** Ubuntu latest, .NET 10.0.x

## Versioning

- **Scheme:** Semantic Versioning (Major.Minor.Patch)
- **Source:** Git tag (e.g., `v3.6.1`)
- **Assembly version:** From `<Version>` in `NuciAPI.csproj`
- **Package version:** Same as assembly version

## Release process

1. Update version in `NuciAPI.csproj` (or use Git tag)
2. Commit changes
3. Create GitHub Release with tag `v<version>`
4. GitHub Actions automatically:
   - Packs NuGet package
   - Uploads to GitHub Release
   - Calculates SHA256
   - Appends checksum to release notes
5. Manual: Publish to NuGet.org (if configured)

## NuGet package

- **Package ID:** `NuciAPI`
- **Target framework:** `net10.0`
- **License:** GPL-3.0-or-later
- **Repository:** https://github.com/hmlendea/nuciapi
- **Dependencies:** `NuciSecurity.HMAC` 4.1.3

## Deployment targets

| Target | Method | Status |
|--------|--------|--------|
| NuGet.org | Manual/Automated | Not configured in CI |
| GitHub Packages | GitHub Release workflow | ✅ Configured |
| Local feed | `dotnet pack` | ✅ Supported |

## Build outputs

```
NuciAPI/bin/
├── Debug/net10.0/
│   ├── NuciAPI.dll
│   ├── NuciAPI.pdb
│   ├── NuciAPI.deps.json
│   └── NuciAPI.xml (if GenerateDocumentationFile=true)
└── Release/net10.0/
    ├── NuciAPI.dll
    ├── NuciAPI.pdb
    ├── NuciAPI.deps.json
    └── NuciAPI.nupkg (after pack)
```

## Requirements

- **.NET SDK:** 10.0.x
- **OS:** Cross-platform (Windows, Linux, macOS)
- **Architecture:** Any (x64, arm64, etc.)

## Troubleshooting build

| Issue | Resolution |
|-------|------------|
| `NuciSecurity.HMAC` not found | `dotnet restore` |
| Version mismatch | Check `NuciAPI.csproj` `<Version>` |
| Test failures | Run `dotnet test --verbosity detailed` |
| Pack fails | Ensure `Release` configuration builds |