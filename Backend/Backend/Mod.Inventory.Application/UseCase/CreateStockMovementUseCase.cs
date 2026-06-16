using Mod.Inventory.Application.Dtos;
using Mod.Inventory.Domain.Entities;
using Mod.Inventory.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mod.Inventory.Application.UseCase
{
    public class CreateStockMovementUseCase
    {
        private readonly IStockMovementRepository _stockMovementRepository;

        public CreateStockMovementUseCase(IStockMovementRepository stockMovementRepository)
        {
            _stockMovementRepository = stockMovementRepository ?? throw new ArgumentNullException(nameof(stockMovementRepository));
        }

        public async Task ExecuteAsync(CreateStockMovementDto command)
        {

            if (command.ProductId <= 0)
                throw new ArgumentException("El ID del producto proporcionado no es válido.");

            if (string.IsNullOrWhiteSpace(command.MovementType))
                throw new ArgumentException("El tipo de movimiento es obligatorio.");

            string type = command.MovementType.Trim().ToUpper();
            if (type != "IN" && type != "OUT")
                throw new ArgumentException("El tipo de movimiento debe ser estrictamente 'IN' o 'OUT'.");

            if (command.Quantity <= 0)
                throw new ArgumentException("La cantidad del movimiento debe ser estrictamente mayor a cero.");

            if (command.UnitCost < 0)
                throw new ArgumentException("El costo unitario no puede ser un valor negativo.");

            if (string.IsNullOrWhiteSpace(command.Concept))
                throw new ArgumentException("Debe proporcionar un concepto o motivo para este movimiento.");

            if (type == "IN" && command.Concept.Contains("Compra", StringComparison.OrdinalIgnoreCase) && !command.SupplierId.HasValue)
            {
                // Nota: Dependiendo de la regla de negocio, esto podría ser una excepción o una alerta léxica
            }

            var movement = new StockMovement
            {
                ProductId = command.ProductId,
                SupplierId = command.SupplierId,
                MovementType = type,
                Quantity = command.Quantity,
                UnitCost = command.UnitCost,
                Concept = command.Concept.Trim(),
                ReferenceDocument = string.IsNullOrWhiteSpace(command.ReferenceDocument) ? null : command.ReferenceDocument.Trim(),

                MovementDate = DateTime.UtcNow
            };

            await _stockMovementRepository.SaveAsync(movement);
        }
    }
}
