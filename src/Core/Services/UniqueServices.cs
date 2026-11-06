using Core.Abstractions;
using Core.Shared;

namespace Core.Services;

public sealed class PositionUniqueService
{
    private readonly IPositionRepository _repository;
    
    public PositionUniqueService(IPositionRepository repository)
    {
        _repository = repository;
    }

    public async Task EnsureNameIsUniqueAsync(DOnotEmptyString name, PositionId? excludeId = null, CancellationToken ct = default)
    {
        if (await _repository.ExistsByNameAsync(name, excludeId, ct))
            throw new InvalidOperationException($"Должность с названием '{name.Value}' уже существует.");
    }
}

public sealed class DepartmentUniqueService
{
    private readonly IDepartmentRepository _repository;

    public DepartmentUniqueService(IDepartmentRepository repository)
    {
        _repository = repository;
    }

    public async Task EnsureNameIsUniqueAsync(DOnotEmptyString name, DepartmentId? excludeId = null, CancellationToken ct = default)
    {
        if (await _repository.ExistsByNameAsync(name, excludeId, ct))
            throw new InvalidOperationException($"Подразделение с названием '{name.Value}' уже существует.");
    }
}
