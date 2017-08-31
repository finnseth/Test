import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';

import { Ship } from 'connection-suite/components/ship/interfaces';
import { ShipCardSearchService } from './shipCardSearch.service';

@Component({
  selector: 'dua-shipcardsearch',
  templateUrl: './shipCardSearch.component.html',
  styleUrls: ['./shipCardSearch.component.scss']
})
export class ShipCardSearchComponent implements OnInit {

  @Input() searchContext: string;
  
  @Output() onShipChanged = new EventEmitter<Ship>();

  selectedShip: Ship;
  isSearching = false;
  title = 'Search for a ship to modify';

  constructor(private cardSearchService: ShipCardSearchService) {
  }

  ngOnInit() {
    this.selectedShip = this.cardSearchService.getSelectedShip();
    if (this.selectedShip !== undefined) {
      this.title = this.selectedShip.name;
    } else {
      this.isSearching = true;
    }
  }

  enableSearch(event: Event): void {
    this.isSearching = true;
  }

  disableSearch(event: MouseEvent): void {
    const isAdvancedClicked = (event.relatedTarget !== null);
    if (this.selectedShip !== undefined && !isAdvancedClicked) {
      this.isSearching = false;
    }
  }

  selectingShip(ship: Ship) {
    this.selectedShip = ship;
    this.cardSearchService.setSelectedShip(this.selectedShip);
    this.title = this.selectedShip.name;
    this.onShipChanged.emit(this.selectedShip);
    this.isSearching = false;
  }
}
