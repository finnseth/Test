import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

import { Ship } from 'connection-suite/components/ship/interfaces'; // todo

@Component({
  selector: 'dua-shipcardheader',
  templateUrl: './shipCardHeader.component.html',
  styleUrls: ['./shipCardHeader.component.scss']
})
export class ShipCardHeaderComponent implements OnInit {

  @Input() searchContext: string;
  @Input() currentShip: Ship;
  
  @Output() onShipChanged = new EventEmitter<Ship>();

  constructor() {}

  ngOnInit() {
  }

  shipChanged(ship: Ship): void {
    this.onShipChanged.emit(ship);
  }
}
