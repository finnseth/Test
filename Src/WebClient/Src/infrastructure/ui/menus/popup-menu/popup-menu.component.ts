import { Component, Input, OnInit } from '@angular/core';
import { MainMenuItem, MainMenuService } from 'infrastructure/services/mainmenu.service';

@Component({
  selector: 'dua-popup-menu',
  templateUrl: './popup-menu.component.html',
  styleUrls: ['./popup-menu.component.scss']
})
export class PopupMenuComponent implements OnInit {

  @Input() menu: Array<MainMenuItem>;

  constructor(private menuService: MainMenuService) { }

  ngOnInit() {
  }

}
