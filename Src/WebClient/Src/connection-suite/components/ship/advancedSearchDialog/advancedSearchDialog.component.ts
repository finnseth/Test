import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { DataTable, MenuItem, SelectItem } from 'primeng/primeng';

import { Ship } from '../interfaces';
import { ShipSelectorService } from '../shipSelector.service';

@Component({
  selector: 'dua-advanced-search-dialog',
  templateUrl: './advancedSearchDialog.component.html',
  styleUrls: ['./advancedSearchDialog.component.scss']
})
export class AdvancedSearchDialogComponent implements OnInit {

  @ViewChild('shipselectoradvancedsearch') shipTable: DataTable;

  @Input() context: string;
  @Input() searchStyle: string;
  @Input('title')
  get title() {
    return this._title;
  }
  set title(title: string){
    if (title !== undefined || title === null) {
      this._title = title;
    }
  }

  @Output() selectedShip = new EventEmitter<Ship>();

  private showsAdvancedSearch = false;
  _selectedShip: Ship;
  _title = 'Custom search';
  ships: Ship[];
  filterShips: Ship[];
  customSetups: SelectItem[];
  exportItems: MenuItem[];
  csvSeparator = ',';
  isSearching: boolean;

  fields = {
    category: false,
    customSetup: false
  }

  constructor(private shipService: ShipSelectorService) {
  }

  ngOnInit() {

    this.exportItems = [
            {label: 'Export to CSV (comma separator)', icon: 'fa-download', command: () => {
                this.csvSeparator = ',';
                this.shipTable.exportCSV();
            }},
            {label: 'Export to CSV (dot separator)', icon: 'fa-download', command: () => {
              this.csvSeparator = '.';
              this.shipTable.exportCSV();
            }},
            {label: 'Export to CSV (semi-colon separator)', icon: 'fa-download', command: () => {
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

  onShipSelected(ship: Ship) {
    this.selectedShip.emit(ship);
    this.showsAdvancedSearch = false;
  }

  showAdvancedSearch(event: Event) {
    this.showsAdvancedSearch = true;
    this.isSearching = true;

    switch (this.context) {
      case 'networkcontrol':
        this.shipService.getShipInComputerRuleContext().subscribe( ships => {
            this.ships = ships;
            this.fields.customSetup = true;
            this.isSearching = false;
        });
      break;
      case 'networkcontrol-computerrules':
        this.shipService.getShipInComputerRuleContext().subscribe( ships => {
            this.ships = ships;
            this.fields.customSetup = true;
            this.isSearching = false;
        });
      break;
      case 'users':
        this.shipService.getShipInUserContext().subscribe( ships => {
            this.ships = ships;
            this.isSearching = false;
        });
      break;
      case 'quarantine':
        this.shipService.getShips().subscribe( ships => {
            this.ships = ships;
            this.filterShips = this.ships;
            this.fields.customSetup = true;
        });
      break;
      default:
        this.shipService.getShips().subscribe( ships => {
            this.ships = ships;
            this.fields.category = true;
            this.isSearching = false;
        });
      break;
    }
  }
}
