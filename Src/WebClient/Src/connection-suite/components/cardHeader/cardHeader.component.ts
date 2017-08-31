import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

import { Ship } from 'connection-suite/components/ship/interfaces'; // todo

@Component({
  selector: 'dua-card-header',
  templateUrl: './cardHeader.component.html',
  styleUrls: ['./cardHeader.component.scss']
})
export class CardHeaderComponent implements OnInit {

  @Input() headerTitle: string;
  @Input() advancedSearchContext: string;
  @Input() emptyAutocompleteCompareShip = false;
  @Input() searchCompareTitle: string;
  @Input() searchShipTitle: string;
  @Output() updateShip = new EventEmitter<Ship>();
  @Output() updateCompare = new EventEmitter<Ship>();

  constructor() { }

  ngOnInit() {
  }

  handleShipChange(ship: Ship) {
    this.updateShip.emit(ship);
  }

  handleCompareChange(ship: Ship) {
    this.updateCompare.emit(ship);
  }

}
