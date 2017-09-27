import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';

import { Ship } from 'common/domain/ship/interfaces';
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
  ships: Ship[];
  resultShips: Ship[];
  isSearching = false;
  title = 'Fleet';

  constructor(private shipService: ShipCardSearchService) {
  }

  ngOnInit() {
    this.shipService.getShips().subscribe( ships => {
      this.ships = ships['value'].sort( (s, r) =>  s.name.localeCompare(r.name) );
    });
  }

  enableSearch(event: Event): void {
    this.isSearching = true;
  }

  disableSearch(event: Event): void {
    this.isSearching = false;
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

  showFleet(event: boolean) {
    this.compareShip = undefined;
    this.title = 'Fleet';
    this.isSearching = false;
    this.onFleetEnabledChanged.emit(true);
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
