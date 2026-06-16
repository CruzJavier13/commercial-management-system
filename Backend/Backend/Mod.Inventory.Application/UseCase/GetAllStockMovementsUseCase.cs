using Mod.Inventory.Application.Dtos;
using Mod.Inventory.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mod.Inventory.Application.UseCase
{
    public class GetAllStockMovementsUseCase
    {
        private readonly IStockMovementRepository _stockMovementRepository;

        public GetAllStockMovementsUseCase(IStockMovementRepository stockMovementRepository)
        {
            _stockMovementRepository = stockMovementRepository ?? throw new ArgumentNullException(nameof(stockMovementRepository));
        }

        public async Task<IEnumerable<InventoryStockMovementDto>> ExecuteAsync()
        {
            var movements = await _stockMovementRepository.GetAllAsync();

            if (movements == null)
            {
                return Enumerable.Empty<InventoryStockMovementDto>();
            }

            return movements.Select(m => new InventoryStockMovementDto
            {
                Id = m.Id,
                ProductId = m.ProductId,
                SupplierId = m.SupplierId,
                MovementType = m.MovementType,
                Quantity = m.Quantity,
                UnitCost = m.UnitCost,
                TotalCost = m.TotalCost, // Propiedad calculada
                Concept = m.Concept,
                ReferenceDocument = m.ReferenceDocument,
                MovementDate = m.MovementDate
            }).ToList();
        }
    }
}
