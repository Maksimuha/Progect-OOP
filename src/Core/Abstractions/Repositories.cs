using Core.Entities;
using Core.Shared;

namespace Core.Abstractions;

public interface IPositionRepository
{
    Task<bool> ExistsByNameAsync(DOnotEmptyString name, PositionId? excludeId = null,
    CancellationToken ct = default);
}

public interface IDepartmentRepository
{
    Task<bool> ExistsByNameAsync(DOnotEmptyString name, DepartmentId? excludeId = null,
    CancellationToken ct = default);
    Task<Location?> GetLocationByIdAsync(LocationId id, CancellationToken ct = default);
    Task<Position?> GetPositionByIdAsync(PositionId id, CancellationToken ct = default);
}

public interface IDepartmentLocationRepository
{
    Task<bool> ExistsAsync(DepartmentId departmentId, LocationId locationId,
    CancellationToken ct = default);
    Task<bool> ExistsByAddressAsync(DepartmentId departmentId, DOnotEmptyString address,
    CancellationToken ct = default);
    Task AddAsync(DepartmentLocation link, CancellationToken ct = default);
}
public interface IDepartmentPositionRepository
{
    Task<bool> ExistsAsync(DepartmentId departmentId, PositionId positionId,
    CancellationToken ct = default);
    Task<bool> ExistsByNameAsync(DepartmentId departmentId, DOnotEmptyString positionName,
    CancellationToken ct = default);
    Task AddAsync(DepartmentPosition link, CancellationToken ct = default);
}