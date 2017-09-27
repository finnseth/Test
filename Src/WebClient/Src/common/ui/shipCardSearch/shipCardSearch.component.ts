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
  ships: Ship[];
  resultShips: Ship[];
  isSearching = false;
  placeholder = 'Search for a ship to modify';

  constructor(private shipService: ShipCardSearchService) {
  }

  ngOnInit() {
    this.shipService.getShips().subscribe( ships => {
      this.ships = ships['value'].sort( (s, r) =>  s.name.localeCompare(r.name) );
    });

    if (this.currentShip === undefined || this.currentShip == null) {
      this.isSearching = true;
    }
  }

  enableSearch(event: Event): void {
    this.isSearching = true;
  }

  onSearch(event) {
    this.resultShips = [];
    for (let i = 0; i < this.ships.length; i++) {
        const ship = this.ships[i];
        if (ship.name.toLowerCase().indexOf(event.query.toLowerCase()) > -1) {
            this.resultShips.push(ship);
        }
    }
  }

  disableSearch(event: MouseEvent): void {
    const isAdvancedClicked = (event.relatedTarget !== null);
    if (this.currentShip !== undefined && !isAdvancedClicked) {
      if (this.currentShip !== undefined) {
        this.selectedShip = this.currentShip;
      }
      this.isSearching = false;
    }
  }

  selectingShip(ship: Ship) {
    this.selectedShip = ship;
    this.onShipChanged.emit(this.selectedShip);
    this.isSearching = false;
  }
}
