import { Observable } from 'rxjs/Rx';
import { Injectable } from '@angular/core';

import { MainMenuItem } from './mainmenu.service';
import { AccessRights } from './../domain/permission/permission';

export interface MainMenuItem {
    text: string;
    icon?: string;
    image?: string;
    gridImage?: string;
    isOpen?: boolean;
    route: string;
    expanded?: boolean;
    access?: AccessRights,
    parent?: MainMenuItem;
    submenu?: Array<MainMenuItem>;
}

@Injectable()
export class MainMenuService {

    items: Array<MainMenuItem>;
    subitems: Array<MainMenuItem>;
    userMenu: Array<MainMenuItem>;
    selectedItem: MainMenuItem;
    topItem: MainMenuItem;

    constructor() {
    }

    public setItems(items: MainMenuItem[]) {
        this.items = items;
        this.SetParentReference(this.items);
    }

    public SetTopItem(item: MainMenuItem) {
        for (const topItem of this.items) {
            topItem.isOpen = false;
        }
        this.topItem = item;
        this.topItem.isOpen = true;
    }

    public SetSelectedItem(item: MainMenuItem): void {
        this.selectedItem = item;
        this.subitems = item.submenu;
        this.SetItemActive(item);
    }

    SetItemExpanded(selectedItem: MainMenuItem) {
        // this.CollapseItems(this.items);
        this.ExpandItems(selectedItem);
    }

    CollapseItems(items: MainMenuItem[]) {
        for (const item of items) {
            if (item.submenu !== undefined && item.submenu !== null) {
                this.CollapseItems(item.submenu);
            }
            item.expanded = false;
        }
    }

    CollapseItemWithChildren(item: MainMenuItem): void {
        item.expanded = false;
        if (item.submenu !== undefined && item.submenu !== null) {
            for (const i of item.submenu) {
                this.CollapseItemWithChildren(i);
            }
        }
    }

    ExpandItems(selectedItem: MainMenuItem): void {
        selectedItem.expanded = true;
        if (selectedItem.parent) {
            if (selectedItem.parent !== undefined) {
                this.ExpandItems(selectedItem.parent);
            }
        }
    }

    SetParentReference(items: MainMenuItem[]) {
        for (const item of items) {
            if (item.submenu !== undefined && item.submenu !== null) {
                for (const subitem of item.submenu) {
                    subitem.parent = item;
                    this.SetParentReference(item.submenu);
                }
            }
        }
    }

    SetItemActive(selectedItem: MainMenuItem) {
        this.DeactivateItems(this.items);
        selectedItem.isOpen = true;
    }

    DeactivateItems(items: MainMenuItem[]) {
        for (const item of items) {
            if (item.submenu !== undefined && item.submenu !== null) {
                this.DeactivateItems(item.submenu);
            }
            if (item === this.topItem && this.topItem.isOpen) {
            } else {
                item.isOpen = false;
            }
        }
    }

    GotMenuAccess(access: AccessRights) {
        return (access !== AccessRights.None);
    }

    public GetMenuItemByRoute(route: string): MainMenuItem {
        for (const menuitem of this.items) {
            const item = this.FindItemByRoute(menuitem, route);
            if (item !== null) {
                return item;
            }
        }
        return null;
    }

    public GetMenuSubItemsByRoute(route: string): MainMenuItem[] {
        const item = this.GetMenuItemByRoute(route);
        if (item.submenu) {
            return item.submenu;
        }
        return [];
    }

    public FindItemByRoute(item: MainMenuItem, route: string): MainMenuItem {
        if (item.route === route) {
            return item;
        } else {
            if (item.submenu !== null) {
                for (const sub of item.submenu) {
                    const output = this.FindItemByRoute(sub, route);
                    if (output !== null) {
                        return output;
                    }
                }
            }
        }
        return null;
    }
}
