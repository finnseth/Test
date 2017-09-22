import { Injectable } from '@angular/core';

import { MainMenuItem } from './mainmenu.service';

export interface MainMenuItem {
    text: string;
    icon?: string;
    image?: string;
    gridImage?: string;
    route: string;
    submenu?: Array<MainMenuItem>;
}

@Injectable()
export class MainMenuService {
    items: Array<MainMenuItem>;
    subitems: Array<MainMenuItem>;
    userMenu: Array<MainMenuItem>;
    selectedItem: MainMenuItem;

    constructor(){
        console.log('MainMenuService created');
    }

    public SetSelectedItem(item: MainMenuItem): void {
        this.selectedItem = item;
        this.subitems = item.submenu;
    }

    public GetMenuItemsByRoute(route: string): MainMenuItem[] {
        console.log('GetMenuItemsByRoute - selectedItem: ');
        console.log(this.selectedItem);
        if (this.selectedItem !== undefined) {
         const item = this.findItemByRoute(this.selectedItem, route);
         if (item.submenu) {
            return item.submenu;
         }
        }
        return [];
    }

    private findItemByRoute(item: MainMenuItem, route: string): MainMenuItem {
        if (item.route === route) {
            return item;
        } else {
            if (item.submenu !== null) {
                for (const sub of item.submenu) {
                    const output = this.findItemByRoute(sub, route);
                    if (output !== null) {
                        return output;
                    }
                }
            }
        }
        return null;
    }
}
