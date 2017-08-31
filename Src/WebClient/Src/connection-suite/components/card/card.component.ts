import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

import { Ship } from 'connection-suite/components/ship/interfaces'; // todo

@Component({
  selector: 'dua-card',
  templateUrl: './card.component.html',
  styleUrls: ['./card.component.scss']
})
export class CardComponent implements OnInit {

  @Input() appliesTo: string;
  @Input() title: string;
  @Input() subtitle: string;
  @Input() icon = '/assets/img/fleet.png';
  @Input() enabled = true;
  @Input() comparemode = false;
  @Input() isReady = true;
  @Input() showNewButton = false;
  @Input() advancedSearchContext: string;

  @Input('newButtonLabel')
  set newButtonLabel(label: string) {
    this._newButtonLabel = label;
  };
  get newButtonLabel() {
    if (this._newButtonLabel === undefined || this._newButtonLabel === null) {
      return 'New';
    }
    return this._newButtonLabel;
  }

  @Output() showFleet = new EventEmitter<boolean>();
  @Output() newClicked = new EventEmitter<any>();
  @Output() updateShip = new EventEmitter<Ship>();
  @Output() updateCompare = new EventEmitter<Ship>();

  _newButtonLabel = 'New';

  constructor() {}

  ngOnInit() {
  }

  rmCompareShip(event: Event) {
    this.comparemode = false;
    this.showFleet.emit(true);
  }

  onNewClicked(event: Event) {
    this.newClicked.emit(event);
  }

  handleShipChange(ship: Ship) {
    this.updateShip.emit(ship);
  }

  handleCompareChange(ship: Ship) {
    this.updateCompare.emit(ship);
  }
}
