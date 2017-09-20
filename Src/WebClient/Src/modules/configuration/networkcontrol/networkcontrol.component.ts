import { Component, OnInit } from '@angular/core';
import { JsonSchema, SchemaFormBuilder } from 'infrastructure/services/schema';

import { FormGroup } from '@angular/forms';
import { NetworkControlService } from './networkcontrol.service';
import { Observable } from 'rxjs/Rx';
import { Ship } from 'common/domain/ship/interfaces';

@Component({
  selector: 'dua-networkcontrol',
  templateUrl: './networkcontrol.component.html',
  styleUrls: ['./networkcontrol.component.css']
})

export class NetworkControlComponent implements OnInit {
    schema: JsonSchema;
    shipForm: Observable<FormGroup>;
    selectedShip: Ship;
    selectedCompareShip: Ship;
    gotCompareShip = false;

    constructor( private ncService: NetworkControlService, private fb: SchemaFormBuilder ) {
    }

    ngOnInit() {
    }

    handleShipChange(ship: Ship) {
        if ( ship !== null ) {
            this.selectedShip = ship;
        }
    }

    handleCompareChange(ship: Ship) {
        if ( ship !== null ) {
          this.gotCompareShip = true;
        }
    }

    showFleet(event: boolean) {
        this.gotCompareShip = false;
    }

    onApplyShipCard(event: Event ) {
        console.log('@todo: apply rule');
    }

    onCancelShipCard(event: Event ) {
        this.handleShipChange(this.selectedShip);
    }
}
