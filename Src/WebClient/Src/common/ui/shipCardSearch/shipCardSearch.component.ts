import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';

import { Ship } from 'common/domain/ship/interfaces';
import { ShipCardSearchService } from './shipCardSearch.service';

@Component({
  selector: 'dua-shipcardsearch',
  templateUrl: './shipCardSearch.component.html',
  styleUrls: ['./shipCardSearch.component.scss']
})
export class ShipCardSearchComponent implements OnInit {

  @Input() searchContext: string;
  @Input() currentShip: Ship;

  @Output() onShipChanged = new EventEmitter<Ship>();

  selectedShip: Ship;
  isSearching = false;
  title = 'Search for a ship to modify';

  constructor(private cardSearchService: ShipCardSearchService) {
  }

  ngOnInit() {

    if (this.currentShip === undefined || this.currentShip == null){
      this.isSearching = true;
    }

  }

  enableSearch(event: Event): void {
    this.isSearching = true;
  }

  disableSearch(event: MouseEvent): void {
    const isAdvancedClicked = (event.relatedTarget !== null);
    if (this.currentShip !== undefined && !isAdvancedClicked) {
      this.isSearching = false;
    }
  }

  selectingShip(ship: Ship) {
    this.selectedShip = ship;
    this.onShipChanged.emit(this.selectedShip);
//    this.cardSearchService.setSelectedShip(this.selectedShip);
    this.isSearching = false;
  }
}
