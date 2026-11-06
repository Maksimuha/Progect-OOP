using Core.Abstractions;
using Core.Entities;
using Core.Shared;

namespace Core.Services;

public sealed class DepartmentLinkServices
{
    private readonly IDepartmentRepository _departmentRepo;
    private readonly IDepartmentLocationRepository _locLinkRepo;
    private readonly IDepartmentPositionRepository _posLinkRepo;

    public DepartmentLinkServices(
        IDepartmentRepository departmentRepo, 
        IDepartmentLocationRepository locLinkRepo,
        IDepartmentPositionRepository posLinkRepo)
    {
        _departmentRepo = departmentRepo;
        _locLinkRepo = locLinkRepo;
        _posLinkRepo = posLinkRepo;
    }
    public async Task AttachLocationAsync(Department department, LocationId locationId, CancellationToken ct = default)
    {
        if (department.LifeTime.IsDeleted)
            throw new InvalidOperationException("Нельзя добавить локацию в архивированное подразделение.");

        // 1. Проверяем, что локация вообще существует
        var location = await _departmentRepo.GetLocationByIdAsync(locationId, ct)
            ?? throw new InvalidOperationException($"Локация с ID {locationId} не найдена.");

        if (location.LifeTime.IsDeleted)
            throw new InvalidOperationException("Нельзя прикрепить архивированную локацию.");

        // 2. Уникальность по ID (нет дублей связи)
        if (await _locLinkRepo.ExistsAsync(department.Id, locationId, ct))
            throw new InvalidOperationException("Эта локация уже прикреплена к данному подразделению.");

        // 3. Уникальность по адресу внутри подразделения
        if (await _locLinkRepo.ExistsByAddressAsync(department.Id, location.Address, ct))
            throw new InvalidOperationException($"В подразделении уже есть локация по адресу '{location.Address.Value}'.");

        await _locLinkRepo.AddAsync(DepartmentLocation.Create(department.Id, locationId), ct);
    }

    // Прикрепление должности к подразделению
    public async Task AttachPositionAsync(Department department, PositionId positionId, CancellationToken ct = default)
    {
        if (department.LifeTime.IsDeleted)
            throw new InvalidOperationException("Нельзя добавить должность в архивированное подразделение.");

        // 1. Проверяем, что должность существует
        var position = await _departmentRepo.GetPositionByIdAsync(positionId, ct)
            ?? throw new InvalidOperationException($"Должность с ID {positionId} не найдена.");

        if (position.LifeTime.IsDeleted)
            throw new InvalidOperationException("Нельзя прикрепить архивированную должность.");

        // 2. Уникальность по ID (нет дублей связи)
        if (await _posLinkRepo.ExistsAsync(department.Id, positionId, ct))
            throw new InvalidOperationException("Эта должность уже прикреплена к данному подразделению.");

        // 3. Уникальность по названию внутри подразделения
        if (await _posLinkRepo.ExistsByNameAsync(department.Id, position.Name, ct))
            throw new InvalidOperationException($"В подразделении уже есть должность с названием '{position.Name.Value}'.");

        await _posLinkRepo.AddAsync(DepartmentPosition.Create(department.Id, positionId), ct);
    }
}

