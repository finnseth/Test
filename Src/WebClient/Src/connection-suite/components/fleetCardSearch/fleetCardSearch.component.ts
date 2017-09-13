import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';

import { Ship } from 'connection-suite/components/ship/interfaces';
import { ShipCardSearchService } from '../shipCardSearch/shipCardSearch.service';

@Component({
  selector: 'dua-fleetcardsearch',
  templateUrl: './fleetcardSearch.component.html',
  styleUrls: ['./fleetcardSearch.component.scss']
})
export class FleetCardSearchComponent implements OnInit {

  @Input() searchContext: string;
  @Input() currentCompareShip: Ship;

  @Output() onCompareShipChanged = new EventEmitter<Ship>();
  @Output() onFleetEnabledChanged = new EventEmitter<boolean>();

  compareShip: Ship;
  isSearching = false;
  title = 'Fleet';

  constructor(private cardSearchService: ShipCardSearchService) {
  }

  ngOnInit() {
  }

  enableSearch(event: Event): void {
    this.isSearching = true;
  }

  disableSearch(event: Event): void {
    this.isSearching = false;
  }

  showFleet(event: boolean) {
    this.compareShip = undefined;
    this.title = 'Fleet';
    this.onFleetEnabledChanged.emit(true);
    this.isSearching = false;
  }

  selectingCompareShip(ship: Ship) {
    if (ship === undefined) {
      this.showFleet(true);
    } else {
      this.compareShip = ship;
      this.title = ship.name;
      this.onCompareShipChanged.emit(this.compareShip);
      this.onFleetEnabledChanged.emit(false);
      this.isSearching = false;
    }
  }
}
