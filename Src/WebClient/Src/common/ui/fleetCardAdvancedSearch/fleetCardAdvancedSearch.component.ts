import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { DataTable, MenuItem, SelectItem } from 'primeng/primeng';

import { Ship } from 'common/domain/ship/interfaces';
import { ShipCardSearchService } from '../shipCardSearch/shipCardSearch.service';

@Component({
  selector: 'dua-fleetcardadvancedsearch',
  templateUrl: './fleetCardAdvancedSearch.component.html',
  styleUrls: ['./fleetCardAdvancedSearch.component.scss']
})
export class FleetCardAdvancedSearchComponent implements OnInit {

  @ViewChild('advancedsearchtable') shipTable: DataTable;
  
  @Input() searchContext: string;

  @Output() onShipSelected = new EventEmitter<Ship>();

  _selectedShip: Ship;
  private enableAdvancedSearch = false;
  currentWorkingShip: Ship;
  currentCompareShip: Ship;
  ships: Ship[];
  filterShips: Ship[];
  customSetups: SelectItem[];
  isSearching: boolean;

  isShipActive: string[] = ['Active', 'Disabled'];
  gotCustomSetting: string[] = ['Ship', 'Fleet'];

  exportItems: MenuItem[];
  csvSeparator = ',';

  fields = {
    quarantineLocalChanges: false
  }

  constructor(private shipService: ShipCardSearchService) {
  }

  ngOnInit() {

    this.currentWorkingShip = this.shipService.getSelectedShip();
    this._selectedShip = this.currentCompareShip;

    this.exportItems = [
        {label: 'CSV (,)', icon: 'fa-download', command: () => {
            this.csvSeparator = ',';
            this.shipTable.exportCSV();
        }},
        {label: 'CSV (.)', icon: 'fa-download', command: () => {
          this.csvSeparator = '.';
          this.shipTable.exportCSV();
        }},
        {label: 'CSV (;)', icon: 'fa-download', command: () => {
          this.csvSeparator = ';';
          this.shipTable.exportCSV();
        }}
    ];

    this.customSetups = [
      { label: 'All ships', value: null },
      { label: 'Only with changes', value: true },
      { label: 'No', value: false }
    ];
  }

  selectingShip(ship: Ship) {
    this.onShipSelected.emit(ship);
    this.currentWorkingShip = this.shipService.getSelectedShip();
    this.currentCompareShip = ship;
    this.enableAdvancedSearch = false;
  }

  closeDialog(event: Event) {
  }

  advancedSearch(event: Event) {
    this.enableAdvancedSearch = true;
    this.isSearching = true;

    switch (this.searchContext) {
      case 'quarantine':
        this.shipService.getQuarantineShips().subscribe( ships => {
          // this.fields.quarantineLocalChanges = true;
          this.shipsRetrieved(ships);
        });
      break;
      default:
        this.shipService.getShips().subscribe( ships => {
          this.shipsRetrieved(ships);
        });
    }
  }

  shipsRetrieved (ships: Ship[]) {
    this.ships = ships;
    this.filterShips = this.ships;
    this.isSearching = false;
  }

  filterOnShipName(event: Event) {
    const target: any = event.target;
    if (this.shipTable.dataToRender) {
       this.shipTable.filter(target.value, 'name', 'contains');
    }
  }

  filterOnShipStatus(event: Event) {
    let isActive: number;
    if ( this.isShipActive.length > 1  || this.isShipActive.length === 0) {
      if (this.shipTable.dataToRender) {
        this.shipTable.filter('.', 'accountEnabled', 'notEquals');
      }
    } else {
      for (const status of this.isShipActive) {
        if (status === 'Active') {
          isActive = 1;
          break;
        } else if (status === 'Disabled') {
          isActive = 0;
          break;
        }
      }
      if (this.shipTable.dataToRender) {
        this.shipTable.filter(isActive, 'accountEnabled', 'equals');
      }
    }
  }

  filterOnShipCustomSettingEnabled(event: Event) {
    let gotCustomSetting: boolean;
    const fieldName = this.getContextField();
    if(fieldName !== null ){
      if ( this.gotCustomSetting.length > 1  || this.gotCustomSetting.length === 0) {
        if (this.shipTable.dataToRender) {
          this.shipTable.filter('.', fieldName, 'notEquals');
        }
      } else {
        for (const settingsOn of this.gotCustomSetting) {
          if (settingsOn === 'Ship') {
            gotCustomSetting = true;
            break;
          } else if (settingsOn === 'Fleet') {
            gotCustomSetting = false;
            break;
          }
        }
        if (this.shipTable.dataToRender) {
          this.shipTable.filter(gotCustomSetting, fieldName, 'equals');
        }
      }
    }
  }

  getContextField (): string {
    switch(this.searchContext) {
      case 'quarantine':
        return 'quarantineLocalChanges';
    }
    return null;
  }
}
