import { Component, OnInit } from '@angular/core';

import { MainMenuService } from '../../services/mainmenu.service';

@Component({
  selector: 'dua-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.scss']
})
export class MenuComponent implements OnInit {

  constructor(public menuService: MainMenuService) { }

  ngOnInit() {
  }

}
