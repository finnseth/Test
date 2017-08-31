import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

import { Ship } from 'connection-suite/components/ship/interfaces'; // todo

@Component({
  selector: 'dua-fleetcardheader',
  templateUrl: './fleetCardHeader.component.html',
  styleUrls: ['./fleetCardHeader.component.scss']
})
export class FleetCardHeaderComponent implements OnInit {

  @Input() searchContext: string;
  
  @Output() onCompareShipChanged = new EventEmitter<Ship>();
  @Output() onFleetEnabledChanged = new EventEmitter<boolean>();

  isComparing = false;

  constructor() {}

  ngOnInit() {
  }

  compareShipChanged(ship: Ship): void {
    this.onCompareShipChanged.emit(ship);
  }

  fleetStatusChanged(isEnabled: boolean): void {
    this.isComparing = !isEnabled;
    this.onFleetEnabledChanged.emit(isEnabled);
  }
}
