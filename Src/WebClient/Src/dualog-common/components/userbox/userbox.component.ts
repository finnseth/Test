import { Component, OnInit } from '@angular/core';

import { MainMenuService } from '../../services/mainmenu.service';

@Component({
  selector: 'dua-userbox',
  templateUrl: './userbox.component.html',
  styleUrls: ['./userbox.component.scss']
})
export class UserboxComponent implements OnInit {

  constructor(private menuService: MainMenuService) { }

  ngOnInit() {
  }

}
