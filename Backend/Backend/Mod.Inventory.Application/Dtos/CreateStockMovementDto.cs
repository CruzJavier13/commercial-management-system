using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Mod.Inventory.Application.Dtos
{
    public class CreateStockMovementDto
    {
        [Required(ErrorMessage = "El ID del producto es obligatorio.")]
        public int ProductId { get; set; }
        public int? SupplierId { get; set; }

        [Required(ErrorMessage = "El tipo de movimiento es obligatorio.")]
        [RegularExpression("^(IN|OUT)$", ErrorMessage = "El tipo de movimiento debe ser 'IN' o 'OUT'.")]
        public string MovementType { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a cero.")]
        public int Quantity { get; set; }

        [Range(0.0, double.MaxValue, ErrorMessage = "El costo unitario no puede ser negativo.")]
        public decimal UnitCost { get; set; }

        [Required(ErrorMessage = "El concepto del movimiento es obligatorio.")]
        [StringLength(250, ErrorMessage = "El concepto no puede superar los 250 caracteres.")]
        public string Concept { get; set; } = string.Empty;

        [StringLength(50, ErrorMessage = "El documento de referencia no puede superar los 50 caracteres.")]
        public string? ReferenceDocument { get; set; }
    }
}
