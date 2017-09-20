import { Injectable } from '@angular/core';
import { MainMenuItem } from './mainmenu.service';

export interface MainMenuItem {
  text: string,
  icon?: string,
  image?: string,
  route: string,
  submenu?: Array<MainMenuItem>
}

@Injectable()
export class MainMenuService {
  items: Array<MainMenuItem>;
  subitems: Array<MainMenuItem>;
  userMenu: Array<MainMenuItem>;
  selectedItem: MainMenuItem;

  public SetSelectedItem(item: MainMenuItem): void {
    this.selectedItem = item;
    this.subitems = item.submenu;
  }
}
