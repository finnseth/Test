import { Injectable } from '@angular/core';

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

  public SetSelectedItem(text: string): void {
    for ( const item of this.items ) {
      if (item.text === text) {
        this.selectedItem = item;
        this.subitems = item.submenu;
      }
    }
  }
}
