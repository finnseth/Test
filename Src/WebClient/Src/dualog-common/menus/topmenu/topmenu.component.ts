import { ActivatedRoute, NavigationEnd, PRIMARY_OUTLET, Params, Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { MainMenuItem, MainMenuService } from '../../services/mainmenu.service';

@Component({
  selector: 'dua-topmenu',
  templateUrl: './topmenu.component.html',
  styleUrls: ['./topmenu.component.scss']
})
export class TopMenuComponent implements OnInit {

  topItems: MainMenuItem[];

  constructor(public menuService: MainMenuService, private router: Router, private activatedRoute: ActivatedRoute) { }

  ngOnInit() {

    const subitems: MainMenuItem[] = [];
    for (const item of this.menuService.items) {
      const topitem: MainMenuItem = Object.assign({}, item);
      topitem.submenu = null;
      subitems.push(topitem);
    }

    this.topItems = [
        {
          text: null,
          route: '/',
          submenu:  subitems
        }
    ];
  }
}
