import { Component, Input, OnInit } from '@angular/core';
import { MainMenuItem, MainMenuService } from '../../services/mainmenu.service';

@Component({
  selector: 'dua-popup-topmenu',
  templateUrl: './popup-topmenu.component.html',
  styleUrls: ['./popup-topmenu.component.scss']
})
export class PopupTopMenuComponent implements OnInit {

  @Input() menu: Array<MainMenuItem>;

  constructor(private menuService: MainMenuService) { }

  ngOnInit() {
  }

}
