import { ActivatedRoute, Router } from '@angular/router';
import { Component, Input, OnInit } from '@angular/core';

import { MenuItem } from 'primeng/primeng';

@Component({
  selector: 'dua-hmenubar',
  templateUrl: './hmenubar.component.html',
  styleUrls: ['./hmenubar.component.scss']
})
export class HMenuBarComponent implements OnInit {

  @Input() items: MenuItem[] = [];

  private activeMenuItem: MenuItem = null;
  private activeMenuDOMItem = null;
  private openMenuItem: MenuItem = null;
  private openMenuDOMItem = null;

  constructor(private activatedRoute: ActivatedRoute, private router: Router) {
    router.events.subscribe( (url: any) => this.updateMenu() );
  }

  ngOnInit() {
    this.updateMenu();
  }

  private updateMenu() {
    this.openMenuItem = this.findActiveMenuItem(this.router.url, this.items);
  }

  private isMenuItemOpen(item: MenuItem) {
    return (item === this.openMenuItem);
  }

  private findActiveMenuItem(url: string, menu: MenuItem[]): MenuItem {
    for (const menuitem of menu) {
      if (url.indexOf(menuitem.routerLink) !== -1) {
        return menuitem;
      } else {
        if(menuitem.items !== undefined) {
          this.findActiveMenuItem(url, menuitem.items);
        }
      }
    }
  }

  public leaveOpenMenu(event: Event) {
    if (this.activeMenuItem !== this.openMenuItem) {
      if (this.activeMenuDOMItem !== null) {
        this.activeMenuDOMItem.classList.add('dualog-blue-second-invert');
      }
      if (this.openMenuDOMItem !== null) {
        this.openMenuDOMItem.classList.remove('dualog-blue-second-invert');
      }
    }
  }

  public openSite(event: Event) {
    if (this.openMenuItem !== null) {
      this.activeMenuItem = this.openMenuItem;
      this.activeMenuDOMItem = this.openMenuDOMItem;
    }
  }

  public activateSubMenu(event: Event, item: MenuItem): void {
    if (this.openMenuDOMItem !== null) {
      this.openMenuDOMItem.classList.remove('dualog-blue-second-invert');
    }
    if (this.activeMenuDOMItem !== null) {
      this.activeMenuDOMItem.classList.remove('dualog-blue-second-invert');
    }
    this.openMenuItem = item;
    this.openMenuDOMItem = event.target;
    this.openMenuDOMItem.classList.add('dualog-blue-second-invert');
  }

  public closeSubMenu(event: Event): void {
    this.activeMenuItem = null;
  }
}
