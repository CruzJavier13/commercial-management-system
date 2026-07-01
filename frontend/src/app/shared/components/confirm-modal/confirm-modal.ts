import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  standalone: true,
  selector: 'app-confirm-modal',
  imports: [CommonModule],
  templateUrl: './confirm-modal.html'
})
export class ConfirmModalComponent {

  @Input() isOpen = false;
  @Input() title = '¿Confirmar Acción?';
  @Input() description = 'Esta acción no se puede deshacer de forma ordinaria en el sistema.';
  @Input() itemId: number = 0;
  @Input() type: 'DANGER' | 'WARNING' | 'INFO' = 'DANGER'; 


  @Output() onConfirm = new EventEmitter<number>();
  @Output() onClose = new EventEmitter<void>();

  confirmAction(): void {
    this.onConfirm.emit(this.itemId); 
  }

  cancelAction(): void {
    this.onClose.emit(); 
  }
}