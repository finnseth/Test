import { ActivatedRoute, Router } from '@angular/router';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

import { AuthenticationService } from 'connection-suite-shore/services/authentication.service';
import { MenuItem } from 'primeng/primeng';
import { Ship } from './interfaces';
import { ShipSelectorService } from './shipSelector.service';

@Component({
  selector: 'dua-shipselector',
  templateUrl: './shipSelector.component.html',
  styleUrls: ['./shipSelector.component.scss']
})
export class ShipSelectorComponent implements OnInit {

  @Input() placeholder: string;
  @Input() advancedSearchContext: string;
  @Input() searchStyle: string;
  @Input() searchTitle: string;
  @Output() shipUpdated = new EventEmitter<Ship>();

  @Input('emptyAutocomplete')
  set emptyAutocomplete(empty: boolean) {
    if (empty) {
      this.ship = null;
    }
  };

  ship: Ship;
  ships: Ship[];
  resultShips: Ship[];
  isChanging = false;

  doAdvancedSearch = false;

  constructor(private authenticationService: AuthenticationService, private shipService: ShipSelectorService) {
  }

  ngOnInit() {
    this.shipService.getShips().subscribe( ships => {
      this.ships = ships.sort( (s, r) =>  s.name.localeCompare(r.name) );
    });
  }

  search(event) {
    this.resultShips = [];
    for (let i = 0; i < this.ships.length; i++) {
        const ship = this.ships[i];
        if (ship.name.toLowerCase().indexOf(event.query.toLowerCase()) > -1) {
            this.resultShips.push(ship);
        }
    }
  }

  showAdvancedSearch(event: Event) {
    this.doAdvancedSearch = true;
  }

  public selectShip(ship: Ship) {
    this.ship = ship;
    this.shipUpdated.emit(ship);
    this.disableChanging(null);
  }

  enableChange(event: Event) {
    this.isChanging = true;
  }

  disableChanging(event: Event) {
    this.isChanging = false;
  }
}
