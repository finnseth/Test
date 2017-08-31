import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

import { CardsService } from './cards.service';
import { Ship } from 'connection-suite/components/ship/interfaces';

@Component({
  selector: 'dua-cards',
  templateUrl: './cards.component.html',
  styleUrls: ['./cards.component.scss']
})
export class CardsComponent implements OnInit {

  @Input() verticalDirection: boolean;
  @Input() cardsTitle: string;
  @Input() showNewButtonOnShipCard: boolean;
  @Input() showNewButtonOnFleetCard: boolean;
  @Input() newButtonLabelOnShip: string;
  @Input() newButtonLabelOnFleet: string;
  @Input() advancedSearchContext: string;

  @Input('selectedShip')
  set selectedShip(ship: Ship) {
    this._selectedShip = ship;
    this.showShip();
  };
  get selectedShip() {
    return this._selectedShip;
  }

  @Input('selectedCompareShip')
  set selectedCompareShip(ship: Ship) {
    this._selectedCompareShip = ship;
    this.showCompare();
  };
  get selectedCompareShip() {
    return this._selectedCompareShip;
  }

  @Output() showFleet = new EventEmitter<boolean>();
  @Output() updateShip = new EventEmitter<Ship>();
  @Output() updateCompare = new EventEmitter<Ship>();

  @Output() shipNewClicked = new EventEmitter<any>();
  @Output() fleetNewClicked = new EventEmitter<any>();

  _selectedShip: Ship;
  _selectedCompareShip: Ship;

  shipName = 'Ship';
  compareShipName = 'Fleet';
  compareIcon = '/assets/img/fleet.png';
  shipIcon = '/assets/img/ship.png';
  gotCompareShip = false;

  constructor(private cardsService: CardsService) {
    this.verticalDirection = true;
    this.showNewButtonOnShipCard = false;
  }

  ngOnInit() {
    setTimeout(() =>  this.handleShipChange(this.cardsService.getSelectedShip()), 1);
  }

  /**
   * A compare ship is set and need to update the view
   *
   * @memberof CardsComponent
   */
  showCompare () {
    if (this._selectedCompareShip !== undefined) {
      this.compareShipName = this.selectedCompareShip.name;
      this.compareIcon = '/assets/img/ship.png';
      this.gotCompareShip = true;
    }
  }

  /**
   * A ship is selected, updating the view
   *
   * @memberof CardsComponent
   */
  showShip () {
    if (this._selectedShip !== undefined) {
      this.shipName = this._selectedShip.name;
    } else {
      this.shipName = 'Ship';
    }
  }

  /**
   * Return from the compare mode and into fleet mode
   *
   * @param {boolean} event
   *
   * @memberof CardsComponent
   */
  handleShowFleet(event: boolean) {
    this.selectedCompareShip = undefined;
    this.compareShipName = 'Fleet';
    this.compareIcon = '/assets/img/fleet.png';
    this.gotCompareShip = false;
    this.showFleet.emit(true);
  }

  /**
   * Set the selected ship
   *
   * @param {Ship} ship
   *
   * @memberof CardsComponent
   */
  handleShipChange(ship: Ship) {
    if (ship !== undefined) {
      this.selectedShip = ship;
      this.cardsService.setSelectedShip(this.selectedShip);
      this.updateShip.emit(this.selectedShip);
    }
  }

  /**
   * Set the selected compare ship
   *
   * @param {Ship} ship
   *
   * @memberof CardsComponent
   */
  handleCompareChange(ship: Ship) {
    this.selectedCompareShip = ship;
    this.gotCompareShip = true;
    this.showFleet.emit(false);
    this.updateCompare.emit(ship);
  }

  changeDirection(event: Event) {
        this.verticalDirection = !this.verticalDirection;
  }

  onShipNew(event: Event) {
    this.shipNewClicked.emit(event);
  }

  onFleetNew(event: Event) {
    this.fleetNewClicked.emit(event);
  }
}
