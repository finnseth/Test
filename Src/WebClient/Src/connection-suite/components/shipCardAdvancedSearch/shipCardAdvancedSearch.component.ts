import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { DataTable, MenuItem, SelectItem } from 'primeng/primeng';

import { Ship } from 'connection-suite/components/ship/interfaces';
import { ShipCardSearchService } from 'connection-suite/components/shipCardSearch/shipCardSearch.service';

@Component({
  selector: 'dua-shipcardadvancedsearch',
  templateUrl: './shipCardAdvancedSearch.component.html',
  styleUrls: ['./shipCardAdvancedSearch.component.scss']
})
export class ShipCardAdvancedSearchComponent implements OnInit {
  
  @ViewChild('advancedsearchtable') shipTable: DataTable;

  @Input() searchContext: string;
  @Input() currentShip: Ship;
  
  @Output() onShipSelected = new EventEmitter<Ship>();
  

  _selectedShip: Ship;
  private enableAdvancedSearch = false;
  currentWorkingShip: Ship;
  ships: Ship[];
  filterShips: Ship[];
  customSetups: SelectItem[];
  isSearching: boolean;

  isShipActive: string[] = ['Active', 'Disabled'];
  gotCustomSetting: string[] = ['Ship', 'Fleet'];

  exportItems: MenuItem[];
  csvSeparator = ',';

  fields = {
    category: false,
    customSetup: false
  }

  constructor(private shipService: ShipCardSearchService) {
  }

  ngOnInit() {
    this.currentWorkingShip = this.currentShip;
    if (this.currentWorkingShip !== undefined) {
      this.onShipSelected.emit(this.currentWorkingShip);
      this._selectedShip = this.currentWorkingShip;
    }

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
    this.currentWorkingShip = this.currentShip;
    this.enableAdvancedSearch = false;
  }

  closeDialog(event: Event) {
  }

  advancedSearch(event: Event) {
    this._selectedShip = this.currentShip;
    this.enableAdvancedSearch = true;
    this.isSearching = true;

    switch (this.searchContext) {
      case 'quarantine':
        this.shipService.getQuarantineShips().subscribe( ships => {
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
    this.fields.customSetup = false;
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
